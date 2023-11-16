####Notebook "PLAN_PROMO_PARAMS_CALCULATION_PROCESS". 
####*Calculate plan promo parameters*.
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

def run(clientTreeDF,cogsDF,brandTechDF,cogsTnDF,tiDF,ratiShopperDF,calcPlanPromoDF,promoDF):
    sc = SparkContext.getOrCreate();
    spark = SparkSession(sc)

    #####*Set COGS&TI percent*

    activeClientTreeList = clientTreeDF.where(col('EndDate').isNull()).collect()

    cogsClientDF = cogsDF\
      .join(clientTreeDF, clientTreeDF.Id == cogsDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == cogsDF.BrandTechId, 'inner')\
      .select(\
               cogsDF.StartDate.alias('cogsStartDate')
              ,cogsDF.EndDate.alias('cogsEndDate')
              ,cogsDF.LSVpercent
              ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('cbtName')
             )

    cogsClientList = cogsClientDF.collect()

    cogsTnClientDF = cogsTnDF\
      .join(clientTreeDF, clientTreeDF.Id == cogsTnDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == cogsTnDF.BrandTechId, 'inner')\
      .select(\
               cogsTnDF.StartDate.alias('cogsStartDate')
              ,cogsTnDF.EndDate.alias('cogsEndDate')
              ,cogsTnDF.TonCost
              ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('cbtName')
             )

    cogsTnClientList = cogsTnClientDF.collect()

    cogsTnClientDF = cogsTnDF\
      .join(clientTreeDF, clientTreeDF.Id == cogsTnDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == cogsTnDF.BrandTechId, 'inner')\
      .select(\
               cogsTnDF.StartDate.alias('cogsStartDate')
              ,cogsTnDF.EndDate.alias('cogsEndDate')
              ,cogsTnDF.TonCost
              ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('cbtName')
             )

    cogsTnClientList = cogsTnClientDF.collect()

    tiClientNullBtDF = tiDF\
      .join(clientTreeDF, clientTreeDF.Id == tiDF.ClientTreeId, 'inner')\
      .select(\
               tiDF.StartDate.alias('tiStartDate')
              ,tiDF.EndDate.alias('tiEndDate')
              ,tiDF.SizePercent
              ,clientTreeDF.ObjectId.alias('tiClientTreeObjectId')
             )\
      .withColumn('tibtName', lit(None).cast(StringType()))

    tiClientNotNullBtDF = tiDF\
      .join(clientTreeDF, clientTreeDF.Id == tiDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == tiDF.BrandTechId, 'inner')\
      .select(\
               tiDF.StartDate.alias('tiStartDate')
              ,tiDF.EndDate.alias('tiEndDate')
              ,tiDF.SizePercent
              ,clientTreeDF.ObjectId.alias('tiClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('tibtName')
             )

    tiClientList = tiClientNullBtDF.union(tiClientNotNullBtDF).collect()

    ratiShopperList = ratiShopperDF.collect()

    import COGS_TI_CALCULATION as cc

    import RA_TI_SHOPPER_CALCULATION as ra

    calcPlanPromoDF = calcPlanPromoDF\
      .withColumn('calcCogsPercent', lit(cc.getCogsPercent(activeClientTreeList,cogsClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
      .withColumn('calcCogsTn', lit(cc.getCogsTnPercent(activeClientTreeList,cogsTnClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
      .withColumn('calcTiPercent', lit(cc.getTiPercent(activeClientTreeList,tiClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('StartDate'))))\
      .withColumn('calcRaTiShopperPercent', lit(ra.getRaTiShopperPercent(activeClientTreeList,ratiShopperList)(col('ClientTreeKeyId'), col('BudgetYear'))))

    logCOGS = calcPlanPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcCogsPercent').alias('COGSMessage')
             )\
      .where(col('calcCogsPercent').isin(*cc.logText))

    logCOGSTn = calcPlanPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcCogsTn').alias('COGSTnMessage')
             )\
      .where(col('calcCogsTn').isin(*cc.logText))

    logTI = calcPlanPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcTiPercent').alias('TIMessage')
             )\
      .where(col('calcTiPercent').isin(*cc.logText))

    logRATIShopper = calcPlanPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcRaTiShopperPercent').alias('TIMessage')
             )\
      .where(col('calcRaTiShopperPercent').isin(*ra.raLogText))

    calcPlanPromoDF = calcPlanPromoDF\
      .withColumn('PlanCOGSPercent', when(~col('calcCogsPercent').isin(*cc.logText), col('calcCogsPercent')).otherwise(col('PlanCOGSPercent')))\
      .withColumn('PlanCOGSTn', when(~col('calcCogsTn').isin(*cc.logText), col('calcCogsTn')).otherwise(col('PlanCOGSTn')))\
      .withColumn('PlanTIBasePercent', when(~col('calcTiPercent').isin(*cc.logText), col('calcTiPercent')).otherwise(col('PlanTIBasePercent')))\
      .withColumn('RATIShopperPercent', when(~col('calcRaTiShopperPercent').isin(*ra.raLogText), col('calcRaTiShopperPercent')).otherwise(0))

    #####*Calculate promo parameters*

    @udf
    def isNullCheck(value):
      if value is None:
        return 0
      else:
        return value

    calcPlanPromoDF = calcPlanPromoDF\
      .withColumn('PlanPromoTIShopper', (col('PlanPromoLSV') * col('MarsMechanicDiscount') / 100).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoCost', (isNullCheck(col('PlanPromoTIShopper')) + isNullCheck(col('PlanPromoTIMarketing')) + isNullCheck(col('PlanPromoBranding'))\
                                  + isNullCheck(col('PlanPromoBTL')) + isNullCheck(col('PlanPromoCostProduction'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalBaseTI', (col('PlanPromoIncrementalLSV') * col('PlanTIBasePercent') / 100).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalCOGS', (col('PlanPromoIncrementalLSV') * col('PlanCOGSPercent') / 100).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoBaseTI', (col('PlanPromoLSV') * col('PlanTIBasePercent') / 100).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoTotalCost', (isNullCheck(col('PlanPromoCost')) + isNullCheck(col('PlanPromoBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoPostPromoEffectLSV', when(col('InOut') == 'False', col('PlanPromoPostPromoEffectLSVW1') + col('PlanPromoPostPromoEffectLSVW2'))\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalLSV', (isNullCheck(col('PlanPromoIncrementalLSV')) + isNullCheck(col('PlanPromoPostPromoEffectLSV'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetLSV', (isNullCheck(col('PlanPromoBaselineLSV')) + isNullCheck(col('PlanPromoNetIncrementalLSV'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalBaseTI', (col('PlanPromoNetIncrementalLSV') * col('PlanTIBasePercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalCOGS', (col('PlanPromoNetIncrementalLSV') * col('PlanCOGSPercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetBaseTI', when(col('InOut') == 'False', col('PlanPromoNetLSV') * col('PlanTIBasePercent') / 100.0).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoBaselineBaseTI', when(col('InOut') == 'False', col('PlanPromoBaselineLSV') * col('PlanTIBasePercent') / 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNSV', (isNullCheck(col('PlanPromoLSV')) - isNullCheck(col('PlanPromoTIShopper'))\
                                 - isNullCheck(col('PlanPromoTIMarketing')) - isNullCheck(col('PlanPromoBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalNSV', (isNullCheck(col('PlanPromoIncrementalLSV')) - isNullCheck(col('PlanPromoTIShopper'))\
                                            - isNullCheck(col('PlanPromoTIMarketing')) - isNullCheck(col('PlanPromoIncrementalBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalNSV', (isNullCheck(col('PlanPromoNetIncrementalLSV')) - isNullCheck(col('PlanPromoTIShopper'))\
                                               - isNullCheck(col('PlanPromoTIMarketing')) - isNullCheck(col('PlanPromoNetIncrementalBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalMACLSV', (isNullCheck(col('PlanPromoNetIncrementalNSV'))\
                                               - isNullCheck(col('PlanPromoNetIncrementalCOGS'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetNSV', (isNullCheck(col('PlanPromoNetLSV')) - isNullCheck(col('PlanPromoTIShopper'))\
                                    - isNullCheck(col('PlanPromoTIMarketing')) - isNullCheck(col('PlanPromoNetBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalMACLSV', (isNullCheck(col('PlanPromoIncrementalNSV')) - isNullCheck(col('PlanPromoIncrementalCOGS'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalEarningsLSV', (isNullCheck(col('PlanPromoIncrementalMACLSV')) - isNullCheck(col('PlanPromoBranding'))\
                                                 - isNullCheck(col('PlanPromoBTL')) - isNullCheck(col('PlanPromoCostProduction'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalEarningsLSV', (isNullCheck(col('PlanPromoNetIncrementalMACLSV')) - isNullCheck(col('PlanPromoBranding'))\
                                                    - isNullCheck(col('PlanPromoBTL')) - isNullCheck(col('PlanPromoCostProduction'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoROIPercentLSV', when(col('PlanPromoCost') != 0, (col('PlanPromoIncrementalEarningsLSV') / col('PlanPromoCost') + 1) * 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetROIPercentLSV', when(col('PlanPromoCost') != 0, (col('PlanPromoNetIncrementalEarningsLSV') / col('PlanPromoCost') + 1) * 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetUpliftPercent', when(col('PlanPromoBaselineLSV') != 0, (col('PlanPromoNetIncrementalLSV') / col('PlanPromoBaselineLSV')) * 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoUpliftPercentPI', col('PlanPromoUpliftPercent').cast(DecimalType(30,6)))\
      .withColumn('PlanAddTIShopperCalculated', (isNullCheck(col('PlanPromoTIShopper')) - isNullCheck(col('PlanPromoNetIncrementalLSV')) * col('RATIShopperPercent') / 100)\
                  .cast(DecimalType(30,6)))\
      .withColumn('PlanAddTIShopperApproved', when(col('LastApprovedDate').isNull(), col('PlanAddTIShopperCalculated')).otherwise(col('PlanAddTIShopperApproved')))\
      .withColumn('PlanPromoPostPromoEffectVolume',  when(col('InOut') == 'False', isNullCheck(col('PlanPromoPostPromoEffectVolumeW1')) + isNullCheck(col('PlanPromoPostPromoEffectVolumeW2'))).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalVolume', (col('PlanPromoIncrementalVolume') + col('PlanPromoPostPromoEffectVolume')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalCOGSTn', (col('PlanPromoIncrementalVolume') * col('PlanCOGSTn')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalCOGSTn', (col('PlanPromoNetIncrementalVolume') * col('PlanCOGSTn')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalMAC', when(col('IsLSVBased') == False, isNullCheck(col('PlanPromoNetIncrementalNSV')) - isNullCheck(col('PlanPromoNetIncrementalCOGSTn')))\
                                              .otherwise(col('PlanPromoNetIncrementalMACLSV'))\
                                              .cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalMAC', when(col('IsLSVBased') == False, isNullCheck(col('PlanPromoIncrementalNSV')) - isNullCheck(col('PlanPromoIncrementalCOGSTn')))\
                                              .otherwise(col('PlanPromoIncrementalMACLSV'))\
                                              .cast(DecimalType(30,6)))\
      .withColumn('PlanPromoIncrementalEarnings', when(col('IsLSVBased') == False, (isNullCheck(col('PlanPromoIncrementalMAC')) - isNullCheck(col('PlanPromoBranding'))\
                                                 - isNullCheck(col('PlanPromoBTL')) - isNullCheck(col('PlanPromoCostProduction')))).otherwise(col('PlanPromoIncrementalEarningsLSV')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetIncrementalEarnings', when(col('IsLSVBased') == False, (isNullCheck(col('PlanPromoNetIncrementalMAC')) - isNullCheck(col('PlanPromoBranding'))\
                                                 - isNullCheck(col('PlanPromoBTL')) - isNullCheck(col('PlanPromoCostProduction')))).otherwise(col('PlanPromoNetIncrementalEarningsLSV')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoROIPercent', when(col('IsLSVBased') == False, when(col("PlanPromoCost") != 0, (col('PlanPromoIncrementalEarnings') / col('PlanPromoCost') + 1) * 100.0)\
                                              .otherwise(0)).otherwise(col('PlanPromoROIPercentLSV')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNetROIPercent', when(col('IsLSVBased') == False, when(col("PlanPromoCost") != 0, (col('PlanPromoNetIncrementalEarnings') / col('PlanPromoCost') + 1) * 100.0)\
                                              .otherwise(0)).otherwise(col('PlanPromoNetROIPercentLSV')).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoVolume', (isNullCheck(col('PlanPromoBaselineVolume')) + isNullCheck(col('PlanPromoIncrementalVolume'))).cast(DecimalType(30,6)))\
      .withColumn('PlanPromoNSVtn', (isNullCheck(col('PlanPromoNSV')) / isNullCheck(col('PlanPromoVolume'))).cast(DecimalType(30,6)))

    inExchangeCalcPromoDF = promoDF\
    .select(\
            col('MasterPromoId').alias('MasterPromo')
            ,col('PlanPromoTIShopper')
            ,col('PlanPromoNetIncrementalLSV')
            )\
    .groupBy('MasterPromo')\
    .agg(sum('PlanPromoTIShopper').alias('sumPlanPromoTIShopper'),
            sum('PlanPromoNetIncrementalLSV').alias('sumPlanPromoNetIncrementalLSV'))

    calcPlanPromoDF = calcPlanPromoDF\
    .join(inExchangeCalcPromoDF, inExchangeCalcPromoDF.MasterPromo == calcPlanPromoDF.Id, 'left')\
    .select(\
                calcPlanPromoDF['*']
            ,col('sumPlanPromoTIShopper')
            ,col('sumPlanPromoNetIncrementalLSV')
            )

    calcPlanPromoDF = calcPlanPromoDF\
    .withColumn('PlanAddTIShopperCalculated', (isNullCheck(col('PlanPromoTIShopper')) - isNullCheck(col('sumPlanPromoTIShopper')) - (isNullCheck(col('PlanPromoNetIncrementalLSV')) - isNullCheck(col('sumPlanPromoNetIncrementalLSV'))) * col('RATIShopperPercent') / 100)\
                .cast(DecimalType(30,6)))\
    .drop('sumPlanPromoTIShopper', 'sumPlanPromoNetIncrementalLSV')

    #####*Get result*

    # calcPlanPromoDF.toPandas().to_csv(OUTPUT_PLANPROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')
    print('Plan promo parameters calculation completed!')
    
    return calcPlanPromoDF,logCOGS,logTI