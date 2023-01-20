####Notebook "ACTUAL_PARAMETERS_CALCULATION". 
####*Main night actual parameters recalculation notebook. Get actual parameters for promo, promoproduct*.
###### *Developer: [LLC Smart-Com](http://smartcom.software/), andrey.philushkin@effem.com*

def is_notebook() -> bool:
    try:
        shell = get_ipython().__class__.__name__
        if shell == 'ZMQInteractiveShell':
            return True   # Jupyter notebook or qtconsole
        elif shell == 'TerminalInteractiveShell':
            return False  # Terminal running IPython
        else:
            return False  # Other type (?)
    except NameError:
        return False      # Probably standard Python interpreter

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
import subprocess

datesDimSchema = StructType([
  StructField("OriginalDate", DateType(), False),
  StructField("MarsYear", IntegerType(), False),
  StructField("MarsPeriod", IntegerType(), False),
  StructField("MarsWeek",  IntegerType(), False),
  StructField("MarsDay", IntegerType(),  False),
  StructField("MarsPeriodName", StringType(), False),
  StructField("MarsPeriodFullName",  StringType(), False),
  StructField("MarsWeekName", StringType(),  False),
  StructField("MarsWeekFullName", StringType(), False),
  StructField("MarsDayName", StringType(), False),
  StructField("MarsDayFullName",  StringType(), False),
  StructField("CalendarYear", IntegerType(),  False),
  StructField("CalendarMonth", IntegerType(), False),
  StructField("CalendarDay", IntegerType(), False),
  StructField("CalendarDayOfYear",  IntegerType(), False),
  StructField("CalendarMonthName", StringType(),  False),
  StructField("CalendarMonthFullName", StringType(), False),
  StructField("CalendarYearWeek", IntegerType(), False),
  StructField("CalendarWeek",  IntegerType(), False)
])

inputLogMessageSchema = StructType([
  StructField("logMessage", StringType(), False)
])

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": '
 '"/JUPITER/RAW/#MAINTENANCE/2023-01-09_scheduled__2023-01-08T22%3A30%3A00%2B00%3A00_", '
 '"ProcessDate": "2023-01-09", "Schema": "Jupiter", "HandlerId": '
 '"f18a98f9-3b2e-449a-ba96-e247d63d5b7c"}']
 
 sc.addPyFile("hdfs:///SRC/SHARED/EXTRACT_SETTING.py")
 sc.addPyFile("hdfs:///SRC/SHARED/SUPPORT_FUNCTIONS.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/SET_PROMO_PRODUCT.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/ACTUAL_PRODUCT_PARAMS_CALCULATION_PROCESS.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/ACTUAL_PROMO_PARAMS_CALCULATION_PROCESS.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/ACTUAL_SUPPORT_PARAMS_CALCULATION_PROCESS.py") 
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/COGS_TI_CALCULATION.py") 
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/RA_TI_SHOPPER_CALCULATION.py")  

spark = SparkSession.builder.appName('Jupiter - PySpark').getOrCreate()
sc = SparkContext.getOrCreate();

import EXTRACT_SETTING as es

SETTING_RAW_DIR = es.SETTING_RAW_DIR
SETTING_PROCESS_DIR = es.SETTING_PROCESS_DIR
SETTING_OUTPUT_DIR = es.SETTING_OUTPUT_DIR

DATE_DIR=es.DATE_DIR

EXTRACT_ENTITIES_AUTO_PATH = f'{es.HDFS_PREFIX}{es.MAINTENANCE_PATH_PREFIX}EXTRACT_ENTITIES_AUTO.csv'
processDate=es.processDate
pipelineRunId=es.pipelineRunId
handlerId=es.input_params.get("HandlerId")

print(f'EXTRACT_ENTITIES_AUTO_PATH={EXTRACT_ENTITIES_AUTO_PATH}')

import SUPPORT_FUNCTIONS as sp

DIRECTORY = SETTING_RAW_DIR + '/SOURCES/'

# PROMO_PATH = DIRECTORY + 'JUPITER/Promo.PARQUET'
# PROMOPRODUCT_PATH = DIRECTORY + 'JUPITER/PromoProduct.PARQUET'
# PROMOSUPPORTPROMO_PATH = DIRECTORY + 'JUPITER/PromoSupportPromo.PARQUET'

PROMO_PATH = SETTING_PROCESS_DIR + '/Promo/Promo.parquet'
PROMOPRODUCT_PATH = SETTING_PROCESS_DIR + '/PromoProduct/PromoProduct.parquet'
PROMOSUPPORTPROMO_PATH = SETTING_PROCESS_DIR + '/PromoSupportPromo/PromoSupportPromo.parquet'

# input promo to compare values after calculation
INPUT_PROMO_PATH = DIRECTORY + 'JUPITER/Promo'

PROMOSTATUS_PATH = DIRECTORY + 'JUPITER/PromoStatus'
PRODUCT_PATH = DIRECTORY + 'JUPITER/Product'
PRODUCTTREE_PATH = DIRECTORY + 'JUPITER/ProductTree'
PROMOPRODUCTTREE_PATH = DIRECTORY + 'JUPITER/PromoProductTree'
PRICELIST_PATH = DIRECTORY + 'JUPITER/PriceList'
BASELINE_PATH = DIRECTORY + 'JUPITER/BaseLine'
SHARES_PATH = DIRECTORY + 'JUPITER/ClientTreeBrandTech'
CLIENTTREE_PATH = DIRECTORY + 'JUPITER/ClientTree'
CLIENTHIERARCHY_PATH = DIRECTORY + 'JUPITER/ClientTreeHierarchyView'
DATESDIM_PATH = DIRECTORY + 'UNIVERSALCATALOG/MARS_UNIVERSAL_CALENDAR.csv'
CORRECTION_PATH = DIRECTORY + 'JUPITER/PromoProductsCorrection'
INCREMENTAL_PATH = DIRECTORY + 'JUPITER/IncrementalPromo'
PROMOSTATUS_PATH = DIRECTORY + 'JUPITER/PromoStatus'
COGS_PATH = DIRECTORY + 'JUPITER/COGS'
COGSTn_PATH = DIRECTORY + 'JUPITER/PlanCOGSTn'
TI_PATH = DIRECTORY + 'JUPITER/TradeInvestment'
ACTUALCOGS_PATH = DIRECTORY + 'JUPITER/ActualCOGS'
ACTUALCOGSTn_PATH = DIRECTORY + 'JUPITER/ActualCOGSTn'
ACTUALTI_PATH = DIRECTORY + 'JUPITER/ActualTradeInvestment'
BTL_PATH = DIRECTORY + 'JUPITER/BTL'
BTLPROMO_PATH = DIRECTORY + 'JUPITER/BTLPromo'
PROMOSUPPORT_PATH = DIRECTORY + 'JUPITER/PromoSupport'
BUDGETITEM_PATH = DIRECTORY + 'JUPITER/BudgetItem'
BUDGETSUBITEM_PATH = DIRECTORY + 'JUPITER/BudgetSubItem'
ASSORTMENTMARTIX_PATH = DIRECTORY + 'JUPITER/AssortmentMatrix'
BRANDTECH_PATH = DIRECTORY + 'JUPITER/BrandTech'
CHANGESINCIDENTS_PATH = DIRECTORY + 'JUPITER/ChangesIncident'
RATISHOPPER_PATH = DIRECTORY + 'JUPITER/RATIShopper'

FILTERED_PROMO_PATH = SETTING_PROCESS_DIR + '/BlockedPromo/BlockedPromo.parquet'

INPUT_FILE_LOG_PATH = SETTING_PROCESS_DIR + '/Logs/' + handlerId + '.csv'
# OUTPUT_LOG_PATH =  SETTING_PROCESS_DIR + '/Logs/'
# OUTPUT_FILE_LOG_PATH = OUTPUT_LOG_PATH + handlerId + '.csv'
# OUTPUT_TEMP_FILE_LOG_PATH = OUTPUT_LOG_PATH + handlerId + 'temp.csv'

PROMO_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_OUTPUT_DIR + '/Promo/Promo.CSV'
PROMOPRODUCT_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_OUTPUT_DIR + '/PromoProduct/PromoProduct.CSV'
PROMOSUPPORTPROMO_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_OUTPUT_DIR + '/PromoSupportPromo/PromoSupportPromo.CSV'

# DEBUG OUTPUT
# OUTPUT_SOURCE_PROMO_PATH = '/dbfs/' + SETTING_OUTPUT_DIR + '/Actual/SourcePromo/SourcePromo.csv'
# OUTPUT_PROMO_PATH = '/dbfs/' + SETTING_OUTPUT_DIR + '/Actual/Promo/Promo.csv'
# OUTPUT_SOURCE_PROMOPRODUCT_PATH = '/dbfs/' + SETTING_OUTPUT_DIR + '/Actual/SourcePromoProduct/SourcePromoProduct.csv'
# OUTPUT_PROMOPRODUCT_PATH = '/dbfs/' + SETTING_OUTPUT_DIR + '/Actual/PromoProduct/PromoProduct.csv'
# OUTPUT_SOURCE_PROMOSUPPORTPROMO_PATH = '/dbfs/' + SETTING_OUTPUT_DIR + '/Actual/SourcePromoSupportPromo/SourcePromoSupportPromo.csv'
# OUTPUT_PROMOSUPPORTPROMO_PATH = '/dbfs/' + SETTING_OUTPUT_DIR + '/Actual/PromoSupportPromo/PromoSupportPromo.csv'

SCHEMAS_DIR=SETTING_RAW_DIR + '/SCHEMAS/'
schemas_map = sp.getSchemasMap(SCHEMAS_DIR)

promoDF = spark.read.format("parquet").load(PROMO_PATH) 
promoSupportPromoDF = spark.read.format("parquet").load(PROMOSUPPORTPROMO_PATH)
promoProductDF = spark.read.format("parquet").load(PROMOPRODUCT_PATH)

priceListDF = spark.read.csv(PRICELIST_PATH,sep="\u0001",header=True,schema=schemas_map["PriceList"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
promoStatusDF = spark.read.csv(PROMOSTATUS_PATH,sep="\u0001",header=True,schema=schemas_map["PromoStatus"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
inputPromoDF = spark.read.csv(INPUT_PROMO_PATH,sep="\u0001",header=True,schema=schemas_map["Promo"])\
.withColumn("Disabled",col("Disabled").cast(BooleanType()))\
.withColumn("IsLSVBased",col("IsLSVBased").cast(BooleanType()))\
.withColumn("InOut",col("InOut").cast(BooleanType()))\
.withColumn("NeedRecountUplift",col("NeedRecountUplift").cast(BooleanType()))\
.withColumn("IsAutomaticallyApproved",col("IsAutomaticallyApproved").cast(BooleanType()))\
.withColumn("IsCMManagerApproved",col("IsCMManagerApproved").cast(BooleanType()))\
.withColumn("IsDemandPlanningApproved",col("IsDemandPlanningApproved").cast(BooleanType()))\
.withColumn("IsDemandFinanceApproved",col("IsDemandFinanceApproved").cast(BooleanType()))\
.withColumn("Calculating",col("Calculating").cast(BooleanType()))\
.withColumn("LoadFromTLC",col("LoadFromTLC").cast(BooleanType()))\
.withColumn("InOutExcludeAssortmentMatrixProductsButtonPressed",col("InOutExcludeAssortmentMatrixProductsButtonPressed").cast(BooleanType()))\
.withColumn("IsGrowthAcceleration",col("IsGrowthAcceleration").cast(BooleanType()))\
.withColumn("IsOnInvoice",col("IsOnInvoice").cast(BooleanType()))\
.withColumn("IsApolloExport",col("IsApolloExport").cast(BooleanType()))\
.withColumn("UseActualTI",col("UseActualTI").cast(BooleanType()))\
.withColumn("UseActualCOGS",col("UseActualCOGS").cast(BooleanType()))\
.withColumn("ManualInputSumInvoice",col("ManualInputSumInvoice").cast(BooleanType()))\
.withColumn("IsSplittable",col("IsSplittable").cast(BooleanType()))\
.withColumn("IsInExchange",col("IsInExchange").cast(BooleanType()))\
.withColumn("IsGAManagerApproved",col("IsGAManagerApproved").cast(BooleanType()))
productDF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["Product"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
product01DF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["Product"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
productTreeDF = spark.read.csv(PRODUCTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["ProductTree"])
promoProductTreeDF = spark.read.csv(PROMOPRODUCTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductTree"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
baselineDF = spark.read.csv(BASELINE_PATH,sep="\u0001",header=True,schema=schemas_map["BaseLine"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
sharesDF = spark.read.csv(SHARES_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTreeBrandTech"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
clientTreeDF = spark.read.csv(CLIENTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTree"])
clientHierarchyDF = spark.read.csv(CLIENTHIERARCHY_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTreeHierarchyView"])
datesDF = spark.read.format("csv").option("delimiter","|").option("header","true").schema(datesDimSchema).load(DATESDIM_PATH)
correctionDF = spark.read.csv(CORRECTION_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductsCorrection"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
btlDF = spark.read.csv(BTL_PATH,sep="\u0001",header=True,schema=schemas_map["BTL"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
btlPromoDF = spark.read.csv(BTLPROMO_PATH,sep="\u0001",header=True,schema=schemas_map["BTLPromo"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
incrementalDF = spark.read.csv(INCREMENTAL_PATH,sep="\u0001",header=True,schema=schemas_map["IncrementalPromo"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
cogsDF = spark.read.csv(COGS_PATH,sep="\u0001",header=True,schema=schemas_map["COGS"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
cogsTnDF = spark.read.csv(COGSTn_PATH,sep="\u0001",header=True,schema=schemas_map["PlanCOGSTn"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
tiDF = spark.read.csv(TI_PATH,sep="\u0001",header=True,schema=schemas_map["TradeInvestment"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
actualCogsDF = spark.read.csv(ACTUALCOGS_PATH,sep="\u0001",header=True,schema=schemas_map["ActualCOGS"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
actualCogsTnDF = spark.read.csv(ACTUALCOGSTn_PATH,sep="\u0001",header=True,schema=schemas_map["ActualCOGSTn"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
actualTiDF = spark.read.csv(ACTUALTI_PATH,sep="\u0001",header=True,schema=schemas_map["ActualTradeInvestment"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
promoSupportDF = spark.read.csv(PROMOSUPPORT_PATH,sep="\u0001",header=True,schema=schemas_map["PromoSupport"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
budgetItemDF = spark.read.csv(BUDGETITEM_PATH,sep="\u0001",header=True,schema=schemas_map["BudgetItem"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
budgetSubItemDF = spark.read.csv(BUDGETSUBITEM_PATH,sep="\u0001",header=True,schema=schemas_map["BudgetSubItem"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
assortmentMatrixDF = spark.read.csv(ASSORTMENTMARTIX_PATH,sep="\u0001",header=True,schema=schemas_map["AssortmentMatrix"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
brandTechDF = spark.read.csv(BRANDTECH_PATH,sep="\u0001",header=True,schema=schemas_map["BrandTech"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
changesIncidentsDF = spark.read.csv(CHANGESINCIDENTS_PATH,sep="\u0001",header=True,schema=schemas_map["ChangesIncident"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
ratiShopperDF = spark.read.csv(RATISHOPPER_PATH,sep="\u0001",header=True,schema=schemas_map["RATIShopper"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))

filteredPromoDF = spark.read.format("parquet").load(FILTERED_PROMO_PATH)

try:
 inputLogMessageDF = spark.read.format("csv").option("delimiter","\u0001").option("header","true").load(INPUT_FILE_LOG_PATH)
 print('Log has been already made')
except:
 inputLogMessageDF = spark.createDataFrame(sc.emptyRDD(), inputLogMessageSchema)
 print('Init log')
  


####*Date transformation*

priceListDF = priceListDF\
  .withColumn('StartDate', date_add(to_date(priceListDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(priceListDF.EndDate, 'yyyy-MM-dd'), 1))

baselineDF = baselineDF\
  .withColumn('StartDate', date_add(to_date(baselineDF.StartDate, 'yyyy-MM-dd'), 1))

assortmentMatrixDF = assortmentMatrixDF\
  .withColumn('StartDate', date_add(to_date(assortmentMatrixDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(assortmentMatrixDF.EndDate, 'yyyy-MM-dd'), 1))\
  .withColumn('CreateDate', date_add(to_date(assortmentMatrixDF.CreateDate, 'yyyy-MM-dd'), 1))

tiDF = tiDF\
  .withColumn('StartDate', date_add(to_date(tiDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(tiDF.EndDate, 'yyyy-MM-dd'), 1))

cogsDF = cogsDF\
  .withColumn('StartDate', date_add(to_date(cogsDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(cogsDF.EndDate, 'yyyy-MM-dd'), 1))

cogsTnDF = cogsTnDF\
  .withColumn('StartDate', date_add(to_date(cogsTnDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(cogsTnDF.EndDate, 'yyyy-MM-dd'), 1))

actualTiDF = actualTiDF\
  .withColumn('StartDate', date_add(to_date(actualTiDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(actualTiDF.EndDate, 'yyyy-MM-dd'), 1))

actualCogsDF = actualCogsDF\
  .withColumn('StartDate', date_add(to_date(actualCogsDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(actualCogsDF.EndDate, 'yyyy-MM-dd'), 1))

actualCogsTnDF = actualCogsTnDF\
  .withColumn('StartDate', date_add(to_date(actualCogsTnDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(actualCogsTnDF.EndDate, 'yyyy-MM-dd'), 1))

####*Prepare dataframes for calculation*

filteredPromoDF = filteredPromoDF.dropDuplicates()

# promoProduct
promoProductCols = promoProductDF.columns
allCalcActualPromoProductDF = promoProductDF.where(col('Disabled') == 'False')
allCalcActualPromoProductIdsDF = allCalcActualPromoProductDF.select(col('Id'))
disabledPromoProductDF = promoProductDF.join(allCalcActualPromoProductIdsDF, 'Id', 'left_anti').select(promoProductDF['*'])

# print('promoProducts count:', promoProductDF.count())
# print('notDisabledPromoProducts count:', allCalcActualPromoProductDF.count())
# print('disabledPromoProducts count:', disabledPromoProductDF.count())

# promo
calcActualPromoDF = promoDF.where(col('Disabled') == 'False')

# all promo
promoCols = promoDF.columns
allCalcActualPromoDF = promoDF.where(col('Disabled') == 'false')
allCalcActualPromoIdsDF = allCalcActualPromoDF.select(col('Id'))
disabledPromoDF = promoDF.join(allCalcActualPromoIdsDF, 'Id', 'left_anti').select(promoDF['*'])

# print('promoDF count:', promoDF.count())
# print('notDisabledPromoDF count:', allCalcActualPromoDF.count())
# print('disabledPromoDF count:', disabledPromoDF.count())

# priceList
actualParamsPriceListDF = priceListDF\
  .where(col('Disabled') == 'False')\
  .select(\
           to_date(col('StartDate'), 'yyyy-MM-dd').alias('priceStartDate')
          ,to_date(col('EndDate'), 'yyyy-MM-dd').alias('priceEndDate')
          ,col('ProductId').alias('priceProductId')
          ,col('Price').cast(DecimalType(30,6))
          ,col('ClientTreeId').alias('priceClientTreeId')
         )

# incremental
actualParamsIncrementalDF = incrementalDF\
  .where(col('Disabled') == 'False')\
  .select(\
           col('PromoId').alias('incrementalPromoId')
          ,col('ProductId').alias('incrementalProductId')
          ,col('PlanPromoIncrementalCases')
         )

# support
promoSupportDF = promoSupportDF.where(col('Disabled') == 'False')
promoSupportPromoCols = promoSupportPromoDF.columns
activePromoSupportPromoDF = promoSupportPromoDF.where(col('Disabled') == 'False').select(promoSupportPromoCols)
activePromoSupportPromoIdsDF = activePromoSupportPromoDF.select(col('Id'))
disabledPromoSupportPromoDF = promoSupportPromoDF.join(activePromoSupportPromoIdsDF, 'Id', 'left_anti').select(promoSupportPromoCols)

# print('promoSupportPromoDF count:', promoSupportPromoDF.count())
# print('activePromoSupportPromoDF count:', activePromoSupportPromoDF.count())
# print('disabledPromoSupportPromoDF count:', disabledPromoSupportPromoDF.count())

# btl
btlDF = btlDF.where(col('Disabled') == 'False')
btlPromoDF = btlPromoDF.where(col('Disabled') == 'False')

# AM
assortmentMatrixDF = assortmentMatrixDF.where(col('Disabled') == 'False')

# COGS, TI, BrandTech
brandTechDF = brandTechDF.where(col('Disabled') == 'false')
tiDF = tiDF.where(col('Disabled') == 'false')
cogsDF = cogsDF.where(col('Disabled') == 'false')
cogsTnDF = cogsTnDF.where(col('Disabled') == 'false')

actualTiDF = actualTiDF.where(col('Disabled') == 'false')
actualCogsDF = actualCogsDF.where(col('Disabled') == 'false')
actualCogsTnDF = actualCogsTnDF.where(col('Disabled') == 'false')

activeChangesIncidentsDF = changesIncidentsDF\
  .withColumn('ItemId', upper(col('ItemId')))\
  .where(col('ProcessDate').isNull())

actualCogsCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoActualCOGS').select(activeChangesIncidentsDF.ItemId.alias('Id'))
actualCogsTnCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoActualCOGSTn').select(activeChangesIncidentsDF.ItemId.alias('Id'))
actualTiCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoActualTradeInvestment').select(activeChangesIncidentsDF.ItemId.alias('Id'))

#status list for actual parameters recalculation
actualParametersStatuses = ['Finished']

# notCheckPromoStatusList = ['Draft','Cancelled','Deleted','Closed']
notCheckPromoStatusList = ['Cancelled','Deleted']

lightPromoDF = promoDF\
  .where((col('Disabled') == 'False'))\
  .select(\
           col('Id').alias('promoIdCol')
          ,col('Number').alias('promoNumber')
          ,col('BrandTechId').alias('promoBrandTechId')
          ,col('PromoStatusId').alias('promoStatusId')
          ,col('StartDate').alias('promoStartDate')
          ,col('EndDate').alias('promoEndDate')
          ,col('DispatchesStart').alias('promoDispatchesStart')
          ,col('ClientTreeKeyId').alias('promoClientTreeKeyId')
          ,col('ClientTreeId').alias('promoClientTreeId')
          ,col('IsOnInvoice').alias('promoIsOnInvoice')
          ,col('InOut').alias('promoInOut')
          ,col('LoadFromTLC').alias('promoLoadFromTLC')
          ,col('PlanPromoBaselineLSV')
          ,col('ActualPromoBaselineLSV')
          ,col('ActualPromoLSVSO')
          ,col('ActualPromoPostPromoEffectLSV')
         )

lightPromoDF = lightPromoDF\
  .join(clientTreeDF, lightPromoDF.promoClientTreeKeyId == clientTreeDF.Id, 'inner')\
  .select(\
           lightPromoDF['*']
          ,to_date(clientTreeDF.EndDate, 'yyyy-MM-dd').alias('ctEndDate')
          ,col('PostPromoEffectW1').alias('promoClientPostPromoEffectW1')
          ,col('PostPromoEffectW2').alias('promoClientPostPromoEffectW2')
          )\
  .where(col('ctEndDate').isNull())\
  .drop('ctEndDate')

calcActualPromoDF = calcActualPromoDF\
  .join(promoStatusDF, promoStatusDF.Id == calcActualPromoDF.PromoStatusId, 'left')\
  .select(\
           calcActualPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )\
  .where((col('promoStatusSystemName').isin(*actualParametersStatuses)) & (col('LoadFromTLC') == 'False'))

actualPromoProductDF = allCalcActualPromoProductDF\
  .join(lightPromoDF, lightPromoDF.promoIdCol == allCalcActualPromoProductDF.PromoId, 'left')\
  .join(promoStatusDF, promoStatusDF.Id == lightPromoDF.promoStatusId, 'left')\
  .select(\
           allCalcActualPromoProductDF['*']
          ,lightPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )\
  .where((col('promoStatusSystemName').isin(*actualParametersStatuses)) & (col('promoLoadFromTLC') == 'False'))

finCloPromoProductDF = allCalcActualPromoProductDF\
  .join(lightPromoDF, lightPromoDF.promoIdCol == allCalcActualPromoProductDF.PromoId, 'left')\
  .join(promoStatusDF, promoStatusDF.Id == lightPromoDF.promoStatusId, 'left')\
  .select(\
           allCalcActualPromoProductDF['*']
          ,lightPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )\
  .where((col('promoStatusSystemName').isin(['Finished','Closed'])) & (col('promoLoadFromTLC') == 'False'))

actualsLoadPromoDF = actualPromoProductDF\
  .select(\
           actualPromoProductDF.promoIdCol
          ,actualPromoProductDF.promoNumber
          ,actualPromoProductDF.ActualProductPCQty
         )

finCloLoadActualsPromoDF = finCloPromoProductDF\
  .select(\
           finCloPromoProductDF.promoIdCol
          ,finCloPromoProductDF.promoNumber
          ,finCloPromoProductDF.ActualProductPCQty
         )

actualsLoadPromoIdDF = actualsLoadPromoDF\
  .groupBy(['promoIdCol','promoNumber'])\
  .agg(sum('ActualProductPCQty').alias('sumActualProductPCQtyByPromo'))\
  .where((~col('sumActualProductPCQtyByPromo').isNull()) | (col('sumActualProductPCQtyByPromo') != 0))\
  .orderBy('promoNumber')

finCloLoadActualsPromoIdDF = finCloLoadActualsPromoDF\
  .groupBy(['promoIdCol','promoNumber'])\
  .agg(sum('ActualProductPCQty').alias('sumActualProductPCQtyByPromo'))\
  .where((~col('sumActualProductPCQtyByPromo').isNull()) | (col('sumActualProductPCQtyByPromo') != 0))\
  .orderBy('promoNumber')

calcActualPromoProductDF = actualPromoProductDF\
  .join(actualsLoadPromoIdDF, 'promoIdCol', 'inner')\
  .select(actualPromoProductDF['*'])

calcActualPromoProductIdsDF = calcActualPromoProductDF.select(col('Id'))
notCalcActualPromoProductDF = allCalcActualPromoProductDF.join(calcActualPromoProductIdsDF, 'Id', 'left_anti').select(allCalcActualPromoProductDF['*'])

calcActualPromoDF = calcActualPromoDF\
  .join(actualsLoadPromoIdDF, actualsLoadPromoIdDF.promoIdCol == calcActualPromoDF.Id, 'inner')\
  .select(calcActualPromoDF['*'])

calcActualPromoProductDF = calcActualPromoProductDF\
  .join(productDF, productDF.Id == calcActualPromoProductDF.ProductId, 'left')\
  .select(\
           calcActualPromoProductDF['*']
          ,productDF.UOM_PC2Case
          ,productDF.CaseVolume
          ,productDF.PCVolume
         )

calcActualPromoDF = calcActualPromoDF\
  .join(lightPromoDF, lightPromoDF.promoNumber == calcActualPromoDF.Number, 'inner')\
  .select(\
           calcActualPromoDF['*']
          ,lightPromoDF.promoClientPostPromoEffectW1
          ,lightPromoDF.promoClientPostPromoEffectW2
         )

# print(allCalcActualPromoProductDF.count())
# print(calcActualPromoProductDF.count())
# print(notCalcActualPromoProductDF.count())
# print(calcActualPromoDF.count())

import ACTUAL_PRODUCT_PARAMS_CALCULATION_PROCESS as actual_product_params_calculation_process
calcActualPromoProductDF,allCalcActualPromoDF,logPricePromoDF = actual_product_params_calculation_process.run(calcActualPromoProductDF,actualParamsPriceListDF,calcActualPromoDF,allCalcActualPromoDF,promoProductCols)

####*Promo support calculation*

allCalcActualPromoDF = allCalcActualPromoDF\
  .join(promoStatusDF, promoStatusDF.Id == allCalcActualPromoDF.PromoStatusId, 'left')\
  .join(lightPromoDF, lightPromoDF.promoIdCol == allCalcActualPromoDF.Id, 'left')\
  .select(\
           allCalcActualPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
          ,lightPromoDF.promoClientPostPromoEffectW1
          ,lightPromoDF.promoClientPostPromoEffectW2
         )

calcActualSupportPromoDF = allCalcActualPromoDF\
  .where(~col('promoStatusSystemName').isin(*notCheckPromoStatusList))

calcActualSupportPromoIdsDF = calcActualSupportPromoDF.select(col('Id'))
notCalcActualSupportPromoDF = allCalcActualPromoDF.join(calcActualSupportPromoIdsDF, 'Id', 'left_anti').select(allCalcActualPromoDF['*'])

import ACTUAL_SUPPORT_PARAMS_CALCULATION_PROCESS as actual_support_params_calculation_process
calcActualSupportPromoDF,allPromoSupportPromoDF = actual_support_params_calculation_process.run(promoSupportDF,activePromoSupportPromoDF,calcActualSupportPromoDF,btlDF,btlPromoDF,budgetItemDF,budgetSubItemDF,promoSupportPromoCols)

####*Actual promo parameters calculation*

allCalcActualPromoDF = calcActualSupportPromoDF.union(notCalcActualSupportPromoDF)

allCalcActualPromoDF = allCalcActualPromoDF\
  .join(brandTechDF, brandTechDF.Id == allCalcActualPromoDF.BrandTechId, 'left')\
  .join(actualCogsCiIdsDF, actualCogsCiIdsDF.Id == allCalcActualPromoDF.Id, 'left')\
  .join(actualCogsTnCiIdsDF, actualCogsTnCiIdsDF.Id == allCalcActualPromoDF.Id, 'left')\
  .join(actualTiCiIdsDF, actualTiCiIdsDF.Id == allCalcActualPromoDF.Id, 'left')\
  .select(\
           allCalcActualPromoDF['*']
          ,brandTechDF.BrandsegTechsub.alias('promoBrandTechName')
          ,actualCogsCiIdsDF.Id.alias('acogsId')
          ,actualCogsCiIdsDF.Id.alias('acogstnId')
          ,actualTiCiIdsDF.Id.alias('atiId')
         )\
  .withColumn('UseActualCOGS', when(~col('acogstnId').isNull(), True).otherwise(col('UseActualCOGS')))\
  .withColumn('UseActualTI', when(~col('atiId').isNull(), True).otherwise(col('UseActualTI')))\
  .dropDuplicates()

calcActualPromoDF = allCalcActualPromoDF\
  .join(finCloLoadActualsPromoIdDF, finCloLoadActualsPromoIdDF.promoIdCol == allCalcActualPromoDF.Id, 'inner')\
  .where((col('promoStatusSystemName') == 'Finished') | ~col('acogstnId').isNull() | ~col('atiId').isNull())

calcActualPromoIdsDF = calcActualPromoDF.select(col('Id'))
notCalcActualPromoDF = allCalcActualPromoDF.join(calcActualPromoIdsDF, 'Id', 'left_anti').select(allCalcActualPromoDF['*'])

# display(calcActualPromoDF.select(col('Number'),col('promoStatusSystemName'),col('UseActualCOGS'),col('UseActualTI'),col('acogstnId'),col('atiId')))

import ACTUAL_PROMO_PARAMS_CALCULATION_PROCESS as actual_promo_params_calculation_process
calcActualPromoDF,logCOGS,logTI,logCOGSTn,logActualCOGS,logActualTI,logActualCOGSTn = actual_promo_params_calculation_process.run(clientTreeDF,cogsDF,brandTechDF,cogsTnDF,tiDF,ratiShopperDF,calcActualPromoDF,promoDF,actualCogsDF,actualCogsTnDF,actualTiDF)


####*Result*

# promoproduct
notCalcActualPromoProductDF = notCalcActualPromoProductDF.select(promoProductCols)
calcActualPromoProductDF = calcActualPromoProductDF.select(promoProductCols)
allCalcActualPromoProductDF = calcActualPromoProductDF.union(notCalcActualPromoProductDF)
resultPromoProductDF = allCalcActualPromoProductDF.union(disabledPromoProductDF)
resultPromoProductDF.drop('$QCCount')
# ---

# promo
notCalcActualPromoDF = notCalcActualPromoDF.select(promoCols)
calcActualPromoDF = calcActualPromoDF.select(promoCols)
allCalcActualPromoDF = calcActualPromoDF.union(notCalcActualPromoDF)
resultPromoDF = allCalcActualPromoDF.union(disabledPromoDF)
resultPromoDF = resultPromoDF.drop('$QCCount')
# ---

# promosuportpromo
resultPromoSupportPromoDF = allPromoSupportPromoDF.union(disabledPromoSupportPromoDF)
# ---

####*Set last changed date*

def isDemandFinanceChanged(value):
  return array_contains(value, 'PlanPromoLSV') | array_contains(value, 'PlanPromoIncrementalLSV') | array_contains(value, 'PlanPromoUpliftPercent') | \
        array_contains(value, 'ActualPromoLSV') | array_contains(value, 'ActualPromoIncrementalLSV') | array_contains(value, 'ActualPromoUpliftPercent') | \
        array_contains(value, 'ActualPromoLSVByCompensation')

inputPromoDF = inputPromoDF.drop('#QCCount')

compareConditions = [when(inputPromoDF[c] != resultPromoDF[c], lit(c)).otherwise("") for c in inputPromoDF.columns if c not in  ('Id',"StartDate","EndDate","DispatchesStart","DispatchesEnd")]

timestamp = datetime.datetime.fromtimestamp(time.time()).strftime('%Y-%m-%d %H:%M:%S')
select_expr = [col("Id"), *[resultPromoDF[c] for c in resultPromoDF.columns if c != 'Id'], array_remove(array(*compareConditions), "").alias("changedColumns")]
resultPromoDF = inputPromoDF\
  .join(resultPromoDF, "Id")\
  .select(*select_expr)\
  .withColumn('isChanged', size(col('changedColumns')) > 0)\
  .withColumn('isDemandFinanceChanged', isDemandFinanceChanged(col('changedColumns')))\
  .withColumn('LastChangedDate', when(col('isChanged') == True, unix_timestamp(lit(timestamp),'yyyy-MM-dd HH:mm:ss').cast("timestamp")).otherwise(col('LastChangedDate')))\
  .withColumn('LastChangedDateDemand', when(col('isDemandFinanceChanged') == True, unix_timestamp(lit(timestamp),'yyyy-MM-dd HH:mm:ss').cast("timestamp"))\
              .otherwise(col('LastChangedDateDemand')))\
  .withColumn('LastChangedDateFinance', when(col('isDemandFinanceChanged') == True, unix_timestamp(lit(timestamp),'yyyy-MM-dd HH:mm:ss').cast("timestamp"))\
              .otherwise(col('LastChangedDateFinance')))\
  .drop('isChanged', 'isDemandFinanceChanged', 'changedColumns')

# print(resultPromoProductDF.count())
# print(resultPromoDF.count())
# print(resultPromoSupportPromoDF.count())

try:
   subprocess.call(["hadoop", "fs", "-rm", "-r",PROMOPRODUCT_PARAMETERS_CALCULATION_RESULT_PATH])
   subprocess.call(["hadoop", "fs", "-rm", "-r",PROMO_PARAMETERS_CALCULATION_RESULT_PATH])
   subprocess.call(["hadoop", "fs", "-rm", "-r",PROMOSUPPORTPROMO_PARAMETERS_CALCULATION_RESULT_PATH])
except Exception as e:
   print(e)

# resultPromoProductDF.coalesce(6).write.mode("overwrite").parquet(PROMOPRODUCT_PARAMETERS_CALCULATION_RESULT_PATH)
# resultPromoDF.write.mode("overwrite").parquet(PROMO_PARAMETERS_CALCULATION_RESULT_PATH)
# resultPromoSupportPromoDF.coalesce(6).write.mode("overwrite").parquet(PROMOSUPPORTPROMO_PARAMETERS_CALCULATION_RESULT_PATH)

resultPromoProductDF.fillna(False,subset=['Disabled',
'AverageMarker'])\
.withColumn("Disabled",col("Disabled").cast(IntegerType()))\
.withColumn("AverageMarker",col("AverageMarker").cast(IntegerType()))\
.repartition(1)\
.write.csv(PROMOPRODUCT_PARAMETERS_CALCULATION_RESULT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultPromoDF.fillna(False,subset=['Disabled',
'LoadFromTLC',
'InOutExcludeAssortmentMatrixProductsButtonPressed',
'IsGrowthAcceleration',
'IsOnInvoice',
'IsApolloExport',
'DeviationCoefficient',
'UseActualTI',
'UseActualCOGS',
'IsSplittable',
'IsLSVBased',
'IsInExchange',
'NeedRecountUplift',
'IsAutomaticallyApproved',
'IsCMManagerApproved',
'IsDemandPlanningApproved',
'IsDemandFinanceApproved',
'Calculating',
'InOut',
'ManualInputSumInvoice',
'IsGAManagerApproved'
]).withColumn("Disabled",col("Disabled").cast(IntegerType()))\
.withColumn("LoadFromTLC",col("LoadFromTLC").cast(IntegerType()))\
.withColumn("InOutExcludeAssortmentMatrixProductsButtonPressed",col("InOutExcludeAssortmentMatrixProductsButtonPressed").cast(IntegerType()))\
.withColumn("IsGrowthAcceleration",col("IsGrowthAcceleration").cast(IntegerType()))\
.withColumn("IsOnInvoice",col("IsOnInvoice").cast(IntegerType()))\
.withColumn("IsApolloExport",col("IsApolloExport").cast(IntegerType()))\
.withColumn("DeviationCoefficient",col("DeviationCoefficient").cast(IntegerType()))\
.withColumn("UseActualTI",col("UseActualTI").cast(IntegerType()))\
.withColumn("UseActualCOGS",col("UseActualCOGS").cast(IntegerType()))\
.withColumn("IsSplittable",col("IsSplittable").cast(IntegerType()))\
.withColumn("IsLSVBased",col("IsLSVBased").cast(IntegerType()))\
.withColumn("IsInExchange",col("IsInExchange").cast(IntegerType()))\
.withColumn("NeedRecountUplift",col("NeedRecountUplift").cast(IntegerType()))\
.withColumn("IsAutomaticallyApproved",col("IsAutomaticallyApproved").cast(IntegerType()))\
.withColumn("IsCMManagerApproved",col("IsCMManagerApproved").cast(IntegerType()))\
.withColumn("IsDemandPlanningApproved",col("IsDemandPlanningApproved").cast(IntegerType()))\
.withColumn("IsDemandFinanceApproved",col("IsDemandFinanceApproved").cast(IntegerType()))\
.withColumn("Calculating",col("Calculating").cast(IntegerType()))\
.withColumn("InOut",col("InOut").cast(IntegerType()))\
.withColumn("ManualInputSumInvoice",col("ManualInputSumInvoice").cast(IntegerType()))\
.withColumn("IsGAManagerApproved",col("IsGAManagerApproved").cast(IntegerType()))\
.repartition(1)\
.write.csv(PROMO_PARAMETERS_CALCULATION_RESULT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss",
escape="",
quote="",
)

resultPromoSupportPromoDF\
.repartition(1)\
.write.csv(PROMOSUPPORTPROMO_PARAMETERS_CALCULATION_RESULT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

####*Logging*

logPromoProductDF = logPricePromoDF\
  .join(logCOGS, 'promoNumber', 'full')

logPromoProductDF = logPromoProductDF\
  .join(logCOGSTn, 'promoNumber', 'full')

logPromoProductDF = logPromoProductDF\
  .join(logTI, 'promoNumber', 'full')

logPromoProductDF = logPromoProductDF\
  .join(logActualCOGS, 'promoNumber', 'full')

logPromoProductDF = logPromoProductDF\
  .join(logActualCOGSTn, 'promoNumber', 'full')

logPromoProductDF = logPromoProductDF\
  .join(logActualTI, 'promoNumber', 'full')

titleMessage = '[INFO]: ACTUAL PARAMETERS CALCULATION'
titleLogMessageDF = spark.createDataFrame([(titleMessage,)], inputLogMessageSchema)

logMessageDF = logPromoProductDF\
  .select(concat(lit('[WARNING]: Promo №:'), col('promoNumber'),\
                 when(col('nullPriceMessage').isNull(), '').otherwise(concat(lit(' There\'re no price for ZREP: '), col('nullPriceMessage'))),\
                 when(col('zeroPriceMessage').isNull(), '').otherwise(concat(lit('.\r\n There\'re zero price for ZREP: '), col('zeroPriceMessage'))),\
                 when(col('COGSMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('COGSMessage'))),\
                 when(col('COGSTnMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('COGSTnMessage'))),\
                 when(col('TIMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('TIMessage'))),\
                 when(col('ActualCOGSMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('ActualCOGSMessage'))),\
                 when(col('ActualCOGSTnMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('ActualCOGSTnMessage'))),\
                 when(col('ActualTIMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('ActualTIMessage'))),\
                 (lit('.\r\n'))
                ).alias('logMessage'))\
  .where((col('logMessage') != "") & ~(col('logMessage').isNull()))

outputLogMessageDF = inputLogMessageDF\
  .union(titleLogMessageDF)\
  .union(logMessageDF)

sc.setCheckpointDir("tmp")

outputLogMessageDF\
.checkpoint(eager=True)\
.repartition(1)\
.write.csv(INPUT_FILE_LOG_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
)

# subprocess.call(["hadoop", "fs", "-mv", OUTPUT_TEMP_FILE_LOG_PATH, OUTPUT_LOG_PATH])
# subprocess.call(["hadoop", "fs", "-rm", "-r", OUTPUT_TEMP_FILE_LOG_PATH])

  

print('ACTUAL_PARAMETERS_CALCULATION_DONE')

# promoProductDF.orderBy('Id').toPandas().to_csv(OUTPUT_SOURCE_PROMOPRODUCT_PATH, encoding='utf-8',index=False,sep = '\u0001')
# resultPromoProductDF.orderBy('Id').toPandas().to_csv(OUTPUT_PROMOPRODUCT_PATH, encoding='utf-8',index=False,sep = '\u0001')

# promoDF.orderBy('Id').toPandas().to_csv(OUTPUT_SOURCE_PROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')
# resultPromoDF.orderBy('Id').toPandas().to_csv(OUTPUT_PROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')

# promoSupportPromoDF.orderBy('Id').toPandas().to_csv(OUTPUT_SOURCE_PROMOSUPPORTPROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')
# resultPromoSupportPromoDF.orderBy('Id').toPandas().to_csv(OUTPUT_PROMOSUPPORTPROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')