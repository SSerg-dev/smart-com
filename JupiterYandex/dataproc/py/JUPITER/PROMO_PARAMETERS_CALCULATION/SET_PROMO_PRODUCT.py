####Notebook "SET_PROMO_PRODUCT". 
####*Get product set for promo*.
###### *Developer: [LLC Smart-Com](http://smartcom.software/), andrey.philushkin@effem.com*

from pyspark.sql import SQLContext, DataFrame, Row, Window
from pyspark.sql import SparkSession
from pyspark.context import SparkContext
from pyspark.sql.types import *
from pyspark.sql.functions import *
import pyspark.sql.functions as F
import pandas as pd
import datetime, time
import os
import json

def run(notInOutCalcPlanPromoDF,notInOutCalcPlanPromoProductDF,promoProductTreeDF,productDF,productTreeDF,assortmentMatrixDF,allProductDF,allProduct01DF,schema):
    sc = SparkContext.getOrCreate();
    spark = SparkSession(sc)
    #####*Get AM Products*
    
    #tpm modes for recalculation: Current, RS, RA
    calcTPMmodes = [0, 1, 2]
    
    notInOutCalcPlanPromoDF = notInOutCalcPlanPromoDF.withColumn("InOutProductIds",upper(col("InOutProductIds")))
    setProductDF = notInOutCalcPlanPromoDF\
      .join(promoProductTreeDF, promoProductTreeDF.PromoId == notInOutCalcPlanPromoDF.Id, 'inner')\
      .join(productTreeDF, productTreeDF.ObjectId == promoProductTreeDF.ProductTreeObjectId, 'inner')\
      .select(\
               notInOutCalcPlanPromoDF.Id.alias('promoId')
              ,notInOutCalcPlanPromoDF.ClientTreeKeyId
              ,notInOutCalcPlanPromoDF.DispatchesStart
              ,promoProductTreeDF.Disabled.alias('pptDisabled')
              ,promoProductTreeDF.TPMmode.alias('pptTPMmode')
              ,notInOutCalcPlanPromoDF.Number.alias('pNumber')
              ,productTreeDF.EndDate
              ,lower(productTreeDF.FilterQuery).alias('FilterQuery')
              ,notInOutCalcPlanPromoDF.InOutProductIds.alias('promoInOutProductIds')
             )\
      .where((col('pptDisabled') == 'false') & col('pptTPMmode').isin(*calcTPMmodes) & (col('EndDate').isNull()))

    setProductDF = setProductDF\
      .withColumn('CheckedProductList', split(setProductDF.promoInOutProductIds, ';'))\
      .drop('EndDate','promoInOutProductIds')

    amProductDF = setProductDF\
      .join(assortmentMatrixDF, 
            [\
              assortmentMatrixDF.ClientTreeId == setProductDF.ClientTreeKeyId
             ,assortmentMatrixDF.StartDate <= setProductDF.DispatchesStart
             ,assortmentMatrixDF.EndDate >= setProductDF.DispatchesStart
            ],
            'inner')\
      .select(setProductDF['*'], assortmentMatrixDF.ProductId)

    amProductDF = amProductDF\
      .join(allProductDF, allProductDF.Id == amProductDF.ProductId, 'inner')\
      .select(amProductDF['*'], allProductDF.EAN_PC)\
      .drop('ProductId')

    amProductDF = amProductDF\
      .join(allProduct01DF, allProduct01DF.EAN_PC == amProductDF.EAN_PC, 'inner')\
      .select(amProductDF['*'], allProduct01DF.Id.alias('ProductId'))

    #####*Convert productDF to lower case*

    lowerCaseProductDF = productDF.select(*[lower(col(c)).name(c) for c in productDF.columns])
    lowerCaseProductDF = lowerCaseProductDF.toDF(*[c.lower() for c in lowerCaseProductDF.columns])

    #####*Get Filtered Products DF*

    setProductList = setProductDF.collect()

    filteredArray = []
    lowerCaseProductDF.registerTempTable("product")

    filteredProductSchema = StructType([
      StructField("Id", StringType(), False),
      StructField("filteredEAN_PC", StringType(), False),
      StructField("filteredZREP", StringType(), False),
      StructField("Number", IntegerType(), False),
      StructField("filteredPromoId", StringType(), False),
    ])

    for i, item in enumerate(setProductList):
    #   print(i, item.pNumber, item.promoId)
    #  print(item.FilterQuery)
      productFilter = item.FilterQuery.replace('['+schema.lower()+'].[', '').replace('].[', '.').replace('[', '').replace(']', '').replace('*', 'id,ean_pc,zrep')
      filteredList = spark.sql(productFilter).collect()
    #   print(productFilter)
    #   print(filteredList)
      for f in filteredList:
        filteredArray.append([f[0], f[1], f[2], item.pNumber, item.promoId])
        
    filteredProductDF = spark.createDataFrame(filteredArray, filteredProductSchema)
    filteredProductDF = filteredProductDF.withColumn('Id',upper(col('Id'))).withColumn('filteredPromoId',upper(col('filteredPromoId')))
    
    # display(filteredProductDF)

    #####*Get Union Filtered Products DF *

    resultFilteredProductDF = amProductDF\
      .join(filteredProductDF,
            [\
              filteredProductDF.Id == amProductDF.ProductId
             ,filteredProductDF.filteredPromoId == amProductDF.promoId
            ],
            'inner')\
      .drop(filteredProductDF.Id)\
      .drop(filteredProductDF.filteredEAN_PC)

    resultFilteredProductDF = resultFilteredProductDF.where(expr("array_contains(CheckedProductList, ProductId)"))

    #####*Get result product set for promoes*

    notInOutCalcPlanPromoProductDF = resultFilteredProductDF\
      .join(notInOutCalcPlanPromoProductDF, 
            [\
              notInOutCalcPlanPromoProductDF.ProductId == resultFilteredProductDF.ProductId
             ,notInOutCalcPlanPromoProductDF.PromoId == resultFilteredProductDF.filteredPromoId
            ],
            'full')\
      .select(\
               notInOutCalcPlanPromoProductDF['*']
              ,resultFilteredProductDF.Number
              ,resultFilteredProductDF.ProductId.alias('ResultFilteredProductId')
              ,resultFilteredProductDF.filteredZREP.alias('ResultFilteredZREP')
              ,resultFilteredProductDF.filteredPromoId.alias('pId')
             )\
      .dropDuplicates()

    notInOutCalcPlanPromoProductDF = notInOutCalcPlanPromoProductDF\
      .withColumn('Action', when(notInOutCalcPlanPromoProductDF.ResultFilteredProductId.isNull(), lit('Deleted'))\
                  .otherwise(when(notInOutCalcPlanPromoProductDF.ProductId.isNull(), lit('Added'))\
                  .otherwise('')))

    notInOutCalcPlanPromoProductDF = notInOutCalcPlanPromoProductDF\
      .withColumn('Disabled', when(notInOutCalcPlanPromoProductDF.Action == lit('Deleted'), True)\
                  .otherwise(when(notInOutCalcPlanPromoProductDF.Action == lit('Added'), False).otherwise(notInOutCalcPlanPromoProductDF.Disabled)))\
      .withColumn('DeletedDate', when(notInOutCalcPlanPromoProductDF.Action == lit('Deleted'), current_timestamp())\
                                     .otherwise(notInOutCalcPlanPromoProductDF.DeletedDate))\
      .withColumn('PromoId', when(notInOutCalcPlanPromoProductDF.Action == lit('Added'), col('pId')).otherwise(col('PromoId')))\
      .withColumn('ProductId', when(notInOutCalcPlanPromoProductDF.Action == lit('Added'), col('ResultFilteredProductId')).otherwise(col('ProductId')))\
      .withColumn('ZREP', when(notInOutCalcPlanPromoProductDF.Action == lit('Added'), col('ResultFilteredZREP')).otherwise(col('ZREP')))\
      .withColumn('promoNumber', when(notInOutCalcPlanPromoProductDF.Action == lit('Added'), col('Number')).otherwise(col('promoNumber')))\
      .withColumn('AverageMarker', when(notInOutCalcPlanPromoProductDF.Action == lit('Added'), False).otherwise(col('AverageMarker')))
      
    print(notInOutCalcPlanPromoProductDF.count())
      
    return notInOutCalcPlanPromoProductDF  