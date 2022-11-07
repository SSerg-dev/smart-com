####Notebook "PLAN_PRODUCT_PARAMS_CALCULATION_PROCESS". 
####*Calculate plan product parameters and plan promo baseline, incremental and LSV*.
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

def run(calcPlanPromoProductDF,planParamsPriceListDF,planParamsBaselineDF,calcPlanPromoDF,allCalcPlanPromoDF,planParamsSharesDF,datesDF,planParamsCorrectionDF,planParamsIncrementalDF,planParametersStatuses,promoProductCols):
    sc = SparkContext.getOrCreate();
    spark = SparkSession(sc)
    
    byPriceStartDate = (Window.partitionBy('PromoId', 'ProductId').orderBy(col("priceStartDate").desc()))

    # calcPlanPromoProductDF = calcPlanPromoProductDF.drop('Price')
    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(planParamsPriceListDF, 
            [\
              planParamsPriceListDF.priceStartDate <= calcPlanPromoProductDF.promoDispatchesStart,
              planParamsPriceListDF.priceEndDate >= calcPlanPromoProductDF.promoDispatchesStart,
              planParamsPriceListDF.priceClientTreeId == calcPlanPromoProductDF.promoClientTreeKeyId,
              planParamsPriceListDF.priceProductId == calcPlanPromoProductDF.ProductId
            ], 
            'left')\
      .select(\
               calcPlanPromoProductDF['*']
              ,planParamsPriceListDF.priceStartDate
              ,planParamsPriceListDF.Price.alias('calcPrice')
             )\
      .withColumn('Row_Number', row_number().over(byPriceStartDate))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .withColumn('Price', col('calcPrice'))\
      .drop('calcPrice')

    #product price logging
    logPricePromoProductDF = calcPlanPromoProductDF\
      .select(\
               calcPlanPromoProductDF.promoNumber
              ,calcPlanPromoProductDF.ZREP
              ,calcPlanPromoProductDF.Price
             )\
      .withColumn('NullPrice', when(calcPlanPromoProductDF.Price.isNull(), True).otherwise(None))\
      .withColumn('ZeroPrice', when(calcPlanPromoProductDF.Price == 0, True).otherwise(None))

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

    calcPlanPromoProductDF = calcPlanPromoProductDF.fillna(0, 'Price')

    #####*Set product baseline*

    # calcPlanPromoProductDF = calcPlanPromoProductDF.drop('PlanProductBaselineCaseQty')

    # set product shares
    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(planParamsSharesDF,
           [\
             planParamsSharesDF.BrandTechId == calcPlanPromoProductDF.promoBrandTechId
            ,planParamsSharesDF.ClientTreeId == calcPlanPromoProductDF.promoClientTreeKeyId
            ,planParamsSharesDF.ParentClientTreeDemandCode == calcPlanPromoProductDF.promoDemandCode
           ],
           'left')\
      .select(\
               calcPlanPromoProductDF['*']
              ,planParamsSharesDF.Share.cast(DecimalType(30,6)).alias('productShare')
             )

    #product share logging
    logSharePromoProductDF = calcPlanPromoProductDF\
      .select(\
               calcPlanPromoProductDF.promoNumber
              ,calcPlanPromoProductDF.ZREP
              ,calcPlanPromoProductDF.productShare
             )\
      .withColumn('NullShare', when(calcPlanPromoProductDF.productShare.isNull(), True).otherwise(None))\
      .withColumn('ZeroShare', when(calcPlanPromoProductDF.productShare == 0, True).otherwise(None))

    logNullSharePromoDF = logSharePromoProductDF\
      .where(col('NullShare') == True)\
      .groupBy('promoNumber')\
      .agg(concat_ws(';', collect_list(col('ZREP'))).alias('nullShareMessage'))

    logZeroSharePromoDF = logSharePromoProductDF\
      .where(col('ZeroShare') == True)\
      .groupBy('promoNumber')\
      .agg(concat_ws(';', collect_list(col('ZREP'))).alias('zeroShareMessage'))

    logSharePromoDF = logNullSharePromoDF\
      .join(logZeroSharePromoDF, 'promoNumber', 'full')

    logPromoProductDF = logPricePromoDF\
      .join(logSharePromoDF, 'promoNumber', 'full')
    # ---

    calcPlanPromoProductDF = calcPlanPromoProductDF.fillna(0, 'productShare')
    # ---

    # split promo duration by weeks
    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(datesDF, 
            [\
              datesDF.OriginalDate >= calcPlanPromoProductDF.promoStartDate
             ,datesDF.OriginalDate <= calcPlanPromoProductDF.promoEndDate
            ], 
            'inner')\
      .select(\
              calcPlanPromoProductDF['*']
             ,datesDF.MarsWeekFullName
             ,datesDF.MarsDay
             )

    cols = calcPlanPromoProductDF.columns
    cols.remove('MarsDay')

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .groupBy(cols)\
      .agg(count('*').cast(DecimalType(30,6)).alias('promoDaysInWeek'))
    #  ---

    # set product baseline
    planParamsBaselineDF = planParamsBaselineDF\
      .join(datesDF, planParamsBaselineDF.baselineStartDate == datesDF.OriginalDate, 'inner')\
      .select(\
              planParamsBaselineDF['*']
             ,datesDF.MarsWeekFullName
             )

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(planParamsBaselineDF, 
           [\
             planParamsBaselineDF.baselineProductId == calcPlanPromoProductDF.ProductId
            ,planParamsBaselineDF.baselineDemandCode == calcPlanPromoProductDF.promoDemandCode
            ,planParamsBaselineDF.MarsWeekFullName == calcPlanPromoProductDF.MarsWeekFullName
           ],
           'left')

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .fillna(0, ['SellInBaselineQTY', 'SellOutBaselineQTY'])\
      .drop('MarsWeekFullName')

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .withColumn(\
                   'weeklyBaseline'
                  ,when(col('promoIsOnInvoice') == 'True', ((col('SellInBaselineQTY') * col('promoDaysInWeek') / 7.0) * (col('productShare') / 100.0)).cast(DecimalType(30,6)))
                       .otherwise(((col('SellOutBaselineQTY') * col('promoDaysInWeek') / 7.0) * (col('productShare') / 100.0)).cast(DecimalType(30,6)))
                 )\
      .drop('baselineDemandCode', 'baselineProductId', 'baselineStartDate', 'promoDaysInWeek', 'SellInBaselineQTY', 'SellOutBaselineQTY')
      
    cols = calcPlanPromoProductDF.columns
    cols.remove('weeklyBaseline')

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .groupBy(cols)\
      .agg(sum('weeklyBaseline').cast(DecimalType(30,6)).alias('calcPlanProductBaselineCaseQty'))

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .withColumn('PlanProductBaselineCaseQty', when(col('promoInOut') == 'False', col('calcPlanProductBaselineCaseQty').cast(DecimalType(30,6))).otherwise(None))\
      .drop('calcPlanProductBaselineCaseQty')

    #product baseline qty logging
    logBaselineQtyPromoProductDF = calcPlanPromoProductDF\
      .select(\
               calcPlanPromoProductDF.promoNumber
              ,calcPlanPromoProductDF.ZREP
              ,calcPlanPromoProductDF.PlanProductBaselineCaseQty
             )\
      .withColumn('ZeroBaselineQty', when(calcPlanPromoProductDF.PlanProductBaselineCaseQty == 0, True).otherwise(None))

    logZeroBaselineQtyPromoDF = logBaselineQtyPromoProductDF\
      .where(col('ZeroBaselineQty') == True)\
      .groupBy('promoNumber')\
      .agg(concat_ws(';', collect_list(col('ZREP'))).alias('zeroBaselineQtyMessage'))

    logPromoProductDF = logPromoProductDF\
      .join(logZeroBaselineQtyPromoDF, 'promoNumber', 'full')
    # ---

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .withColumn('PlanProductBaselineLSV', when(col('promoInOut') == 'False', (col('PlanProductBaselineCaseQty') * col('Price')).cast(DecimalType(30,6))).otherwise(None))
    #  ---

    #####*Calculate plan product parameters*

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(planParamsCorrectionDF, planParamsCorrectionDF.correctionPromoProductId == calcPlanPromoProductDF.Id, 'left')\
      .select(\
               calcPlanPromoProductDF['*']
              ,when(planParamsCorrectionDF.correctionPlanProductUpliftPercentCorrected.isNull(), calcPlanPromoProductDF.PlanProductUpliftPercent)\
                    .otherwise(planParamsCorrectionDF.correctionPlanProductUpliftPercentCorrected).cast(DecimalType(30,6)).alias('productUpliftPercent')
             )

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(planParamsIncrementalDF, 
            [\
              planParamsIncrementalDF.incrementalPromoId == calcPlanPromoProductDF.PromoId
             ,planParamsIncrementalDF.incrementalProductId == calcPlanPromoProductDF.ProductId
            ]
            ,'left')\
      .select(\
               calcPlanPromoProductDF['*']
              ,planParamsIncrementalDF.PlanPromoIncrementalCases.cast(DecimalType(30,6))
             )

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .withColumn('PlanProductIncrementalLSV', when(col('promoInOut') == 'False', col('PlanProductBaselineLSV') * col('productUpliftPercent') / 100.0)\
                                                            .otherwise(col('PlanPromoIncrementalCases') * col('Price')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductLSV', when(col('promoInOut') == 'False', col('PlanProductBaselineLSV') + col('PlanProductIncrementalLSV'))\
                                                            .otherwise(col('PlanProductIncrementalLSV')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPCPrice', (col('Price') / col('UOM_PC2Case')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductIncrementalCaseQty', when(col('promoInOut') == 'False', col('PlanProductBaselineCaseQty') * col('productUpliftPercent') / 100.0)\
                                                          .otherwise(col('PlanPromoIncrementalCases')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductCaseQty', when(col('promoInOut') == 'False', col('PlanProductBaselineCaseQty') + col('PlanProductIncrementalCaseQty'))\
                                                          .otherwise(col('PlanProductIncrementalCaseQty')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPCQty', (col('PlanProductCaseQty') * col('UOM_PC2Case')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductCaseLSV', when(col('promoInOut') == 'False', col('PlanProductBaselineCaseQty') * col('Price'))\
                                              .otherwise(col('PlanProductCaseQty') * col('Price')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPCLSV', (col('PlanProductCaseLSV') / col('UOM_PC2Case')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW1', when(col('promoInOut') == 'False', (col('PlanProductBaselineCaseQty') * col('promoClientPostPromoEffectW1') / 100.0))\
                                                    .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW1', when(col('PlanProductPostPromoEffectQtyW1').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectQtyW1')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW2', when(col('promoInOut') == 'False', (col('PlanProductBaselineCaseQty') * col('promoClientPostPromoEffectW2') / 100.0))\
                                                    .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW2', when(col('PlanProductPostPromoEffectQtyW2').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectQtyW2')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQty', when(col('promoInOut') == 'False', col('PlanProductPostPromoEffectQtyW1') + col('PlanProductPostPromoEffectQtyW2'))\
                                                    .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW1', when(col('promoInOut') == 'False', (col('PlanProductBaselineLSV') * col('promoClientPostPromoEffectW1') / 100.0))\
                                                    .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW1', when(col('PlanProductPostPromoEffectLSVW1').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectLSVW1')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW2', when(col('promoInOut') == 'False', (col('PlanProductBaselineLSV') * col('promoClientPostPromoEffectW2') / 100.0))\
                                                    .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW2', when(col('PlanProductPostPromoEffectLSVW2').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectLSVW2')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSV', when(col('promoInOut') == 'False', col('PlanProductPostPromoEffectLSVW1') + col('PlanProductPostPromoEffectLSVW2'))\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductBaselineVolume', (col('PlanProductBaselineCaseQty') * col('CaseVolume')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductIncrementalVolume', when(col('promoInOut') == 'True', col('PlanProductIncrementalCaseQty') * col('CaseVolume'))\
                                                          .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectVolumeW1', when(col('promoInOut') == 'False', col('PlanProductBaselineVolume') * col('promoClientPostPromoEffectW1') / 100).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectVolumeW2', when(col('promoInOut') == 'False', col('PlanProductBaselineVolume') * col('promoClientPostPromoEffectW2') / 100).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectVolume', when(col('promoInOut') == 'False', col('PlanProductPostPromoEffectVolumeW1') + col('PlanProductPostPromoEffectVolumeW2')).otherwise(0).cast(DecimalType(30,6)))

    #####*Calculate PlanPromoIncrementalLSV, PlanPromoBaselineLSV, PlanPromoLSV*

    sumPlanProductParamsList = calcPlanPromoProductDF\
      .select(\
               col('promoNumber')
              ,col('PlanProductIncrementalLSV')
              ,col('PlanProductBaselineLSV')
              ,col('PlanProductBaselineVolume')
              ,col('PlanProductIncrementalVolume')
              ,col('PlanProductLSV')
             )\
      .groupBy('promoNumber')\
      .agg(sum('PlanProductIncrementalLSV').alias('calcPlanPromoIncrementalLSV'),
           sum('PlanProductBaselineLSV').alias('calcPlanPromoBaselineLSV'),
           sum('PlanProductIncrementalVolume').alias('calcPlanProductIncrementalVolume'),
           sum('PlanProductBaselineVolume').alias('calcPlanProductBaselineVolume'))\
      .withColumn('tempPlanPromoIncrementalLSV', when(col('calcPlanPromoIncrementalLSV').isNull(), 0).otherwise(col('calcPlanPromoIncrementalLSV')))\
      .withColumn('tempPlanPromoBaselineLSV', when(col('calcPlanPromoBaselineLSV').isNull(), 0).otherwise(col('calcPlanPromoBaselineLSV')))\
      .withColumn('calcPlanPromoLSV', col('tempPlanPromoIncrementalLSV') + col('tempPlanPromoBaselineLSV'))\
      .withColumn('calcPlanProductBaselineVolume', when(col('calcPlanProductBaselineVolume').isNull(), 0).otherwise(col('calcPlanProductBaselineVolume')))\
      .withColumn('calcPlanProductIncrementalVolume', when(col('calcPlanProductIncrementalVolume').isNull(), 0).otherwise(col('calcPlanProductIncrementalVolume')))\
      .drop('tempPlanPromoIncrementalLSV','tempPlanPromoBaselineLSV')

    sumPlanProductParamsList = sumPlanProductParamsList.collect()

    planParSchema = StructType([
      StructField("promoNumber", StringType(), True),
      StructField("calcPlanPromoIncrementalLSV", DecimalType(30,6), True),
      StructField("calcPlanPromoBaselineLSV", DecimalType(30,6), True),
      StructField("calcPlanProductIncrementalVolume", DecimalType(30,6), True),
      StructField("calcPlanProductBaselineVolume", DecimalType(30,6), True),
      StructField("calcPlanPromoLSV", DecimalType(30,6), True)
    ])

    planParDF = spark.createDataFrame(sumPlanProductParamsList, planParSchema)

    calcPlanPromoDF = calcPlanPromoDF\
      .join(planParDF, planParDF.promoNumber == calcPlanPromoDF.Number, 'inner')

    allCalcPlanPromoDF = allCalcPlanPromoDF\
      .join(calcPlanPromoDF, 'Id', 'left')\
      .select(\
               allCalcPlanPromoDF['*']
              ,calcPlanPromoDF.calcPlanPromoIncrementalLSV
              ,calcPlanPromoDF.calcPlanPromoBaselineLSV
              ,calcPlanPromoDF.calcPlanPromoLSV
              ,calcPlanPromoDF.calcPlanProductBaselineVolume
              ,calcPlanPromoDF.calcPlanProductIncrementalVolume
             )\
      .withColumn('PlanPromoIncrementalLSV', when(calcPlanPromoDF.calcPlanPromoIncrementalLSV.isNull(), allCalcPlanPromoDF.PlanPromoIncrementalLSV)\
                                             .otherwise(calcPlanPromoDF.calcPlanPromoIncrementalLSV).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoBaselineLSV', when(calcPlanPromoDF.calcPlanPromoBaselineLSV.isNull(), allCalcPlanPromoDF.PlanPromoBaselineLSV)\
                                             .otherwise(calcPlanPromoDF.calcPlanPromoBaselineLSV).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoLSV', when(calcPlanPromoDF.calcPlanPromoLSV.isNull(), allCalcPlanPromoDF.PlanPromoLSV)\
                                             .otherwise(calcPlanPromoDF.calcPlanPromoLSV).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoUpliftPercent', when(((col('PlanPromoBaselineLSV') != 0) & (col('NeedRecountUplift') == True)\
                                                  & col('promoStatusSystemName').isin(*planParametersStatuses))\
                                                 ,col('PlanPromoIncrementalLSV') / col('PlanPromoBaselineLSV') * 100.0)\
                                             .otherwise(col('PlanPromoUpliftPercent')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoBaselineVolume', when(calcPlanPromoDF.calcPlanProductBaselineVolume.isNull(), allCalcPlanPromoDF.PlanPromoBaselineVolume)\
                                             .otherwise(calcPlanPromoDF.calcPlanProductBaselineVolume).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalVolume', when(col('InOut') == 'False', col('PlanPromoBaselineVolume') * col('PlanPromoUpliftPercent') / 100).otherwise(col('calcPlanProductIncrementalVolume')).cast(DecimalType(30,6)))\
      .drop('calcPlanPromoIncrementalLSV','calcPlanPromoBaselineLSV','calcPlanPromoLSV','calcPlanProductBaselineVolume','calcPlanProductIncrementalVolume')

    #####*Get result*

    # newPromoProductDF = calcPlanPromoProductDF.where(col('Action') == 'Added').select(promoProductCols)
    calcPlanPromoProductDF = calcPlanPromoProductDF.select(promoProductCols)
    print('Plan product parameters calculation completed!')
    
    return calcPlanPromoProductDF,logPromoProductDF
    