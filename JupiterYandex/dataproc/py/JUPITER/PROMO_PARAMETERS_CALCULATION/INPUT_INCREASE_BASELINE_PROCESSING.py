####Notebook "INPUT_BASELINE_PROCESSING". 
####*Get valid baseline items from input file*.
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
from pyspark import SparkConf, SparkContext
from pyspark.sql import SparkSession
from pyspark.sql.types import *
from pyspark.sql.functions import *
import pandas as pd
from functools import reduce
import datetime, time
from datetime import timedelta
from pyspark.sql.functions import lag
from pyspark.sql.functions import lead
import sys

inputBaselineSchema = StructType([
  StructField("REP", StringType(), True),
  StructField("DMDGroup", StringType(), True),
  StructField("LOC", StringType(), True),
  StructField("STARTDATE", StringType(), True),
  StructField("DurInMinutes", IntegerType(), True),
  StructField("QTY", DoubleType(), True),
  StructField("MOE", StringType(), True),
  StructField("SALES_ORG", IntegerType(), True),
  StructField("SALES_DIST_CHANNEL", IntegerType(), True),
  StructField("SALES_DIVISON", IntegerType(), True),
  StructField("BUS_SEG", StringType(), True),
  StructField("MKT_SEG", StringType(), True),
  StructField("DELETION_FLAG", StringType(), True),
  StructField("DELETION_DATE", StringType(), True),
  StructField("INTEGRATION_STAMP", StringType(), True)
])

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": "/JUPITER/RAW/#MAINTENANCE/2022-08-01_manual__2022-08-01T12%3A37%3A59.726096%2B00%3A00_", "ProcessDate": "2022-08-01", "FileName":"BASELINE_0_20220726_200209.dat"}']
 
 sc.addPyFile("hdfs:///SRC/SHARED/EXTRACT_SETTING.py")
 os.environ["HADOOP_USER_NAME"] = "airflow"

spark = SparkSession.builder.appName('Jupiter - PySpark').getOrCreate()
import EXTRACT_SETTING as es

SETTING_RAW_DIR = es.SETTING_RAW_DIR
DATE_DIR=es.DATE_DIR

EXTRACT_ENTITIES_AUTO_PATH = f'{es.HDFS_PREFIX}{es.MAINTENANCE_PATH_PREFIX}EXTRACT_ENTITIES_AUTO.csv'
processDate=es.processDate
pipelineRunId=es.pipelineRunId

print(f'EXTRACT_ENTITIES_AUTO_PATH={EXTRACT_ENTITIES_AUTO_PATH}')

spark.sql("set spark.sql.legacy.timeParserPolicy=LEGACY")

today = datetime.datetime.today().date()
fileName = es.input_params.get("FileName")

DIRECTORY = SETTING_RAW_DIR + '/SOURCES/'

INPUT_BASELINE_PATH = DIRECTORY + 'INCREASE_BASELINE/' + fileName
PRODUCT_PATH = DIRECTORY + 'JUPITER/Product'
CLIENTTREE_PATH = DIRECTORY + 'JUPITER/ClientTree'
PRICELIST_PATH = DIRECTORY + 'JUPITER/PriceList'
PROMO_PATH = DIRECTORY + 'JUPITER/Promo'
PROMOSTATUS_PATH = DIRECTORY + 'JUPITER/PromoStatus'
PROMOPRODUCT_PATH = DIRECTORY + 'JUPITER/PromoProduct'
BASELINE_PATH = DIRECTORY + 'JUPITER/IncreaseBaseLine'

# OUTPUT_UPDATEDBASELINE_PATH = es.SETTING_OUTPUT_DIR + '/BASELINE/' + today.strftime('%Y/%m/%d') + '/BaseLine.PARQUET'
# OUTPUT_NEWBASELINE_PATH = es.SETTING_OUTPUT_DIR + '/BASELINE/' + today.strftime('%Y/%m/%d') + '/NewBaseLine.PARQUET'

OUTPUT_UPDATEDBASELINE_PATH = es.SETTING_OUTPUT_DIR + '/BASELINE/' + today.strftime('%Y/%m/%d') + '/IncreaseBaseLine.CSV'
OUTPUT_NEWBASELINE_PATH = es.SETTING_OUTPUT_DIR + '/BASELINE/' + today.strftime('%Y/%m/%d') + '/NewIncreaseBaseLine.CSV'

print(INPUT_BASELINE_PATH)

print(OUTPUT_UPDATEDBASELINE_PATH)
print(OUTPUT_NEWBASELINE_PATH)

baselineSchema = StructType([
    StructField("Id",StringType(),True),
StructField("Disabled",BooleanType(),True),
StructField("DeletedDate",TimestampType(),True),
StructField("ProductId",StringType(),True),
StructField("DemandCode",StringType(),True),
StructField("StartDate",DateType(),True),
StructField("InputBaselineQTY",DoubleType(),True),
StructField("SellInBaselineQTY",DoubleType(),True),
StructField("SellOutBaselineQTY",DoubleType(),True),
StructField("Type",IntegerType(),True),
StructField("LastModifiedDate",TimestampType(),True),
StructField("NeedProcessing",BooleanType(),True),
StructField("$QCCount",IntegerType(),True)])

inputBaselineDF = spark.read.format("csv").option("delimiter","\u0009").option("header","true").schema(inputBaselineSchema).load(INPUT_BASELINE_PATH)
productDF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True)
clientTreeDF = spark.read.csv(CLIENTTREE_PATH,sep="\u0001",header=True)
priceListDF = spark.read.csv(PRICELIST_PATH,sep="\u0001",header=True)
promoDF = spark.read.csv(PROMO_PATH,sep="\u0001",header=True)
promoStatusDF = spark.read.csv(PROMOSTATUS_PATH,sep="\u0001",header=True)
promoProductDF = spark.read.csv(PROMOPRODUCT_PATH,sep="\u0001",header=True)
baselineDF = spark.read.csv(BASELINE_PATH,sep="\u0001",header=True,schema=baselineSchema)

print(inputBaselineDF.count())

baselineDF=baselineDF.na.fill({"Disabled":False,"NeedProcessing":0})

validBaselineDF = inputBaselineDF\
  .withColumn('REP', regexp_replace('REP', r'^[0]*', ''))\
  .withColumn('DMDGroup', regexp_replace('DMDGroup', r'^[0]*', ''))

validBaselineDF = validBaselineDF\
  .where(\
          (col('REP').rlike("^0*[\d]{6}"))
         &(col('DMDGroup').rlike("^0*[\d]{8}"))
         &(col('DurInMinutes') == 10080)
         &(col('SALES_ORG') == 261)
         &(col('SALES_DIVISON') == 51)
         &(col('BUS_SEG') == '05')
         &(col('MOE') == '0125')
         &(col('SALES_DIST_CHANNEL').isin([11,22]))
        )

productDF = productDF.where(col('Disabled') == False)

validBaselineDF = validBaselineDF\
  .join(productDF, productDF.ZREP == validBaselineDF.REP, 'left')\
  .select(validBaselineDF['*'], productDF.Id.alias('ProductId'))\
  .where(~col('ProductId').isNull())

clientTreeDMDGroupDF = clientTreeDF.where((col('EndDate').isNull()) & ~(col('DMDGroup').isNull()) & (col('DMDGroup') != ''))
validBaselineDF = validBaselineDF\
  .join(clientTreeDMDGroupDF, clientTreeDMDGroupDF.DMDGroup == validBaselineDF.DMDGroup, 'left')\
  .select(validBaselineDF['*'], clientTreeDMDGroupDF.DMDGroup.alias('clientDMDGroup'), clientTreeDMDGroupDF.DemandCode.alias('clientDemandCode'))\
  .where(~col('clientDMDGroup').isNull())

validBaselineDF = validBaselineDF\
  .withColumn('STARTDATE', to_date(col('STARTDATE'), 'yyyyMMdd'))\
  .withColumn('WeekDay', date_format(col("STARTDATE"), "u"))\
  .where(col('WeekDay') == 7)\
  .drop('WeekDay')\
  .orderBy(col('STARTDATE').desc())

validBaselineDF = validBaselineDF\
  .withColumn('QTY', when(col('QTY') < 0, 0).otherwise(col('QTY')))

tempBaselineDF = validBaselineDF.drop('ProductId')

print(validBaselineDF.count())

activeClientTreeList = clientTreeDF.where(col('EndDate').isNull()).collect()
baseClientTreeDF = clientTreeDF.where((col('EndDate').isNull()) & (col('IsBaseClient') == True))

@udf
def getDMDGroup(objectId):
  c = [x for x in activeClientTreeList if x.ObjectId == objectId]
  while (c is not None) & (c[0].Type != 'root') & ((c[0].DMDGroup == '') | (c[0].DMDGroup is None)):
    c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
    if c is None: break
  return c[0].DMDGroup
  
baseClientTreeDF = baseClientTreeDF\
  .withColumn('baseClientDMDGroup', lit(getDMDGroup(col('ObjectId'))))

validBaselineClientDF = validBaselineDF\
  .join(baseClientTreeDF,  baseClientTreeDF.baseClientDMDGroup == validBaselineDF.DMDGroup, 'left')\
  .select(validBaselineDF['*'], baseClientTreeDF.Id.alias('ClientTreeId'))\
  .where(~col('ClientTreeId').isNull())

validBaselinePriceDF = validBaselineClientDF\
  .join(priceListDF,
        [\
          priceListDF.StartDate <= validBaselineClientDF.STARTDATE
         ,priceListDF.EndDate >= validBaselineClientDF.STARTDATE
         ,priceListDF.ProductId == validBaselineClientDF.ProductId
         ,priceListDF.ClientTreeId == validBaselineClientDF.ClientTreeId
        ],
       'left')\
  .select(validBaselineClientDF['*'], priceListDF.Price)\
  .where(~col('Price').isNull())\
  .drop('Price','ProductId','ClientTreeId')\
  .dropDuplicates()

notPriceBaselineDF = tempBaselineDF.exceptAll(validBaselinePriceDF)
print(notPriceBaselineDF.count())

statusList = ["Started", "Planned", "Approved"]

promoDF = promoDF.where((col('Disabled') == False))
promoProductDF = promoProductDF.where((col('Disabled') == False))

lightPromoDF = promoDF\
  .select(\
           col('Id').alias('promoId')
          ,col('Number').alias('promoNumber')
          ,col('PromoStatusId').alias('promoStatusId')
          ,date_add(to_date(promoDF.StartDate, 'yyyy-MM-dd'), 1).alias('promoStartDate')
          ,date_add(to_date(promoDF.EndDate, 'yyyy-MM-dd'), 1).alias('promoEndDate')
          ,col('ClientTreeKeyId').alias('promoClientTreeKeyId')
          ,col('ClientTreeId').alias('promoClientTreeId')
         )

checkPromoDF = lightPromoDF\
  .join(promoStatusDF, promoStatusDF.Id == lightPromoDF.promoStatusId, 'left')\
  .select(\
           lightPromoDF['*']
          ,promoStatusDF.SystemName.alias('promoStatusSystemName')
         )\
  .where(col('promoStatusSystemName').isin(*statusList))

checkPromoProductDF = checkPromoDF\
  .join(promoProductDF, promoProductDF.PromoId == checkPromoDF.promoId, 'inner')\
  .select(\
           checkPromoDF['*']
          ,promoProductDF.ProductId
         )

zeroQtyBaselineDF = validBaselineClientDF\
  .where(col('QTY') == 0)\
  .withColumn('nextStartDate', date_add(col('STARTDATE'), 6))

zeroQtyBaselinePromoDF = zeroQtyBaselineDF\
  .join(checkPromoProductDF,\
        [\
          checkPromoProductDF.ProductId == zeroQtyBaselineDF.ProductId
         ,(checkPromoProductDF.promoClientTreeId == zeroQtyBaselineDF.ClientTreeId)
        ]
       ,'inner')\
  .where((checkPromoProductDF.promoStartDate > zeroQtyBaselineDF.nextStartDate) | (checkPromoProductDF.promoEndDate < zeroQtyBaselineDF.STARTDATE))

print(checkPromoProductDF.count())
print(zeroQtyBaselineDF.count())

baselineDF = baselineDF\
  .withColumn('StartDate', date_add(to_date(col('StartDate'), 'yyyy-MM-dd'), 1))

print(baselineDF.count())

joinedValidBaselineDF = baselineDF\
  .join(validBaselineDF,
        [\
          validBaselineDF.STARTDATE == baselineDF.StartDate
         ,validBaselineDF.ProductId == baselineDF.ProductId
         ,validBaselineDF.clientDemandCode == baselineDF.DemandCode
        ],\
        'outer')\
  .select(\
           baselineDF['*']\
          ,validBaselineDF.clientDemandCode.alias('inputDemandCode')
          ,validBaselineDF.ProductId.alias('inputProductId')
          ,validBaselineDF.STARTDATE.alias('inputStartDate')
          ,validBaselineDF.QTY
         )

updatedBaselineDF = joinedValidBaselineDF\
  .where(~col('Id').isNull())\
  .withColumn('InputBaselineQTY', when(~col('QTY').isNull(), col('QTY')).otherwise(col('InputBaselineQTY')))\
  .withColumn('NeedProcessing', when((~col('QTY').isNull()), True).otherwise(col('NeedProcessing')))\
  .withColumn('LastModifiedDate', when(~col('QTY').isNull(), lit(today)).otherwise(col('LastModifiedDate')))\
  .drop('inputDemandCode', 'inputProductId', 'inputStartDate', 'QTY', '$QCCount')

newBaselineDF = joinedValidBaselineDF\
  .where(col('Id').isNull())\
  .withColumn('StartDate', col('inputStartDate'))\
  .withColumn('ProductId', col('inputProductId'))\
  .withColumn('DemandCode', col('inputDemandCode'))\
  .withColumn('InputBaselineQTY', when(~col('QTY').isNull(), col('QTY')).otherwise(0))\
  .drop('inputDemandCode', 'inputProductId', 'inputStartDate', 'QTY', '$QCCount')

print(updatedBaselineDF.count())
print(newBaselineDF.count())

windowBaseline = Window.partitionBy(['ProductId', 'DemandCode']).orderBy('StartDate')

updatedBaselineDF = updatedBaselineDF.withColumn('isNewBasline', lit(False))
newBaselineDF = newBaselineDF\
  .withColumn('NeedProcessing', lit(True))\
  .withColumn('isNewBasline', lit(True))

unionBaselineDF = updatedBaselineDF\
  .union(newBaselineDF)\
  .withColumn('lag_1', when(lag('NeedProcessing',1).over(windowBaseline).isNull(), lit(False)).otherwise(lag('NeedProcessing',1).over(windowBaseline)))\
  .withColumn('lag_2', when(lag('NeedProcessing',2).over(windowBaseline).isNull(), lit(False)).otherwise(lag('NeedProcessing',2).over(windowBaseline)))\
  .withColumn('lead_1',when(lead('NeedProcessing',1).over(windowBaseline).isNull(), lit(False)).otherwise(lead('NeedProcessing',1).over(windowBaseline)))\
  .withColumn('lead_2',when(lead('NeedProcessing',2).over(windowBaseline).isNull(), lit(False)).otherwise(lead('NeedProcessing',2).over(windowBaseline)))\
  .withColumn('NeedProcessing', col('NeedProcessing') | col('lag_1') | col('lag_2') | col('lead_1') | col('lead_2'))

updatedBaselineDF = unionBaselineDF\
  .where(col('isNewBasline') == False)\
  .drop('lag_1','lag_2','lead_1','lead_2','isNewBasline')

newBaselineDF = unionBaselineDF\
  .where(col('isNewBasline') == True)\
  .select(updatedBaselineDF.columns)

  
print(f'UPDATED_BASELINE_COUNT={updatedBaselineDF.count()}')
print(f'NEW_BASELINE_COUNT={newBaselineDF.count()}')

# updatedBaselineDF.write.mode("overwrite").parquet(OUTPUT_UPDATEDBASELINE_PATH)
# newBaselineDF.write.mode("overwrite").parquet(OUTPUT_NEWBASELINE_PATH)

updatedBaselineDF=updatedBaselineDF\
.withColumn("NeedProcessing",col("NeedProcessing").cast(IntegerType()))\
.withColumn("Disabled",col("Disabled").cast(IntegerType()))



updatedBaselineDF\
.select("Id","Disabled","DeletedDate","StartDate","Type""ProductId","LastModifiedDate","DemandCode","InputBaselineQTY","SellInBaselineQTY","SellOutBaselineQTY","NeedProcessing","$QCCount")\
.repartition(1)\
.write.csv(OUTPUT_UPDATEDBASELINE_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

newBaselineDF\
.select("Id","Disabled","DeletedDate","StartDate","Type""ProductId","LastModifiedDate","DemandCode","InputBaselineQTY","SellInBaselineQTY","SellOutBaselineQTY","NeedProcessing","$QCCount")\
.repartition(1)\
.write.csv(OUTPUT_NEWBASELINE_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)