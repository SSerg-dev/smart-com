####Notebook "ACTUAL_PROMO_PARAMS_CALCULATION_PROCESS". 
####*Calculate actual promo parameters*.
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

def run(clientTreeDF,cogsDF,brandTechDF,cogsTnDF,tiDF,ratiShopperDF,calcActualPromoDF,promoDF,actualCogsDF,actualCogsTnDF,actualTiDF):
    sc = SparkContext.getOrCreate()
    spark = SparkSession(sc)

    #####*Set COGS&TI Percent*

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

    cogsClientTnDF = cogsTnDF\
      .join(clientTreeDF, clientTreeDF.Id == cogsTnDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == cogsTnDF.BrandTechId, 'inner')\
      .select(\
               cogsTnDF.StartDate.alias('cogsStartDate')
              ,cogsTnDF.EndDate.alias('cogsEndDate')
              ,cogsTnDF.TonCost
              ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('cbtName')
             )

    cogsTnClientList = cogsClientTnDF.collect()

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

    actualCogsClientDF = actualCogsDF\
      .join(clientTreeDF, clientTreeDF.Id == actualCogsDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == actualCogsDF.BrandTechId, 'inner')\
      .select(\
               actualCogsDF.StartDate.alias('cogsStartDate')
              ,actualCogsDF.EndDate.alias('cogsEndDate')
              ,actualCogsDF.LSVpercent
              ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('cbtName')
             )

    actualCogsClientList = actualCogsClientDF.collect()

    actualCogsTnClientDF = actualCogsTnDF\
      .join(clientTreeDF, clientTreeDF.Id == actualCogsTnDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == actualCogsTnDF.BrandTechId, 'inner')\
      .select(\
               actualCogsTnDF.StartDate.alias('cogsStartDate')
              ,actualCogsTnDF.EndDate.alias('cogsEndDate')
              ,actualCogsTnDF.TonCost
              ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('cbtName')
             )

    actualCogsTnClientList = actualCogsTnClientDF.collect()

    actualTiClientNullBtDF = actualTiDF\
      .join(clientTreeDF, clientTreeDF.Id == actualTiDF.ClientTreeId, 'inner')\
      .select(\
               actualTiDF.StartDate.alias('tiStartDate')
              ,actualTiDF.EndDate.alias('tiEndDate')
              ,actualTiDF.SizePercent
              ,clientTreeDF.ObjectId.alias('tiClientTreeObjectId')
             )\
      .withColumn('tibtName', lit(None).cast(StringType()))

    actualTiClientNotNullBtDF = actualTiDF\
      .join(clientTreeDF, clientTreeDF.Id == actualTiDF.ClientTreeId, 'inner')\
      .join(brandTechDF, brandTechDF.Id == actualTiDF.BrandTechId, 'inner')\
      .select(\
               actualTiDF.StartDate.alias('tiStartDate')
              ,actualTiDF.EndDate.alias('tiEndDate')
              ,actualTiDF.SizePercent
              ,clientTreeDF.ObjectId.alias('tiClientTreeObjectId')
              ,brandTechDF.BrandsegTechsub.alias('tibtName')
             )

    actualTiClientList = actualTiClientNullBtDF.union(actualTiClientNotNullBtDF).collect()

    import COGS_TI_CALCULATION as cc

    import RA_TI_SHOPPER_CALCULATION as ra

    calcActualPromoDF = calcActualPromoDF\
      .withColumn('calcCogsPercent', when(col('UseActualCOGS') == False, lit(cc.getCogsPercent(activeClientTreeList,cogsClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
                 .otherwise(None))\
      .withColumn('calcCogsTn', when(col('UseActualCOGS') == False, lit(cc.getCogsTnPercent(activeClientTreeList,cogsTnClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
                 .otherwise(None))\
      .withColumn('calcTiPercent', when(col('UseActualTI') == False, lit(cc.getTiPercent(activeClientTreeList,tiClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('StartDate'))))\
                 .otherwise(None))\
      .withColumn('calcActualCogsPercent', when(col('UseActualCOGS') == True,lit(cc.getActualCogsPercent(activeClientTreeList,actualCogsClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
                 .otherwise(None))\
      .withColumn('calcActualCogsTn', when(col('UseActualCOGS') == True,lit(cc.getActualCogsTnPercent(activeClientTreeList,actualCogsTnClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
                 .otherwise(None))\
      .withColumn('calcActualTiPercent', when(col('UseActualTI') == True, lit(cc.getActualTiPercent(activeClientTreeList,actualTiClientList)(col('ClientTreeId'), col('promoBrandTechName'), col('StartDate'))))\
                 .otherwise(None))\
      .withColumn('calcRaTiShopperPercent', lit(ra.getRaTiShopperPercent(activeClientTreeList,ratiShopperList)(col('ClientTreeKeyId'), col('BudgetYear'))))

    logCOGS = calcActualPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcCogsPercent').alias('COGSMessage')
             )\
      .where(col('calcCogsPercent').isin(*cc.logText))

    logCOGSTn = calcActualPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcCogsTn').alias('COGSTnMessage')
             )\
      .where(col('calcCogsTn').isin(*cc.logTnText))

    logTI = calcActualPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcTiPercent').alias('TIMessage')
             )\
      .where(col('calcTiPercent').isin(*cc.logText))

    logActualCOGS = calcActualPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcActualCogsPercent').alias('ActualCOGSMessage')
             )\
      .where(col('calcActualCogsPercent').isin(*cc.actualLogText))

    logActualCOGSTn = calcActualPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcActualCogsTn').alias('ActualCOGSTnMessage')
             )\
      .where(col('calcActualCogsTn').isin(*cc.actualLogText))

    logActualTI = calcActualPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcActualTiPercent').alias('ActualTIMessage')
             )\
      .where(col('calcActualTiPercent').isin(*cc.actualLogText))

    logRATIShopper = calcActualPromoDF\
      .select(\
               col('Number').alias('promoNumber')\
              ,col('calcRaTiShopperPercent').alias('TIMessage')
             )\
      .where(col('calcRaTiShopperPercent').isin(*ra.raLogText))

    calcActualPromoDF = calcActualPromoDF\
      .withColumn('PlanCOGSPercent', when(((~col('calcCogsPercent').isin(*cc.logText)) & (~col('calcCogsPercent').isNull())), col('calcCogsPercent'))\
                  .otherwise(col('PlanCOGSPercent')))\
      .withColumn('PlanCOGSTn', when(((~col('calcCogsTn').isin(*cc.logTnText)) & (~col('calcCogsTn').isNull())), col('calcCogsTn'))\
                  .otherwise(col('PlanCOGSTn')))\
      .withColumn('PlanTIBasePercent', when(((~col('calcTiPercent').isin(*cc.logText)) & (~col('calcTiPercent').isNull())), col('calcTiPercent'))\
                  .otherwise(col('PlanTIBasePercent')))\
      .withColumn('ActualCOGSPercent', when(((~col('calcActualCogsPercent').isin(*cc.actualLogText)) & (col('UseActualCOGS') == True)\
                                             & (~col('calcActualCogsPercent').isNull())), col('calcActualCogsPercent'))\
                  .otherwise(col('ActualCOGSPercent')).cast(DecimalType(30,2)))\
      .withColumn('ActualCOGSTn', when(((~col('calcActualCogsTn').isin(*cc.actualLogText)) & (col('UseActualCOGS') == True)\
                                             & (~col('calcActualCogsTn').isNull())), col('calcActualCogsTn'))\
                  .otherwise(col('ActualCOGSTn')).cast(DecimalType(30,2)))\
      .withColumn('ActualTIBasePercent', when(((~col('calcActualTiPercent').isin(*cc.actualLogText)) & (col('UseActualTI') == True)\
                                             & (~col('calcActualTiPercent').isNull())), col('calcActualTiPercent'))\
                  .otherwise(col('ActualTIBasePercent')).cast(DecimalType(30,2)))\
      .withColumn('RATIShopperPercent', when(~col('calcRaTiShopperPercent').isin(*ra.raLogText), col('calcRaTiShopperPercent')).otherwise(0))

    #####*Calculate promo parameters*

    @udf
    def isNullCheck(value):
      if value is None:
        return 0
      else:
        return value

    calcActualPromoDF = calcActualPromoDF\
      .withColumn('isActualPromoBaseLineLSVChangedByDemand', when((~col('ActualPromoBaselineLSV').isNull()) & (col('ActualPromoBaselineLSV') != col('PlanPromoBaselineLSV')), True).otherwise(False))\
      .withColumn('isActualPromoLSVChangedByDemand', when((~col('ActualPromoLSVSO').isNull()) & (col('ActualPromoLSVSO') != 0), True).otherwise(False))\
      .withColumn('isActualPromoProstPromoEffectLSVChangedByDemand', when((~col('ActualPromoPostPromoEffectLSV').isNull()) & (col('ActualPromoPostPromoEffectLSV') != 0), True).otherwise(False))\
      .withColumn('TIBasePercent', when((col('UseActualTI') == True) & (~col('ActualTIBasePercent').isNull()), col('ActualTIBasePercent'))
                                        .otherwise(col('PlanTIBasePercent')).cast(DecimalType(30,2)))\
      .withColumn('COGSPercent', when((col('UseActualCOGS') == True) & (~col('ActualCOGSPercent').isNull()), col('ActualCOGSPercent'))
                                        .otherwise(col('PlanCOGSPercent')).cast(DecimalType(30,2)))\
      .withColumn('COGSTn', when((col('UseActualCOGS') == True) & (~col('ActualCOGSTn').isNull()), col('ActualCOGSTn'))
                                        .otherwise(col('PlanCOGSTn')).cast(DecimalType(30,2)))

    calcActualPromoDF = calcActualPromoDF\
      .withColumn('ActualPromoLSV', when((col('IsOnInvoice') == True), col('ActualPromoLSVSI'))\
                                    .otherwise(when(col('isActualPromoLSVChangedByDemand') == False, 0)
                                               .otherwise(col('ActualPromoLSV'))).cast(DecimalType(30,6)))\
      .withColumn('SumInvoice', when((col('ManualInputSumInvoice') == True), col('SumInvoice'))\
                                .otherwise(col('ActualPromoLSVByCompensation') * col('MarsMechanicDiscount') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoTIShopper', col('SumInvoice'))\
      .withColumn('ActualPromoCost', (isNullCheck(col('ActualPromoTIShopper')) + isNullCheck(col('ActualPromoTIMarketing'))\
                                    + isNullCheck(col('ActualPromoBranding')) + isNullCheck(col('ActualPromoBTL'))\
                                    + isNullCheck(col('ActualPromoCostProduction'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoBaseTI', (isNullCheck(col('ActualPromoLSV')) * col('TIBasePercent') / 100).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoBaselineLSV', when((col('InOut') == True), 1)\
                                    .otherwise(when(col('isActualPromoBaseLineLSVChangedByDemand') == False, col('PlanPromoBaselineLSV'))
                                               .otherwise(col('ActualPromoBaselineLSV'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalLSV', when((col('InOut') == False), isNullCheck(col('ActualPromoLSV')) - isNullCheck(col('ActualPromoBaselineLSV')))\
                                    .otherwise(when(isNullCheck(col('ActualPromoLSV')) - isNullCheck(col('ActualPromoBaselineLSV')) < 0, 0)
                                               .otherwise(col('ActualPromoLSV') - col('ActualPromoBaselineLSV'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoPostPromoEffectLSV', when(col('InOut') == False, when(col('IsOnInvoice') == False, col('ActualPromoPostPromoEffectLSVW1') \
                                               + col('ActualPromoPostPromoEffectLSVW2')).otherwise(isNullCheck(col('ActualPromoLSVSO')) - isNullCheck(col('ActualPromoLSVSI'))))\
                                                .otherwise(col('ActualPromoPostPromoEffectLSV')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalLSV', when(col('InOut') == False, isNullCheck(col('ActualPromoIncrementalLSV'))\
                                                   + isNullCheck(col('ActualPromoPostPromoEffectLSV')))\
                                                  .otherwise(isNullCheck(col('ActualPromoIncrementalLSV'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoUpliftPercent', when(col('InOut') == False, 
                                                     when(col('ActualPromoBaselineLSV') != 0, col('ActualPromoIncrementalLSV') / col('ActualPromoBaselineLSV') * 100.0)\
                                                     .otherwise(0))\
                                                .otherwise(None).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetUpliftPercent', when(col('InOut') == False, 
                                                     when(col('ActualPromoBaselineLSV') != 0, col('ActualPromoNetIncrementalLSV') / col('ActualPromoBaselineLSV') * 100.0)\
                                                     .otherwise(0))\
                                                .otherwise(None).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalBaseTI', (col('ActualPromoIncrementalLSV') * col('TIBasePercent') / 100).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalBaseTI', (col('ActualPromoNetIncrementalLSV') * col('TIBasePercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalCOGS', (col('ActualPromoIncrementalLSV') * col('COGSPercent') / 100).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalCOGS', (col('ActualPromoNetIncrementalLSV') * col('COGSPercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNSVtn', (isNullCheck(col('ActualPromoNSV')) / (isNullCheck(col('ActualPromoVolumeSI')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetLSV', (isNullCheck(col('ActualPromoBaselineLSV')) + isNullCheck(col('ActualPromoNetIncrementalLSV'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetBaseTI', (col('ActualPromoNetLSV') * col('TIBasePercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoTotalCost', (isNullCheck(col('ActualPromoCost')) + isNullCheck(col('ActualPromoBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalNSV', (isNullCheck(col('ActualPromoIncrementalLSV')) - isNullCheck(col('ActualPromoTIShopper'))\
                                              - isNullCheck(col('ActualPromoTIMarketing')) - isNullCheck(col('ActualPromoIncrementalBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalNSV', (isNullCheck(col('ActualPromoNetIncrementalLSV')) - isNullCheck(col('ActualPromoTIShopper'))\
                                                 - isNullCheck(col('ActualPromoTIMarketing')) - isNullCheck(col('ActualPromoNetIncrementalBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetNSV', (isNullCheck(col('ActualPromoNetLSV')) - isNullCheck(col('ActualPromoTIShopper'))\
                                      - isNullCheck(col('ActualPromoTIMarketing')) - isNullCheck(col('ActualPromoNetBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalMACLSV', (isNullCheck(col('ActualPromoNetIncrementalNSV')) - isNullCheck(col('ActualPromoNetIncrementalCOGS'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoBaselineBaseTI', (isNullCheck(col('ActualPromoBaselineLSV')) * col('TIBasePercent') / 100.0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNSV', (isNullCheck(col('ActualPromoLSV')) - isNullCheck(col('ActualPromoTIShopper'))\
                                   - isNullCheck(col('ActualPromoTIMarketing')) - isNullCheck(col('ActualPromoBaseTI'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalMACLSV', (isNullCheck(col('ActualPromoIncrementalNSV')) - isNullCheck(col('ActualPromoIncrementalCOGS'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalEarningsLSV', (isNullCheck(col('ActualPromoIncrementalMACLSV')) - isNullCheck(col('ActualPromoBranding'))\
                                                   - isNullCheck(col('ActualPromoBTL')) - isNullCheck(col('ActualPromoCostProduction'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalEarningsLSV', (isNullCheck(col('ActualPromoNetIncrementalMACLSV')) - isNullCheck(col('ActualPromoBranding'))\
                                                      - isNullCheck(col('ActualPromoBTL')) - isNullCheck(col('ActualPromoCostProduction'))).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoROIPercentLSV', when(col('ActualPromoCost') != 0, (col('ActualPromoIncrementalEarningsLSV') / col('ActualPromoCost') + 1) * 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetROIPercentLSV', when(col('ActualPromoCost') != 0, (col('ActualPromoNetIncrementalEarningsLSV') / col('ActualPromoCost') + 1) * 100.0)\
                                              .otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualAddTIShopper', (isNullCheck(col('ActualPromoTIShopper')) - isNullCheck(col('ActualPromoNetIncrementalLSV')) * col('RATIShopperPercent') / 100)\
                  .cast(DecimalType(30,6)))\
      .withColumn('ActualAddTIMarketing', (isNullCheck(col('ActualPromoTIMarketing')) - (isNullCheck(col('PlanPromoTIMarketing'))\
                                                                            - isNullCheck(col('PlanAddTIMarketingApproved')))).cast(DecimalType(30,6)))\
      .withColumn('ActualAddTIMarketing', when(col('ActualAddTIMarketing') < 0, 0).otherwise(col('ActualAddTIMarketing')))\
      .withColumn('ActualPromoVolumeSI', when(col('InOut') == False, isNullCheck(col('ActualPromoVolumeByCompensation'))).otherwise(0).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoVolume',  when(((col('IsOnInvoice') == True) & (col('InOut') == 'False')), col('ActualPromoVolumeSI')).otherwise(col('ActualPromoVolume')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalVolume', (col('ActualPromoVolume') - col('ActualPromoBaselineVolume')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalVolume', (col('ActualPromoIncrementalVolume') + col('ActualPromoPostPromoEffectVolume')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalCOGSTn', (col('ActualPromoIncrementalVolume') * col('COGSTn')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalCOGSTn', (col('ActualPromoNetIncrementalVolume') * col('COGSTn')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalMAC', when(col('IsLSVBased') == False, isNullCheck(col('ActualPromoNetIncrementalNSV')) - isNullCheck(col('ActualPromoNetIncrementalCOGSTn')))\
                                              .otherwise(col('ActualPromoNetIncrementalMACLSV'))\
                                              .cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalMAC', when(col('IsLSVBased') == False, isNullCheck(col('ActualPromoIncrementalNSV')) - isNullCheck(col('ActualPromoIncrementalCOGSTn')))\
                                              .otherwise(col('ActualPromoIncrementalMACLSV'))\
                                              .cast(DecimalType(30,6)))\
      .withColumn('ActualPromoIncrementalEarnings', when(col('IsLSVBased') == False, isNullCheck(col('ActualPromoIncrementalMAC')) - isNullCheck(col('ActualPromoBranding'))\
                                                 - isNullCheck(col('ActualPromoBTL')) - isNullCheck(col('ActualPromoCostProduction')))\
                                                   .otherwise(col('ActualPromoIncrementalEarningsLSV'))\
                                                   .cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetIncrementalEarnings', when(col('IsLSVBased') == False, isNullCheck(col('ActualPromoNetIncrementalMAC')) - isNullCheck(col('ActualPromoBranding'))\
                                                 - isNullCheck(col('ActualPromoBTL')) - isNullCheck(col('ActualPromoCostProduction')))\
                                                   .otherwise(col('ActualPromoNetIncrementalEarningsLSV'))\
                                                   .cast(DecimalType(30,6)))\
      .withColumn('ActualPromoROIPercent', when(col('IsLSVBased') == False, when(col("ActualPromoCost") != 0, (col('ActualPromoIncrementalEarnings') / col('ActualPromoCost') + 1) * 100.0)\
                                              .otherwise(0)).otherwise(col('ActualPromoROIPercentLSV')).cast(DecimalType(30,6)))\
      .withColumn('ActualPromoNetROIPercent', when(col('IsLSVBased') == False, when(col("ActualPromoCost") != 0, (col('ActualPromoNetIncrementalEarnings') / col('ActualPromoCost') + 1) * 100.0)\
                                              .otherwise(0)).otherwise(col('ActualPromoNetROIPercentLSV')).cast(DecimalType(30,6)))      

    if "MasterPromoId" in promoDF.schema.fieldNames():
      inExchangeCalcPromoDF = promoDF\
        .select(\
                col('MasterPromoId').alias('MasterPromo')
                ,col('PlanPromoTIShopper')
                ,col('PlanPromoNetIncrementalLSV')
               )\
        .groupBy('MasterPromo')\
        .agg(sum('PlanPromoTIShopper').alias('sumPlanPromoTIShopper'),
             sum('PlanPromoNetIncrementalLSV').alias('sumPlanPromoNetIncrementalLSV'))

      calcActualPromoDF = calcActualPromoDF\
        .join(inExchangeCalcPromoDF, inExchangeCalcPromoDF.MasterPromo == calcActualPromoDF.Id, 'left')\
        .select(\
                 calcActualPromoDF['*']
                ,col('sumPlanPromoTIShopper')
                ,col('sumPlanPromoNetIncrementalLSV')
               )

      calcActualPromoDF = calcActualPromoDF\
        .withColumn('ActualAddTIShopper', (isNullCheck(col('ActualPromoTIShopper')) - isNullCheck(col('sumPlanPromoTIShopper')) - (isNullCheck(col('ActualPromoNetIncrementalLSV')) - isNullCheck(col('sumPlanPromoNetIncrementalLSV'))) * col('RATIShopperPercent') / 100)\
                    .cast(DecimalType(30,6)))\
        .drop('sumPlanPromoTIShopper', 'sumPlanPromoNetIncrementalLSV')

    #####*Get result*

    print('Actual promo parameters calculation completed!')
    
    return calcActualPromoDF,logCOGS,logTI,logCOGSTn,logActualCOGS,logActualTI,logActualCOGSTn