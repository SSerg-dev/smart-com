####Notebook "PI_PLAN_PRODUCT_PARAMS_CALCULATION_PROCESS". 
####*Calculate plan price increase product parameters and plan promo baseline, incremental and LSV*.
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

def run(calcPlanPromoProductDF,planParamsPriceListDF,planParamsIncreasePriceListDF,planParamsBaselineDF,planParamsIncreaseBaselineDF,calcPlanPromoDF,allCalcPlanPromoDF,planParamsSharesDF,datesDF,planParamsCorrectionDF,planParamsIncrementalDF,planParametersStatuses,promoProductCols):
    sc = SparkContext.getOrCreate();
    spark = SparkSession(sc)
    
    
    byPriceStartDate = (Window.partitionBy('PromoPriceIncreaseId', 'ProductId').orderBy(col("priceStartDate").desc()))

    # calcPlanPromoProductDF = calcPlanPromoProductDF.drop('Price')
    
    #Increase Price
    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(planParamsIncreasePriceListDF, 
            [\
              planParamsIncreasePriceListDF.priceStartDate <= calcPlanPromoProductDF.promoDispatchesStart,
              planParamsIncreasePriceListDF.priceEndDate >= calcPlanPromoProductDF.promoDispatchesStart,
              planParamsIncreasePriceListDF.priceClientTreeId == calcPlanPromoProductDF.promoClientTreeKeyId,
              planParamsIncreasePriceListDF.priceProductId == calcPlanPromoProductDF.ProductId
            ], 
            'left')\
      .select(\
               calcPlanPromoProductDF['*']
              ,planParamsIncreasePriceListDF.priceStartDate
              ,planParamsIncreasePriceListDF.Price.alias('calcPrice')
             )\
      .withColumn('Row_Number', row_number().over(byPriceStartDate))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    
    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .withColumn('IncreaseFound', when(col('calcPrice').isNull(),lit('false')).otherwise(lit('true')))\
      .withColumn('Price', col('calcPrice'))\
      .drop('calcPrice')\
      .drop('priceStartDate')

    #Regular Price
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
      .withColumn('Price', when(col('Price').isNull(),col('calcPrice')).otherwise(col('Price')))\
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
    
    planParamsIncreaseBaselineDF = planParamsIncreaseBaselineDF\
      .join(datesDF, planParamsIncreaseBaselineDF.baselineStartDate == datesDF.OriginalDate, 'inner')\
      .select(\
              planParamsIncreaseBaselineDF['*']
             ,datesDF.MarsWeekFullName
             )

    calcRegularPlanPromoProductDF = calcPlanPromoProductDF\
      .where(col('IncreaseFound') == 'false')\
      .join(planParamsBaselineDF, 
           [\
             planParamsBaselineDF.baselineProductId == calcPlanPromoProductDF.ProductId
            ,planParamsBaselineDF.baselineDemandCode == calcPlanPromoProductDF.promoDemandCode
            ,planParamsBaselineDF.MarsWeekFullName == calcPlanPromoProductDF.MarsWeekFullName
           ],
           'left')
    
    calcIncreasePlanPromoProductDF = calcPlanPromoProductDF\
      .where(col('IncreaseFound') == 'true')\
      .join(planParamsIncreaseBaselineDF, 
           [\
             planParamsIncreaseBaselineDF.baselineProductId == calcPlanPromoProductDF.ProductId
            ,planParamsIncreaseBaselineDF.baselineDemandCode == calcPlanPromoProductDF.promoDemandCode
            ,planParamsIncreaseBaselineDF.MarsWeekFullName == calcPlanPromoProductDF.MarsWeekFullName
           ],
           'left')

    calcPlanPromoProductDF = calcRegularPlanPromoProductDF.union(calcIncreasePlanPromoProductDF)

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
      .withColumn('PlanProductBaselineCaseQty', col('calcPlanProductBaselineCaseQty').cast(DecimalType(30,6)))\
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
      .withColumn('PlanProductBaselineLSV', (col('PlanProductBaselineCaseQty') * col('Price')).cast(DecimalType(30,6)))
    #  ---

    #####*Calculate plan product parameters*
    #??
    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .join(planParamsCorrectionDF, planParamsCorrectionDF.correctionPromoProductPriceIncreaseId == calcPlanPromoProductDF.Id, 'left')\
      .select(\
               calcPlanPromoProductDF['*']
              ,when(planParamsCorrectionDF.correctionPlanProductUpliftPercentCorrected.isNull(), calcPlanPromoProductDF.PlanProductUpliftPercent)\
                    .otherwise(planParamsCorrectionDF.correctionPlanProductUpliftPercentCorrected).cast(DecimalType(30,6)).alias('productUpliftPercent')
             )

    calcPlanPromoProductDF = calcPlanPromoProductDF\
      .withColumn('PlanProductIncrementalLSV', (col('PlanProductBaselineLSV') * col('productUpliftPercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductLSV', (col('PlanProductBaselineLSV') + col('PlanProductIncrementalLSV')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPCPrice', (col('Price') / col('UOM_PC2Case')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductIncrementalCaseQty', (col('PlanProductBaselineCaseQty') * col('productUpliftPercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductCaseQty', (col('PlanProductBaselineCaseQty') + col('PlanProductIncrementalCaseQty')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPCQty', (col('PlanProductCaseQty') * col('UOM_PC2Case')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductCaseLSV', (col('PlanProductBaselineCaseQty') * col('Price')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPCLSV', (col('PlanProductCaseLSV') / col('UOM_PC2Case')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW1', (col('PlanProductBaselineCaseQty') * col('promoClientPostPromoEffectW1') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW1', when(col('PlanProductPostPromoEffectQtyW1').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectQtyW1')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW2', (col('PlanProductBaselineCaseQty') * col('promoClientPostPromoEffectW2') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQtyW2', when(col('PlanProductPostPromoEffectQtyW2').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectQtyW2')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectQty', (col('PlanProductPostPromoEffectQtyW1') + col('PlanProductPostPromoEffectQtyW2')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW1', (col('PlanProductBaselineLSV') * col('promoClientPostPromoEffectW1') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW1', when(col('PlanProductPostPromoEffectLSVW1').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectLSVW1')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW2', (col('PlanProductBaselineLSV') * col('promoClientPostPromoEffectW2') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSVW2', when(col('PlanProductPostPromoEffectLSVW2').isNull(), 0)\
                  .otherwise(col('PlanProductPostPromoEffectLSVW2')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectLSV', (col('PlanProductPostPromoEffectLSVW1') + col('PlanProductPostPromoEffectLSVW2')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductBaselineVolume', (col('PlanProductBaselineCaseQty') * col('CaseVolume')).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectVolumeW1', (col('PlanProductBaselineVolume') * col('promoClientPostPromoEffectW1') / 100).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectVolumeW2', (col('PlanProductBaselineVolume') * col('promoClientPostPromoEffectW2') / 100).cast(DecimalType(30,6)))\
      .withColumn('PlanProductPostPromoEffectVolume', (col('PlanProductPostPromoEffectVolumeW1') + col('PlanProductPostPromoEffectVolumeW2')).cast(DecimalType(30,6)))

    #####*Calculate PlanPromoIncrementalLSV, PlanPromoBaselineLSV, PlanPromoLSV*

    sumPlanProductParamsList = calcPlanPromoProductDF\
      .select(\
               col('promoNumber')
              ,col('PlanProductIncrementalLSV')
              ,col('PlanProductBaselineLSV')
              ,col('PlanProductBaselineVolume')
              ,col('PlanProductLSV')
             )\
      .groupBy('promoNumber')\
      .agg(sum('PlanProductIncrementalLSV').alias('calcPlanPromoIncrementalLSV'),
           sum('PlanProductBaselineLSV').alias('calcPlanPromoBaselineLSV'),
           sum('PlanProductBaselineVolume').alias('calcPlanProductBaselineVolume'))\
      .withColumn('tempPlanPromoIncrementalLSV', when(col('calcPlanPromoIncrementalLSV').isNull(), 0).otherwise(col('calcPlanPromoIncrementalLSV')))\
      .withColumn('tempPlanPromoBaselineLSV', when(col('calcPlanPromoBaselineLSV').isNull(), 0).otherwise(col('calcPlanPromoBaselineLSV')))\
      .withColumn('calcPlanPromoLSV', col('tempPlanPromoIncrementalLSV') + col('tempPlanPromoBaselineLSV'))\
      .withColumn('calcPlanProductBaselineVolume', when(col('calcPlanProductBaselineVolume').isNull(), 0).otherwise(col('calcPlanProductBaselineVolume')))\
      .drop('tempPlanPromoIncrementalLSV','tempPlanPromoBaselineLSV')

    sumPlanProductParamsList = sumPlanProductParamsList.collect()

    planParSchema = StructType([
      StructField("promoNumber", StringType(), True),
      StructField("calcPlanPromoIncrementalLSV", DecimalType(30,6), True),
      StructField("calcPlanPromoBaselineLSV", DecimalType(30,6), True),
      StructField("calcPlanProductBaselineVolume", DecimalType(30,6), True),
      StructField("calcPlanPromoLSV", DecimalType(30,6), True)
    ])

    planParDF = spark.createDataFrame(sumPlanProductParamsList, planParSchema)

    calcPlanPromoDF = calcPlanPromoDF\
      .join(planParDF, planParDF.promoNumber == calcPlanPromoDF.Number, 'inner')

    @udf
    def isNullCheck(value):
      if value is None:
        return 0
      else:
        return value
        
    allCalcPlanPromoDF = allCalcPlanPromoDF\
      .join(calcPlanPromoDF, 'Id', 'left')\
      .select(\
               allCalcPlanPromoDF['*']
              ,calcPlanPromoDF.calcPlanPromoIncrementalLSV
              ,calcPlanPromoDF.calcPlanPromoBaselineLSV
              ,calcPlanPromoDF.calcPlanPromoLSV
              ,calcPlanPromoDF.calcPlanProductBaselineVolume
             )\
      .withColumn('PlanPromoIncrementalLSV', when(calcPlanPromoDF.calcPlanPromoIncrementalLSV.isNull(), allCalcPlanPromoDF.PlanPromoIncrementalLSV)\
                                             .otherwise(calcPlanPromoDF.calcPlanPromoIncrementalLSV).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoBaselineLSV', when(calcPlanPromoDF.calcPlanPromoBaselineLSV.isNull(), allCalcPlanPromoDF.PlanPromoBaselineLSV)\
                                             .otherwise(calcPlanPromoDF.calcPlanPromoBaselineLSV).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoLSV', when(calcPlanPromoDF.calcPlanPromoLSV.isNull(), allCalcPlanPromoDF.PlanPromoLSV)\
                                             .otherwise(calcPlanPromoDF.calcPlanPromoLSV).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoPostPromoEffectLSV', (isNullCheck(col('PlanPromoBaselineLSV')) * isNullCheck(col('promoClientPostPromoEffectW1')) / 100.0 \
                                                + isNullCheck(col('PlanPromoBaselineLSV')) * isNullCheck(col('promoClientPostPromoEffectW2')) / 100.0)\
                                              .cast(DecimalType(30,6)))\
      .drop('calcPlanPromoIncrementalLSV','calcPlanPromoBaselineLSV','calcPlanPromoLSV','calcPlanProductBaselineVolume')

    #####*Get result*

    # newPromoProductDF = calcPlanPromoProductDF.where(col('Action') == 'Added').select(promoProductCols)
    calcPlanPromoProductDF = calcPlanPromoProductDF.select(promoProductCols)
    print('Plan product parameters calculation completed!')
    
    return calcPlanPromoProductDF,calcPlanPromoDF,allCalcPlanPromoDF,logPromoProductDF