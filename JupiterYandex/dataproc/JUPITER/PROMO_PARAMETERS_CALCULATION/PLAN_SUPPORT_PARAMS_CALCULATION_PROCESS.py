####Notebook "PLAN_SUPPORT_PARAMS_CALCULATION_PROCESS". 
####*Calculate costs and BTL plan support parameters*.
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

def run(allPromoSupportDF,activePromoSupportPromoDF,calcPlanSupportPromoDF,allBtlDF,budgetItemDF,budgetSubItemDF,promoSupportPromoCols,btlPromoDF):
    sc = SparkContext.getOrCreate();
    spark = SparkSession(sc)
    ####*Calculate Costs*

    #####*Separate promo by closed and not closed. Get sum PlanPromoLSV and closed promo budget*

    # allPromoSupportDF = promoSupportDF\
    #   .select(\
    #            promoSupportDF.Id.alias('psId')
    #           ,promoSupportDF.BudgetSubItemId
    #           ,promoSupportDF.Number.alias('promoSupportNumber')
    #           ,promoSupportDF.PlanCostTE.cast(DecimalType(30,6))
    #           ,promoSupportDF.PlanProdCost.cast(DecimalType(30,6))
    #          )

    allCalcPlanPromoSupportDF = allPromoSupportDF\
      .join(activePromoSupportPromoDF, activePromoSupportPromoDF.PromoSupportId == allPromoSupportDF.psId, 'inner')\
      .join(calcPlanSupportPromoDF, calcPlanSupportPromoDF.Id == activePromoSupportPromoDF.PromoId, 'inner')\
      .select(\
               calcPlanSupportPromoDF['*']
              ,allPromoSupportDF['*']
              ,activePromoSupportPromoDF['*']
             )\
      .withColumn('pspId', activePromoSupportPromoDF.Id)\
      .withColumn('pspDisabled', activePromoSupportPromoDF.Disabled)\
      .withColumn('pspDeletedDate', activePromoSupportPromoDF.DeletedDate)\
      .withColumn('pspTPMmode', activePromoSupportPromoDF.TPMmode)\
      .drop(activePromoSupportPromoDF.Id)\
      .drop(activePromoSupportPromoDF.Disabled)\
      .drop(activePromoSupportPromoDF.DeletedDate)\
      .drop(activePromoSupportPromoDF.TPMmode)\
      .dropDuplicates()
    
    allCalcPlanPromoSupportCurrentDF = allCalcPlanPromoSupportDF.where(col('TPMmode') == 0)  

    notClosedSumWindow = Window.partitionBy('psId').orderBy(col('isPromoNotClosed').desc())
    closedSumWindow = Window.partitionBy('psId').orderBy(col('isPromoClosed').desc())

    sumNotClosedGroup = allCalcPlanPromoSupportCurrentDF\
      .groupBy([(col('promoStatusSystemName') != 'Closed').alias('isPromoNotClosed'), 'psId'])\
      .agg(sum('PlanPromoLSV').alias('sumPromoLSV'))\
      .withColumn('Row_Number', row_number().over(notClosedSumWindow))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    # sumNotClosedGroup.show()

    sumClosedGroup = allCalcPlanPromoSupportCurrentDF\
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

    # sumClosedGroup.show()

    allCalcPlanPromoSupportCurrentDF = allCalcPlanPromoSupportCurrentDF\
      .join(sumNotClosedGroup, 'psId', 'left')\
      .join(sumClosedGroup, 'psId', 'left')\
      .drop('isPromoNotClosed','isPromoClosed','_closedBudgetMarketingTi','_closedBudgetCostProd')

    notClosedCalcPlanPromoSupportDF = allCalcPlanPromoSupportCurrentDF\
      .where(col('promoStatusSystemName') != 'Closed')

    closedCalcPlanPromoSupportDF = allCalcPlanPromoSupportCurrentDF\
      .where(col('promoStatusSystemName') == 'Closed')

    # RS
    allCalcPlanPromoSupportRSDF = allCalcPlanPromoSupportDF.where(col('TPMmode') == 1)

    notClosedSumWindow = Window.partitionBy('psId').orderBy(col('isPromoNotClosed').desc())
    closedSumWindow = Window.partitionBy('psId').orderBy(col('isPromoClosed').desc())

    sumNotClosedGroup = allCalcPlanPromoSupportRSDF\
      .groupBy([(col('promoStatusSystemName') != 'Closed').alias('isPromoNotClosed'), 'psId'])\
      .agg(sum('PlanPromoLSV').alias('sumPromoLSV'))\
      .withColumn('Row_Number', row_number().over(notClosedSumWindow))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    # sumNotClosedGroup.show()

    sumClosedGroup = allCalcPlanPromoSupportRSDF\
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

    # sumClosedGroup.show()

    allCalcPlanPromoSupportRSDF = allCalcPlanPromoSupportRSDF\
      .join(sumNotClosedGroup, 'psId', 'left')\
      .join(sumClosedGroup, 'psId', 'left')\
      .drop('isPromoNotClosed','isPromoClosed','_closedBudgetMarketingTi','_closedBudgetCostProd')

    notClosedCalcPlanPromoSupportRSDF = allCalcPlanPromoSupportRSDF\
      .where(col('promoStatusSystemName') != 'Closed')

    closedCalcPlanPromoSupportRSDF = allCalcPlanPromoSupportRSDF\
      .where(col('promoStatusSystemName') == 'Closed')


    # Current

    notClosedCalcPlanPromoSupportDF = notClosedCalcPlanPromoSupportDF\
      .withColumn('kPlan', when(((col('sumPromoLSV') != 0) & (col('PlanPromoLSV').isNotNull())), col('PlanPromoLSV') / col('sumPromoLSV')).otherwise(0))\
      .withColumn('PlanCalculation', when(col('onlyFinishedClosed') == True, col('PlanCalculation'))\
                  .otherwise((col('PlanCostTE') - col('closedBudgetMarketingTi')) * col('kPlan')).cast(DecimalType(30,2)))\
      .withColumn('PlanCostProd', when(col('onlyFinishedClosed') == True, col('PlanCostProd'))\
                  .otherwise((col('PlanProdCost') - col('closedBudgetCostProd')) * col('kPlan')).cast(DecimalType(30,2)))

    notClosedCalcPlanPromoSupportDF = notClosedCalcPlanPromoSupportDF\
      .join(budgetSubItemDF, budgetSubItemDF.Id == notClosedCalcPlanPromoSupportDF.BudgetSubItemId, 'left')\
      .join(budgetItemDF, budgetItemDF.Id == budgetSubItemDF.BudgetItemId, 'left')\
      .select(\
               notClosedCalcPlanPromoSupportDF['*']
              ,budgetItemDF.Name.alias('BudgetName')
             )
    # RS
    notClosedCalcPlanPromoSupportRSDF = notClosedCalcPlanPromoSupportRSDF\
      .withColumn('kPlan', when(((col('sumPromoLSV') != 0) & (col('PlanPromoLSV').isNotNull())), col('PlanPromoLSV') / col('sumPromoLSV')).otherwise(0))\
      .withColumn('PlanCalculation', when(col('onlyFinishedClosed') == True, col('PlanCalculation'))\
              .otherwise((col('PlanCostTE') - col('closedBudgetMarketingTi')) * col('kPlan')).cast(DecimalType(30,2)))\
      .withColumn('PlanCostProd', when(col('onlyFinishedClosed') == True, col('PlanCostProd'))\
              .otherwise((col('PlanProdCost') - col('closedBudgetCostProd')) * col('kPlan')).cast(DecimalType(30,2)))

    notClosedCalcPlanPromoSupportRSDF = notClosedCalcPlanPromoSupportRSDF\
      .join(budgetSubItemDF, budgetSubItemDF.Id == notClosedCalcPlanPromoSupportRSDF.BudgetSubItemId, 'left')\
      .join(budgetItemDF, budgetItemDF.Id == budgetSubItemDF.BudgetItemId, 'left')\
      .select(\
           notClosedCalcPlanPromoSupportRSDF['*']
          ,budgetItemDF.Name.alias('BudgetName')
             )

    xsitesDF = notClosedCalcPlanPromoSupportDF.where(col('BudgetName') == 'X-sites').select('Id','PlanCalculation','PlanCostProd')
    catalogDF = notClosedCalcPlanPromoSupportDF.where(col('BudgetName') == 'Catalog').select('Id','PlanCalculation','PlanCostProd')
    posmDF = notClosedCalcPlanPromoSupportDF.where(col('BudgetName') == 'POSM').select('Id','PlanCalculation','PlanCostProd')
    tiMarketCostProdDF = notClosedCalcPlanPromoSupportDF.select('Id','PlanCalculation','PlanCostProd')
    
    xsitesRSDF = notClosedCalcPlanPromoSupportRSDF.where(col('BudgetName') == 'X-sites').select('Id','PlanCalculation','PlanCostProd')
    catalogRSDF = notClosedCalcPlanPromoSupportRSDF.where(col('BudgetName') == 'Catalog').select('Id','PlanCalculation','PlanCostProd')
    posmRSDF = notClosedCalcPlanPromoSupportRSDF.where(col('BudgetName') == 'POSM').select('Id','PlanCalculation','PlanCostProd')
    tiMarketCostProdRSDF = notClosedCalcPlanPromoSupportRSDF.select('Id','PlanCalculation','PlanCostProd')


    supportSchema = StructType([
      StructField("Id", StringType(), True),
      StructField("planSupportParameter", DecimalType(30,2), True),
      StructField("planCostProdParameter", DecimalType(30,2), True)
    ])

    xsitesList = xsitesDF\
      .groupBy('Id')\
      .agg(sum('PlanCalculation').alias('calcPlanPromoXSites')\
          ,sum('PlanCostProd').alias('calcPlanPromoCostProdXSites'))\
      .collect()
    xsitesDF = spark.createDataFrame(xsitesList, supportSchema)
    
    xsitesRSList = xsitesRSDF\
      .groupBy('Id')\
      .agg(sum('PlanCalculation').alias('calcPlanPromoXSites')\
       ,sum('PlanCostProd').alias('calcPlanPromoCostProdXSites'))\
      .collect()
    xsitesRSDF = spark.createDataFrame(xsitesRSList, supportSchema)

    # xsitesDF.show()

    catalogList = catalogDF\
      .groupBy('Id')\
      .agg(sum('PlanCalculation').alias('calcPlanPromoCatalogue')\
          ,sum('PlanCostProd').alias('calcPlanPromoCostProdCatalogue'))\
      .collect()
    catalogDF = spark.createDataFrame(catalogList, supportSchema)
    
    catalogRSList = catalogRSDF\
      .groupBy('Id')\
      .agg(sum('PlanCalculation').alias('calcPlanPromoCatalogue')\
       ,sum('PlanCostProd').alias('calcPlanPromoCostProdCatalogue'))\
      .collect()
    catalogRSDF = spark.createDataFrame(catalogRSList, supportSchema)

    posmList = posmDF\
      .groupBy('Id')\
      .agg(sum('PlanCalculation').alias('calcPlanPromoPOSMInClient')\
          ,sum('PlanCostProd').alias('calcPlanPromoCostProdPOSMInClient'))\
      .collect()
    posmDF = spark.createDataFrame(posmList, supportSchema)
    
    posmRSList = posmRSDF\
     .groupBy('Id')\
     .agg(sum('PlanCalculation').alias('calcPlanPromoPOSMInClient')\
       ,sum('PlanCostProd').alias('calcPlanPromoCostProdPOSMInClient'))\
     .collect()
    posmRSDF = spark.createDataFrame(posmRSList, supportSchema)

    tiMarketCostProdList = tiMarketCostProdDF\
      .groupBy('Id')\
      .agg(sum('PlanCalculation').alias('calcPlanPromoTIMarketing')\
          ,sum('PlanCostProd').alias('calcPlanPromoCostProduction'))\
      .collect()
    tiMarketCostProdDF = spark.createDataFrame(tiMarketCostProdList, supportSchema)
    
    tiMarketCostProdRSList = tiMarketCostProdRSDF\
      .groupBy('Id')\
      .agg(sum('PlanCalculation').alias('calcPlanPromoTIMarketing')\
       ,sum('PlanCostProd').alias('calcPlanPromoCostProduction'))\
      .collect()
    tiMarketCostProdRSDF = spark.createDataFrame(tiMarketCostProdRSList, supportSchema)


    notClosedPromoSupportDF = notClosedCalcPlanPromoSupportDF\
      .drop('Id', 'Disabled', 'DeletedDate')\
      .withColumn('Id', col('pspId'))\
      .withColumn('Disabled', col('pspDisabled'))\
      .withColumn('DeletedDate', col('pspDeletedDate'))

    closedPromoSupportDF = closedCalcPlanPromoSupportDF\
      .drop('Id', 'Disabled', 'DeletedDate')\
      .withColumn('Id', col('pspId'))\
      .withColumn('Disabled', col('pspDisabled'))\
      .withColumn('DeletedDate', col('pspDeletedDate'))

    notClosedPromoSupportDF = notClosedPromoSupportDF.select(promoSupportPromoCols)
    closedPromoSupportDF = closedPromoSupportDF.select(promoSupportPromoCols)
    #RS

    notClosedPromoSupportRSDF = notClosedCalcPlanPromoSupportRSDF\
      .drop('Id', 'Disabled', 'DeletedDate')\
      .withColumn('Id', col('pspId'))\
      .withColumn('Disabled', col('pspDisabled'))\
      .withColumn('DeletedDate', col('pspDeletedDate'))

    closedPromoSupportRSDF = closedCalcPlanPromoSupportRSDF\
      .drop('Id', 'Disabled', 'DeletedDate')\
      .withColumn('Id', col('pspId'))\
      .withColumn('Disabled', col('pspDisabled'))\
      .withColumn('DeletedDate', col('pspDeletedDate'))
  
    notClosedPromoSupportRSDF = notClosedPromoSupportRSDF.select(promoSupportPromoCols)
    closedPromoSupportRSDF = closedPromoSupportRSDF.select(promoSupportPromoCols)

    allCalcPlanPromoSupportPromoDF = notClosedPromoSupportDF.union(closedPromoSupportDF)
    allCalcPlanPromoSupportPromoRSDF = notClosedPromoSupportRSDF.union(closedPromoSupportRSDF)

    allCalcPlanPromoSupportPromoDF = allCalcPlanPromoSupportPromoDF.union(allCalcPlanPromoSupportPromoRSDF)


    allCalcPlanPromoSupportPromoIdsDF = allCalcPlanPromoSupportPromoDF.select(col('Id'))
    notCalcPlanPromoSupportPromoDF = activePromoSupportPromoDF.join(allCalcPlanPromoSupportPromoIdsDF, 'Id', 'left_anti').select(activePromoSupportPromoDF['*'])

    allPromoSupportPromoDF = allCalcPlanPromoSupportPromoDF.union(notCalcPlanPromoSupportPromoDF)

    # print(notClosedPromoSupportDF.count())
    # print(closedPromoSupportDF.count())
    # print(allCalcPlanPromoSupportPromoDF.count())

    # print(notCalcPlanPromoSupportPromoDF.count())
    # print(allPromoSupportPromoDF.count())

    ####*Calculate BTL*

    #####*Separate promo by closed and not closed. Get sum PlanPromoLSV and closed promo btl*

    # allBtlDF = btlDF\
    #   .select(\
    #            btlDF.Id.alias('bId')
    #           ,btlDF.Number.alias('btlNumber')
    #           ,btlDF.PlanBTLTotal.cast(DecimalType(30,6))
    #          )

    allCalcPlanBtlDF = allBtlDF\
      .join(btlPromoDF, btlPromoDF.BTLId == allBtlDF.bId, 'inner')\
      .join(calcPlanSupportPromoDF, calcPlanSupportPromoDF.Id == btlPromoDF.PromoId, 'inner')\
      .select(\
               calcPlanSupportPromoDF['*']
              ,allBtlDF['*']
              ,btlPromoDF.Id.alias('bpId')
             )\
      .dropDuplicates()

    notClosedSumWindowBtl = Window.partitionBy('bId').orderBy(col('isPromoNotClosed').desc())
    closedSumWindowBtl = Window.partitionBy('bId').orderBy(col('isPromoClosed').desc())

    sumNotClosedGroupBtl = allCalcPlanBtlDF\
      .groupBy([(col('promoStatusSystemName') != 'Closed').alias('isPromoNotClosed'), 'bId'])\
      .agg(sum('PlanPromoLSV').alias('sumPromoLSV'))\
      .withColumn('Row_Number', row_number().over(notClosedSumWindowBtl))\
      .where(col('Row_Number') == 1).drop('Row_Number')

    sumClosedGroupBtl = allCalcPlanBtlDF\
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

    allCalcPlanBtlDF = allCalcPlanBtlDF\
      .join(sumNotClosedGroupBtl, 'bId', 'left')\
      .join(sumClosedGroupBtl, 'bId', 'left')\
      .drop('isPromoNotClosed','isPromoClosed','_closedBudgetBTL')

    #####*Calculate BTL budgets*

    allCalcPlanBtlDF = allCalcPlanBtlDF\
      .withColumn('kPlan', when(((col('sumPromoLSV') != 0) & (col('PlanPromoLSV').isNotNull())), col('PlanPromoLSV') / col('sumPromoLSV')).otherwise(0))\
      .withColumn('PlanPromoBTL', when((col('promoStatusSystemName') != 'Closed') & (col('onlyFinishedClosed') == False),\
                                 ((col('PlanBTLTotal') - col('closedBudgetBTL')) * col('kPlan'))).otherwise(col('PlanPromoBTL')).cast(DecimalType(30,2)))

    btlSchema = StructType([
      StructField("Id", StringType(), True),
      StructField("planBtlParameter", DecimalType(30,2), True)
    ])

    promoBtlList = allCalcPlanBtlDF.select(col('Id'), col('PlanPromoBTL').alias('calcPlanPromoBTL')).collect()
    promoBtlDF = spark.createDataFrame(promoBtlList, btlSchema)

    #####*Get result*

    cols = calcPlanSupportPromoDF.columns

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(xsitesDF, 'Id', 'left')\
      .withColumn('PlanPromoXSites', when(col('planSupportParameter').isNull(), col('PlanPromoXSites')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProdXSites', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProdXSites')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(xsitesRSDF, 'Id', 'left')\
      .withColumn('PlanPromoXSites', when(col('planSupportParameter').isNull(), col('PlanPromoXSites')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProdXSites', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProdXSites')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(catalogDF, 'Id', 'left')\
      .withColumn('PlanPromoCatalogue', when(col('planSupportParameter').isNull(), col('PlanPromoCatalogue')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProdCatalogue', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProdCatalogue')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(catalogRSDF, 'Id', 'left')\
      .withColumn('PlanPromoCatalogue', when(col('planSupportParameter').isNull(), col('PlanPromoCatalogue')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProdCatalogue', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProdCatalogue')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(posmDF, 'Id', 'left')\
      .withColumn('PlanPromoPOSMInClient', when(col('planSupportParameter').isNull(), col('PlanPromoPOSMInClient')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProdPOSMInClient', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProdPOSMInClient')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(posmRSDF, 'Id', 'left')\
      .withColumn('PlanPromoPOSMInClient', when(col('planSupportParameter').isNull(), col('PlanPromoPOSMInClient')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProdPOSMInClient', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProdPOSMInClient')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(tiMarketCostProdDF, 'Id', 'left')\
      .withColumn('PlanPromoTIMarketing', when(col('planSupportParameter').isNull(), col('PlanPromoTIMarketing')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProduction', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProduction')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(tiMarketCostProdRSDF, 'Id', 'left')\
      .withColumn('PlanPromoTIMarketing', when(col('planSupportParameter').isNull(), col('PlanPromoTIMarketing')).otherwise(col('planSupportParameter')))\
      .withColumn('PlanPromoCostProduction', when(col('planCostProdParameter').isNull(), col('PlanPromoCostProduction')).otherwise(col('planCostProdParameter')))\
      .select(cols)

    calcPlanSupportPromoDF = calcPlanSupportPromoDF\
      .join(promoBtlDF, 'Id', 'left')\
      .withColumn('PlanPromoBTL', when(col('planBtlParameter').isNull(), col('PlanPromoBTL')).otherwise(col('planBtlParameter')))\
      .select(cols)

    print('Plan support parameters calculation completed!')
    
    return calcPlanSupportPromoDF,allPromoSupportPromoDF