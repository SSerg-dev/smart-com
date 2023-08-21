####Notebook "PROMO_FILTERING_FOR_RECALCULATION". 
####*Get promoes filtering by changes incidents*.
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
 '"/JUPITER/RAW/#MAINTENANCE/2023-07-13_manual__2023-07-13T14%3A31%3A58.337289%2B00%3A00_", '
 '"ProcessDate": "2023-07-13", "Schema": "Jupiter", "HandlerId": '
 '"a822371a-9f2a-4925-833e-9562a4e496ee"}']
 
 sc.addPyFile("hdfs:///SRC/SHARED/EXTRACT_SETTING.py")
 sc.addPyFile("hdfs:///SRC/SHARED/SUPPORT_FUNCTIONS.py")

spark = SparkSession.builder.appName('Jupiter - PySpark').getOrCreate()
sc = SparkContext.getOrCreate();

import EXTRACT_SETTING as es

SETTING_RAW_DIR = es.SETTING_RAW_DIR
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
PROMOSTATUS_PATH = DIRECTORY + 'JUPITER/PromoStatus'
PRODUCT_PATH = DIRECTORY + 'JUPITER/Product'
BRANDTECH_PATH = DIRECTORY + 'JUPITER/BrandTech'
PROMOPRODUCT_PATH = DIRECTORY + 'JUPITER/PromoProduct'
CLIENTHIERARCHY_PATH = DIRECTORY + 'JUPITER/ClientTreeHierarchyView'
PROMOPRODUCTTREE_PATH = DIRECTORY + 'JUPITER/PromoProductTree'
CHANGESINCIDENTS_PATH = DIRECTORY + 'JUPITER/ChangesIncident'
PRODUCTCHANGEINCIDENTS_PATH = DIRECTORY + 'JUPITER/ProductChangeIncident'

DATESDIM_PATH = DIRECTORY + 'UNIVERSALCATALOG/MARS_UNIVERSAL_CALENDAR.csv'

ASSORTMENTMATRIX_PATH = DIRECTORY + 'JUPITER/AssortmentMatrix'
PRICELIST_PATH = DIRECTORY + 'JUPITER/PriceList'
BASELINE_PATH = DIRECTORY + 'JUPITER/BaseLine'
INCREASEBASELINE_PATH = DIRECTORY + 'JUPITER/IncreaseBaseLine'
SHARES_PATH = DIRECTORY + 'JUPITER/ClientTreeBrandTech'
CLIENTTREE_PATH = DIRECTORY + 'JUPITER/ClientTree'
PRODUCTTREE_PATH = DIRECTORY + 'JUPITER/ProductTree'
INCREMENTAL_PATH = DIRECTORY + 'JUPITER/IncrementalPromo'
COGS_PATH = DIRECTORY + 'JUPITER/COGS'
COGSTN_PATH = DIRECTORY + 'JUPITER/PlanCOGSTn'
TI_PATH = DIRECTORY + 'JUPITER/TradeInvestment'
CORRECTION_PATH = DIRECTORY + 'JUPITER/PromoProductsCorrection'
PLANPOSTPROMOEFFECT_PATH = DIRECTORY + 'JUPITER/PlanPostPromoEffect'

BLOCKEDPROMO_OUTPUT_PATH = es.SETTING_PROCESS_DIR + '/BlockedPromo/BlockedPromo.parquet'
BLOCKEDPROMO_OUTPUT_PATH_CSV = es.SETTING_PROCESS_DIR + '/BlockedPromo/BlockedPromo.CSV'

BLOCKEDINCREASEPROMO_OUTPUT_PATH = es.SETTING_PROCESS_DIR + '/BlockedPromo/BlockedIncreasePromo.parquet'
BLOCKEDINCREASEPROMO_OUTPUT_PATH_CSV = es.SETTING_PROCESS_DIR + '/BlockedPromo/BlockedIncreasePromo.CSV'


CHANGESINCIDENTS_OUTPUT_PATH = es.SETTING_OUTPUT_DIR + '/ChangesIncident/ChangesIncident.CSV'
PRODUCTCHANGEINCIDENTS_OUTPUT_PATH = es.SETTING_OUTPUT_DIR + '/ProductChangeIncident/ProductChangeIncident.CSV'

INPUT_FILE_LOG_PATH = es.SETTING_PROCESS_DIR + '/Logs/' + handlerId + '.csv'
# OUTPUT_LOG_PATH =  es.SETTING_PROCESS_DIR + '/Logs/'
# OUTPUT_FILE_LOG_PATH = OUTPUT_LOG_PATH + handlerId + '.csv'
# OUTPUT_TEMP_FILE_LOG_PATH = OUTPUT_LOG_PATH + handlerId + 'temp.csv'

SCHEMAS_DIR=SETTING_RAW_DIR + '/SCHEMAS/'
schemas_map = sp.getSchemasMap(SCHEMAS_DIR)

promoDF = spark.read.csv(PROMO_PATH,sep="\u0001",header=True,schema=schemas_map["Promo"])\
.withColumn("Disabled",col("Disabled").cast(BooleanType()))\
.withColumn("InOut",col("InOut").cast(BooleanType()))
promoStatusDF = spark.read.csv(PROMOSTATUS_PATH,sep="\u0001",header=True,schema=schemas_map["PromoStatus"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
productDF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["Product"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
brandTechDF = spark.read.csv(BRANDTECH_PATH,sep="\u0001",header=True,schema=schemas_map["BrandTech"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
promoProductDF = spark.read.csv(PROMOPRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProduct"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
clientHierarchyDF = spark.read.csv(CLIENTHIERARCHY_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTreeHierarchyView"])
promoProductTreeDF = spark.read.csv(PROMOPRODUCTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductTree"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
changesIncidentsDF = spark.read.csv(CHANGESINCIDENTS_PATH,sep="\u0001",header=True,schema=schemas_map["ChangesIncident"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
productChangeIncidentsDF = spark.read.csv(PRODUCTCHANGEINCIDENTS_PATH,sep="\u0001",header=True,schema=schemas_map["ProductChangeIncident"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
assortmentMatrixDF = spark.read.csv(ASSORTMENTMATRIX_PATH,sep="\u0001",header=True,schema=schemas_map["AssortmentMatrix"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
priceListDF = spark.read.csv(PRICELIST_PATH,sep="\u0001",header=True,schema=schemas_map["PriceList"]).withColumn("Disabled",col("Disabled").cast(BooleanType())).withColumn("FuturePriceMarker",col("FuturePriceMarker").cast(BooleanType()))
baselineDF = spark.read.csv(BASELINE_PATH,sep="\u0001",header=True,schema=schemas_map["BaseLine"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
increaseBaselineDF = spark.read.csv(INCREASEBASELINE_PATH,sep="\u0001",header=True,schema=schemas_map["BaseLine"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
sharesDF = spark.read.csv(SHARES_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTreeBrandTech"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
clientTreeDF = spark.read.csv(CLIENTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["ClientTree"])
productTreeDF = spark.read.csv(PRODUCTTREE_PATH,sep="\u0001",header=True,schema=schemas_map["ProductTree"])
correctionDF = spark.read.csv(CORRECTION_PATH,sep="\u0001",header=True,schema=schemas_map["PromoProductsCorrection"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
planPostPromoEffectDF = spark.read.csv(PLANPOSTPROMOEFFECT_PATH,sep="\u0001",header=True,schema=schemas_map["PlanPostPromoEffect"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
incrementalDF = spark.read.csv(INCREMENTAL_PATH,sep="\u0001",header=True,schema=schemas_map["IncrementalPromo"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
cogsDF = spark.read.csv(COGS_PATH,sep="\u0001",header=True,schema=schemas_map["COGS"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
cogsTnDF = spark.read.csv(COGSTN_PATH,sep="\u0001",header=True,schema=schemas_map["PlanCOGSTn"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
tiDF = spark.read.csv(TI_PATH,sep="\u0001",header=True,schema=schemas_map["TradeInvestment"]).withColumn("Disabled",col("Disabled").cast(BooleanType()))
datesDF = spark.read.format("csv").option("delimiter","|").option("header","true").schema(datesDimSchema).load(DATESDIM_PATH)



try:
#   dbutils.fs.ls(INPUT_FILE_LOG_PATH)
 inputLogMessageDF = spark.read.format("csv").option("delimiter","\u0001").option("header","true").load(INPUT_FILE_LOG_PATH)
 print('Log has been already made')
except:
 inputLogMessageDF = spark.createDataFrame(sc.emptyRDD(), inputLogMessageSchema)
 print('Init log')
  

notCheckPromoStatusList = ['Draft', 'Cancelled', 'Deleted', 'Closed', 'Started', 'Finished']
actualCogsTiRecalculationPromoStatusList = ['Finished', 'Closed']

promoFilterDF = promoDF\
  .join(promoStatusDF, promoStatusDF.Id == promoDF.PromoStatusId, 'inner')\
  .select(\
           promoDF.Id
          ,promoDF.Disabled
          ,promoDF.TPMmode
          ,promoDF.Number
          ,promoDF.BrandTechId
          ,promoDF.ClientTreeKeyId
          ,promoDF.ClientTreeId
          ,promoDF.InOut
          ,date_add(to_date(promoDF.DispatchesStart, 'yyyy-MM-dd'), 1).alias('DispatchesStart')
          ,date_add(to_date(promoDF.StartDate, 'yyyy-MM-dd'), 1).alias('StartDate')
          ,date_add(to_date(promoDF.EndDate, 'yyyy-MM-dd'), 1).alias('EndDate')
          ,promoStatusDF.SystemName.alias('promoStatusName')
         )\
  .where((~col('promoStatusName').isin(*notCheckPromoStatusList)) & (col('Disabled') == 'False') & (col('TPMmode') != 3))

actualCogsTiRecalculationPromoDF = promoDF\
  .join(promoStatusDF, promoStatusDF.Id == promoDF.PromoStatusId, 'inner')\
  .select(\
           promoDF.Id
          ,promoDF.Disabled
          ,promoDF.TPMmode
          ,promoDF.Number
          ,promoStatusDF.SystemName.alias('promoStatusName')
         )\
  .where((col('promoStatusName').isin(*actualCogsTiRecalculationPromoStatusList)) & (col('Disabled') == 'False') & (col('TPMmode') != 3))

promoFilterDF = promoFilterDF\
  .join(brandTechDF, brandTechDF.Id == promoFilterDF.BrandTechId, 'left')\
  .select(\
           promoFilterDF['*']
          ,brandTechDF.BrandsegTechsub.alias('promoBrandTechName')
         )\

activePromoProductDF = promoProductDF.where((col('Disabled') == 'false') & (col('TPMmode') != 3))
activeClientTreeDF = clientTreeDF.where(col('EndDate').isNull())

activeClientTreeList = activeClientTreeDF.collect()

activeChangesIncidentsDF = changesIncidentsDF\
  .withColumn('ItemId', upper(col('ItemId')))\
  .where(col('ProcessDate').isNull())

processChangesIncidentsDF = changesIncidentsDF\
  .where(~col('ProcessDate').isNull())\
  .drop('#QCCount')

assortmentMatrixCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'AssortmentMatrix').select(activeChangesIncidentsDF.ItemId.alias('Id'))
priceListCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PriceList').select(activeChangesIncidentsDF.ItemId.alias('Id'))
baselineCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'BaseLine').select(activeChangesIncidentsDF.ItemId.alias('Id'))
increaseBaselineCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'IncreaseBaseLine').select(activeChangesIncidentsDF.ItemId.alias('Id'))
sharesCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'ClientTreeBrandTech').select(activeChangesIncidentsDF.ItemId.alias('Id'))
clientTreeCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'ClientTree').select(activeChangesIncidentsDF.ItemId.alias('Id'))
productTreeCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'ProductTree').select(activeChangesIncidentsDF.ItemId.alias('Id'))
correctionCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoProductsCorrection').select(activeChangesIncidentsDF.ItemId.alias('Id'))
incrementalCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'IncrementalPromo').select(activeChangesIncidentsDF.ItemId.alias('Id'))
cogsCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'COGS').select(activeChangesIncidentsDF.ItemId.alias('Id'))
cogsTnCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PlanCOGSTn').select(activeChangesIncidentsDF.ItemId.alias('Id'))
tiCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'TradeInvestment').select(activeChangesIncidentsDF.ItemId.alias('Id'))
actualCogsCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoActualCOGS').select(activeChangesIncidentsDF.ItemId.alias('Id'))
actualCogsTnCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoActualCOGSTn').select(activeChangesIncidentsDF.ItemId.alias('Id'))
actualTiCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoActualTradeInvestment').select(activeChangesIncidentsDF.ItemId.alias('Id'))
promoScenarioIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PromoScenario').select(activeChangesIncidentsDF.ItemId.alias('Id'))
ppeCiIdsDF = activeChangesIncidentsDF.where(col('DirectoryName') == 'PlanPostPromoEffect').select(activeChangesIncidentsDF.ItemId.alias('Id'))

print('total incidents count:', activeChangesIncidentsDF.count())
print('assortmentMatrix incidents count:', assortmentMatrixCiIdsDF.count())
print('priceList incidents count:', priceListCiIdsDF.count())
print('baseline incidents count:', baselineCiIdsDF.count())
print('increase baseline incidents count:', increaseBaselineCiIdsDF.count())
print('shares incidents count:', sharesCiIdsDF.count())
print('clientTree incidents count:', clientTreeCiIdsDF.count())
print('productTree incidents count:', productTreeCiIdsDF.count())
print('correction incidents count:', correctionCiIdsDF.count())
print('incremental incidents count:', incrementalCiIdsDF.count())
print('cogs incidents count:', cogsCiIdsDF.count())
print('cogstn incidents count:', cogsTnCiIdsDF.count())
print('ti incidents count:', tiCiIdsDF.count())
print('actual cogs incidents count:', actualCogsCiIdsDF.count())
print('actual cogstn incidents count:', actualCogsTnCiIdsDF.count())
print('actual ti incidents count:', actualTiCiIdsDF.count())
print('promo scenario incidents count:', promoScenarioIdsDF.count())
print('plan ppe incidents count:', ppeCiIdsDF.count())

activeProductChangeIncidentsDF = productChangeIncidentsDF\
  .where(col('RecalculationProcessDate').isNull())

processProductChangeIncidentsDF = productChangeIncidentsDF\
  .where(~col('RecalculationProcessDate').isNull())\
  .drop('#QCCount')

print('total product incidents count:', activeProductChangeIncidentsDF.count())

#####*Get promo numbers filtered by assortment matrix  changes incidents*

assortmentMatrixCiDF = assortmentMatrixCiIdsDF\
  .join(assortmentMatrixDF, 'Id', 'inner')\
  .select(\
           assortmentMatrixDF.ClientTreeId
          ,date_add(to_date(assortmentMatrixDF.StartDate, 'yyyy-MM-dd'), 1).alias('StartDate')
          ,date_add(to_date(assortmentMatrixDF.EndDate, 'yyyy-MM-dd'), 1).alias('EndDate')
         )

promoByAssortmentMatrixCiDF = assortmentMatrixCiDF\
  .join(promoFilterDF,
        [\
          assortmentMatrixCiDF.ClientTreeId == promoFilterDF.ClientTreeKeyId
         ,assortmentMatrixCiDF.StartDate <= promoFilterDF.DispatchesStart
         ,assortmentMatrixCiDF.EndDate >= promoFilterDF.DispatchesStart
        ]\
        ,'inner')\
  .where(promoFilterDF.InOut == 'false')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by pricelist changes incidents*

priceListCiDF = priceListCiIdsDF\
  .join(priceListDF, 'Id', 'inner')\
  .select(\
           priceListDF.ClientTreeId
          ,priceListDF.FuturePriceMarker 
          ,date_add(to_date(priceListDF.StartDate, 'yyyy-MM-dd'), 1).alias('StartDate')
          ,date_add(to_date(priceListDF.EndDate, 'yyyy-MM-dd'), 1).alias('EndDate')
         )

promoByPriceListCiDF = priceListCiDF\
  .where(priceListCiDF.FuturePriceMarker == 'false')\
  .join(promoFilterDF,
        [\
          priceListCiDF.ClientTreeId == promoFilterDF.ClientTreeKeyId
         ,priceListCiDF.StartDate <= promoFilterDF.DispatchesStart
         ,priceListCiDF.EndDate >= promoFilterDF.DispatchesStart
        ]\
        ,'inner')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

promoByIncreasePriceListCiDF = priceListCiDF\
  .where(priceListCiDF.FuturePriceMarker == 'true')\
  .join(promoFilterDF,
        [\
          priceListCiDF.ClientTreeId == promoFilterDF.ClientTreeKeyId
         ,priceListCiDF.StartDate <= promoFilterDF.DispatchesStart
         ,priceListCiDF.EndDate >= promoFilterDF.DispatchesStart
        ]\
        ,'inner')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by baseline changes incidents*

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

baselineCiDF = baselineCiIdsDF\
  .join(baselineDF, 'Id', 'inner')\
  .select(\
           baselineDF.ProductId
          ,baselineDF.DemandCode
          ,date_add(to_date(baselineDF.StartDate, 'yyyy-MM-dd'), 1).alias('StartDate')
         )

increaseBaselineCiDF = increaseBaselineCiIdsDF\
  .join(increaseBaselineDF, 'Id', 'inner')\
  .select(\
           increaseBaselineDF.ProductId
          ,increaseBaselineDF.DemandCode
          ,date_add(to_date(increaseBaselineDF.StartDate, 'yyyy-MM-dd'), 1).alias('StartDate')
         )

#baselineCiDF = baselineCiDF.union(increaseBaselineCiDF)

baselineCiDF = baselineCiDF\
  .join(datesDF, datesDF.OriginalDate == baselineCiDF.StartDate, 'inner')\
  .select(\
          baselineCiDF['*']
         ,datesDF.MarsWeekFullName
         )

increaseBaselineCiDF = increaseBaselineCiDF\
  .join(datesDF, datesDF.OriginalDate == increaseBaselineCiDF.StartDate, 'inner')\
  .select(\
          increaseBaselineCiDF['*']
         ,datesDF.MarsWeekFullName
         )

promoDemandDF = promoFilterDF\
  .select(\
           col('Id').alias('promoId')
          ,col('Number').alias('promoNumber')
          ,col('BrandTechId').alias('promoBrandTechId')
          ,promoFilterDF.StartDate.alias('promoStartDate')
          ,promoFilterDF.EndDate.alias('promoEndDate')
          ,promoFilterDF.DispatchesStart.alias('promoDispatchesStart')
          ,col('ClientTreeKeyId').alias('promoClientTreeKeyId')
          ,col('ClientTreeId').alias('promoClientTreeId')
          ,col('InOut').alias('promoInOut')
         )

promoDemandDF = promoDemandDF\
  .withColumn('productDemandCode', lit(getDemandCode(col('promoClientTreeId'))))

tempList = promoDemandDF.collect()
newPromoDemandDF = spark.createDataFrame(tempList, promoDemandDF.schema)

# join with promoProduct to get ZREP
promoProductDemandDF = newPromoDemandDF\
  .join(promoProductDF, promoProductDF.PromoId == newPromoDemandDF.promoId, 'inner')\
  .select(\
           newPromoDemandDF['*']
          ,promoProductDF.ProductId
         )

promoSplittedByWeekDF = promoProductDemandDF\
  .join(datesDF, 
        [\
          datesDF.OriginalDate >= promoProductDemandDF.promoStartDate
         ,datesDF.OriginalDate <= promoProductDemandDF.promoEndDate
        ], 
        'inner')\
  .select(\
          promoProductDemandDF.promoId
         ,promoProductDemandDF.promoNumber
         ,promoProductDemandDF.promoStartDate
         ,promoProductDemandDF.promoEndDate
         ,promoProductDemandDF.productDemandCode
         ,promoProductDemandDF.ProductId
         ,promoProductDemandDF.promoInOut
         ,datesDF.MarsWeekFullName
         ,datesDF.MarsDay
         )

cols = promoSplittedByWeekDF.columns
cols.remove('MarsDay')

promoSplittedByWeekDF = promoSplittedByWeekDF\
  .groupBy(cols)\
  .agg(count('*').cast(DecimalType(30,6)).alias('promoDaysInWeek'))

promoByBaselineCiDF = baselineCiDF\
  .join(promoSplittedByWeekDF,
        [\
          promoSplittedByWeekDF.productDemandCode == baselineCiDF.DemandCode
         ,promoSplittedByWeekDF.ProductId == baselineCiDF.ProductId
         ,promoSplittedByWeekDF.MarsWeekFullName == baselineCiDF.MarsWeekFullName
        ])\
  .where(promoSplittedByWeekDF.promoInOut == 'false')\
  .select(promoSplittedByWeekDF.promoId.alias('Id'), promoSplittedByWeekDF.promoNumber.alias('Number'))\
  .dropDuplicates()
 
promoByIncreaseBaselineCiDF = increaseBaselineCiDF\
  .join(promoSplittedByWeekDF,
        [\
          promoSplittedByWeekDF.productDemandCode == increaseBaselineCiDF.DemandCode
         ,promoSplittedByWeekDF.ProductId == increaseBaselineCiDF.ProductId
         ,promoSplittedByWeekDF.MarsWeekFullName == increaseBaselineCiDF.MarsWeekFullName
        ])\
  .where(promoSplittedByWeekDF.promoInOut == 'false')\
  .select(promoSplittedByWeekDF.promoId.alias('Id'), promoSplittedByWeekDF.promoNumber.alias('Number'))\
  .dropDuplicates()

#####*Get promo numbers filtered by shares changes incidents*

sharesCiDF = sharesCiIdsDF\
  .join(sharesDF, 'Id', 'inner')\
  .select(\
           sharesDF.ClientTreeId
          ,sharesDF.BrandTechId
         )

promoBySharesCiDF = sharesCiDF\
  .join(promoFilterDF,
        [\
          promoFilterDF.ClientTreeKeyId == sharesCiDF.ClientTreeId
         ,promoFilterDF.BrandTechId == sharesCiDF.BrandTechId
        ]\
        ,'inner')\
  .where(promoFilterDF.InOut == 'false')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by clienttree changes incidents*

clientTreeCiDF = clientTreeCiIdsDF\
  .join(clientTreeDF, 'Id', 'inner')\
  .select(\
           clientTreeDF.ObjectId
         )

promoByClientTreeCiDF = clientTreeCiDF\
  .join(promoFilterDF,
        [\
          promoFilterDF.ClientTreeId == clientTreeDF.ObjectId
        ]\
        ,'inner')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by producttree changes incidents*

productTreeCiDF = productTreeCiIdsDF\
  .join(productTreeDF, 'Id', 'inner')\
  .select(\
           productTreeDF.ObjectId
         )

promoByProductTreeCiDF = productTreeCiDF\
  .join(promoProductTreeDF, promoProductTreeDF.ProductTreeObjectId == productTreeCiDF.ObjectId, 'inner')\
  .join(promoFilterDF, promoFilterDF.Id == promoProductTreeDF.PromoId, 'inner')\
  .where(promoFilterDF.InOut == 'false')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by plan post promo effect incidents*


ppeCiDF = ppeCiIdsDF\
  .join(planPostPromoEffectDF, 'Id', 'inner')\
  .select(\
           planPostPromoEffectDF.ClientTreeId,
           planPostPromoEffectDF.BrandTechId
         )

promoByPPECiDF = ppeCiDF\
    .join(promoFilterDF,
            [\
              ppeCiDF.ClientTreeId == promoFilterDF.ClientTreeKeyId
             ,ppeCiDF.BrandTechId == promoFilterDF.BrandTechId
            ]\
            ,'inner')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by correction changes incidents*

correctionCiDF = correctionCiIdsDF\
  .join(correctionDF, 'Id', 'inner')\
  .select(\
           correctionDF.PromoProductId
         )

promoByCorrectionCiDF = correctionCiDF\
  .join(promoProductDF, promoProductDF.Id == correctionCiDF.PromoProductId, 'inner')\
  .join(promoFilterDF, promoFilterDF.Id == promoProductDF.PromoId, 'inner')\
  .where(promoFilterDF.InOut == 'false')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by incremental changes incidents*

incrementalCiDF = incrementalCiIdsDF\
  .join(incrementalDF, 'Id', 'inner')\
  .select(\
           incrementalDF.PromoId
         )

promoByIncrementalCiDF = incrementalCiDF\
  .join(promoFilterDF, promoFilterDF.Id == incrementalCiDF.PromoId, 'inner')\
  .where(promoFilterDF.InOut == 'true')\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by cogs changes incidents*

cogsDF = cogsDF\
  .withColumn('StartDate', date_add(to_date(cogsDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(cogsDF.EndDate, 'yyyy-MM-dd'), 1))
cogsTnDF = cogsTnDF\
  .withColumn('StartDate', date_add(to_date(cogsTnDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(cogsTnDF.EndDate, 'yyyy-MM-dd'), 1))

cogsClientDF = cogsDF\
  .join(clientTreeDF, clientTreeDF.Id == cogsDF.ClientTreeId, 'inner')\
  .join(brandTechDF, brandTechDF.Id == cogsDF.BrandTechId, 'inner')\
  .select(\
           cogsDF.Id
          ,cogsDF.StartDate.alias('cogsStartDate')
          ,cogsDF.EndDate.alias('cogsEndDate')
          ,cogsDF.LSVpercent
          ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
          ,brandTechDF.BrandsegTechsub.alias('cbtName')
         )
cogsTnClientDF = cogsTnDF\
  .join(clientTreeDF, clientTreeDF.Id == cogsTnDF.ClientTreeId, 'inner')\
  .join(brandTechDF, brandTechDF.Id == cogsTnDF.BrandTechId, 'inner')\
  .select(\
           cogsTnDF.Id
          ,cogsTnDF.StartDate.alias('cogsStartDate')
          ,cogsTnDF.EndDate.alias('cogsEndDate')
          ,cogsTnDF.TonCost
          ,clientTreeDF.ObjectId.alias('cogsClientTreeObjectId')
          ,brandTechDF.BrandsegTechsub.alias('cbtName')
         )

cogsClientList = cogsClientDF.collect()
cogsCiIdsList = [row.Id for row in cogsCiIdsDF.collect()]

cogsTnClientList = cogsTnClientDF.collect()
cogsTnCiIdsList = [row.Id for row in cogsTnCiIdsDF.collect()]

@udf
def isPromoFilteredByGogs(objectId, brandTechName, dispatchesStart):
  c = [x for x in activeClientTreeList if x.ObjectId == objectId]
  cogs = []
  while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
    cogs = [x for x in cogsClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName == brandTechName\
                                         and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
    c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
  
  if len(cogs) == 0:
    while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
      cogs = [x for x in cogsClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName is None\
                                         and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
      c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
  isFiltered = False
  
  if len(cogs) == 0:
    return False
  else:
    for item in cogs:
      if item.Id in cogsCiIdsList:
        isFiltered = True
    return isFiltered

@udf
def isPromoFilteredByCogsTn(objectId, brandTechName, dispatchesStart):
  c = [x for x in activeClientTreeList if x.ObjectId == int(objectId)]
  cogs = []
  while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
    cogs = [x for x in cogsTnClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName == brandTechName\
                                         and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
    c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
  
  if len(cogs) == 0:
    while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
      cogs = [x for x in cogsTnClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName is None\
                                         and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
      c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
  isFiltered = False
  
  if len(cogs) == 0:
    return False
  else:
    for item in cogs:
      if item.Id in cogsTnCiIdsList:
        isFiltered = True
    return isFiltered

promoByCogsCiDF = promoFilterDF\
  .withColumn('isFilteredByGogs', lit(isPromoFilteredByGogs(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
  .where(col('isFilteredByGogs') == True)\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

promoByCogsTnCiDF = promoFilterDF\
  .withColumn('isFilteredByGogs', lit(isPromoFilteredByCogsTn(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
  .where(col('isFilteredByGogs') == True)\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

#####*Get promo numbers filtered by ti changes incidents*

tiDF = tiDF\
  .withColumn('StartDate', date_add(to_date(tiDF.StartDate, 'yyyy-MM-dd'), 1))\
  .withColumn('EndDate', date_add(to_date(tiDF.EndDate, 'yyyy-MM-dd'), 1))

tiClientNullBtDF = tiDF\
  .join(clientTreeDF, clientTreeDF.Id == tiDF.ClientTreeId, 'inner')\
  .select(\
           tiDF.Id
          ,tiDF.StartDate.alias('tiStartDate')
          ,tiDF.EndDate.alias('tiEndDate')
          ,tiDF.SizePercent
          ,clientTreeDF.ObjectId.alias('tiClientTreeObjectId')
         )\
  .withColumn('tibtName', lit(None).cast(StringType()))

tiClientNotNullBtDF = tiDF\
  .join(clientTreeDF, clientTreeDF.Id == tiDF.ClientTreeId, 'inner')\
  .join(brandTechDF, brandTechDF.Id == tiDF.BrandTechId, 'inner')\
  .select(\
           tiDF.Id
          ,tiDF.StartDate.alias('tiStartDate')
          ,tiDF.EndDate.alias('tiEndDate')
          ,tiDF.SizePercent
          ,clientTreeDF.ObjectId.alias('tiClientTreeObjectId')
          ,brandTechDF.BrandsegTechsub.alias('tibtName')
         )

tiClientList = tiClientNullBtDF.union(tiClientNotNullBtDF).collect()
tiCiIdsList = [row.Id for row in tiCiIdsDF.collect()]

@udf
def isPromoFilteredByTi(objectId, brandTechName, startDate):
  c = [x for x in activeClientTreeList if x.ObjectId == objectId]
  ti = []
  while (len(ti) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
    ti = [x for x in tiClientList if x.tiClientTreeObjectId == c[0].ObjectId and (x.tibtName == brandTechName or x.tibtName is None)\
                                     and x.tiStartDate <= startDate and x.tiEndDate >= startDate]
    c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
    
  isFiltered = False  
    
  if len(ti) == 0:
    return False
  else:
    for item in ti:
      if item.Id in tiCiIdsList:
        isFiltered = True
    return isFiltered

promoByTiCiDF = promoFilterDF\
  .withColumn('isFilteredByTi', lit(isPromoFilteredByTi(col('ClientTreeId'), col('promoBrandTechName'), col('DispatchesStart'))))\
  .where(col('isFilteredByTi') == True)\
  .select(promoFilterDF.Id, promoFilterDF.Number)

#####*Get promo numbers filtered by actual cogs changes incidents*

promoByActualCogsCiDF = actualCogsTiRecalculationPromoDF\
  .join(actualCogsCiIdsDF, actualCogsCiIdsDF.Id == actualCogsTiRecalculationPromoDF.Id, 'inner')\
  .where(~actualCogsCiIdsDF.Id.isNull())\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

promoByActualCogsTnCiDF = actualCogsTiRecalculationPromoDF\
  .join(actualCogsTnCiIdsDF, actualCogsTnCiIdsDF.Id == actualCogsTiRecalculationPromoDF.Id, 'inner')\
  .where(~actualCogsTnCiIdsDF.Id.isNull())\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

promoByActualCogsCiDF.show()

#####*Get promo numbers filtered by actual ti changes incidents*

promoByActualTiCiDF = actualCogsTiRecalculationPromoDF\
  .join(actualTiCiIdsDF, actualTiCiIdsDF.Id == actualCogsTiRecalculationPromoDF.Id, 'inner')\
  .where(~actualTiCiIdsDF.Id.isNull())\
  .select(promoFilterDF.Id, promoFilterDF.Number)\
  .dropDuplicates()

promoByActualTiCiDF.show()

#####*Get promo numbers filtered by product changes incidents*

promoByProductCiDF = promoFilterDF\
  .join(promoProductTreeDF, promoProductTreeDF.PromoId == promoFilterDF.Id, 'inner')\
  .join(productTreeDF, productTreeDF.ObjectId == promoProductTreeDF.ProductTreeObjectId, 'inner')\
  .select(\
           promoFilterDF.Id.alias('promoId')
          ,promoFilterDF.ClientTreeKeyId
          ,promoFilterDF.DispatchesStart
          ,promoFilterDF.Disabled.alias('pptDisabled')
          ,promoFilterDF.Number.alias('pNumber')
          ,productTreeDF.EndDate
          ,promoFilterDF.InOut
          ,lower(productTreeDF.FilterQuery).alias('FilterQuery')
         )\
  .where((col('pptDisabled') == 'false') & (col('EndDate').isNull()))

inoutPromoByProductCiDF = promoByProductCiDF.where(col('InOut') == 'true')
notInoutPromoByProductCiDF = promoByProductCiDF.where(col('InOut') == 'false')

lowerCaseProductDF = productDF.select(*[lower(col(c)).name(c) for c in productDF.columns])
lowerCaseProductDF = lowerCaseProductDF.toDF(*[c.lower() for c in lowerCaseProductDF.columns])

promoByProductCiList = notInoutPromoByProductCiDF.collect()

filteredArray = []
lowerCaseProductDF.registerTempTable("product")

filteredProductSchema = StructType([
  StructField("Id", StringType(), False),
  StructField("Number", IntegerType(), False),
  StructField("fPromoId", StringType(), False),
])

for i, item in enumerate(promoByProductCiList):
#   print(i, item.pNumber, item.promoId)
  productFilter = item.FilterQuery.replace('['+schema.lower()+'].[', '').replace('].[', '.').replace('[', '').replace(']', '').replace('*', 'id')
  filteredIdsList = spark.sql(productFilter).collect()
#   print(productFilter)
#   print(filteredIdsList)
  for productId in filteredIdsList:
    filteredArray.append([productId[0], item.pNumber, item.promoId])
    
filteredProductDF = spark.createDataFrame(filteredArray, filteredProductSchema)

print(filteredProductDF.count())

activeAssortmentMatrixDF = assortmentMatrixDF.where(col('Disabled') == 'false')

filteredProductDF = filteredProductDF.withColumn('Id', upper(filteredProductDF.Id))

cols = filteredProductDF.columns

resultFilteredProductDF = filteredProductDF\
  .join(activeAssortmentMatrixDF, activeAssortmentMatrixDF.ProductId == filteredProductDF.Id, 'inner')\
  .select(filteredProductDF['*'])\
  .dropDuplicates()

print(resultFilteredProductDF.count())

notInoutPromoByProductCiDF = resultFilteredProductDF\
  .join(activeProductChangeIncidentsDF, activeProductChangeIncidentsDF.ProductId == resultFilteredProductDF.Id, 'inner')\
  .select(resultFilteredProductDF.fPromoId.alias('Id'), resultFilteredProductDF.Number)\
  .dropDuplicates()

inoutPromoByProductCiDF = inoutPromoByProductCiDF\
  .join(activePromoProductDF, activePromoProductDF.PromoId == inoutPromoByProductCiDF.promoId, 'inner')\
  .join(activeProductChangeIncidentsDF, activeProductChangeIncidentsDF.ProductId == activePromoProductDF.ProductId, 'inner')\
  .select(inoutPromoByProductCiDF.promoId.alias('Id'), inoutPromoByProductCiDF.pNumber.alias('Number'))\
  .dropDuplicates()

promoByProductCiDF = notInoutPromoByProductCiDF.union(inoutPromoByProductCiDF)

titleMessage = '[INFO]: PROMO FILTERING'
titleLogMessageDF = spark.createDataFrame([(titleMessage,)], inputLogMessageSchema)

promoNumbersByAssortmentMatrixCiDF = promoByAssortmentMatrixCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Assortment matrix incidents: '))
promoNumbersByAssortmentMatrixCiDF = promoNumbersByAssortmentMatrixCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByPriceListCiDF = promoByPriceListCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Pricelist incidents: '))
promoNumbersByPriceListCiDF = promoNumbersByPriceListCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByIncreasePriceListCiDF = promoByIncreasePriceListCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Increase Pricelist incidents: '))
promoNumbersByIncreasePriceListCiDF = promoNumbersByIncreasePriceListCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByBaselineCiDF = promoByBaselineCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Baseline incidents: '))
promoNumbersByBaselineCiDF = promoNumbersByBaselineCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))
  
promoNumbersByIncreaseBaselineCiDF = promoByIncreaseBaselineCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Increase Baseline incidents: '))
promoNumbersByIncreaseBaselineCiDF = promoNumbersByIncreaseBaselineCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersBySharesCiDF = promoBySharesCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Shares incidents: '))
promoNumbersBySharesCiDF = promoNumbersBySharesCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByClientTreeCiDF = promoByClientTreeCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by ClientTree incidents: '))
promoNumbersByClientTreeCiDF = promoNumbersByClientTreeCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByProductTreeCiDF = promoByProductTreeCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by ProductTree incidents: '))
promoNumbersByProductTreeCiDF = promoNumbersByProductTreeCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))
  
promoNumbersByPpeCiDF = promoByPPECiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Plan Post Promo Effect incidents: '))
promoNumbersByPpeCiDF = promoNumbersByPpeCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByCorrectionCiDF = promoByCorrectionCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Correction incidents: '))
promoNumbersByCorrectionCiDF = promoNumbersByCorrectionCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByIncrementalCiDF = promoByIncrementalCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Incremental incidents: '))
promoNumbersByIncrementalCiDF = promoNumbersByIncrementalCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByCogsCiDF = promoByCogsCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by COGS incidents: '))
promoNumbersByCogsCiDF = promoNumbersByCogsCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByCogsTnCiDF = promoByCogsTnCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by COGSTn incidents: '))
promoNumbersByCogsTnCiDF = promoNumbersByCogsTnCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByTiCiDF = promoByTiCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by TI incidents: '))
promoNumbersByTiCiDF = promoNumbersByTiCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByActualCogsCiDF = promoByActualCogsCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Actual COGS incidents: '))
promoNumbersByActualCogsCiDF = promoNumbersByActualCogsCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByActualCogsTnCiDF = promoByActualCogsTnCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Actual COGSTn incidents: '))
promoNumbersByActualCogsTnCiDF = promoNumbersByActualCogsTnCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByActualTiCiDF = promoByActualTiCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Actual TI incidents: '))
promoNumbersByActualTiCiDF = promoNumbersByActualTiCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersByProductCiDF = promoByProductCiDF.select(col('Number')).withColumn('Title', lit('[INFO]: Promo filtered by Product incidents: '))
promoNumbersByProductCiDF = promoNumbersByProductCiDF\
  .groupBy('Title')\
  .agg(concat_ws(';', collect_list(col('Number'))).alias('Number'))

promoNumbersFilteredByCiDF = promoNumbersByAssortmentMatrixCiDF\
  .union(promoNumbersByIncreasePriceListCiDF)\
  .union(promoNumbersByPriceListCiDF)\
  .union(promoNumbersByIncreaseBaselineCiDF)\
  .union(promoNumbersByBaselineCiDF)\
  .union(promoNumbersBySharesCiDF)\
  .union(promoNumbersByClientTreeCiDF)\
  .union(promoNumbersByProductTreeCiDF)\
  .union(promoNumbersByPpeCiDF)\
  .union(promoNumbersByCorrectionCiDF)\
  .union(promoNumbersByIncrementalCiDF)\
  .union(promoNumbersByCogsCiDF)\
  .union(promoNumbersByCogsTnCiDF)\
  .union(promoNumbersByTiCiDF)\
  .union(promoNumbersByActualCogsCiDF)\
  .union(promoNumbersByActualCogsTnCiDF)\
  .union(promoNumbersByActualTiCiDF)\
  .union(promoNumbersByProductCiDF)

logMessageDF = promoNumbersFilteredByCiDF\
  .withColumn('logMessage', concat(col('Title'), col('Number')))\
  .drop('Title', 'Number')

outputLogMessageDF = inputLogMessageDF\
  .union(titleLogMessageDF)\
  .union(logMessageDF)

promoByCiDF = promoByAssortmentMatrixCiDF\
  .union(promoByPriceListCiDF)\
  .union(promoByBaselineCiDF)\
  .union(promoBySharesCiDF)\
  .union(promoByClientTreeCiDF)\
  .union(promoByProductTreeCiDF)\
  .union(promoByPPECiDF)\
  .union(promoByCorrectionCiDF)\
  .union(promoByIncrementalCiDF)\
  .union(promoByCogsCiDF)\
  .union(promoByCogsTnCiDF)\
  .union(promoByTiCiDF)\
  .union(promoByActualCogsCiDF)\
  .union(promoByActualCogsTnCiDF)\
  .union(promoByActualTiCiDF)\
  .union(promoByProductCiDF)

increasePromoByCiDF = promoByIncreaseBaselineCiDF\
  .union(promoByIncreasePriceListCiDF)

blockedPromoDF = promoByCiDF\
  .select(col('Id').alias('PromoId'))\
  .withColumn('HandlerId', lit(handlerId))

blockedPromoDF = blockedPromoDF.dropDuplicates()

blockedIncreasePromoDF = increasePromoByCiDF\
  .select(col('Id').alias('PromoId'))\
  .withColumn('HandlerId', lit(handlerId))

blockedIncreasePromoDF = blockedIncreasePromoDF.dropDuplicates()

activeChangesIncidentsDF = activeChangesIncidentsDF\
  .withColumn('Disabled', lit(True))\
  .drop('#QCCount')

activeProductChangeIncidentsDF = activeProductChangeIncidentsDF\
  .withColumn('Disabled', lit(True))\
  .drop('#QCCount')

resultChangesIncidentsDF = activeChangesIncidentsDF.union(processChangesIncidentsDF)
resultProductChangeIncidentsDF = activeProductChangeIncidentsDF.union(processProductChangeIncidentsDF)



blockedPromoDF.write.mode("overwrite").parquet(BLOCKEDPROMO_OUTPUT_PATH)
blockedPromoDF\
.repartition(1)\
.write.csv(BLOCKEDPROMO_OUTPUT_PATH_CSV,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue=""
)

blockedIncreasePromoDF.write.mode("overwrite").parquet(BLOCKEDINCREASEPROMO_OUTPUT_PATH)
blockedIncreasePromoDF\
.repartition(1)\
.write.csv(BLOCKEDINCREASEPROMO_OUTPUT_PATH_CSV,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue=""
)


#resultChangesIncidentsDF.write.mode("overwrite").parquet(CHANGESINCIDENTS_OUTPUT_PATH)
#resultProductChangeIncidentsDF.write.mode("overwrite").parquet(PRODUCTCHANGEINCIDENTS_OUTPUT_PATH)

resultChangesIncidentsDF=resultChangesIncidentsDF\
.na.fill({"Disabled":False})\
.withColumn("Disabled",col("Disabled").cast(IntegerType()))

resultChangesIncidentsDF\
.repartition(1)\
.write.csv(CHANGESINCIDENTS_OUTPUT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultProductChangeIncidentsDF\
.repartition(1)\
.write.csv(PRODUCTCHANGEINCIDENTS_OUTPUT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)


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

#subprocess.call(["hadoop", "fs", "-mv", OUTPUT_TEMP_FILE_LOG_PATH, OUTPUT_LOG_PATH])
#subprocess.call(["hadoop", "fs", "-rm", "-r", OUTPUT_TEMP_FILE_LOG_PATH])