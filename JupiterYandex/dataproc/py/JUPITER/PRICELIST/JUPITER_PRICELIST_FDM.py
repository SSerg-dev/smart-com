### Calculate pricelist with ghierarchy's client id mapped to
### Developer: LLC Smart-Com, alexei.loktev@effem.com
### Last Updated - 06th Jun 2020 - alexei.loktev@effem.com

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
from pyspark.sql.window import Window
import datetime as datetime
import pandas as pd
from dateutil.relativedelta import relativedelta
import math
import subprocess

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": '
 '"/JUPITER/OUTPUT/#MAINTENANCE/2023-01-31_scheduled__2023-01-31T17%3A00%3A00%2B00%3A00_", '
 '"ProcessDate": "2023-01-31", "Schema": "Jupiter", "PipelineName": '
 '"jupiter_pricelist_fdm"}']
 
 sc.addPyFile("hdfs:///SRC/SHARED/EXTRACT_SETTING.py")
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
pipelineSubfolderName = es.input_params.get("PipelineName")

print(f'EXTRACT_ENTITIES_AUTO_PATH={EXTRACT_ENTITIES_AUTO_PATH}')

# Get current date
today = datetime.datetime.today()
print(today)

# Inputs
CUST_SALES_ATTR_DIR=SETTING_RAW_DIR+'/SOURCES/HYDRATEATLAS/0CUST_SALES_ATTR.PARQUET'
CUSTOMER_HIER_DIR=SETTING_RAW_DIR+'/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_CUSTOMERS.csv'
PRICELIST_DIR=SETTING_RAW_DIR+'/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_PRICELIST_GENERAL.csv'

# Outputs
OUTPUT_DIR=SETTING_OUTPUT_DIR+"/"+pipelineSubfolderName
PRICELIST_FDM_OUTPUT_DIR=OUTPUT_DIR+"/PRICELIST_FDM.CSV"
PRICELIST_FDM_ARCHIVE_OUTPUT_DIR=OUTPUT_DIR+"/#ARCHIVE/PRICELIST_FDM.CSV"

pricelistSchema=StructType([StructField('SOLD_TO' , StringType(), False),
                              StructField('MATERIAL' , StringType(), False),
                              StructField('ZREP' , StringType(), False),
                              StructField('PRICE_CS' , DoubleType(), False),
                              StructField('PRICE_ST' , DoubleType(), False),
                              StructField('PRICE_KG' , DoubleType(), False),
                              StructField('START_DATE' , DateType(), False),
                              StructField('END_DATE' , DateType(), False),
                              StructField('CURRENCY' , StringType(), False), 
                              StructField('PRICELIST' , StringType(), False), 
                              StructField('DELETION_INDICATOR' , StringType(), False),
                              StructField('RELEASE_STATUS' , StringType(), False), 
                              StructField('CREATED_ON' , DateType(), False)])

customerSalesAttributesDf=spark.read.parquet(CUST_SALES_ATTR_DIR)

gHierarchyDf = spark.read.format("csv").option("header", "true").option("delimiter", "\u0001").load(CUSTOMER_HIER_DIR)

pricelistDf=spark.read.format("csv").schema(pricelistSchema).option("header", "true").load(PRICELIST_DIR)

pricelistDf=pricelistDf\
            .where((col("DELETION_INDICATOR").isNull()) & (col("PRICE_CS").isNotNull())& (col("PRICE_ST").isNotNull())& (col("PRICE_KG").isNotNull()))
print("Price list rows:",pricelistDf.count())

print("GH Full rows:",gHierarchyDf.count())

# Choose only active data from GHierarchy
gHierarchyActiveDf=gHierarchyDf\
.withColumn("Active_From",to_date(col('Active_From'), 'yyyy/MM/dd'))\
.withColumn("Active_Till",to_date(col('Active_Till'), 'yyyy/MM/dd'))\
.where(((col("Active_From") <= today )&(col("Active_Till") >=today)) | ((col("Active_From") <= today )&(col("Active_Till").isNull())) )

print("GH active rows:",gHierarchyActiveDf.count())

gHierarchyActiveDf=gHierarchyActiveDf\
.withColumn('SoldToPoint',regexp_replace('SoldToPoint','^0*',''))\
.withColumn('0CUST_SALES',regexp_replace('0CUST_SALES','^0*',''))

customerSalesSubsetDf=customerSalesAttributesDf.select("KUNNR_PK","VKORG_PK","VTWEG_PK","SPART_PK","PLTYP","KTGRD","ERDAT","LOEVM","ODQ_CHANGEMODE").withColumn('KUNNR_PK',regexp_replace('KUNNR_PK','^0*',''))
customerSalesSubsetDf=customerSalesSubsetDf.where((col("VKORG_PK")==261)&(col("SPART_PK")==51) & (col("VTWEG_PK").isin(11,22,33))& (col("LOEVM").isNull()) & (col("PLTYP").isNotNull())).select(col("KUNNR_PK"),col("VTWEG_PK").alias("DISTR_CH"),col("PLTYP"),col("ERDAT"),col("ODQ_CHANGEMODE"))
print("0CUST_SALES_ATTR rows:",customerSalesSubsetDf.count())

# 10104987
# display(pricelistDf.where(col("ZREP")=="10104987"))

# display(pricelistDf.where(col("PRICE_CS").isNull()))

ghHierarchySoldToDf=gHierarchyActiveDf.select(col("SoldToPoint"),col("0DISTR_CHAN"),col("ZCUSTHG04").alias("GHierarchyId"),col("ZCUSTHG04___T").alias("GHierarchyName"))

ghHierarchySoldToDf=ghHierarchySoldToDf.withColumn('GHierarchyId',regexp_replace('GHierarchyId','^0*',''))

print("Soldto count:",ghHierarchySoldToDf.count())


gHierPricelistTypeDf=customerSalesSubsetDf.join(ghHierarchySoldToDf,(customerSalesSubsetDf["KUNNR_PK"]==ghHierarchySoldToDf["SoldToPoint"]) & (customerSalesSubsetDf["DISTR_CH"]==ghHierarchySoldToDf["0DISTR_CHAN"]),how="inner").dropDuplicates()
print("Soldto count:",gHierPricelistTypeDf.count())

gHierPricelistTypeDf=gHierPricelistTypeDf.select("DISTR_CH","PLTYP","ERDAT","ODQ_CHANGEMODE","GHierarchyId","GHierarchyName").dropDuplicates()
print("Soldto count:",gHierPricelistTypeDf.count())

window = Window.\
              partitionBy("DISTR_CH","PLTYP","ODQ_CHANGEMODE","GHierarchyId","GHierarchyName").\
              orderBy(gHierPricelistTypeDf["ERDAT"].desc())
custPartDf = gHierPricelistTypeDf\
            .where(col("PLTYP").isNotNull())\
            .withColumn("MAX_ERDAT",max("ERDAT").over(window))\

custSubsetDf=custPartDf.where(col("ERDAT")==col("MAX_ERDAT")).drop("MAX_ERDAT")
custSubsetDf.dropDuplicates().count()

# display(custSubsetDf.orderBy("GHierarchyName"))

custSubsetDf=custSubsetDf.fillna({"ODQ_CHANGEMODE":"I"})

window = Window.\
              partitionBy("GHierarchyId","GHierarchyName","DISTR_CH","ERDAT").\
              orderBy(custSubsetDf["ODQ_CHANGEMODE"].desc())
custPart2Df = custSubsetDf.withColumn("GROUP_ODQ_CHANGEMODE",first("ODQ_CHANGEMODE",True).over(window))
clientToPricelistMapDf=custPart2Df.where(col("ODQ_CHANGEMODE")==col("GROUP_ODQ_CHANGEMODE")).drop("GROUP_ODQ_CHANGEMODE")
clientToPricelistMapDf=clientToPricelistMapDf.select("GHierarchyId","GHierarchyName","PLTYP").dropDuplicates(["GHierarchyId","GHierarchyName"])

clientToPricelistMapDf.persist()
clientToPricelistMapDf.count()

# display(clientToPricelistMapDf.orderBy("GHierarchyName"))

dupsDf=clientToPricelistMapDf\
.groupBy("GHierarchyId","GHierarchyName","PLTYP")\
.agg((count("*")>1).cast("int").alias("DUPLICATE"))\
.where(col("DUPLICATE")>0)

dupsDf.count()

priceLevelPricesDf=pricelistDf.where(col("SOLD_TO").isNull())
priceLevelPricesDf.count()

pricelistClientsDf=priceLevelPricesDf.join(clientToPricelistMapDf,priceLevelPricesDf["PRICELIST"]==clientToPricelistMapDf["PLTYP"],how="left")
pricelistClientsDf.count()

print("clients price type codes:",clientToPricelistMapDf.select("PLTYP").distinct().count())
print("pricelist price type codes:",pricelistDf.select("PRICELIST").distinct().count())
print("Not mapped price level prices:",pricelistClientsDf.where(col("GHierarchyId").isNull()).count())

# display(pricelistClientsDf.where((col("GHierarchyId").isNull()) ).select("PRICELIST").distinct())

soldtoLevelPricesDf=pricelistDf.where(col("SOLD_TO").isNotNull())
soldtoLevelPricesDf.count()

ghHierarchySoldToSubsetDf=ghHierarchySoldToDf.withColumn('SoldToPoint',regexp_replace('SoldToPoint','^0*',''))

ghHierarchySoldToSubsetDf=ghHierarchySoldToSubsetDf\
.dropDuplicates()\
.where((col("SoldToPoint").isNotNull()) & ~(col("ZCUSTHG04")=="0"))\
.withColumn('GHierarchyId',regexp_replace('GHierarchyId','^0*',''))

print(ghHierarchySoldToSubsetDf.count())

pricelistSoldtoClientsDf=soldtoLevelPricesDf.join(ghHierarchySoldToSubsetDf,soldtoLevelPricesDf["SOLD_TO"]==ghHierarchySoldToSubsetDf["SoldToPoint"],how="left")
pricelistSoldtoClientsDf.count()

print("Not mapped price level prices:",pricelistSoldtoClientsDf.where(col("GHierarchyId").isNull()).count())

pricelistClientsDf=pricelistClientsDf.where(col("GHierarchyId").isNotNull()).dropDuplicates()
# Sold to level prices with soldto's belonging to same ghierachy id must by collapsed to one record
pricelistSoldtoClientsDf=pricelistSoldtoClientsDf.where(col("GHierarchyId").isNotNull()).dropDuplicates(['GHierarchyId','MATERIAL','START_DATE','END_DATE'])

pricelistClientsDf.count()
pricelistSoldtoClientsDf.count()

pricelistPriceLevelFinalDf=pricelistClientsDf.select(*pricelistDf.columns,col("GHierarchyId").alias("G_HIERARCHY_ID"))
pricelistSoldtoLevelFinalDf=pricelistSoldtoClientsDf.select(*pricelistDf.columns,col("GHierarchyId").alias("G_HIERARCHY_ID"))

mergedDf=pricelistPriceLevelFinalDf.unionByName(pricelistSoldtoLevelFinalDf)
# Giving the priority to prices. Soltdo level prices have more priority than price level ones.
mergedDf=mergedDf.withColumn("PRIORITY",when(col("SOLD_TO").isNull(),10).otherwise(20))
mergedDf.count()

window = Window.\
              partitionBy('G_HIERARCHY_ID','MATERIAL','START_DATE','END_DATE').\
              orderBy(mergedDf["PRIORITY"].desc())
finalPartDf = mergedDf.withColumn("MAX_PRIORITY",max("PRIORITY").over(window))
finalDf=finalPartDf.where(col("PRIORITY")==col("MAX_PRIORITY")).drop("MAX_PRIORITY")
finalDf=finalDf.dropDuplicates(['G_HIERARCHY_ID','ZREP','START_DATE','END_DATE'])
finalDf.count()


finalJupiterDf=finalDf.withColumn("UNIT_OF_MEASURE",lit("CS")).select('G_HIERARCHY_ID','ZREP',col('PRICE_CS').alias('PRICE'),'START_DATE',col('END_DATE').alias('FINISH_DATE'),'UNIT_OF_MEASURE','CURRENCY')
finalDf=finalDf.select('G_HIERARCHY_ID','MATERIAL','ZREP','PRICE_CS','PRICE_KG','PRICE_ST','START_DATE','END_DATE','RELEASE_STATUS')

dupsDf=finalDf\
.groupBy('G_HIERARCHY_ID','ZREP','START_DATE','END_DATE')\
.agg((count("*")>1).cast("int").alias("DUPLICATE"))\
.where(col("DUPLICATE")>0)

dupsDf.count()

# display(finalJupiterDf)

# display(finalJupiterDf.where(col("ZREP").isNull()))

finalJupiterDf.count()

try:
  subprocess.call(["hadoop", "fs", "-rm","-r", PRICELIST_FDM_ARCHIVE_OUTPUT_DIR])
  subprocess.call(["hadoop", "fs", "-cp", PRICELIST_FDM_OUTPUT_DIR, PRICELIST_FDM_ARCHIVE_OUTPUT_DIR])
except Exception as e:
  print(str(e))

# finalJupiterDf.write.mode("overwrite").parquet(PRICELIST_FDM_OUTPUT_DIR)

finalJupiterDf\
.withColumn('START_DATE',col('START_DATE').cast('timestamp'))\
.withColumn('FINISH_DATE',col('START_DATE').cast('timestamp'))\
.repartition(1)\
.write.csv(PRICELIST_FDM_OUTPUT_DIR,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)