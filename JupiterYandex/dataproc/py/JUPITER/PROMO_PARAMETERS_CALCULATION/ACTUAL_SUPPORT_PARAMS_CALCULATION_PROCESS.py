####Notebook "ACTUAL_SUPPORT_PARAMS_CALCULATION_PROCESS". 
####*Calculate costs and BTL actual support parameters*.
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

def run(promoSupportDF,activePromoSupportPromoDF,calcActualSupportPromoDF,btlDF,btlPromoDF,budgetItemDF,budgetSubItemDF,promoSupportPromoCols):
    sc = SparkContext.getOrCreate();
    spark = SparkSession(sc)
    ####*Calculate Costs*

    #####*Separate promo by closed and not closed. Get sum PlanPromoLSV and closed promo budget*

    allPromoSupportDF = promoSupportDF\
      .select(\
               promoSupportDF.Id.alias('psId')
              ,promoSupportDF.BudgetSubItemId
              ,promoSupportDF.Number.alias('promoSupportNumber')
              ,promoSupportDF.ActualCostTE.cast(DecimalType(30,6))
              ,promoSupportDF.ActualProdCost.cast(DecimalType(30,6))
             )

    allCalcActualPromoSupportDF = allPromoSupportDF\
      .join(activePromoSupportPromoDF, activePromoSupportPromoDF.PromoSupportId == allPromoSupportDF.psId, 'inner')\
      .join(calcActualSupportPromoDF, calcActualSupportPromoDF.Id == activePromoSupportPromoDF.PromoId, 'inner')\
      .select(\
               calcActualSupportPromoDF['*']
              ,allPromoSupportDF['*']
              ,activePromoSupportPromoDF['*']
             )\
      .withColumn('pspId', activePromoSupportPromoDF.Id)\
      .withColumn('pspTPMmode', activePromoSupportPromoDF.TPMmode)\
      .withColumn('pspDisabled', activePromoSupportPromoDF.Disabled)\
      .withColumn('pspDeletedDate', activePromoSupportPromoDF.DeletedDate)\
      .drop(activePromoSupportPromoDF.Id)\
      .drop(activePromoSupportPromoDF.TPMmode)\
      .drop(activePromoSupportPromoDF.Disabled)\
      .drop(activePromoSupportPromoDF.DeletedDate)\
      .dropDuplicates()

    notClosedSumWindow = Window.partitionBy('psId').orderBy(col('isPromoNotClosed').desc())
    closedSumWindow = Window.partitionBy('psId').orderBy(col('isPromoClosed').desc())

    sumNotClosedGroup = allCalcActualPromoSupportDF\
      .groupBy([(col('promoStatusSystemName') != 'Closed').alias('isPromoNotClosed'), 'psId'])\
      .agg(sum('PlanPromoLSV').alias('sumPromoLSV'))\
      .withColumn('Row_Number', row_number().over(notClosedSumWindow))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    sumClosedGroup = allCalcActualPromoSupportDF\
      .groupBy([(col('promoStatusSystemName') == 'Closed').alias('isPromoClosed'), 'psId'])\
      .agg(sum('FactCalculation').alias('_closedBudgetMarketingTi'), sum('FactCostProd').alias('_closedBudgetCostProd'))\
      .withColumn('Row_Number', row_number().over(closedSumWindow))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    sumClosedGroup = sumClosedGroup\
      .select(\
               sumClosedGroup.psId
              ,sumClosedGroup.isPromoClosed
              ,sumClosedGroup._closedBudgetMarketingTi
              ,sumClosedGroup._closedBudgetCostProd
             )\
      .withColumn('closedBudgetMarketingTi', when(col('isPromoClosed') == 'true', col('_closedBudgetMarketingTi')).otherwise(0))\
      .withColumn('closedBudgetCostProd', when(col('isPromoClosed') == 'true', col('_closedBudgetCostProd')).otherwise(0))

    allCalcActualPromoSupportDF = allCalcActualPromoSupportDF\
      .join(sumNotClosedGroup, 'psId', 'left')\
      .join(sumClosedGroup, 'psId', 'left')\
      .drop('isPromoNotClosed','isPromoClosed','_closedBudgetMarketingTi','_closedBudgetCostProd')

    notClosedCalcActualPromoSupportDF = allCalcActualPromoSupportDF\
      .where(col('promoStatusSystemName') != 'Closed')

    closedCalcActualPromoSupportDF = allCalcActualPromoSupportDF\
      .where(col('promoStatusSystemName') == 'Closed')

    #####*Calculate budgets*

    notClosedCalcActualPromoSupportDF = notClosedCalcActualPromoSupportDF\
      .withColumn('kPlan', when(((col('sumPromoLSV') != 0) & (col('PlanPromoLSV').isNotNull())), col('PlanPromoLSV') / col('sumPromoLSV')).otherwise(0))\
      .withColumn('FactCalculation', ((col('ActualCostTE') - col('closedBudgetMarketingTi')) * col('kPlan')).cast(DecimalType(30,2)))\
      .withColumn('FactCostProd', ((col('ActualProdCost') - col('closedBudgetCostProd')) * col('kPlan')).cast(DecimalType(30,2)))

    notClosedCalcActualPromoSupportDF = notClosedCalcActualPromoSupportDF\
      .join(budgetSubItemDF, budgetSubItemDF.Id == notClosedCalcActualPromoSupportDF.BudgetSubItemId, 'left')\
      .join(budgetItemDF, budgetItemDF.Id == budgetSubItemDF.BudgetItemId, 'left')\
      .select(\
               notClosedCalcActualPromoSupportDF['*']
              ,budgetItemDF.Name.alias('BudgetName')
             )

    xsitesDF = notClosedCalcActualPromoSupportDF.where(col('BudgetName') == 'X-sites').select('Id','FactCalculation','FactCostProd')
    catalogDF = notClosedCalcActualPromoSupportDF.where(col('BudgetName') == 'Catalog').select('Id','FactCalculation','FactCostProd')
    posmDF = notClosedCalcActualPromoSupportDF.where(col('BudgetName') == 'POSM').select('Id','FactCalculation','FactCostProd')
    tiMarketCostProdDF = notClosedCalcActualPromoSupportDF.select('Id','FactCalculation','FactCostProd')

    supportSchema = StructType([
      StructField("Id", StringType(), True),
      StructField("actualSupportParameter", DecimalType(30,2), True),
      StructField("actualCostProdParameter", DecimalType(30,2), True)
    ])

    xsitesList = xsitesDF\
      .groupBy('Id')\
      .agg(sum('FactCalculation').alias('calcActualPromoXSites')\
          ,sum('FactCostProd').alias('calcActualPromoCostProdXSites'))\
      .collect()
    xsitesDF = spark.createDataFrame(xsitesList, supportSchema)

    catalogList = catalogDF\
      .groupBy('Id')\
      .agg(sum('FactCalculation').alias('calcActualPromoCatalogue')\
          ,sum('FactCostProd').alias('calcActualPromoCostProdCatalogue'))\
      .collect()
    catalogDF = spark.createDataFrame(catalogList, supportSchema)

    posmList = posmDF\
      .groupBy('Id')\
      .agg(sum('FactCalculation').alias('calcActualPromoPOSMInClient')\
          ,sum('FactCostProd').alias('calcActualPromoCostProdPOSMInClient'))\
      .collect()
    posmDF = spark.createDataFrame(posmList, supportSchema)

    tiMarketCostProdList = tiMarketCostProdDF\
      .groupBy('Id')\
      .agg(sum('FactCalculation').alias('calcActualPromoTIMarketing')\
          ,sum('FactCostProd').alias('calcActualPromoCostProduction'))\
      .collect()
    tiMarketCostProdDF = spark.createDataFrame(tiMarketCostProdList, supportSchema)

    notClosedPromoSupportDF = notClosedCalcActualPromoSupportDF\
      .drop('Id', 'Disabled', 'DeletedDate')\
      .withColumn('Id', col('pspId'))\
      .withColumn('Disabled', col('pspDisabled'))\
      .withColumn('DeletedDate', col('pspDeletedDate'))

    closedPromoSupportDF = closedCalcActualPromoSupportDF\
      .drop('Id', 'Disabled', 'DeletedDate')\
      .withColumn('Id', col('pspId'))\
      .withColumn('Disabled', col('pspDisabled'))\
      .withColumn('DeletedDate', col('pspDeletedDate'))

    notClosedPromoSupportDF = notClosedPromoSupportDF.select(promoSupportPromoCols)
    closedPromoSupportDF = closedPromoSupportDF.select(promoSupportPromoCols)
    allCalcActualPromoSupportPromoDF = notClosedPromoSupportDF.union(closedPromoSupportDF)

    allCalcActualPromoSupportPromoIdsDF = allCalcActualPromoSupportPromoDF.select(col('Id'))
    notCalcActualPromoSupportPromoDF = activePromoSupportPromoDF.join(allCalcActualPromoSupportPromoIdsDF, 'Id', 'left_anti').select(activePromoSupportPromoDF['*'])

    allPromoSupportPromoDF = allCalcActualPromoSupportPromoDF.union(notCalcActualPromoSupportPromoDF)

    ####*Calculate BTL*

    #####*Separate promo by closed and not closed. Get sum PlanPromoLSV and closed promo btl*

    allBtlDF = btlDF\
      .select(\
               btlDF.Id.alias('bId')
              ,btlDF.Number.alias('btlNumber')
              ,btlDF.ActualBTLTotal.cast(DecimalType(30,6))
             )

    allCalcActualBtlDF = allBtlDF\
      .join(btlPromoDF, btlPromoDF.BTLId == allBtlDF.bId, 'inner')\
      .join(calcActualSupportPromoDF, calcActualSupportPromoDF.Id == btlPromoDF.PromoId, 'inner')\
      .select(\
               calcActualSupportPromoDF['*']
              ,allBtlDF['*']
              ,btlPromoDF.Id.alias('bpId')
             )\
      .dropDuplicates()

    notClosedSumWindowBtl = Window.partitionBy('bId').orderBy(col('isPromoNotClosed').desc())
    closedSumWindowBtl = Window.partitionBy('bId').orderBy(col('isPromoClosed').desc())

    sumNotClosedGroupBtl = allCalcActualBtlDF\
      .groupBy([(col('promoStatusSystemName') != 'Closed').alias('isPromoNotClosed'), 'bId'])\
      .agg(sum('PlanPromoLSV').alias('sumPromoLSV'))\
      .withColumn('Row_Number', row_number().over(notClosedSumWindowBtl))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    sumClosedGroupBtl = allCalcActualBtlDF\
      .groupBy([(col('promoStatusSystemName') == 'Closed').alias('isPromoClosed'), 'bId'])\
      .agg(sum('ActualPromoBTL').alias('_closedBudgetBTL'))\
      .withColumn('Row_Number', row_number().over(closedSumWindowBtl))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    sumClosedGroupBtl = sumClosedGroupBtl\
      .select(\
               sumClosedGroupBtl.bId
              ,sumClosedGroupBtl.isPromoClosed
              ,sumClosedGroupBtl._closedBudgetBTL
             )\
      .withColumn('closedBudgetBTL', when(col('isPromoClosed') == 'true', col('_closedBudgetBTL')).otherwise(0))

    allCalcActualBtlDF = allCalcActualBtlDF\
      .join(sumNotClosedGroupBtl, 'bId', 'left')\
      .join(sumClosedGroupBtl, 'bId', 'left')\
      .drop('isPromoNotClosed','isPromoClosed','_closedBudgetBTL')

    #####*Calculate BTL budgets*

    allCalcActualBtlDF = allCalcActualBtlDF\
      .withColumn('kPlan', when(((col('sumPromoLSV') != 0) & (col('PlanPromoLSV').isNotNull())), col('PlanPromoLSV') / col('sumPromoLSV')).otherwise(0))\
      .withColumn('ActualPromoBTL', when(col('promoStatusSystemName') != 'Closed', ((col('ActualBTLTotal') - col('closedBudgetBTL')) * col('kPlan')))\
                  .otherwise(col('ActualPromoBTL')).cast(DecimalType(30,2)))

    btlSchema = StructType([
      StructField("Id", StringType(), True),
      StructField("actualBtlParameter", DecimalType(30,2), True)
    ])

    promoBtlList = allCalcActualBtlDF.select(col('Id'), col('ActualPromoBTL').alias('calcActualPromoBTL')).collect()
    promoBtlDF = spark.createDataFrame(promoBtlList, btlSchema)

    #####*Get result*

    cols = calcActualSupportPromoDF.columns

    calcActualSupportPromoDF = calcActualSupportPromoDF\
      .join(xsitesDF, 'Id', 'left')\
      .withColumn('ActualPromoXSites', when(col('actualSupportParameter').isNull(), col('ActualPromoXSites')).otherwise(col('actualSupportParameter')))\
      .withColumn('ActualPromoCostProdXSites', when(col('actualCostProdParameter').isNull(), col('ActualPromoCostProdXSites')).otherwise(col('actualCostProdParameter')))\
      .select(cols)

    calcActualSupportPromoDF = calcActualSupportPromoDF\
      .join(catalogDF, 'Id', 'left')\
      .withColumn('ActualPromoCatalogue', when(col('actualSupportParameter').isNull(), col('ActualPromoCatalogue')).otherwise(col('actualSupportParameter')))\
      .withColumn('ActualPromoCostProdCatalogue', when(col('actualCostProdParameter').isNull(), col('ActualPromoCostProdCatalogue')).otherwise(col('actualCostProdParameter')))\
      .select(cols)

    calcActualSupportPromoDF = calcActualSupportPromoDF\
      .join(posmDF, 'Id', 'left')\
      .withColumn('ActualPromoPOSMInClient', when(col('actualSupportParameter').isNull(), col('ActualPromoPOSMInClient')).otherwise(col('actualSupportParameter')))\
      .withColumn('ActualPromoCostProdPOSMInClient', when(col('actualCostProdParameter').isNull(), col('ActualPromoCostProdPOSMInClient')).otherwise(col('actualCostProdParameter')))\
      .select(cols)

    calcActualSupportPromoDF = calcActualSupportPromoDF\
      .join(tiMarketCostProdDF, 'Id', 'left')\
      .withColumn('ActualPromoTIMarketing', when(col('actualSupportParameter').isNull(), col('ActualPromoTIMarketing')).otherwise(col('actualSupportParameter')))\
      .withColumn('ActualPromoCostProduction', when(col('actualCostProdParameter').isNull(), col('ActualPromoCostProduction')).otherwise(col('actualCostProdParameter')))\
      .select(cols)

    calcActualSupportPromoDF = calcActualSupportPromoDF\
      .join(promoBtlDF, 'Id', 'left')\
      .withColumn('ActualPromoBTL', when(col('actualBtlParameter').isNull(), col('ActualPromoBTL')).otherwise(col('actualBtlParameter')))\
      .select(cols)

    print('Actual support parameters calculation completed!') 
    
    return calcActualSupportPromoDF,allPromoSupportPromoDF