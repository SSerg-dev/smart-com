####Notebook "MERGE_SCENARIO_DATA". 
####*Reading scenario from ScenarioList, union, data checking, building result dataframes*.
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
from pyspark.sql import *
from pyspark.sql.functions import *
import os.path
import datetime
import subprocess

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": "/JUPITER/OUTPUT/#MAINTENANCE/2022-08-24_manual__2022-08-24T07%3A48%3A10%2B00%3A00_", "ProcessDate": "2022-08-24", "Schema": "Jupiter", "PipelineName": "jupiter_merge_scenario_data", "BudgetYear": 2022, "ScenarioList": "5000004_20220810063833;5000020_20220810063833;5000025_20220810063833;5000027_20220810063833", "IdShiftValue": 1000000}']
 
 sc.addPyFile("hdfs:///SRC/SHARED/EXTRACT_SETTING.py")
 sc.addPyFile("hdfs:///SRC/SHARED/SUPPORT_FUNCTIONS.py")
 os.environ["HADOOP_USER_NAME"] = "airflow"

spark = SparkSession.builder.appName('Jupiter - PySpark').getOrCreate()
import EXTRACT_SETTING as es

SETTING_RAW_DIR = es.SETTING_RAW_DIR
SETTING_PROCESS_DIR = es.SETTING_PROCESS_DIR
SETTING_OUTPUT_DIR = es.SETTING_OUTPUT_DIR

DATE_DIR=es.DATE_DIR

EXTRACT_ENTITIES_AUTO_PATH = f'{es.HDFS_PREFIX}{es.MAINTENANCE_PATH_PREFIX}EXTRACT_ENTITIES_AUTO.csv'
processDate=es.processDate
pipelineRunId=es.pipelineRunId

print(f'EXTRACT_ENTITIES_AUTO_PATH={EXTRACT_ENTITIES_AUTO_PATH}')

import SUPPORT_FUNCTIONS as sp

SCHEMAS_DIR=SETTING_RAW_DIR + '/UPLOAD_FROM_SCENARIO/SCHEMAS/Jupiter/'
schemas_map = sp.getSchemasMapScenario(SCHEMAS_DIR)

# SETTING_RAW_DIR = '/mnt/RAW/FILES/RUSSIA_PETCARE_JUPITER/#DEVELOPMENT/CI-BASELINE-ANAPLAN'
# SETTING_PROCESS_DIR = '/mnt/PROCESS/RUSSIA_PETCARE_JUPITER/#DEVELOPMENT/CI-BASELINE-ANAPLAN'

scenarioList = es.input_params.get("ScenarioList")
budgetYear = es.input_params.get("BudgetYear")

SCENARIO_CLIENT_PROMO_PATH = SETTING_RAW_DIR + '/CLIENT_PROMO/'
SOURCE_SCENARIO_PATH = SETTING_RAW_DIR + '/UPLOAD_FROM_SCENARIO/Scenario/'
TARGET_JUPITER_PATH = SETTING_RAW_DIR + '/UPLOAD_FROM_SCENARIO/Jupiter/'
PROCESS_SCENARIO_PATH = SETTING_PROCESS_DIR + '/UPLOAD_FROM_SCENARIO/'

def file_exists(path):
    return subprocess.call(["hadoop", "fs", "-ls", path]) == 0

#this is because not all scenario snapshots have necessary entities
def getSourceDF(entityName, dfSchema):
  if not file_exists(path + '/Scenario.' + entityName + '.csv'):
    print('Scenario.' + entityName + '.csv file not found. Current DB table will be used.')
    return spark.read.format("csv").load(SOURCE_SCENARIO_PATH + entityName + '.csv')
    # return spark.read.format("parquet").load(SOURCE_SCENARIO_PATH + entityName + '.PARQUET')
  else:
    return spark.read.csv(path + '/Scenario.' + entityName + '.csv', header=True, sep='\u0001', schema = dfSchema)

def checkTargetColumn(sourceValue, targetValue, fieldName, promoNumber):
  if targetValue is None and sourceValue is not None:
    print(str(promoNumber) + '\t' + str(fieldName) + '\t' + str(sourceValue))

spark.conf.set("spark.sql.legacy.timeParserPolicy","LEGACY")

targetPromoDF = spark.read.csv(TARGET_JUPITER_PATH + 'Promo',sep="\u0001",header=True,schema=schemas_map["Promo"])
targetBrandDF = spark.read.csv(TARGET_JUPITER_PATH + 'Brand',sep="\u0001",header=True,schema=schemas_map["Brand"])
targetBrandTechDF = spark.read.csv(TARGET_JUPITER_PATH + 'BrandTech',sep="\u0001",header=True,schema=schemas_map["BrandTech"])
targetClientTreeDF = spark.read.csv(TARGET_JUPITER_PATH + 'ClientTree',sep="\u0001",header=True,schema=schemas_map["ClientTree"])
targetProductTreeDF = spark.read.csv(TARGET_JUPITER_PATH + 'ProductTree',sep="\u0001",header=True,schema=schemas_map["ProductTree"])
targetColorDF = spark.read.csv(TARGET_JUPITER_PATH + 'Color',sep="\u0001",header=True,schema=schemas_map["Color"])
targetEventDF = spark.read.csv(TARGET_JUPITER_PATH + 'Event',sep="\u0001",header=True,schema=schemas_map["Event"])
targetPromoTypesDF = spark.read.csv(TARGET_JUPITER_PATH + 'PromoTypes',sep="\u0001",header=True,schema=schemas_map["PromoTypes"])
targetMechanicDF = spark.read.csv(TARGET_JUPITER_PATH + 'Mechanic',sep="\u0001",header=True,schema=schemas_map["Mechanic"])
targetMechanicTypeDF = spark.read.csv(TARGET_JUPITER_PATH + 'MechanicType',sep="\u0001",header=True,schema=schemas_map["MechanicType"])
targetPromoStatusDF = spark.read.csv(TARGET_JUPITER_PATH + 'PromoStatus',sep="\u0001",header=True,schema=schemas_map["PromoStatus"])
targetRejectReasonDF = spark.read.csv(TARGET_JUPITER_PATH + 'RejectReason',sep="\u0001",header=True,schema=schemas_map["RejectReason"])
targetTechnologyDF = spark.read.csv(TARGET_JUPITER_PATH + 'Technology',sep="\u0001",header=True,schema=schemas_map["Technology"])
targetProductDF = spark.read.csv(TARGET_JUPITER_PATH + 'Product',sep="\u0001",header=True,schema=schemas_map["Product"])
targetPromoProductDF = spark.read.csv(TARGET_JUPITER_PATH + 'PromoProduct',sep="\u0001",header=True,schema=schemas_map["PromoProduct"])
targetPromoProductTreeDF = spark.read.csv(TARGET_JUPITER_PATH + 'PromoProductTree',sep="|",header=True,schema=schemas_map["PromoProductTree"])
targetPromoProductsCorrectionDF = spark.read.csv(TARGET_JUPITER_PATH + 'PromoProductsCorrection',sep="\u0001",header=True,schema=schemas_map["PromoProductsCorrection"])
targetUserDF = spark.read.csv(TARGET_JUPITER_PATH + 'User',sep="\u0001",header=True,schema=schemas_map["User"])
targetPromoSupportDF = spark.read.csv(TARGET_JUPITER_PATH + 'PromoSupport',sep="\u0001",header=True,schema=schemas_map["PromoSupport"])
targetPromoSupportPromoDF = spark.read.csv(TARGET_JUPITER_PATH + 'PromoSupportPromo',sep="\u0001",header=True,schema=schemas_map["PromoSupportPromo"])
targetBudgetSubItemDF = spark.read.csv(TARGET_JUPITER_PATH + 'BudgetSubItem',sep="\u0001",header=True,schema=schemas_map["BudgetSubItem"])
targetBTLDF = spark.read.csv(TARGET_JUPITER_PATH + 'BTL',sep="\u0001",header=True,schema=schemas_map["BTL"])
targetBTLPromoDF = spark.read.csv(TARGET_JUPITER_PATH + 'BTLPromo',sep="\u0001",header=True,schema=schemas_map["BTLPromo"])

promoSchema = targetPromoDF.schema
brandSchema = targetBrandDF.schema
brandTechSchema = targetBrandTechDF.schema
clientTreeSchema = targetClientTreeDF.schema
productTreeSchema = targetProductTreeDF.schema
colorSchema = targetColorDF.schema
eventSchema = targetEventDF.schema
promoTypesSchema = targetPromoTypesDF.schema
mechanicSchema = targetMechanicDF.schema
mechanicTypeSchema = targetMechanicTypeDF.schema
promoStatusSchema = targetPromoStatusDF.schema 
rejectReasonSchema = targetRejectReasonDF.schema
technologySchema = targetTechnologyDF.schema
productSchema = targetProductDF.schema
promoProductSchema = targetPromoProductDF.schema
promoProductTreeSchema = targetPromoProductTreeDF.schema
promoProductsCorrectionSchema = targetPromoProductsCorrectionDF.schema
userSchema = targetUserDF.schema
promoSupportSchema = targetPromoSupportDF.schema
promoSupportPromoSchema = targetPromoSupportPromoDF.schema
budgetSubItemSchema = targetBudgetSubItemDF.schema
BTLSchema =	 targetBTLDF.schema
BTLPromoSchema = targetBTLPromoDF.schema

scenarioListSplit = scenarioList.split(';')

promoDF = None
promoProductDF = None
promoProductTreeDF = None
promoProductsCorrectionDF = None
promoSupportDF = None
promoSupportPromoDF = None
BTLDF = None
BTLPromoDF = None

for s in scenarioListSplit:
  print(s)
  
  path = SCENARIO_CLIENT_PROMO_PATH + s
  if not file_exists(path):
    print('path not found:', path)
  else:
    promoClientTreeId = s.split('_')[0]
    print('promoClientTreeId =', promoClientTreeId)
    print('budgetYear =', budgetYear)
    
    scenarioPromoDF = spark.read.csv(path + '/Scenario.Promo.csv', header=True, sep='\u0001', schema = promoSchema)
    scenarioPromoProductDF = spark.read.csv(path + '/Scenario.PromoProduct.csv', header=True, sep='\u0001', schema = promoProductSchema)
    scenarioPromoProductTreeDF = spark.read.csv(path + '/Scenario.PromoProductTree.csv', header=True, sep='\u0001', schema = promoProductTreeSchema)
    scenarioPromoProductsCorrectionDF = spark.read.csv(path + '/Scenario.PromoProductsCorrection.csv', header=True, sep='\u0001', schema = promoProductsCorrectionSchema)
    scenarioPromoSupportDF = spark.read.csv(path + '/Scenario.PromoSupport.csv', header=True, sep='\u0001', schema = promoSupportSchema)
    scenarioPromoSupportPromoDF = spark.read.csv(path + '/Scenario.PromoSupportPromo.csv', header=True, sep='\u0001', schema = promoSupportPromoSchema)
    scenarioBTLDF = spark.read.csv(path + '/Scenario.BTL.csv', header=True, sep='\u0001', schema = BTLSchema)
    scenarioBTLPromoDF = spark.read.csv(path + '/Scenario.BTLPromo.csv', header=True, sep='\u0001', schema = BTLPromoSchema)
    
    print("initial scenarioPromoDF count =", scenarioPromoDF.count())
    print("initial scenarioPromoProductDF count =", scenarioPromoProductDF.count())
    print("initial scenarioPromoProductTreeDF count =", scenarioPromoProductTreeDF.count())
    print("initial scenarioPromoProductsCorrectionDF count =", scenarioPromoProductsCorrectionDF.count())
    print("initial scenarioPromoSupportDF count =", scenarioPromoSupportDF.count())
    print("initial scenarioPromoSupportPromoDF count =", scenarioPromoSupportPromoDF.count())
    print("initial scenarioBTLDF count =", scenarioBTLDF.count())
    print("initial scenarioBTLPromoDF count =", scenarioBTLPromoDF.count())
    
    currentPromoDF = scenarioPromoDF.alias('p')\
      .where((col('p.ClientTreeId') == promoClientTreeId) &(col('p.BudgetYear') == budgetYear) & (col('p.Disabled') == False))
      
    currentPromoProductDF = scenarioPromoProductDF.alias('pp')\
      .join(currentPromoDF.alias('p'), col('p.Id') == col('pp.PromoId'), 'inner')\
      .select(scenarioPromoProductDF['*'], col('p.Number').alias('PromoNumber'))
    
    currentPromoProductTreeDF = scenarioPromoProductTreeDF.alias('ppt')\
      .join(currentPromoDF.alias('p'), col('p.Id') == col('ppt.PromoId'), 'inner')\
      .select(scenarioPromoProductTreeDF['*'], col('p.Number').alias('PromoNumber'))
    
    currentPromoProductsCorrectionDF = scenarioPromoProductsCorrectionDF.alias('ppc')\
      .join(currentPromoProductDF.alias('pp'), col('pp.Id') == col('ppc.PromoProductId'), 'inner')\
      .select(scenarioPromoProductsCorrectionDF['*'])
#---------------------------------------------------------------------------------------------------------------------------------------
    # we should get promo in all budget year to detect which promo support and BTL have only 'budgetYear' promo
    currentClientPromoDF = scenarioPromoDF.alias('p')\
      .where((col('p.ClientTreeId') == promoClientTreeId) & (col('p.Disabled') == False))

    currentPromoSupportPromoDF = scenarioPromoSupportPromoDF.alias('psp')\
      .join(currentClientPromoDF.alias('p'), [col('p.Id') == col('psp.PromoId'), col('psp.Disabled') == False], 'inner')\
      .select(scenarioPromoSupportPromoDF['*'], col('p.Number').alias('PromoNumber'), col('p.BudgetYear').alias('PromoBudgetYear'))
    
    currentPromoSupportDF = scenarioPromoSupportDF.alias('ps')\
      .join(currentPromoSupportPromoDF.alias('psp'), col('psp.PromoSupportId') == col('ps.Id'), 'inner')\
      .select(scenarioPromoSupportDF['*'])\
      .dropDuplicates()
      
    currentBTLPromoDF = scenarioBTLPromoDF.alias('btlp')\
      .join(currentClientPromoDF.alias('p'), [col('p.Id') == col('btlp.PromoId'), col('btlp.Disabled') == False], 'inner')\
      .select(scenarioBTLPromoDF['*'], col('p.Number').alias('PromoNumber'), col('p.BudgetYear').alias('PromoBudgetYear'))
    
    currentBTLDF = scenarioBTLDF.alias('btl')\
      .join(currentBTLPromoDF.alias('btlp'), col('btlp.BTLId') == col('btl.Id'), 'inner')\
      .select(scenarioBTLDF['*'])\
      .dropDuplicates()
    
    print("currentPromoDF count =", currentPromoDF.count())
    print("currentPromoProductDF count =", currentPromoProductDF.count())
    print("currentPromoProductTreeDF count =", currentPromoProductTreeDF.count())
    print("currentPromoProductsCorrectionDF count =", currentPromoProductsCorrectionDF.count())
    print("currentPromoSupportDF count =", currentPromoSupportDF.count())
    print("currentPromoSupportPromoDF count =", currentPromoSupportPromoDF.count())
    print("currentBTLDF count =", currentBTLDF.count())
    print("currentBTLPromoDF count =", currentBTLPromoDF.count())
    
    if promoDF:
      promoDF = promoDF.union(currentPromoDF)
    else:
      promoDF = currentPromoDF
      
    if promoProductDF:
      promoProductDF = promoProductDF.union(currentPromoProductDF)
    else:
      promoProductDF = currentPromoProductDF
      
    if promoProductTreeDF:
      promoProductTreeDF = promoProductTreeDF.union(currentPromoProductTreeDF)
    else:
      promoProductTreeDF = currentPromoProductTreeDF
      
    if promoProductsCorrectionDF:
      promoProductsCorrectionDF = promoProductsCorrectionDF.union(currentPromoProductsCorrectionDF)
    else:
      promoProductsCorrectionDF = currentPromoProductsCorrectionDF
      
    if promoSupportDF:
      promoSupportDF = promoSupportDF.union(currentPromoSupportDF)
    else:
      promoSupportDF = currentPromoSupportDF
      
    if promoSupportPromoDF:
      promoSupportPromoDF = promoSupportPromoDF.union(currentPromoSupportPromoDF)
    else:
      promoSupportPromoDF = currentPromoSupportPromoDF
      
    if BTLDF:
      BTLDF = BTLDF.union(currentBTLDF)
    else:
      BTLDF = currentBTLDF
      
    if BTLPromoDF:
      BTLPromoDF = BTLPromoDF.union(currentBTLPromoDF)
    else:
      BTLPromoDF = currentBTLPromoDF
      
    print("---------------------------------------------------------------------")
    print()
    
print("promoDF count =", promoDF.count())
print("promoProductDF count =", promoProductDF.count())
print("promoProductTreeDF count =", promoProductTreeDF.count())
print("promoProductsCorrectionDF count =", promoProductsCorrectionDF.count())
print("promoSupportDF count =", promoSupportDF.count())
print("promoSupportPromoDF count =", promoSupportPromoDF.count())
print("BTLDF count =", BTLDF.count())
print("BTLPromoDF count =", BTLPromoDF.count())

promoCols = promoDF.columns

#sp - sourcePromo
resultPromoDF = promoDF.alias('p')\
  .join(targetBrandDF.alias('tb'), col('tb.Id') == col('p.BrandId'), 'left')\
  .join(targetBrandTechDF.alias('tbt'), col('tbt.Id') == col('p.BrandTechId'), 'left')\
  .join(targetClientTreeDF.alias('tct'), [col('tct.ObjectId') == col('p.ClientTreeId'), col('tct.EndDate').isNull()], 'left')\
  .join(targetColorDF.alias('tc'), col('tc.Id') == col('p.ColorId'), 'left')\
  .join(targetEventDF.alias('te'), col('te.Id') == col('p.EventId'), 'left')\
  .join(targetMechanicDF.alias('taim'), col('taim.Id') == col('p.ActualInStoreMechanicId'), 'left')\
  .join(targetMechanicDF.alias('tpim'), col('tpim.Id') == col('p.PlanInstoreMechanicId'), 'left')\
  .join(targetMechanicDF.alias('tmm'), col('tmm.Id') == col('p.MarsMechanicId'), 'left')\
  .join(targetMechanicTypeDF.alias('taimt'), col('taimt.Id') == col('p.ActualInStoreMechanicTypeId'), 'left')\
  .join(targetMechanicTypeDF.alias('tpimt'), col('tpimt.Id') == col('p.PlanInstoreMechanicTypeId'), 'left')\
  .join(targetMechanicTypeDF.alias('tmmt'), col('tmmt.Id') == col('p.MarsMechanicTypeId'), 'left')\
  .join(targetPromoTypesDF.alias('tpt'), col('tpt.Id') == col('p.PromoTypesId'), 'left')\
  .join(targetPromoStatusDF.alias('tps'), col('tps.Id') == col('p.PromoStatusId'), 'left')\
  .join(targetRejectReasonDF.alias('trr'), col('trr.Id') == col('p.RejectReasonId'), 'left')\
  .join(targetTechnologyDF.alias('tt'), col('tt.Id') == col('p.TechnologyId'), 'left')\
  .join(targetUserDF.alias('tu'), col('tu.Id') == col('p.CreatorId'), 'left')\
  .select(\
           promoDF['*']
          ,col('tb.Id').alias('tBrandId')
          ,col('tbt.Id').alias('tBrandTechId')
          ,col('tct.Id').alias('tClientTreeKeyId')
          ,col('tct.ObjectId').alias('tClientTreeId')
          ,col('tc.Id').alias('tColorId')
          ,col('te.Id').alias('tEventId')
          ,col('taim.Id').alias('tActualInStoreMechanicId')
          ,col('tpim.Id').alias('tPlanInstoreMechanicId')
          ,col('tmm.Id').alias('tMarsMechanicId')
          ,col('taimt.Id').alias('tActualInStoreMechanicTypeId')
          ,col('tpimt.Id').alias('tPlanInstoreMechanicTypeId')
          ,col('tmmt.Id').alias('tMarsMechanicTypeId')
          ,col('tpt.Id').alias('tPromoTypesId')
          ,col('tps.Id').alias('tPromoStatusId')
          ,col('trr.Id').alias('tRejectReasonId')
          ,col('tt.Id').alias('tTechnologyId')
          ,col('tu.Id').alias('tCreatorId')
         )

checkPromoList = resultPromoDF\
  .select(\
            col('Number')
           ,col('tBrandId'), col('BrandId')
           ,col('tBrandTechId'), col('BrandTechId')
           ,col('tClientTreeKeyId'),col('ClientTreeKeyId')
           ,col('tClientTreeId'), col('ClientTreeId')
           ,col('tColorId'),col('ColorId')
           ,col('tEventId'),col('EventId')
           ,col('tPromoTypesId'), col('PromoTypesId')
           ,col('tActualInStoreMechanicId'), col('ActualInStoreMechanicId')
           ,col('tPlanInstoreMechanicId'), col('PlanInstoreMechanicId')
           ,col('tMarsMechanicId'), col('MarsMechanicId')
           ,col('tActualInStoreMechanicTypeId'), col('ActualInStoreMechanicTypeId')
           ,col('tPlanInstoreMechanicTypeId'), col('PlanInstoreMechanicTypeId')
           ,col('tMarsMechanicTypeId'), col('MarsMechanicTypeId')
           ,col('tPromoStatusId'), col('PromoStatusId')
           ,col('tRejectReasonId'), col('RejectReasonId')
           ,col('tTechnologyId'), col('TechnologyId')
#            ,col('tCreatorId'), col('CreatorId')
         )\
  .collect()

print('PromoNumber\tField\tValue')
for i in checkPromoList:
  checkTargetColumn(i.BrandId, i.tBrandId, 'BrandId', i.Number)
  checkTargetColumn(i.BrandTechId, i.tBrandTechId, 'BrandTechId', i.Number)
  checkTargetColumn(i.ClientTreeKeyId, i.tClientTreeKeyId, 'ClientTreeKeyId', i.Number)
  checkTargetColumn(i.ClientTreeId, i.tClientTreeId, 'ClientTreeId', i.Number)
  checkTargetColumn(i.ColorId, i.tColorId, 'ColorId', i.Number)
  checkTargetColumn(i.EventId, i.tEventId, 'EventId', i.Number)
  checkTargetColumn(i.PromoTypesId, i.tPromoTypesId, 'PromoTypesId', i.Number)
  checkTargetColumn(i.ActualInStoreMechanicId, i.tActualInStoreMechanicId, 'ActualInStoreMechanicId', i.Number)
  checkTargetColumn(i.PlanInstoreMechanicId, i.tPlanInstoreMechanicId, 'PlanInstoreMechanicId', i.Number)
  checkTargetColumn(i.MarsMechanicId, i.tMarsMechanicId, 'MarsMechanicId', i.Number)
  checkTargetColumn(i.ActualInStoreMechanicTypeId, i.tActualInStoreMechanicTypeId, 'ActualInStoreMechanicTypeId', i.Number)
  checkTargetColumn(i.PlanInstoreMechanicTypeId, i.tPlanInstoreMechanicTypeId, 'PlanInstoreMechanicTypeId', i.Number)
  checkTargetColumn(i.MarsMechanicTypeId, i.tMarsMechanicTypeId, 'MarsMechanicTypeId', i.Number)
  checkTargetColumn(i.PromoStatusId, i.tPromoStatusId, 'PromoStatusId', i.Number)
  checkTargetColumn(i.RejectReasonId, i.tRejectReasonId, 'RejectReasonId', i.Number)
  checkTargetColumn(i.TechnologyId, i.tTechnologyId, 'TechnologyId', i.Number)
#   checkTargetColumn(i.CreatorId, i.tCreatorId, 'CreatorId', i.Number)
  
resultPromoDF = resultPromoDF\
    .where((col('Disabled') == False) &
            (~(
                (col('tBrandId').isNull()                      & col('BrandId').isNotNull())\
              | (col('tBrandTechId').isNull()                  & col('BrandTechId').isNotNull())\
              | (col('tClientTreeKeyId').isNull()              & col('ClientTreeKeyId').isNotNull())\
              | (col('tClientTreeId').isNull()                 & col('ClientTreeId').isNotNull())\
              | (col('tColorId').isNull()                      & col('ColorId').isNotNull())\
              | (col('tEventId').isNull()                      & col('EventId').isNotNull())\
              | (col('tPromoTypesId').isNull()                 & col('PromoTypesId').isNotNull())\
              | (col('tActualInStoreMechanicId').isNull()      & col('ActualInStoreMechanicId').isNotNull())\
              | (col('tPlanInstoreMechanicId').isNull()        & col('PlanInstoreMechanicId').isNotNull())\
              | (col('tMarsMechanicId').isNull()               & col('MarsMechanicId').isNotNull())\
              | (col('tActualInStoreMechanicTypeId').isNull()  & col('ActualInStoreMechanicTypeId').isNotNull())\
              | (col('tPlanInstoreMechanicTypeId').isNull()    & col('PlanInstoreMechanicTypeId').isNotNull())\
              | (col('tMarsMechanicTypeId').isNull()           & col('MarsMechanicTypeId').isNotNull())\
              | (col('tPromoStatusId').isNull()                & col('PromoStatusId').isNotNull())\
              | (col('tRejectReasonId').isNull()               & col('RejectReasonId').isNotNull())\
              | (col('tTechnologyId').isNull()                 & col('TechnologyId').isNotNull())\
            ))
          )
  
print("---------------------------------------------------------------------")
print()

splittedInOutProductIdsDF = promoDF\
  .select(\
           col('Number')
          ,col('InOutProductIds')
         )\
  .withColumn('InOutProductId', explode_outer(split(expr("substring(InOutProductIds, 1, length(InOutProductIds)-1)"),';')))

badInOutProductIdsList = splittedInOutProductIdsDF.alias('splitted')\
  .join(targetProductDF.alias('tpr'), [col('tpr.Id') == col('splitted.InOutProductId')], 'left')\
  .where(col('tpr.Id').isNull())\
  .select(\
           col('Number')
          ,col('InOutProductId')
         )\
  .collect()

print('PromoNumber\tField\tValue')
for i in badInOutProductIdsList:
  print(str(i.Number) + '\tProductId\t' + i.InOutProductId)

resultPromoProductDF = promoProductDF.alias('pp')\
  .join(targetProductDF.alias('tp'), col('tp.Id') == col('pp.ProductId'), 'left')\
  .select(\
           promoProductDF['*']
          ,col('tp.Id').alias('targetProductId')
         )
  
badPromoProductList = resultPromoProductDF\
  .where(col('targetProductId').isNull())\
  .select(\
           col('PromoNumber')
          ,col('ProductId')
          ,col('ZREP')
         )\
  .collect()

resultPromoProductDF = resultPromoProductDF.alias('pp')\
  .where(col('targetProductId').isNotNull())\
  .join(resultPromoDF.alias('p'), col('p.Id') == col('pp.PromoId'), 'inner')\
  .select(resultPromoProductDF['*'])\
  .drop('targetProductId')

print('badPromoProduct count:', len(badPromoProductList))
print('PromoNumber\tField\tValue')
for i in badPromoProductList:
  print(str(i.PromoNumber) + '\tProductId\t' + i.ProductId)

resultPromoProductTreeDF = promoProductTreeDF.alias('ppt')\
  .join(targetProductTreeDF.alias('tpt'), [col('tpt.ObjectId') == col('ppt.ProductTreeObjectId'), col('tpt.EndDate').isNull()], 'left')\
  .select(\
           promoProductTreeDF['*']
          ,col('tpt.Id').alias('targetProductTreeId')
         )

badPromoProductTreeList = resultPromoProductTreeDF\
  .where(col('targetProductTreeId').isNull())\
  .select(\
           col('PromoNumber')
          ,col('ProductTreeObjectId')
         )\
  .collect()

resultPromoProductTreeDF = resultPromoProductTreeDF.alias('ppt')\
  .where(col('targetProductTreeId').isNotNull())\
  .join(resultPromoDF.alias('p'), col('p.Id') == col('ppt.PromoId'), 'inner')\
  .select(resultPromoProductTreeDF['*'])\
  .drop('targetProductTreeId')

print('badPromoProductTree count:', len(badPromoProductTreeList))
print('PromoNumber\tField\tValue')
for i in badPromoProductTreeList:
  print(str(i.PromoNumber) + '\tProductTreeObjectId\t' + str(i.ProductTreeObjectId))

resultPromoProductsCorrectionDF = promoProductsCorrectionDF.alias('ppc')\
  .join(resultPromoProductDF.alias('pp'), col('pp.Id') == col('ppc.PromoProductId'), 'inner')\
  .select(\
           promoProductsCorrectionDF['*']
          ,col('pp.PromoNumber')
          ,col('pp.ZREP')
         )

checkPromoProductsCorrectionList = resultPromoProductsCorrectionDF.alias('ppc')\
  .join(targetUserDF.alias('tu'), col('tu.Id') == col('ppc.UserId'), 'left')\
  .select(\
           col('ppc.PromoNumber')
          ,col('ppc.UserId')
          ,col('tu.Id').alias('tUserId')
         )\
  .collect()

resultPromoProductsCorrectionDF = resultPromoProductsCorrectionDF.alias('ppc')\
  .join(resultPromoProductDF.alias('pp'), col('pp.Id') == col('ppc.PromoProductId'), 'inner')\
  .select(resultPromoProductsCorrectionDF['*'])

for i in checkPromoProductsCorrectionList:
  checkTargetColumn(i.UserId, i.tUserId, 'UserId', i.PromoNumber)

resultPromoSupportDF = promoSupportDF.alias('ps')\
  .join(targetClientTreeDF.alias('tct'), [col('tct.Id') == col('ps.ClientTreeId'), col('tct.EndDate').isNull()], 'left')\
  .join(targetBudgetSubItemDF.alias('tbsi'), col('tbsi.Id') == col('ps.BudgetSubItemId'), 'left')\
  .select(\
           promoSupportDF['*']
          ,col('tct.Id').alias('tClientTreeId')
          ,col('tct.ObjectId').alias('tClientTreeObjectId')
          ,col('tbsi.Id').alias('tBudgetSubItemId')
         )

checkPromoSupportList = resultPromoSupportDF\
  .select(\
            col('Number')
           ,col('tClientTreeId'), col('ClientTreeId')
           ,col('tBudgetSubItemId'), col('BudgetSubItemId')
         )\
  .collect()

print('PromoSupportNumber\tField\tValue')
for i in checkPromoSupportList:
  checkTargetColumn(i.ClientTreeId, i.tClientTreeId, 'ClientTreeId', i.Number)
  checkTargetColumn(i.BudgetSubItemId, i.tBudgetSubItemId, 'BudgetSubItemId', i.Number)
  
resultPromoSupportDF = resultPromoSupportDF\
  .where((col('Disabled') == False) &
          (~(
              (col('tClientTreeId').isNull() & col('ClientTreeId').isNotNull())\
            | (col('tBudgetSubItemId').isNull()  & col('BudgetSubItemId').isNotNull())\
          ))
        )\
  .drop('tClientTreeId','tClientTreeObjectId','tBudgetSubItemId')

resultPromoSupportPromoDF = promoSupportPromoDF.alias('psp')\
  .join(resultPromoDF.alias('p'), col('p.Id') == col('psp.PromoId'), 'inner')\
  .join(resultPromoSupportDF.alias('ps'), col('ps.Id') == col('psp.PromoSupportId'), 'inner')\
  .select(promoSupportPromoDF['*'], col('ps.Number').alias('PromoSupportNumber'))

resultPromoSupportNumberDF = resultPromoSupportPromoDF.select(col('PromoSupportNumber')).distinct()

#select promoSupports having only 'budgetYear' promo
promoSupportCols = resultPromoSupportDF.columns
resultPromoSupportDF = resultPromoSupportDF.alias('ps')\
  .join(promoSupportPromoDF.alias('psp'), col('psp.PromoSupportId') == col('ps.Id'), 'inner')\
  .join(resultPromoSupportNumberDF.alias('n'), col('n.PromoSupportNumber') == col('ps.Number'), 'inner')\
  .select(resultPromoSupportDF['*'], col('psp.PromoNumber'), col('psp.PromoBudgetYear'))\
  .groupBy(promoSupportCols)\
  .agg(
         concat_ws(';', array_distinct(collect_list(col('psp.PromoNumber')))).alias('PromoNumbers')
        ,concat_ws(';', array_distinct(collect_list(col('psp.PromoBudgetYear')))).alias('PromoBudgetYears')
      )\
  .where(col('PromoBudgetYears') == budgetYear)\
  .select(promoSupportCols)

#BTL
resultBTLDF = BTLDF.alias('btl')\
  .join(targetEventDF.alias('te'), col('te.Id') == col('btl.EventId'), 'left')\
  .select(\
           BTLDF['*']
          ,col('te.Id').alias('tEventId')
         )

checkBTLList = resultBTLDF\
  .select(\
            col('Number')
           ,col('tEventId'), col('EventId')
         )\
  .collect()

print('BTLNumber\tField\tValue')
for i in checkBTLList:
  checkTargetColumn(i.EventId, i.tEventId, 'tEventId', i.Number)

resultBTLDF = resultBTLDF\
  .where((col('Disabled') == False) & (~(col('tEventId').isNull() & col('EventId').isNotNull())))\
  .drop('tEventId')

#BTLPromo
resultBTLPromoDF = BTLPromoDF.alias('btlp')\
  .join(resultPromoDF.alias('p'), col('p.Id') == col('btlp.PromoId'), 'inner')\
  .join(resultBTLDF.alias('btl'), col('btl.Id') == col('btlp.BTLId'), 'inner')\
  .select(BTLPromoDF['*'], col('btl.Number').alias('BTLNumber'))

resultBTLNumberDF = resultBTLPromoDF.select(col('BTLNumber')).distinct()

#select promoSupports having only 'budgetYear' promo
btlCols = resultBTLDF.columns
resultBTLDF = resultBTLDF.alias('btl')\
  .join(BTLPromoDF.alias('btlp'), col('btlp.BTLId') == col('btl.Id'), 'inner')\
  .join(resultBTLNumberDF.alias('n'), col('n.BTLNumber') == col('btl.Number'), 'inner')\
  .select(resultBTLDF['*'], col('btlp.PromoNumber'), col('btlp.PromoBudgetYear'))\
  .groupBy(btlCols)\
  .agg(
         concat_ws(';', array_distinct(collect_list(col('btlp.PromoNumber')))).alias('PromoNumbers')
        ,concat_ws(';', array_distinct(collect_list(col('btlp.PromoBudgetYear')))).alias('PromoBudgetYears')
      )\
  .where(col('PromoBudgetYears') == budgetYear)\
  .select(btlCols)

print('resultPromoDF count:', resultPromoDF.count())
print('resultPromoProductDF count:', resultPromoProductDF.count())
print('resultPromoProductTreeDF count:', resultPromoProductTreeDF.count())
print('resultPromoProductsCorrectionDF count:', resultPromoProductsCorrectionDF.count())
print('resultPromoSupportDF count:', resultPromoSupportDF.count())
print('resultPromoSupportPromoDF count:', resultPromoSupportPromoDF.count())
print('resultBTLDF count:', resultBTLDF.count())
print('resultBTLPromoDF count:', resultBTLPromoDF.count())

# resultPromoDF.select(promoCols).drop('#QCCount').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/Promo/Promo.parquet')
# resultPromoProductDF.drop('#QCCount').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/PromoProduct/PromoProduct.parquet')
# resultPromoProductTreeDF.drop('#QCCount').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/PromoProductTree/PromoProductTree.parquet')
# resultPromoProductsCorrectionDF.drop('#QCCount').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/PromoProductsCorrection/PromoProductsCorrection.parquet')
# resultPromoSupportDF.drop('#QCCount').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/PromoSupport/PromoSupport.parquet')
# resultPromoSupportPromoDF.drop('#QCCount','PromoBudgetYear').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/PromoSupportPromo/PromoSupportPromo.parquet')
# resultBTLDF.drop('#QCCount').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/BTL/BTL.parquet')
# resultBTLPromoDF.drop('#QCCount','PromoBudgetYear').write.mode("overwrite").parquet(PROCESS_SCENARIO_PATH + '/BTLPromo/BTLPromo.parquet')

resultPromoDF.select(promoCols).drop('#QCCount')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/Promo/Promo.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultPromoProductDF.drop('#QCCount')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/PromoProduct/PromoProduct.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultPromoProductTreeDF.drop('#QCCount')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/PromoProductTree/PromoProductTree.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultPromoProductsCorrectionDF.drop('#QCCount')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/PromoProductsCorrection/PromoProductsCorrection.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultPromoSupportDF.drop('#QCCount')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/PromoSupport/PromoSupport.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultPromoSupportPromoDF.drop('#QCCount','PromoBudgetYear')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/PromoSupportPromo/PromoSupportPromo.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultBTLDF.drop('#QCCount')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/BTL/BTL.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

resultBTLPromoDF.drop('#QCCount','PromoBudgetYear')\
.repartition(1)\
.write.csv(PROCESS_SCENARIO_PATH + '/BTLPromo/BTLPromo.CSV',
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)