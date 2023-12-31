####Notebook "ACTUAL_PRODUCT_PARAMS_CALCULATION_PROCESS". 
####*Calculate actual product parameters and ActualPromoLSVSI, ActualPromoLSVByCompensation*.
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

def run(calcActualPromoProductDF,actualParamsPriceListDF,calcActualPromoDF,allCalcActualPromoDF,promoProductCols):
    sc = SparkContext.getOrCreate();
    spark = SparkSession(sc)
    
    #####*Set product price*

    byPriceStartDate = Window.partitionBy(calcActualPromoProductDF.columns).orderBy(col("priceStartDate").desc())

    calcActualPromoProductDF = calcActualPromoProductDF\
      .join(actualParamsPriceListDF, 
            [\
              actualParamsPriceListDF.priceStartDate <= calcActualPromoProductDF.promoDispatchesStart,
              actualParamsPriceListDF.priceEndDate >= calcActualPromoProductDF.promoDispatchesStart,
              actualParamsPriceListDF.priceClientTreeId == calcActualPromoProductDF.promoClientTreeKeyId,
              actualParamsPriceListDF.priceProductId == calcActualPromoProductDF.ProductId
            ], 
            'left')\
      .select(\
               calcActualPromoProductDF['*']
              ,actualParamsPriceListDF.priceStartDate
              ,actualParamsPriceListDF.Price.alias('newPrice')
             )\
      .withColumn('Row_Number', row_number().over(byPriceStartDate))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    calcActualPromoProductDF = calcActualPromoProductDF\
      .withColumn('Price', when(((col('Price').isNull()) | (col('Price') == 0)), col('newPrice')).otherwise(col('Price')))\
      .drop('newPrice')

    #product price logging
    logPricePromoProductDF = calcActualPromoProductDF\
      .select(\
               calcActualPromoProductDF.promoNumber
              ,calcActualPromoProductDF.ZREP
              ,calcActualPromoProductDF.Price
             )\
      .withColumn('NullPrice', when(calcActualPromoProductDF.Price.isNull(), True).otherwise(None))\
      .withColumn('ZeroPrice', when(calcActualPromoProductDF.Price == 0, True).otherwise(None))

    logNullPricePromoDF = logPricePromoProductDF\
      .where(col('NullPrice') == True)\
      .groupBy('promoNumber')\
      .agg(concat_ws(';', collect_list(col('ZREP'))).alias('nullPriceMessage'))

    logZeroPricePromoDF = logPricePromoProductDF\
      .where(col('ZeroPrice') == True)\
      .groupBy('promoNumber')\
      .agg(concat_ws(';', collect_list(col('ZREP'))).alias('zeroPriceMessage'))

    logPricePromoDF = logNullPricePromoDF\
      .join(logZeroPricePromoDF, 'promoNumber', 'full')
    #---

    #####*Calculate actual product parameters*

    # calcPlanPromoProductDF = calcPlanPromoProductDF\
    #   .join(planParamsIncrementalDF, 
    #         [\
    #           planParamsIncrementalDF.incrementalPromoId == calcPlanPromoProductDF.PromoId
    #          ,planParamsIncrementalDF.incrementalProductId == calcPlanPromoProductDF.ProductId
    #         ]
    #         ,'left')\
    #   .select(\
    #            calcPlanPromoProductDF['*']
    #           ,planParamsIncrementalDF.PlanPromoIncrementalCases.cast(DecimalType(30,6))
    #          )

    calcActualPromoProductDF = calcActualPromoProductDF\
      .withColumn('isActualPromoBaseLineLSVChangedByDemand', when((~col('ActualPromoBaselineLSV').isNull()) & (col('ActualPromoBaselineLSV') != col('PlanPromoBaselineLSV')), True).otherwise(False))\
      .withColumn('isActualPromoLSVChangedByDemand', when((~col('ActualPromoLSVSO').isNull()) & (col('ActualPromoLSVSO') != 0), True).otherwise(False))\
      .withColumn('isActualPromoProstPromoEffectLSVChangedByDemand', when((~col('ActualPromoPostPromoEffectLSV').isNull()) & (col('ActualPromoPostPromoEffectLSV') != 0), True).otherwise(False))\
      .withColumn('ActualProductLSVByCompensation', (col('ActualProductPCQty') * col('ActualProductSellInPrice')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductLSVByCompensation', when(col('ActualProductLSVByCompensation').isNull(), 0)\
                  .otherwise(col('ActualProductLSVByCompensation')).cast(DecimalType(30,6)))

    sumActualProductParamsList = calcActualPromoProductDF\
      .select(\
                col('promoIdCol')
               ,col('ActualProductLSVByCompensation')
             )\
      .groupBy('promoIdCol')\
      .agg(sum('ActualProductLSVByCompensation').cast(DecimalType(30,6)).alias('calcActualPromoLSVByCompensation'))\
      .collect()

    actualParSchema = StructType([
      StructField("promoIdCol", StringType(), True),
      StructField("calcActualPromoLSVByCompensation", DecimalType(30,6), True)
    ])

    actualParDF = spark.createDataFrame(sumActualProductParamsList, actualParSchema)

    calcActualPromo1DF = calcActualPromoDF\
      .join(actualParDF, actualParDF.promoIdCol == calcActualPromoDF.Id, 'inner')\
      .withColumn('ActualPromoLSVByCompensation', when(col('calcActualPromoLSVByCompensation') == 0, col('ActualPromoLSVByCompensation')).otherwise(col('calcActualPromoLSVByCompensation')))

    calcActualPromo1DF = calcActualPromo1DF\
      .select('Id', 'ActualPromoLSVByCompensation')

    allCalcActualPromoDF = allCalcActualPromoDF\
      .join(calcActualPromo1DF, 'Id', 'left')\
      .select(\
               allCalcActualPromoDF['*']
              ,calcActualPromo1DF.ActualPromoLSVByCompensation.alias('calcActualPromoLSVByCompensation')
             )\
      .withColumn('ActualPromoLSVByCompensation', when(col('calcActualPromoLSVByCompensation').isNull(), col('ActualPromoLSVByCompensation'))\
                  .otherwise(col('calcActualPromoLSVByCompensation')))\
      .withColumn('ActualPromoLSVSI', when(col('calcActualPromoLSVByCompensation') == 0, None).otherwise(col('calcActualPromoLSVByCompensation')))\
      .drop('calcActualPromoLSVByCompensation')

    calcActualPromoProductDF.show()

    calcActualPromoProductDF = calcActualPromoProductDF.alias('pr')\
      .join(allCalcActualPromoDF.alias('p'), col('pr.promoIdCol') == col('p.Id'), 'inner')\
      .select(allCalcActualPromoDF.ActualPromoLSVByCompensation, col('p.ActualPromoLSVSO'), col('p.ActualPromoLSVSI'), calcActualPromoProductDF['*'])

    #calcActualPromoProductDF = calcActualPromoProductDF\
    #  .join(allCalcActualPromoDF, calcActualPromoProductDF.promoIdCol == allCalcActualPromoDF.Id, 'inner')\
    #  .select(allCalcActualPromoDF.ActualPromoLSVByCompensation, allCalcActualPromoDF.ActualPromoLSVSO, calcActualPromoProductDF['*'])

    #####*Get result*
    calcActualPromoProductDF = calcActualPromoProductDF\
      .withColumn('ActualProductBaselineLSV', when((col('promoInOut') == 'False') & (~col('isActualPromoBaseLineLSVChangedByDemand')), col('PlanProductBaselineLSV'))\
                                          .otherwise(col('ActualProductBaselineLSV')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductCaseQty', when(((col('UOM_PC2Case') != 0) & ~(col('ActualProductPCQty').isNull())), col('ActualProductPCQty') / col('UOM_PC2Case'))\
                  .otherwise(0).cast(IntegerType()))\
      .withColumn('ActualProductSellInPrice', when(col('UOM_PC2Case') != 0, col('Price') / col('UOM_PC2Case')).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualProductBaselineCaseQty', when((col('Price') != 0) & ~(col('Price').isNull()), col('ActualProductBaselineLSV') / col('Price'))\
                                          .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPCLSV', col('ActualProductPCQty') * col('ActualProductSellInPrice').cast(DecimalType(30,6)))\
      .withColumn('ActualProductPCLSV', when(col('ActualProductPCLSV').isNull(), 0).otherwise(col('ActualProductPCLSV')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductLSV', when((col('promoIsOnInvoice') == True), col('ActualProductPCLSV'))\
                                          .otherwise(when(col('isActualPromoLSVChangedByDemand') == False, 0).otherwise(col('ActualProductLSV'))).cast(DecimalType(30,6)))\
      .withColumn('ActualProductIncrementalLSV', when(col('ActualProductBaselineLSV').isNull(), col('ActualProductLSV'))\
                                          .otherwise(col('ActualProductLSV') - col('ActualProductBaselineLSV')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductUpliftPercent', when(col('promoInOut') == 'False', 
                                                     when(col('ActualProductBaselineLSV') != 0, col('ActualProductIncrementalLSV') / col('ActualProductBaselineLSV') * 100.0)\
                                                     .otherwise(0))\
                                                .otherwise(None).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPostPromoEffectQtyW1', when(col('promoInOut') == 'False', col('PlanProductBaselineCaseQty') * col('PlanProductPostPromoEffectW1') / 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPostPromoEffectQtyW2', when(col('promoInOut') == 'False', col('PlanProductBaselineCaseQty') * col('PlanProductPostPromoEffectW2') / 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPostPromoEffectQty', when(col('promoInOut') == 'False', col('ActualProductPostPromoEffectQtyW1') + col('ActualProductPostPromoEffectQtyW2'))\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualProductIncrementalPCQty', when(col('ActualProductSellInPrice') != 0, col('ActualProductIncrementalLSV') / col('ActualProductSellInPrice'))\
                                                  .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualProductIncrementalPCLSV', when(col('UOM_PC2Case') != 0, col('ActualProductIncrementalLSV') / col('UOM_PC2Case'))\
                                                  .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualProductQtySO', (col('ActualProductLSV') / (col('Price') /col('UOM_PC2Case'))).cast(DecimalType(30,6)))\
      .withColumn('PCPrice', (col('Price') / col('UOM_PC2Case')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductBaselineVolume', (col('ActualProductBaselineLSV') / col('PCPrice') * col('PCVolume')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPostPromoEffectLSVW1', when((col('promoIsOnInvoice') == True), (col('PlanProductPostPromoEffectW1') / 100) * col('ActualProductBaselineLSV') +\
                                          (col('p.ActualPromoLSVSO') * (col('ActualProductLSVByCompensation') / col('ActualPromoLSVByCompensation')) - col('ActualProductLSV')) *\
                                          (col('PlanProductPostPromoEffectW1') / (col('PlanProductPostPromoEffectW1') + col('PlanProductPostPromoEffectW2'))) )\
                                          .otherwise((col('PlanProductPostPromoEffectW1') / 100) * col('ActualProductBaselineLSV')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPostPromoEffectLSVW2', when((col('promoIsOnInvoice') == True), (col('PlanProductPostPromoEffectW2') / 100) * col('ActualProductBaselineLSV') +\
                                          (col('p.ActualPromoLSVSO') * (col('ActualProductLSVByCompensation') / col('ActualPromoLSVByCompensation')) - col('ActualProductLSV')) *\
                                          (col('PlanProductPostPromoEffectW2') / (col('PlanProductPostPromoEffectW1') + col('PlanProductPostPromoEffectW2'))) )\
                                          .otherwise((col('PlanProductPostPromoEffectW2') / 100) * col('ActualProductBaselineLSV')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPostPromoEffectLSV', (col('ActualProductPostPromoEffectLSVW1') + col('ActualProductPostPromoEffectLSVW2')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductPostPromoEffectVolume', (col('ActualProductPostPromoEffectLSV') / col('PCPrice') * col('PCVolume')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductVolumeByCompensation', (col('ActualProductPCQty') * col('PCVolume')).cast(DecimalType(30,6)))\
      .withColumn('ActualProductVolume', (col('ActualProductQtySO') * col('PCVolume')).cast(DecimalType(30,6)))

    #####*Calculate ActualPromoLSVByCompensation, ActualPromoLSVSI*

    sumActualProductParamsList = calcActualPromoProductDF\
      .select(\
                col('promoIdCol')
               ,col('ActualProductBaselineVolume')
               ,col('ActualProductPostPromoEffectVolume')
               ,col('ActualProductVolumeByCompensation')
               ,col('ActualProductPostPromoEffectLSVW1')
               ,col('ActualProductPostPromoEffectLSVW2')
               ,col('ActualProductVolume')
             )\
      .groupBy('promoIdCol')\
      .agg(sum('ActualProductBaselineVolume').cast(DecimalType(30,6)).alias('calcActualPromoBaselineVolume'),
           sum('ActualProductPostPromoEffectVolume').cast(DecimalType(30,6)).alias('calcActualPromoPostPromoEffectVolume'),
           sum('ActualProductVolumeByCompensation').cast(DecimalType(30,6)).alias('calcActualPromoVolumeByCompensation'),
           sum('ActualProductPostPromoEffectLSVW1').cast(DecimalType(30,6)).alias('calcActualPromoPostPromoEffectLSVW1'),
           sum('ActualProductPostPromoEffectLSVW2').cast(DecimalType(30,6)).alias('calcActualPromoPostPromoEffectLSVW2'),
           sum('ActualProductVolume').cast(DecimalType(30,6)).alias('calcActualPromoVolume'))\
      .collect()

    actualParSchema = StructType([
      StructField("promoIdCol", StringType(), True),
      StructField("calcActualPromoBaselineVolume", DecimalType(30,6), True),
      StructField("calcActualPromoPostPromoEffectVolume", DecimalType(30,6), True),
      StructField("calcActualPromoVolumeByCompensation", DecimalType(30,6), True),
      StructField("calcActualPromoPostPromoEffectLSVW1", DecimalType(30,6), True),
      StructField("calcActualPromoPostPromoEffectLSVW2", DecimalType(30,6), True),
      StructField("calcActualPromoVolume", DecimalType(30,6), True)
    ])

    actualParDF = spark.createDataFrame(sumActualProductParamsList, actualParSchema)

    calcActualPromoDF = calcActualPromoDF\
      .join(actualParDF, actualParDF.promoIdCol == calcActualPromoDF.Id, 'inner')\
      .withColumn('ActualPromoPostPromoEffectVolume', when(col('InOut') == False, col('calcActualPromoPostPromoEffectVolume')).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoVolumeByCompensation', when(col('InOut') == False, col('calcActualPromoVolumeByCompensation')).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoPostPromoEffectLSVW1', when(col('calcActualPromoPostPromoEffectLSVW1') == 0, None).otherwise(col('calcActualPromoPostPromoEffectLSVW1')))\
      .withColumn('ActualPromoPostPromoEffectLSVW2', when(col('calcActualPromoPostPromoEffectLSVW2') == 0, None).otherwise(col('calcActualPromoPostPromoEffectLSVW2')))\
      .withColumn('ActualPromoPostPromoEffectLSV', (col('ActualPromoPostPromoEffectLSVW1') + col('ActualPromoPostPromoEffectLSVW2')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoVolume',  when(col('IsOnInvoice') == False, col('calcActualPromoVolume')).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoVolume',  when(col('InOut') == True, col('calcActualPromoVolume')).otherwise(col('ActualPromoVolume')).cast(DecimalType(30,6)))

    calcActualPromoDF = calcActualPromoDF\
      .select('Id', 'calcActualPromoBaselineVolume', 'ActualPromoPostPromoEffectVolume', 'ActualPromoVolumeByCompensation', 'ActualPromoPostPromoEffectLSVW1', 'ActualPromoPostPromoEffectLSVW2', 'ActualPromoPostPromoEffectLSV', 'ActualPromoVolume')

    allCalcActualPromoDF = allCalcActualPromoDF\
      .join(calcActualPromoDF, 'Id', 'left')\
      .select(\
               allCalcActualPromoDF['*']
              ,calcActualPromoDF.calcActualPromoBaselineVolume.alias('calcActualPromoBaselineVolume')
              ,calcActualPromoDF.ActualPromoPostPromoEffectVolume.alias('calcActualPromoPostPromoEffectVolume')
              ,calcActualPromoDF.ActualPromoVolumeByCompensation.alias('calcActualPromoVolumeByCompensation')
              ,calcActualPromoDF.ActualPromoPostPromoEffectLSVW1.alias('calcActualPromoPostPromoEffectLSVW1')
              ,calcActualPromoDF.ActualPromoPostPromoEffectLSVW2.alias('calcActualPromoPostPromoEffectLSVW2')
              ,calcActualPromoDF.ActualPromoPostPromoEffectLSV.alias('calcActualPromoPostPromoEffectLSV')
              ,calcActualPromoDF.ActualPromoVolume.alias('calcActualPromoVolume')
             )\
      .withColumn('ActualPromoBaselineVolume', when(col('calcActualPromoBaselineVolume').isNull(), col('ActualPromoBaselineVolume')).otherwise(col('calcActualPromoBaselineVolume')))\
      .withColumn('ActualPromoPostPromoEffectVolume', when(col('calcActualPromoPostPromoEffectVolume').isNull(), col('ActualPromoPostPromoEffectVolume')).otherwise(col('calcActualPromoPostPromoEffectVolume')))\
      .withColumn('ActualPromoVolumeByCompensation', when(col('calcActualPromoVolumeByCompensation').isNull(), col('ActualPromoVolumeByCompensation')).otherwise(col('calcActualPromoVolumeByCompensation')))\
      .withColumn('ActualPromoPostPromoEffectLSVW1', when(col('calcActualPromoPostPromoEffectLSVW1').isNull(), col('ActualPromoPostPromoEffectLSVW1')).otherwise(col('calcActualPromoPostPromoEffectLSVW1')))\
      .withColumn('ActualPromoPostPromoEffectLSVW2', when(col('calcActualPromoPostPromoEffectLSVW2').isNull(), col('ActualPromoPostPromoEffectLSVW2')).otherwise(col('calcActualPromoPostPromoEffectLSVW2')))\
      .withColumn('ActualPromoPostPromoEffectLSV', when(col('calcActualPromoPostPromoEffectLSV').isNull(), col('ActualPromoPostPromoEffectLSV')).otherwise(col('calcActualPromoPostPromoEffectLSV')))\
      .withColumn('ActualPromoVolume', when(col('calcActualPromoVolume').isNull(), col('ActualPromoVolume')).otherwise(col('calcActualPromoVolume')))\
      .drop('calcActualPromoBaselineVolume','calcActualPromoPostPromoEffectVolume','calcActualPromoVolumeByCompensation','calcActualPromoVolume')

    #####*Get result*

    calcActualPromoProductDF = calcActualPromoProductDF.select(promoProductCols)
    print('Actual product parameters calculation completed!')
    
    return calcActualPromoProductDF,allCalcActualPromoDF,logPricePromoDF