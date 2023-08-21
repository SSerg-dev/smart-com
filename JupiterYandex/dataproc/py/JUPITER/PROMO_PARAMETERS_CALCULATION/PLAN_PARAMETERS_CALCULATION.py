####Notebook "PLAN_PARAMETERS_CALCULATION". 
####*Main night plan parameters recalculation notebook. Get plan parameters for promo, promoproduct*.
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
]);

outputProductChangeIncidentsSchema = StructType([
  StructField("RecalculatedPromoId", StringType(), False),
  StructField("AddedProductIds", StringType(), True),
  StructField("ExcludedProductIds", StringType(), True)
]);

if is_notebook():
 sys.argv=['','']
 
 sc.addPyFile("hdfs:///SRC/SHARED/EXTRACT_SETTING.py")
 sc.addPyFile("hdfs:///SRC/SHARED/SUPPORT_FUNCTIONS.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/SET_PROMO_PRODUCT.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/PLAN_PRODUCT_PARAMS_CALCULATION_PROCESS.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/PI_PLAN_PRODUCT_PARAMS_CALCULATION_PROCESS.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/PLAN_PROMO_PARAMS_CALCULATION_PROCESS.py")
 sc.addPyFile("hdfs:///SRC/JUPITER/PROMO_PARAMETERS_CALCULATION/PLAN_SUPPORT_PARAMS_CALCULATION_PROCESS.py") 
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

schema = es.input_params.get("Schema")

DIRECTORY = SETTING_RAW_DIR + '/SOURCES/'

PROMO_PATH = DIRECTORY + 'JUPITER/Promo'
PROMO_PRICEINCREASE_PATH = DIRECTORY + 'JUPITER/PromoPriceIncrease'
PROMOSTATUS_PATH = DIRECTORY + 'JUPITER/PromoStatus'
PROMOPRODUCT_PATH = DIRECTORY + 'JUPITER/PromoProduct'
PROMOPRODUCT_PRICEINCREASE_PATH = DIRECTORY + 'JUPITER/PromoProductPriceIncrease'
PRODUCT_PATH = DIRECTORY + 'JUPITER/Product'
PRODUCTTREE_PATH = DIRECTORY + 'JUPITER/ProductTree'
PROMOPRODUCTTREE_PATH = DIRECTORY + 'JUPITER/PromoProductTree'
PRICELIST_PATH = DIRECTORY + 'JUPITER/PriceList'
BASELINE_PATH = DIRECTORY + 'JUPITER/BaseLine'
INCREASEBASELINE_PATH = DIRECTORY + 'JUPITER/IncreaseBaseLine'
SHARES_PATH = DIRECTORY + 'JUPITER/ClientTreeBrandTech'
CLIENTTREE_PATH = DIRECTORY + 'JUPITER/ClientTree'
CLIENTHIERARCHY_PATH = DIRECTORY + 'JUPITER/ClientTreeHierarchyView'
DATESDIM_PATH = DIRECTORY + 'UNIVERSALCATALOG/MARS_UNIVERSAL_CALENDAR.csv'
CORRECTION_PRICEINCREASE_PATH = DIRECTORY + 'JUPITER/PromoProductCorrectionPriceIncrease'
CORRECTION_PATH = DIRECTORY + 'JUPITER/PromoProductsCorrection'
INCREMENTAL_PATH = DIRECTORY + 'JUPITER/IncrementalPromo'
PROMOSTATUS_PATH = DIRECTORY + 'JUPITER/PromoStatus'
COGS_PATH = DIRECTORY + 'JUPITER/COGS'
COGSTn_PATH = DIRECTORY + 'JUPITER/PlanCOGSTn'
TI_PATH = DIRECTORY + 'JUPITER/TradeInvestment'
BTL_PATH = DIRECTORY + 'JUPITER/BTL'
BTLPROMO_PATH = DIRECTORY + 'JUPITER/BTLPromo'
PROMOSUPPORT_PATH = DIRECTORY + 'JUPITER/PromoSupport'
PROMOSUPPORTPROMO_PATH = DIRECTORY + 'JUPITER/PromoSupportPromo'
BUDGETITEM_PATH = DIRECTORY + 'JUPITER/BudgetItem'
BUDGETSUBITEM_PATH = DIRECTORY + 'JUPITER/BudgetSubItem'
ASSORTMENTMARTIX_PATH = DIRECTORY + 'JUPITER/AssortmentMatrix'
BRANDTECH_PATH = DIRECTORY + 'JUPITER/BrandTech'
SERVICEINFO_PATH = DIRECTORY + 'JUPITER/ServiceInfo'
RATISHOPPER_PATH = DIRECTORY + 'JUPITER/RATIShopper'
PLANPOSTPROMOEFFECT_PATH = DIRECTORY + 'JUPITER/PlanPostPromoEffect'
DISCOUNTRANGE_PATH = DIRECTORY + 'JUPITER/DiscountRange'
DURATIONRANGE_PATH = DIRECTORY + 'JUPITER/DurationRange'

FILTERED_PROMO_PATH = SETTING_PROCESS_DIR + '/BlockedPromo/BlockedPromo.parquet'
FILTERED_INCREASE_PROMO_PATH = SETTING_PROCESS_DIR + '/BlockedPromo/BlockedIncreasePromo.parquet'

PLAN_PROMO_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_PROCESS_DIR + '/Promo/Promo.parquet'
PLAN_PROMOPRODUCT_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_PROCESS_DIR + '/PromoProduct/PromoProduct.parquet'
PLAN_PROMOSUPPORTPROMO_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_PROCESS_DIR + '/PromoSupportPromo/PromoSupportPromo.parquet'
PLAN_PROMOPRODUCT_PRICEINCREASE_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_OUTPUT_DIR + '/PromoProduct/PromoProductPriceIncrease.CSV'
PLAN_PROMO_PRICEINCREASE_PARAMETERS_CALCULATION_RESULT_PATH = SETTING_OUTPUT_DIR + '/Promo/PromoPriceIncrease.CSV'
NEW_PROMOPRODUCT_PATH = SETTING_PROCESS_DIR + '/PromoProduct/NewPromoProduct.CSV'
SERVICEINFO_RESULT_PATH = SETTING_OUTPUT_DIR + '/ServiceInfo/ServiceInfo.CSV'
NEW_PRODUCTCHANGEINCIDENTS_PATH = SETTING_OUTPUT_DIR + '/ProductChangeIncident/NewProductChangeIncident.CSV'

INPUT_FILE_LOG_PATH = SETTING_PROCESS_DIR + '/Logs/' +  handlerId + '.csv'
OUTPUT_LOG_PATH =  SETTING_PROCESS_DIR + '/Logs/'
OUTPUT_FILE_LOG_PATH = OUTPUT_LOG_PATH + handlerId + '.csv'

INPUT_FILE_LOG_PATH = es.SETTING_PROCESS_DIR + '/Logs/' + handlerId + '.csv'
# OUTPUT_LOG_PATH =  es.SETTING_PROCESS_DIR + '/Logs/'
# OUTPUT_FILE_LOG_PATH = OUTPUT_LOG_PATH + handlerId + '.csv'
# OUTPUT_TEMP_FILE_LOG_PATH = OUTPUT_LOG_PATH + handlerId + 'temp.csv'

# DEBUG OUTPUT
# OUTPUT_SOURCE_PROMO_PATH = '/dbfs/' + SETTING_PROCESS_DIR + '/SourcePromo/SourcePromo.csv'
# OUTPUT_PROMO_PATH = '/dbfs/' + SETTING_PROCESS_DIR + '/Promo/Promo.csv'
# OUTPUT_SOURCE_PROMOPRODUCT_PATH = '/dbfs/' + SETTING_PROCESS_DIR + '/SourcePromoProduct/SourcePromoProduct.csv'
# OUTPUT_PROMOPRODUCT_PATH = '/dbfs/' + SETTING_PROCESS_DIR + '/PromoProduct/PromoProduct.csv'
# OUTPUT_SOURCE_PROMOSUPPORTPROMO_PATH = '/dbfs/' + SETTING_PROCESS_DIR + '/SourcePromoSupportPromo/SourcePromoSupportPromo.csv'
# OUTPUT_PROMOSUPPORTPROMO_PATH = '/dbfs/' + SETTING_PROCESS_DIR + '/PromoSupportPromo/PromoSupportPromo.csv'

SCHEMAS_DIR=SETTING_RAW_DIR + '/SCHEMAS/'
schemas_map = sp.getSchemasMap(SCHEMAS_DIR)

priceListDF = spark.read.csv(PRICELIST_PATH,sep="\u0001",header=True,schema=schemas_map["PriceList"]).withColumn("Disabled",col("Disabled").cast(BooleanType())).withColumn("FuturePriceMarker",col("FuturePriceMarker").cast(BooleanType()))
promoDF = spark.read.csv(PROMO_PATH,sep="\u0001",header=True,schema=schemas_map["Promo"])\
.withColumn("Disabled",col("Disabled").cast(BooleanType()))\
.withColumn("IsLSVBased",col("IsLSVBased").cast(BooleanType()))\
.withColumn("InOut",col("InOut").cast(BooleanType()))\
.withColumn("NeedRecountUplift",col("NeedRecountUplift").cast(BooleanType()))\
.withColumn("NeedRecountUpliftPI",col("NeedRecountUpliftPI").cast(BooleanType()))\
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
promoPriceIncreaseDF = spark.read.csv(PROMO_PRICEINCREASE_PATH,sep="\u0001",header=True,schema=schemas_map["PromoPriceIncrease"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
promoStatusDF = spark.read.csv(PROMOSTATUS_PATH,sep="\u0001",header=True,schema=schemas_map["PromoStatus"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
promoProductDF = spark.read.csv(PROMOPRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProduct"])\
.withColumn("Disabled",col("Disabled").cast(BooleanType()))\
.withColumn("AverageMarker",col("AverageMarker").cast(BooleanType()))
promoProductPriceIncreaseDF = spark.read.csv(PROMOPRODUCT_PRICEINCREASE_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductPriceIncrease"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
productDF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["Product"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
allProductDF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["Product"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
allProduct01DF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["Product"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
productTreeDF = spark.read.csv(PRODUCTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["ProductTree"])
promoProductTreeDF = spark.read.csv(PROMOPRODUCTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductTree"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
baselineDF = spark.read.csv(BASELINE_PATH,sep="\u0001",header=True,schema=schemas_map["BaseLine"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
baselineIncreaseDF = spark.read.csv(INCREASEBASELINE_PATH,sep="\u0001",header=True,schema=schemas_map["BaseLine"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
sharesDF = spark.read.csv(SHARES_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTreeBrandTech"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
clientTreeDF = spark.read.csv(CLIENTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTree"]).withColumn("DemandCode", when(col("DemandCode")=="\0",lit(None)).otherwise(col("DemandCode")))
clientHierarchyDF = spark.read.csv(CLIENTHIERARCHY_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTreeHierarchyView"])
datesDF = spark.read.format("csv").option("delimiter","|").option("header","true").schema(datesDimSchema).load(DATESDIM_PATH)
correctionDF = spark.read.csv(CORRECTION_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductsCorrection"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
correctionPriceIncreaseDF = spark.read.csv(CORRECTION_PRICEINCREASE_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductCorrectionPriceIncrease"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
btlDF = spark.read.csv(BTL_PATH,sep="\u0001",header=True,schema=schemas_map["BTL"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
btlPromoDF = spark.read.csv(BTLPROMO_PATH,sep="\u0001",header=True,schema=schemas_map["BTLPromo"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
incrementalDF = spark.read.csv(INCREMENTAL_PATH,sep="\u0001",header=True,schema=schemas_map["IncrementalPromo"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
cogsDF = spark.read.csv(COGS_PATH,sep="\u0001",header=True,schema=schemas_map["COGS"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
cogsTnDF = spark.read.csv(COGSTn_PATH,sep="\u0001",header=True,schema=schemas_map["PlanCOGSTn"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
tiDF = spark.read.csv(TI_PATH,sep="\u0001",header=True,schema=schemas_map["TradeInvestment"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
promoSupportDF = spark.read.csv(PROMOSUPPORT_PATH,sep="\u0001",header=True,schema=schemas_map["PromoSupport"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
promoSupportPromoDF = spark.read.csv(PROMOSUPPORTPROMO_PATH,sep="\u0001",header=True,schema=schemas_map["PromoSupportPromo"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
budgetItemDF = spark.read.csv(BUDGETITEM_PATH,sep="\u0001",header=True,schema=schemas_map["BudgetItem"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
budgetSubItemDF = spark.read.csv(BUDGETSUBITEM_PATH,sep="\u0001",header=True,schema=schemas_map["BudgetSubItem"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
assortmentMatrixDF = spark.read.csv(ASSORTMENTMARTIX_PATH,sep="\u0001",header=True,schema=schemas_map["AssortmentMatrix"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
brandTechDF = spark.read.csv(BRANDTECH_PATH,sep="\u0001",header=True,schema=schemas_map["BrandTech"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
serviceInfoDF = spark.read.csv(SERVICEINFO_PATH,sep="\u0001",header=True,schema=schemas_map["ServiceInfo"])
ratiShopperDF = spark.read.csv(RATISHOPPER_PATH,sep="\u0001",header=True,schema=schemas_map["RATIShopper"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
planPostPromoEffectDF = spark.read.csv(PLANPOSTPROMOEFFECT_PATH,sep="\u0001",header=True,schema=schemas_map["PlanPostPromoEffect"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
discountRangeDF = spark.read.csv(DISCOUNTRANGE_PATH,sep="\u0001",header=True,schema=schemas_map["Range"])
durationRangeDF = spark.read.csv(DURATIONRANGE_PATH,sep="\u0001",header=True,schema=schemas_map["Range"])

filteredPromoDF = spark.read.format("parquet").load(FILTERED_PROMO_PATH)
filteredIncreasePromoDF = spark.read.format("parquet").load(FILTERED_INCREASE_PROMO_PATH)

print(filteredPromoDF.count())
print(filteredIncreasePromoDF.count())

try:
 inputLogMessageDF = spark.read.format("csv").option("delimiter","\u0001").option("header","true").load(INPUT_FILE_LOG_PATH)
 print('Log has been already made')
except:
 inputLogMessageDF = spark.createDataFrame(sc.emptyRDD(), inputLogMessageSchema)
 print('Init log')
  

####*Date transformation*

promoDF = promoDF\
  .withColumn('StartDate', date_add(to_date(promoDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(promoDF.EndDate, 'yyyy-MM-dd'), 1))\
  .withColumn('DispatchesStart', date_add(to_date(promoDF.DispatchesStart, 'yyyy-MM-dd'), 1))\
  .withColumn('DispatchesEnd', date_add(to_date(promoDF.DispatchesEnd, 'yyyy-MM-dd'), 1))

priceListDF = priceListDF\
  .withColumn('StartDate', date_add(to_date(priceListDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(priceListDF.EndDate, 'yyyy-MM-dd'), 1))

baselineDF = baselineDF\
  .withColumn('StartDate', date_add(to_date(baselineDF.StartDate, 'yyyy-MM-dd'), 1))
  
baselineIncreaseDF = baselineIncreaseDF\
  .withColumn('StartDate', date_add(to_date(baselineIncreaseDF.StartDate, 'yyyy-MM-dd'), 1))

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

####*Prepare dataframes for calculation*

filteredPromoDF = filteredPromoDF.dropDuplicates()
filteredIncreasePromoDF = filteredIncreasePromoDF.dropDuplicates()
# print('filtered promo count:', filteredPromoDF.count())

# promoProduct
promoProductCols = promoProductDF.columns
increasePromoProductCols = promoProductPriceIncreaseDF.columns
increasePromoCols = promoPriceIncreaseDF.columns
allCalcPlanPromoProductDF = promoProductDF.where((col('Disabled') == 'False') & (col('TPMmode') != 3))
allCalcPlanPromoProductIdsDF = allCalcPlanPromoProductDF.select(col('Id'))
disabledPromoProductDF = promoProductDF.join(allCalcPlanPromoProductIdsDF, 'Id', 'left_anti').select(promoProductDF['*'])

# print('promoProducts count:', promoProductDF.count())
# print('notDisabledPromoProducts count:', allCalcPlanPromoProductDF.count())
# print('disabledPromoProducts count:', disabledPromoProductDF.count())

# promo
calcPlanPromoDF = promoDF.where(col('Disabled') == 'False')

# all promo
promoCols = promoDF.columns
allCalcPlanPromoDF = promoDF.where((col('Disabled') == 'false') & (col('TPMmode') != 3))
allCalcPlanPromoIdsDF = allCalcPlanPromoDF.select(col('Id'))
disabledPromoDF = promoDF.join(allCalcPlanPromoIdsDF, 'Id', 'left_anti').select(promoDF['*'])

# print('promoDF count:', promoDF.count())
# print('calcPlanPromoDF count:', allCalcPlanPromoDF.count())
# print('disabledPromoDF count:', disabledPromoDF.count())

# priceList
planParamsPriceListDF = priceListDF\
  .where(col('Disabled') == 'False')\
  .where(col('FuturePriceMarker') == 'False')\
  .select(\
           col('StartDate').alias('priceStartDate')
          ,col('EndDate').alias('priceEndDate')
          ,col('ProductId').alias('priceProductId')
          ,col('Price').cast(DecimalType(30,6))
          ,col('ClientTreeId').alias('priceClientTreeId')
         )
         

# future priceList 
planParamsFuturePriceListDF = priceListDF\
  .where(col('Disabled') == 'False')\
  .where(col('FuturePriceMarker') == 'True')\
  .select(\
           col('StartDate').alias('priceStartDate')
          ,col('EndDate').alias('priceEndDate')
          ,col('ProductId').alias('priceProductId')
          ,col('Price').cast(DecimalType(30,6))
          ,col('ClientTreeId').alias('priceClientTreeId')
         )
         
# baseline
planParamsBaselineDF = baselineDF\
  .where(col('Disabled') == 'False')\
  .select(\
           col('ProductId').alias('baselineProductId')
          ,col('DemandCode').alias('baselineDemandCode')
          ,col('StartDate').alias('baselineStartDate')\
          ,col('SellInBaselineQTY').cast(DecimalType(30,6))\
          ,col('SellOutBaselineQTY').cast(DecimalType(30,6))\
         )

# increase baseline
planParamsBaselineIncreaseDF = baselineIncreaseDF\
  .where(col('Disabled') == 'False')\
  .select(\
           col('ProductId').alias('baselineProductId')
          ,col('DemandCode').alias('baselineDemandCode')
          ,col('StartDate').alias('baselineStartDate')\
          ,col('SellInBaselineQTY').cast(DecimalType(30,6))\
          ,col('SellOutBaselineQTY').cast(DecimalType(30,6))\
         )

# shares
planParamsSharesDF = sharesDF.where(col('Disabled') == 'False').drop('Disabled')

# product correction
planParamsCorrectionDF = correctionDF\
  .where(col('TPMmode') != 3)\
  .where(col('Disabled') == '0')\
  .select(\
           upper(col('PromoProductId')).alias('correctionPromoProductId')
          ,col('PlanProductUpliftPercentCorrected').alias('correctionPlanProductUpliftPercentCorrected')
         )

# pi product correction
planParamsIncreaseCorrectionDF = correctionPriceIncreaseDF\
  .where((col('Disabled') == '0'))\
  .select(\
           upper(col('PromoProductPriceIncreaseId')).alias('correctionPromoProductPriceIncreaseId')
          ,col('PlanProductUpliftPercentCorrected').alias('correctionPlanProductUpliftPercentCorrected')
         )

# incremental
planParamsIncrementalDF = incrementalDF\
  .where(col('TPMmode') != 3)\
  .where(col('Disabled') == 'False')\
  .select(\
           col('PromoId').alias('incrementalPromoId')
          ,col('ProductId').alias('incrementalProductId')
          ,col('PlanPromoIncrementalCases')
         )

# support
promoSupportDF = promoSupportDF.where(col('Disabled') == 'False')
promoSupportPromoCols = promoSupportPromoDF.columns
promoSupportPromoCols.remove('#QCCount')
activePromoSupportPromoDF = promoSupportPromoDF.where((col('Disabled') == 'False') & (col('TPMmode') != 3)).select(promoSupportPromoCols)
activePromoSupportPromoIdsDF = activePromoSupportPromoDF.select(col('Id'))
disabledPromoSupportPromoDF = promoSupportPromoDF.join(activePromoSupportPromoIdsDF, 'Id', 'left_anti').select(promoSupportPromoCols)

# btl
btlDF = btlDF.where(col('Disabled') == 'False')
btlPromoDF = btlPromoDF.where(col('Disabled') == 'False')

# print('promoSupportPromoDF count:', promoSupportPromoDF.count())
# print('activePromoSupportPromoDF count:', activePromoSupportPromoDF.count())
# print('disabledPromoSupportPromoDF count:', disabledPromoSupportPromoDF.count())

# AM
assortmentMatrixDF = assortmentMatrixDF.where(col('Disabled') == 'False')

# Product
productDF = productDF.where(col('Disabled') == 'False')

# COGS, TI, BrandTech
brandTechDF = brandTechDF.where(col('Disabled') == 'false')

tiDF = tiDF.where(col('Disabled') == False)
cogsDF = cogsDF.where(col('Disabled') == False)
cogsTnDF = cogsTnDF.where(col('Disabled') == False)

# RA TI Shopper
ratiShopperDF = ratiShopperDF.where(col('Disabled') == False)

# PPE
planPostPromoEffectDF = planPostPromoEffectDF.where(col('Disabled') == False)
planPostPromoEffectDF = planPostPromoEffectDF\
  .join(brandTechDF, planPostPromoEffectDF.BrandTechId == brandTechDF.Id, 'inner')\
  .select(\
           planPostPromoEffectDF['*']
          ,col('BrandTech_code').alias('BrandTech_code')
          )
planPostPromoEffectDF = planPostPromoEffectDF\
  .join(discountRangeDF, planPostPromoEffectDF.DiscountRangeId == discountRangeDF.Id, 'inner')\
  .select(\
           planPostPromoEffectDF['*']
          ,col('MinValue').alias('MinDiscount')
          ,col('MaxValue').alias('MaxDiscount')
          )
planPostPromoEffectDF = planPostPromoEffectDF\
  .join(durationRangeDF, planPostPromoEffectDF.DurationRangeId == durationRangeDF.Id, 'inner')\
  .select(\
           planPostPromoEffectDF['*']
          ,col('MinValue').alias('MinDuration')
          ,col('MaxValue').alias('MaxDuration')
          )

#status list for plan parameters recalculation
planParametersStatuses = ['DraftPublished','OnApproval','Approved','Planned']

#notCheckPromoStatusList = ['Draft','Cancelled','Deleted','Closed']
notCheckPromoStatusList = ['Cancelled','Deleted']

activeClientTreeDF = clientTreeDF.where(col('EndDate').isNull())
activeClientTreeList = activeClientTreeDF.collect()

@udf
def getDemandCode(objectId):
  c = [x for x in activeClientTreeList if x.ObjectId == objectId]
  if len(c) == 0:
    return ''
  while (len(c) != 0) & (c[0].Type != 'root') & ((c[0].DemandCode == '') | (c[0].DemandCode is None)):
    c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
    if len(c) == 0: 
      break
  if len(c) == 0:
    return ''
  else:
    return c[0].DemandCode

activeClientTreeList

lightPromoDF = promoDF\
  .where(col('Disabled') == 'False')\
  .where(col('TPMmode') != 3)\
  .select(\
           col('Id').alias('promoIdCol')
          ,col('Number').alias('promoNumber')
          ,col('BrandTechId').alias('promoBrandTechId')
          ,col('PromoDuration').alias('promoPromoDuration')
          ,col('MarsMechanicDiscount').alias('promoMarsMechanicDiscount')
          ,col('PromoStatusId').alias('promoStatusId')
          ,col('StartDate').alias('promoStartDate')
          ,col('EndDate').alias('promoEndDate')
          ,col('DispatchesStart').alias('promoDispatchesStart')
          ,col('ClientTreeKeyId').alias('promoClientTreeKeyId')
          ,col('ClientTreeId').alias('promoClientTreeId')
          ,col('IsOnInvoice').alias('promoIsOnInvoice')
          ,col('InOut').alias('promoInOut')
         )

lightPromoDF = lightPromoDF\
  .withColumn('promoDemandCode', lit(getDemandCode(col('promoClientTreeId'))))

allCalcPlanPromoProductDF = allCalcPlanPromoProductDF\
  .join(lightPromoDF, lightPromoDF.promoIdCol == allCalcPlanPromoProductDF.PromoId, 'left')\
  .join(promoStatusDF, promoStatusDF.Id == lightPromoDF.promoStatusId, 'left')\
  .select(\
           allCalcPlanPromoProductDF['*']
          ,lightPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )

calcPlanPromoProductDF = allCalcPlanPromoProductDF\
  .join(filteredPromoDF, filteredPromoDF.PromoId == allCalcPlanPromoProductDF.PromoId, 'inner')\
  .select(allCalcPlanPromoProductDF['*'])\
  .where(col('promoStatusSystemName').isin(*planParametersStatuses))

calcPlanPromoProductIdsDF = calcPlanPromoProductDF.select(col('Id'))
notCalcPlanPromoProductDF = allCalcPlanPromoProductDF.join(calcPlanPromoProductIdsDF, 'Id', 'left_anti').select(allCalcPlanPromoProductDF['*'])

calcPlanPromoProductDF = calcPlanPromoProductDF\
  .join(productDF, productDF.Id == calcPlanPromoProductDF.ProductId, 'left')\
  .select(\
           calcPlanPromoProductDF['*']
          ,productDF.UOM_PC2Case
          ,productDF.CaseVolume
          ,productDF.Size
         )

calcPlanPromoDF = calcPlanPromoDF\
  .join(promoStatusDF, promoStatusDF.Id == calcPlanPromoDF.PromoStatusId, 'left')\
  .select(\
           calcPlanPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )

calcPlanPromoDF = calcPlanPromoDF\
  .join(filteredPromoDF, filteredPromoDF.PromoId == calcPlanPromoDF.Id, 'inner')\
  .select(calcPlanPromoDF['*'])\
  .where(col('promoStatusSystemName').isin(*planParametersStatuses))

#increase
calcPromoPriceIncreaseDF = promoPriceIncreaseDF\
  .join(filteredIncreasePromoDF, promoPriceIncreaseDF.Id == filteredIncreasePromoDF.PromoId, 'left')\
  .join(calcPlanPromoDF, promoPriceIncreaseDF.Id == calcPlanPromoDF.Id, 'left')\
  .select(\
           promoPriceIncreaseDF['*']
          ,calcPlanPromoDF.promoStatusSystemName.alias('promoStatusSystemName')
         )

calcIncreasePlanPromoProductDF = promoProductPriceIncreaseDF\
  .join(promoProductDF, promoProductPriceIncreaseDF.PromoProductId == promoProductDF.Id, 'inner')\
  .join(calcPromoPriceIncreaseDF, calcPromoPriceIncreaseDF.Id == promoProductDF.PromoId, 'inner')\
  .join(lightPromoDF, lightPromoDF.promoIdCol == calcPromoPriceIncreaseDF.Id, 'left')\
  .where(col('promoStatusSystemName').isin(*planParametersStatuses))\
  .select(promoProductPriceIncreaseDF['*'],lightPromoDF['*'],promoProductDF.ProductId,promoProductDF.PlanProductPostPromoEffectW1,promoProductDF.PlanProductPostPromoEffectW2)
  
calcIncreasePlanPromoProductDF.show()

calcPromoPriceIncreaseDF = calcPromoPriceIncreaseDF\
  .where(col('promoStatusSystemName').isin(*planParametersStatuses))\
  .where(col('Disabled') == 'False')\
  .where(col('InOut') == 'False')\
  .drop('promoStatusSystemName')

calcIncreasePlanPromoProductDF = calcIncreasePlanPromoProductDF\
  .join(productDF, productDF.Id == calcIncreasePlanPromoProductDF.ProductId, 'left')\
  .select(\
           calcIncreasePlanPromoProductDF['*']
          ,productDF.UOM_PC2Case
          ,productDF.CaseVolume
          ,productDF.Size
         )
allCalcPlanPromoProductDF = allCalcPlanPromoProductDF\
  .join(lightPromoDF, lightPromoDF.promoIdCol == allCalcPlanPromoProductDF.PromoId, 'left')\
  .join(promoStatusDF, promoStatusDF.Id == lightPromoDF.promoStatusId, 'left')\
  .select(\
           allCalcPlanPromoProductDF['*']
          ,lightPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )

notInOutCalcPlanPromoProductDF = calcPlanPromoProductDF.where(col('promoInOut') == False)
inOutCalcPlanPromoProductDF = calcPlanPromoProductDF.where(col('promoInOut') == True)

notInOutCalcPlanPromoDF = calcPlanPromoDF.where(col('InOut') == False)
inOutCalcPlanPromoDF = calcPlanPromoDF.where(col('InOut') == True)

import SET_PROMO_PRODUCT as set_promo_product
notInOutCalcPlanPromoProductDF = set_promo_product.run(notInOutCalcPlanPromoDF,notInOutCalcPlanPromoProductDF,promoProductTreeDF,productDF,productTreeDF,assortmentMatrixDF,allProductDF,allProduct01DF,schema)

promoWithChangedProductDF = notInOutCalcPlanPromoProductDF\
  .where(notInOutCalcPlanPromoProductDF.Action.isin('Added', 'Deleted')).select(notInOutCalcPlanPromoProductDF.promoNumber)

addedPromoProductDF = notInOutCalcPlanPromoProductDF\
  .where(col('Action') == 'Added')\
  .groupBy('PromoId')\
  .agg(concat_ws(';', collect_list(col('ZREP'))).alias('AddedProductIds'))

deletedPromoProductDF = notInOutCalcPlanPromoProductDF\
  .where(col('Action') == 'Deleted')\
  .groupBy('PromoId')\
  .agg(concat_ws(';', collect_list(col('ZREP'))).alias('ExcludedProductIds'))

newProductChangeIncidentDF = addedPromoProductDF\
  .join(deletedPromoProductDF, 'PromoId', 'outer')

print(promoWithChangedProductDF.collect())

unlinkedPromoProductDF = notInOutCalcPlanPromoProductDF\
  .where(notInOutCalcPlanPromoProductDF.Disabled == True)\
  .where(col('TPMmode') != 3)

notInOutCalcPlanPromoProductDF = notInOutCalcPlanPromoProductDF\
  .where(notInOutCalcPlanPromoProductDF.Disabled == False)\
  .where(col('TPMmode') != 3)\
  .drop('Number', 'ResultFilteredProductId', 'ResultFilteredZREP', 'pId')

inOutCalcPlanPromoProductDF = inOutCalcPlanPromoProductDF.withColumn('Action', lit(None))

serviceInfoDF = serviceInfoDF\
  .withColumn('Value', when(col('Name') == 'PROMO_NUMBERS_FOR_REAPPROVAL', ';'.join([str(elem.promoNumber) for elem in promoWithChangedProductDF.collect()]))\
              .otherwise(col('Value')))

tempDF = calcPlanPromoProductDF\
  .select(\
           calcPlanPromoProductDF.promoIdCol.alias('_promoIdCol')
          ,calcPlanPromoProductDF.promoNumber.alias('_promoNumber')
          ,calcPlanPromoProductDF.promoBrandTechId.alias('_promoBrandTechId')
          ,calcPlanPromoProductDF.promoPromoDuration.alias('_promoPromoDuration')
          ,calcPlanPromoProductDF.promoMarsMechanicDiscount.alias('_promoMarsMechanicDiscount')
          ,calcPlanPromoProductDF.promoStatusId.alias('_promoStatusId')
          ,calcPlanPromoProductDF.promoStartDate.alias('_promoStartDate')
          ,calcPlanPromoProductDF.promoEndDate.alias('_promoEndDate')
          ,calcPlanPromoProductDF.promoDispatchesStart.alias('_promoDispatchesStart')
          ,calcPlanPromoProductDF.promoClientTreeKeyId.alias('_promoClientTreeKeyId')
          ,calcPlanPromoProductDF.promoClientTreeId.alias('_promoClientTreeId')
          ,calcPlanPromoProductDF.promoIsOnInvoice.alias('_promoIsOnInvoice')
          ,calcPlanPromoProductDF.promoInOut.alias('_promoInOut')
          ,calcPlanPromoProductDF.promoDemandCode.alias('_promoDemandCode')
          ,calcPlanPromoProductDF.promoStatusSystemName.alias('_promoStatusSystemName')
         )\
  .dropDuplicates()

cols = notInOutCalcPlanPromoProductDF.columns

notInOutCalcPlanPromoProductDF = notInOutCalcPlanPromoProductDF\
  .join(tempDF, tempDF._promoNumber == notInOutCalcPlanPromoProductDF.promoNumber, 'left')\
  .withColumn('promoIdCol', when(notInOutCalcPlanPromoProductDF.promoIdCol.isNull(),tempDF._promoIdCol)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoIdCol))\
  .withColumn('promoBrandTechId', when(notInOutCalcPlanPromoProductDF.promoBrandTechId.isNull(),tempDF._promoBrandTechId)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoBrandTechId))\
  .withColumn('promoPromoDuration', when(notInOutCalcPlanPromoProductDF.promoPromoDuration.isNull(),tempDF._promoPromoDuration)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoPromoDuration))\
  .withColumn('promoMarsMechanicDiscount', when(notInOutCalcPlanPromoProductDF.promoMarsMechanicDiscount.isNull(),tempDF._promoMarsMechanicDiscount)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoMarsMechanicDiscount))\
  .withColumn('promoStatusId', when(notInOutCalcPlanPromoProductDF.promoStatusId.isNull(),tempDF._promoStatusId)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoStatusId))\
  .withColumn('promoStartDate', when(notInOutCalcPlanPromoProductDF.promoStartDate.isNull(),tempDF._promoStartDate)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoStartDate))\
  .withColumn('promoEndDate', when(notInOutCalcPlanPromoProductDF.promoEndDate.isNull(),tempDF._promoEndDate)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoEndDate))\
  .withColumn('promoDispatchesStart', when(notInOutCalcPlanPromoProductDF.promoDispatchesStart.isNull(),tempDF._promoDispatchesStart)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoDispatchesStart))\
  .withColumn('promoClientTreeKeyId', when(notInOutCalcPlanPromoProductDF.promoClientTreeKeyId.isNull(),tempDF._promoClientTreeKeyId)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoClientTreeKeyId))\
  .withColumn('promoClientTreeId', when(notInOutCalcPlanPromoProductDF.promoClientTreeId.isNull(),tempDF._promoClientTreeId)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoClientTreeId))\
  .withColumn('promoIsOnInvoice', when(notInOutCalcPlanPromoProductDF.promoIsOnInvoice.isNull(),tempDF._promoIsOnInvoice)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoIsOnInvoice))\
  .withColumn('promoInOut', when(notInOutCalcPlanPromoProductDF.promoInOut.isNull(),tempDF._promoInOut)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoInOut))\
  .withColumn('promoDemandCode', when(notInOutCalcPlanPromoProductDF.promoDemandCode.isNull(),tempDF._promoDemandCode)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoDemandCode))\
  .withColumn('promoStatusSystemName', when(notInOutCalcPlanPromoProductDF.promoStatusSystemName.isNull(),tempDF._promoStatusSystemName)\
          .otherwise(notInOutCalcPlanPromoProductDF.promoStatusSystemName))\
  .select(cols)

tempProductDF = productDF\
  .select(\
           productDF.Id.alias('_Id')
          ,productDF.ZREP.alias('_ZREP')
          ,productDF.EAN_Case.alias('_EAN_Case')
          ,productDF.ProductEN.alias('_ProductEN')
          ,productDF.EAN_PC.alias('_EAN_PC')
          ,productDF.UOM_PC2Case.alias('_UOM_PC2Case')
          ,productDF.Size.alias('_Size')
         )

notInOutCalcPlanPromoProductDF = notInOutCalcPlanPromoProductDF\
  .join(tempProductDF, tempProductDF._Id == notInOutCalcPlanPromoProductDF.ProductId, 'left')\
  .withColumn('ZREP', when(notInOutCalcPlanPromoProductDF.ZREP.isNull(),tempProductDF._ZREP)\
          .otherwise(notInOutCalcPlanPromoProductDF.ZREP))\
  .withColumn('EAN_Case', when(notInOutCalcPlanPromoProductDF.EAN_Case.isNull(),tempProductDF._EAN_Case)\
          .otherwise(notInOutCalcPlanPromoProductDF.EAN_Case))\
  .withColumn('ProductEN', when(notInOutCalcPlanPromoProductDF.ProductEN.isNull(),tempProductDF._ProductEN)\
          .otherwise(notInOutCalcPlanPromoProductDF.ProductEN))\
  .withColumn('EAN_PC', when(notInOutCalcPlanPromoProductDF.EAN_PC.isNull(),tempProductDF._EAN_PC)\
          .otherwise(notInOutCalcPlanPromoProductDF.EAN_PC))\
  .withColumn('UOM_PC2Case', when(notInOutCalcPlanPromoProductDF.UOM_PC2Case.isNull(),tempProductDF._UOM_PC2Case)\
          .otherwise(notInOutCalcPlanPromoProductDF.UOM_PC2Case))\
  .withColumn('Size', when(notInOutCalcPlanPromoProductDF.Size.isNull(),tempProductDF._Size)\
          .otherwise(notInOutCalcPlanPromoProductDF.Size))\
  .select(cols)

calcPlanPromoProductDF = notInOutCalcPlanPromoProductDF.union(inOutCalcPlanPromoProductDF)
calcPlanPromoDF = notInOutCalcPlanPromoDF.union(inOutCalcPlanPromoDF)

allCalcPlanPromoDF = allCalcPlanPromoDF\
  .join(promoStatusDF, promoStatusDF.Id == allCalcPlanPromoDF.PromoStatusId, 'left')\
  .select(\
           allCalcPlanPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )

print(calcPlanPromoProductDF.schema)

planPostPromoEffectDF.show()


import PLAN_PRODUCT_PARAMS_CALCULATION_PROCESS as plan_product_params_calculation_process
calcPlanPromoProductDF,calcPlanPromoDF,allCalcPlanPromoDF,logPromoProductDF = plan_product_params_calculation_process.run(calcPlanPromoProductDF,planParamsPriceListDF,planParamsBaselineDF,calcPlanPromoDF,allCalcPlanPromoDF,planParamsSharesDF,datesDF,planParamsCorrectionDF,planParamsIncrementalDF,planParametersStatuses,promoProductCols,planPostPromoEffectDF)

####*Promo support calculation*

@udf
def isStatusInList(list):
  c = [x for x in list if x in ['DraftPublished','OnApproval','Approved','Planned','Started','Finished']]
  return len(c) > 0

@udf
def isStatusInFinishedClosedList(list):
  if any(x in list for x in ['DraftPublished','OnApproval','Approved','Planned','Started']):
    return False
  else:
    return True

calcPlanSupportPromoDF = allCalcPlanPromoDF\
  .where(~col('promoStatusSystemName').isin(*notCheckPromoStatusList))

calcPlanSupportPromoIdsDF = calcPlanSupportPromoDF.select(col('Id'))
notCalcPlanSupportPromoDF = allCalcPlanPromoDF.join(calcPlanSupportPromoIdsDF, 'Id', 'left_anti').select(allCalcPlanPromoDF['*'])

allPromoSupportDF = allCalcPlanPromoDF\
  .join(activePromoSupportPromoDF, activePromoSupportPromoDF.PromoId == allCalcPlanPromoDF.Id, 'inner')\
  .join(promoSupportDF, promoSupportDF.Id == activePromoSupportPromoDF.PromoSupportId, 'inner')\
  .select(\
           promoSupportDF.Id.alias('psId')
          ,promoSupportDF.BudgetSubItemId
          ,promoSupportDF.Number.alias('promoSupportNumber')
          ,promoSupportDF.PlanCostTE.cast(DecimalType(30,6))
          ,promoSupportDF.PlanProdCost.cast(DecimalType(30,6))
          ,allCalcPlanPromoDF.promoStatusSystemName
        )\
  .groupBy('psId','BudgetSubItemId','promoSupportNumber','PlanCostTE','PlanProdCost')\
  .agg(collect_list(col('promoStatusSystemName')).alias('promoStatuses'))\
  .withColumn('isInList', isStatusInList(col('promoStatuses')).cast(BooleanType()))\
  .withColumn('onlyFinishedClosed', isStatusInFinishedClosedList(col('promoStatuses')).cast(BooleanType()))\
  .where(col('isInList') == True)\
  .drop('promoStatuses','isInList')

allBtlDF = allCalcPlanPromoDF\
  .join(btlPromoDF, btlPromoDF.PromoId == allCalcPlanPromoDF.Id, 'inner')\
  .join(btlDF, btlDF.Id == btlPromoDF.BTLId, 'inner')\
  .select(\
           btlDF.Id.alias('bId')
          ,btlDF.Number.alias('btlNumber')
          ,btlDF.PlanBTLTotal.cast(DecimalType(30,6))\
          ,allCalcPlanPromoDF.promoStatusSystemName
         )\
  .groupBy('bId','btlNumber','PlanBTLTotal')\
  .agg(collect_list(col('promoStatusSystemName')).alias('promoStatuses'))\
  .withColumn('isInList', isStatusInList(col('promoStatuses')).cast(BooleanType()))\
  .withColumn('onlyFinishedClosed', isStatusInFinishedClosedList(col('promoStatuses')).cast(BooleanType()))\
  .where(col('isInList') == True)\
  .drop('promoStatuses','isInList')


import PLAN_SUPPORT_PARAMS_CALCULATION_PROCESS as plan_support_params_calculation_process
calcPlanSupportPromoDF,allPromoSupportPromoDF= plan_support_params_calculation_process.run(allPromoSupportDF,activePromoSupportPromoDF,calcPlanSupportPromoDF,allBtlDF,budgetItemDF,budgetSubItemDF,promoSupportPromoCols,btlPromoDF)

####*Plan promo parameters calculation*

allCalcPlanPromoDF = calcPlanSupportPromoDF.union(notCalcPlanSupportPromoDF)

calcPlanPromoDF = allCalcPlanPromoDF\
  .join(brandTechDF, brandTechDF.Id == allCalcPlanPromoDF.BrandTechId, 'left')\
  .select(\
           allCalcPlanPromoDF['*']
          ,brandTechDF.BrandsegTechsub.alias('promoBrandTechName')
         )\
  .where(col('promoStatusSystemName').isin(*planParametersStatuses))

notCalcPlanPromoDF = allCalcPlanPromoDF\
  .where(~col('promoStatusSystemName').isin(*planParametersStatuses))


import PLAN_PROMO_PARAMS_CALCULATION_PROCESS as plan_promo_params_calculation_process
calcPlanPromoDF,logCOGS,logTI= plan_promo_params_calculation_process.run(clientTreeDF,cogsDF,brandTechDF,cogsTnDF,tiDF,ratiShopperDF,calcPlanPromoDF,promoDF)




calcPromoPriceIncreaseDF = calcPromoPriceIncreaseDF\
  .join(calcPlanPromoDF, calcPromoPriceIncreaseDF.Id == calcPlanPromoDF.Id, 'inner')\
  .join(lightPromoDF, lightPromoDF.promoIdCol == calcPromoPriceIncreaseDF.Id)\
  .select(\
           calcPromoPriceIncreaseDF['*']
          ,calcPlanPromoDF.Number
          ,calcPlanPromoDF.ClientTreeKeyId
          ,calcPlanPromoDF.ClientTreeId
          ,calcPlanPromoDF.StartDate
          ,calcPlanPromoDF.DispatchesStart
          ,calcPlanPromoDF.BudgetYear
          ,calcPlanPromoDF.PlanPromoTIMarketing
          ,calcPlanPromoDF.PlanPromoBranding
          ,calcPlanPromoDF.PlanPromoBTL
          ,calcPlanPromoDF.PlanPromoCostProduction
          ,calcPlanPromoDF.PlanCOGSTn
          ,calcPlanPromoDF.PlanCOGSPercent
          ,calcPlanPromoDF.promoBrandTechName
          ,calcPlanPromoDF.promoStatusSystemName
          ,calcPlanPromoDF.InOut.alias('promoInOut')
          ,calcPlanPromoDF.NeedRecountUpliftPI.alias('promoNeedRecountUpliftPI')
          ,calcPlanPromoDF.MarsMechanicDiscount
          ,calcPlanPromoDF.PlanTIBasePercent
          ,calcPlanPromoDF.RATIShopperPercent
          ,calcPlanPromoDF.LastApprovedDate
         )

print(calcPlanPromoDF.schema)

print(calcPromoPriceIncreaseDF.schema)


import PI_PLAN_PRODUCT_PARAMS_CALCULATION_PROCESS as pi_plan_product_params_calculation_process
calcIncreasePlanPromoProductDF,calcPromoPriceIncreaseDF,logIncreasePromoProductDF = pi_plan_product_params_calculation_process.run(calcIncreasePlanPromoProductDF,planParamsPriceListDF,planParamsFuturePriceListDF,planParamsBaselineDF,planParamsBaselineIncreaseDF,calcPromoPriceIncreaseDF,calcPromoPriceIncreaseDF,planParamsSharesDF,datesDF,planParamsIncreaseCorrectionDF,planParamsIncrementalDF,planParametersStatuses,increasePromoProductCols,clientTreeDF,tiDF,ratiShopperDF,increasePromoCols,brandTechDF)

####*Result*

# promoproduct
unlinkedPromoProductDF = unlinkedPromoProductDF.select(promoProductCols)
notCalcPlanPromoProductDF = notCalcPlanPromoProductDF.select(promoProductCols)

newPromoProductDF = calcPlanPromoProductDF.where(col('Id').isNull()).drop('#QCCount')
calcPlanPromoProductDF = calcPlanPromoProductDF.where(~col('Id').isNull())

calcPlanPromoProductDF = calcPlanPromoProductDF.union(unlinkedPromoProductDF)
allCalcPlanPromoProductDF = calcPlanPromoProductDF.union(notCalcPlanPromoProductDF)

resultPromoProductDF = allCalcPlanPromoProductDF.union(disabledPromoProductDF)
resultPromoProductDF = resultPromoProductDF.drop('#QCCount')
# ---

# promo
calcPlanPromoDF = calcPlanPromoDF.select(promoCols)
notCalcPlanPromoDF = notCalcPlanPromoDF.select(promoCols)
allCalcPlanPromoDF = calcPlanPromoDF.union(notCalcPlanPromoDF)

resultPromoDF = allCalcPlanPromoDF.union(disabledPromoDF)
resultPromoDF = resultPromoDF.drop('#QCCount')
# ---


# promosuportpromo
resultPromoSupportPromoDF = allPromoSupportPromoDF.union(disabledPromoSupportPromoDF)
# ---

# serviceInfo
serviceInfoDF = serviceInfoDF.drop('#QCCount')
# ---

# print(newPromoProductDF.count())
# print(resultPromoProductDF.count())
# print(resultPromoDF.count())
# print(resultPromoSupportPromoDF.count())

try:
   subprocess.call(["hadoop", "fs", "-rm", "-r",NEW_PROMOPRODUCT_PATH])
   subprocess.call(["hadoop", "fs", "-rm", "-r",PLAN_PROMOPRODUCT_PARAMETERS_CALCULATION_RESULT_PATH])
   subprocess.call(["hadoop", "fs", "-rm", "-r",PLAN_PROMO_PARAMETERS_CALCULATION_RESULT_PATH])
   subprocess.call(["hadoop", "fs", "-rm", "-r",PLAN_PROMOSUPPORTPROMO_PARAMETERS_CALCULATION_RESULT_PATH])
   subprocess.call(["hadoop", "fs", "-rm", "-r",SERVICEINFO_RESULT_PATH])
   subprocess.call(["hadoop", "fs", "-rm", "-r",NEW_PRODUCTCHANGEINCIDENTS_PATH])
except Exception as e:
   print(e)

print('PLAN_PARAMETERS_CALCULATION_DONE')

# newPromoProductDF.coalesce(6).write.mode("overwrite").parquet(NEW_PROMOPRODUCT_PATH)
newPromoProductDF\
.repartition(1)\
.write.csv(NEW_PROMOPRODUCT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultPromoDF = resultPromoDF.persist()

resultPromoProductDF.coalesce(6).write.mode("overwrite").parquet(PLAN_PROMOPRODUCT_PARAMETERS_CALCULATION_RESULT_PATH)
resultPromoDF.write.mode("overwrite").parquet(PLAN_PROMO_PARAMETERS_CALCULATION_RESULT_PATH)
resultPromoSupportPromoDF.coalesce(6).write.mode("overwrite").parquet(PLAN_PROMOSUPPORTPROMO_PARAMETERS_CALCULATION_RESULT_PATH)

# serviceInfoDF.write.mode("overwrite").parquet(SERVICEINFO_RESULT_PATH)

# newProductChangeIncidentDF.write.mode("overwrite").parquet(NEW_PRODUCTCHANGEINCIDENTS_PATH)
# save increase promo + products


resultIncreasePromoDF = calcPromoPriceIncreaseDF.drop('#QCCount')
resultIncreasePromoProductDF = calcIncreasePlanPromoProductDF.drop('#QCCount')

resultIncreasePromoDF = resultIncreasePromoDF.persist()

resultIncreasePromoProductDF.fillna(False,subset=['Disabled',
'AverageMarker'])\
.withColumn("Disabled",col("Disabled").cast(IntegerType()))\
.withColumn("AverageMarker",col("AverageMarker").cast(IntegerType()))\
.repartition(1)\
.write.csv(PLAN_PROMOPRODUCT_PRICEINCREASE_PARAMETERS_CALCULATION_RESULT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultIncreasePromoDF.fillna(False,subset=['Disabled'])\
.repartition(1)\
.write.csv(PLAN_PROMO_PRICEINCREASE_PARAMETERS_CALCULATION_RESULT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss",
escape="",
quote="",
)

serviceInfoDF\
.repartition(1)\
.write.csv(SERVICEINFO_RESULT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

newProductChangeIncidentDF\
.repartition(1)\
.write.csv(NEW_PRODUCTCHANGEINCIDENTS_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

####*Logging*


logPromoProductDF = logPromoProductDF\
  .join(logCOGS, 'promoNumber', 'full')

logPromoProductDF = logPromoProductDF\
  .join(logTI, 'promoNumber', 'full')

titleMessage = '[INFO]: PLAN PARAMETERS CALCULATION'
titleLogMessageDF = spark.createDataFrame([(titleMessage,)], inputLogMessageSchema)

logMessageDF = logPromoProductDF\
  .select(concat(lit('[WARNING]: Promo №:'), col('promoNumber'),\
                 when(col('nullPriceMessage').isNull(), '').otherwise(concat(lit(' There\'re no price for ZREP: '), col('nullPriceMessage'))),\
                 when(col('zeroPriceMessage').isNull(), '').otherwise(concat(lit('.\r\n There\'re zero price for ZREP: '), col('zeroPriceMessage'))),\
                 when(col('nullShareMessage').isNull(), '').otherwise(concat(lit('.\r\n There\'re no share for ZREP: '), col('nullShareMessage'))),\
                 when(col('zeroShareMessage').isNull(), '').otherwise(concat(lit('.\r\n There\'re zero share for ZREP: '), col('zeroShareMessage'))),\
                 when(col('zeroBaselineQtyMessage').isNull(), '').otherwise(concat(lit('.\r\n There\'re zero PlanBaselineCaseQty for ZREP: '), col('zeroBaselineQtyMessage'))),\
                 when(col('COGSMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('COGSMessage'))),\
                 when(col('TIMessage').isNull(), '').otherwise(concat(lit('.\r\n '), col('TIMessage'))),\
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

print(f'PLAN_PARAMETERS_CALCULATION_ALL_WRITES_DONE')

# promoProductDF.orderBy('Id').toPandas().to_csv(OUTPUT_SOURCE_PROMOPRODUCT_PATH, encoding='utf-8',index=False,sep = '\u0001')
# resultPromoProductDF.orderBy('Id').toPandas().to_csv(OUTPUT_PROMOPRODUCT_PATH, encoding='utf-8',index=False,sep = '\u0001')

# promoDF.orderBy('Id').toPandas().to_csv(OUTPUT_SOURCE_PROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')
# resultPromoDF.orderBy('Id').toPandas().to_csv(OUTPUT_PROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')

# promoSupportPromoDF.orderBy('Id').toPandas().to_csv(OUTPUT_SOURCE_PROMOSUPPORTPROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')
# resultPromoSupportPromoDF.orderBy('Id').toPandas().to_csv(OUTPUT_PROMOSUPPORTPROMO_PATH, encoding='utf-8',index=False,sep = '\u0001')