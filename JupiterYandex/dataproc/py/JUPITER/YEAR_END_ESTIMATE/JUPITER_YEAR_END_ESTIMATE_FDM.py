### Calculate Year end estimate for Jupiter
###Developer: LLC Smart-Com, alexei.loktev@effem.com
###Last Updated - 10th Jan 2020 - alexei.loktev@effem.com
###</br>Update 20/05/2020 - aggregation made on 'Brand Seg Tech Sub' Level.
###</br>YEE forecast 'SubBrand' columns must contain Core,Prem or empty suffix.

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
from pyspark.sql import *
import datetime as datetime
import pandas as pd
import re
import math
import subprocess

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": "/JUPITER/OUTPUT/#MAINTENANCE/2022-08-26_manual__2022-08-26T12%3A42%3A44%2B00%3A00_", "ProcessDate": "2022-08-26", "Schema": "Jupiter", "PipelineName": "jupiter_year_end_estimate_fdm"}']
 
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
pipelineSubfolderName = es.input_params.get("PipelineName")

print(f'EXTRACT_ENTITIES_AUTO_PATH={EXTRACT_ENTITIES_AUTO_PATH}')

forecastSchema=StructType([StructField('Type' , StringType(), False),
                              StructField('Type_1' , StringType(), False),
                              StructField('Comment' , StringType(), False),
                              StructField('Year' , StringType(), False),
                              StructField('Region' , StringType(), False),
                              StructField('SubBrand' , StringType(), False),
                              StructField('ZREP' , StringType(), False),
                              StructField('GRD' , StringType(), False),
                              StructField('SKU' , StringType(), False),
                              StructField('PERIOD' , StringType(), False),
                              StructField('LSV' , DoubleType(), False),
                              StructField('NSV' , DoubleType(), False),
                              StructField('TNS' , DoubleType(), False),
                              StructField('VERSION' , StringType(), False)])   

# Inputs
HELIOS_ACTUALS_DIR=SETTING_RAW_DIR+'/SOURCES/SELLIN/HELIOS_ACTUALS_BDM.parquet'
YEAR_END_ESTIMATE_BDM_DIR=SETTING_RAW_DIR+'/SOURCES/SELLIN/YEAR_END_ESTIMATE_BDM.parquet'
CUSTOMER_HIER_DIR=SETTING_RAW_DIR+'/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_CUSTOMERS.csv'
MATERIAL_DIR=SETTING_RAW_DIR+'/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_MATERIALS.csv'
CALENDAR_DIR=SETTING_RAW_DIR+'/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_CALENDAR.csv'

# Outputs
OUTPUT_DIR=SETTING_OUTPUT_DIR+"/"+pipelineSubfolderName
YEAR_END_ESTIMATE_FDM_OUTPUT_DIR=OUTPUT_DIR+"/YEAR_END_ESTIMATE_FDM.CSV"
YEAR_END_ESTIMATE_FDM_ARCHIVE_OUTPUT_DIR=OUTPUT_DIR+"/#ARCHIVE/YEAR_END_ESTIMATE_FDM.CSV"

# Import Helios Actuals Mapped data
heliosActualsDf = spark.read.format("parquet").option("header", "true").load(HELIOS_ACTUALS_DIR)


materialHydrateDf = spark.read.format("csv").option("header", "true").option("delimiter", "\u0001").load(MATERIAL_DIR)
gHierarchyDf = spark.read.format("csv").option("header", "true").option("delimiter", "\u0001").load(CUSTOMER_HIER_DIR)

forecastBdmDf = spark.read.format("parquet").option("header", "true").schema(forecastSchema).load(YEAR_END_ESTIMATE_BDM_DIR)

calendarDf = spark.read.format("csv").option("header", "true").option("delimiter", "|").load(CALENDAR_DIR)

# Some revenue recognition's codes mapped to not correct brand tech, we need apply this correction to fix final result
rrCorrectionsSchema = StructType([StructField('MATNR', StringType()), StructField('BST_CODE_CORRECTION',StringType())])
rrCorrectionsData =[Row(MATNR="10170507",	BST_CODE_CORRECTION="474-04-003"),
Row(MATNR="TV069"	,BST_CODE_CORRECTION="073-04-004"),
Row(MATNR="XL282"	,BST_CODE_CORRECTION="016-07-007")]


rrCorrectionsDf = spark.createDataFrame(rrCorrectionsData, rrCorrectionsSchema)

# display(forecastBdmDf)

today = datetime.datetime.today().strftime('%Y-%m-%d')
print(today)

# Convert version to number
forecastBdmVersionNumericDf=forecastBdmDf\
.withColumn('VERSION_NUM',regexp_replace('VERSION','DMR',''))\
.withColumn('VERSION_NUM',regexp_replace('VERSION_NUM','P',''))\
.withColumn('VERSION_NUM',col('VERSION_NUM').cast(IntegerType()))
# Select maximum version number
MAX_DMR_VERSION_NUM=forecastBdmVersionNumericDf.agg({"VERSION_NUM": "max"}).collect()[0][0]
print("MAX DMR VERSION:",MAX_DMR_VERSION_NUM)

p = re.compile("(\d{4})(\d{1,2})")
result = p.search(str(MAX_DMR_VERSION_NUM))

# Year and period from version
YEAR=result.group(1)
PERIOD=result.group(2).lstrip("0")

print(YEAR)
print(PERIOD)

forecastBdmMaxVersionDf=forecastBdmVersionNumericDf.where(col("VERSION_NUM")==MAX_DMR_VERSION_NUM)
forecastBdmMaxVersionDf.count()

forecastBdmMaxVersionDf.select("VERSION").distinct().show()

# Getting forecast for max version year and period equal and greater than max version period
forecastBdmFuturePeriodsDf=forecastBdmMaxVersionDf.withColumn("PERIOD_NUM",expr("substring(PERIOD, 2, length(PERIOD)-1)").cast(IntegerType()))
forecastBdmFuturePeriodsDf=forecastBdmFuturePeriodsDf.where((col("YEAR")==YEAR) & (col("PERIOD_NUM")>=PERIOD))

# Getting actuals for previous years and periods 
heliosPreviosPeriodsDf=heliosActualsDf\
                      .withColumn("PERIOD_NUM",expr("substring(PERIOD, 2, length(PERIOD)-1)"))\
                      .withColumn("YEARPERIOD_NUM",concat(col("YEAR"),col("PERIOD_NUM")).cast(IntegerType()))

YEARPERIOD_NUM=YEAR+PERIOD.zfill(2)
print(int(YEARPERIOD_NUM))
heliosPreviosPeriodsDf=heliosPreviosPeriodsDf.where(col("YEARPERIOD_NUM")<YEARPERIOD_NUM)


forecastBdmFuturePeriodsDf.select("VERSION").distinct().show()

# Append g-hierarchy to helios data
gHierarchyCurrentDataDf= gHierarchyDf.withColumn("Active_From",unix_timestamp('Active_From', 'yyyy/MM/dd').cast("timestamp")).withColumn("Active_Till",unix_timestamp('Active_Till', 'yyyy/MM/dd').cast("timestamp"))
gHierarchyCurrentDataDf=gHierarchyCurrentDataDf.where( ((col("Active_Till")>=today) & (col("Active_From")<=today)) |  (col("Active_Till").isNull() & (col("Active_From")<=today)) )

gHierarchySubsetDf = gHierarchyCurrentDataDf.select("0CUST_SALES","ZCUSTHG02", "ZCUSTHG03","ZCUSTHG03___T", "ZCUSTHG04","ZCUSTHG04___T","Active_From","Active_Till")\
                      .withColumn('0CUST_SALES',regexp_replace('0CUST_SALES','^0*',''))\
                      .withColumn('ZCUSTHG03',regexp_replace('ZCUSTHG03','^0*',''))\
                      .withColumn('ZCUSTHG04',regexp_replace('ZCUSTHG04','^0*',''))
gHierarchySubsetDf= gHierarchySubsetDf.dropDuplicates(["0CUST_SALES","ZCUSTHG02","ZCUSTHG03","ZCUSTHG03___T","ZCUSTHG04","ZCUSTHG04___T"])

gHierarchySubsetHeliosDf= gHierarchySubsetDf
heliosPreviosPeriodsHierDf = heliosPreviosPeriodsDf.join(gHierarchySubsetHeliosDf, heliosPreviosPeriodsDf["G_HIERARCHY_ID"] == gHierarchySubsetHeliosDf["0CUST_SALES"] , "left").select(heliosPreviosPeriodsDf["*"],gHierarchySubsetHeliosDf["ZCUSTHG02"] ,gHierarchySubsetHeliosDf["ZCUSTHG03"], gHierarchySubsetHeliosDf["ZCUSTHG04"], gHierarchySubsetHeliosDf["ZCUSTHG03___T"], gHierarchySubsetHeliosDf["ZCUSTHG04___T"])

heliosPreviosPeriodsHierDf = heliosPreviosPeriodsHierDf.withColumn("CLIENT_CODE", when(col("ZCUSTHG02") == "0070015696", col("ZCUSTHG04")).otherwise(col("ZCUSTHG03"))).withColumn("CLIENT_NAME", when(col("ZCUSTHG02") == "0070015696", col("ZCUSTHG04___T")).otherwise(col("ZCUSTHG03___T")))

forecastBdmFuturePeriodsDf=forecastBdmFuturePeriodsDf.withColumn('Region',regexp_replace('Region','Perekrestok','Big Boxes'))

# Append g-hierarchy to forecast data
gHierarchySubsetYeeDf = gHierarchyCurrentDataDf.where(col("G_H_level")==4).select("0CUST_SALES___T","ZCUSTHG02", "ZCUSTHG03","ZCUSTHG03___T", "ZCUSTHG04","ZCUSTHG04___T")\
                        .withColumn('ZCUSTHG03',regexp_replace('ZCUSTHG03','^0*',''))\
                        .withColumn('ZCUSTHG04',regexp_replace('ZCUSTHG04','^0*',''))\
                        .withColumn('0CUST_SALES___T',regexp_replace('0CUST_SALES___T','\s+Indirect',''))\
                        .withColumn('0CUST_SALES___T',regexp_replace('0CUST_SALES___T','Ural','Urals'))

gHierarchySubsetYeeDf= gHierarchySubsetYeeDf.dropDuplicates()

forecastBdmFuturePeriodsHierDf = forecastBdmFuturePeriodsDf.join(gHierarchySubsetYeeDf, forecastBdmFuturePeriodsDf["Region"] == gHierarchySubsetYeeDf["0CUST_SALES___T"] , "left").select(forecastBdmFuturePeriodsDf["*"],gHierarchySubsetYeeDf["ZCUSTHG02"] ,gHierarchySubsetYeeDf["ZCUSTHG03"], gHierarchySubsetYeeDf["ZCUSTHG04"], gHierarchySubsetYeeDf["ZCUSTHG03___T"], gHierarchySubsetYeeDf["ZCUSTHG04___T"])

forecastBdmFuturePeriodsHierDf = forecastBdmFuturePeriodsHierDf.withColumn("CLIENT_CODE", when(col("ZCUSTHG02") == "0070015696", col("ZCUSTHG04")).otherwise(col("ZCUSTHG03"))).withColumn("CLIENT_NAME", when(col("ZCUSTHG02") == "0070015696", col("ZCUSTHG04___T")).otherwise(col("ZCUSTHG03___T")))

forecastBdmFuturePeriodsHierDf.count()

materialHydrateDf=materialHydrateDf.withColumn("START_DATE",to_date(col("START_DATE"))).withColumn("END_DATE",to_date(col("END_DATE")))
materialHydrateDf=materialHydrateDf.where((col("START_DATE") <= today )&(col("END_DATE") >=today) )
materialHydrateDf.count()

materialHydrateDf = materialHydrateDf.filter(col("Segmen").isNotNull()).withColumn('ZREP',regexp_replace('ZREP','^0*',''))
materialHydrateDf = materialHydrateDf.dropDuplicates(subset = ["MATNR"])

materialHydrateDf=materialHydrateDf.join(rrCorrectionsDf,["MATNR"],how="left").withColumn("BrandsegTech_code",when(col("BST_CODE_CORRECTION").isNotNull(),col("BST_CODE_CORRECTION")).otherwise(col("BrandsegTech_code")))

heliosPreviosPeriodsHierMatDf = heliosPreviosPeriodsHierDf.join(materialHydrateDf, heliosPreviosPeriodsHierDf["MATERIAL_CODE"] == materialHydrateDf["MATNR"],"left").select(heliosPreviosPeriodsHierDf["*"], materialHydrateDf["BrandSegTechSub_code"])

heliosPreviosPeriodsHierMatDf=heliosPreviosPeriodsHierMatDf.drop("G_HIERARCHY_ID")

heliosPreviosPeriodsHierFinalDf=heliosPreviosPeriodsHierMatDf.select(col("BrandSegTechSub_code").alias("BRAND_SEG_TECH_CODE"),col("CLIENT_CODE").alias("G_HIERARCHY_ID"),col("YEAR"),col("PERIOD"),col("LSV").alias("ACTUAL_LSV"))

forecastBdmFuturePeriodsHierDf.count()

# Create zrep to brand-tech-seg code mapping and brandsegtech to  brand-tech-seg code mapping
zrepDf=materialHydrateDf.select("ZREP","BrandSegTechSub_code").dropDuplicates(["ZREP"])
brandSegTechDf=materialHydrateDf.select("BrandSegTechSub","BrandSegTechSub_code").dropDuplicates(["BrandSegTechSub"])

#Change 'Catsan Cat Litters' subbrand to 'Catsan Litter' for propper mapping with materials dictionary.
forecastBdmFuturePeriodsHierDf=forecastBdmFuturePeriodsHierDf.withColumn("SubBrand",when(col("SubBrand")=="Catsan Cat Litters","Catsan Litter").otherwise(col("SubBrand")))

# display(forecastBdmFuturePeriodsHierDf.where(col("SubBrand")=="CATSAN Litter").select("SubBrand","ZREP","GRD").dropDuplicates())

# Determine Brandsegtechcode by grd
forecastBdmFuturePeriodsHierMatGrdDf = forecastBdmFuturePeriodsHierDf.join(materialHydrateDf, forecastBdmFuturePeriodsHierDf["GRD"] == materialHydrateDf["MATNR"],"left").select(forecastBdmFuturePeriodsHierDf["*"], materialHydrateDf["BrandSegTechSub_code"].alias("BRAND_SEG_TECH_GRD"))
print(forecastBdmFuturePeriodsHierMatGrdDf.count())
# Determine Brandsegtechcode by zrep
forecastBdmFuturePeriodsHierMatZrepDf = forecastBdmFuturePeriodsHierMatGrdDf.join(zrepDf, forecastBdmFuturePeriodsHierMatGrdDf["Zrep"] == zrepDf["ZREP"],"left").select(forecastBdmFuturePeriodsHierMatGrdDf["*"], zrepDf["BrandSegTechSub_code"].alias("BRAND_SEG_TECH_ZREP"))
print(forecastBdmFuturePeriodsHierMatZrepDf.count())
# Determine Brandsegtechcode by subbrand
forecastBdmFuturePeriodsHierMatBSTDf = forecastBdmFuturePeriodsHierMatZrepDf.join(brandSegTechDf, forecastBdmFuturePeriodsHierMatZrepDf["SubBrand"] == brandSegTechDf["BrandSegTechSub"],"left").select(forecastBdmFuturePeriodsHierMatZrepDf["*"], brandSegTechDf["BrandSegTechSub_code"].alias("BRAND_SEG_TECH_BST"))
print(forecastBdmFuturePeriodsHierMatBSTDf.count())


# Choose value of BRAND_SEG_TECH_CODE. First use from grd code, if grd not exits from zrep code and if zrep not exits from brand tech 
forecastBdmFuturePeriodsHierMatFinalDf=forecastBdmFuturePeriodsHierMatBSTDf.withColumn("BRAND_SEG_TECH_CODE",when(col("BRAND_SEG_TECH_GRD").isNotNull(),col("BRAND_SEG_TECH_GRD")).otherwise(when(col("BRAND_SEG_TECH_ZREP").isNotNull(),col("BRAND_SEG_TECH_ZREP")).otherwise(col("BRAND_SEG_TECH_BST"))))

forecastBdmFuturePeriodsHierMatFinalDf=forecastBdmFuturePeriodsHierMatFinalDf.select(col("BRAND_SEG_TECH_CODE"),col("CLIENT_CODE").alias("G_HIERARCHY_ID"),col("Year").alias("YEAR"),col("PERIOD"),col("LSV").alias("FORECAST_LSV")).withColumn("FORECAST_LSV",col("FORECAST_LSV")*1000000)

heliosPreviosPeriodsHierAggDf=heliosPreviosPeriodsHierFinalDf.groupBy("BRAND_SEG_TECH_CODE","G_HIERARCHY_ID","YEAR").agg({"ACTUAL_LSV":"sum"})
forecastBdmFuturePeriodsHierAggDf=forecastBdmFuturePeriodsHierMatFinalDf.groupBy("BRAND_SEG_TECH_CODE","G_HIERARCHY_ID","YEAR").agg({"FORECAST_LSV":"sum"})

forecastActualsDf=forecastBdmFuturePeriodsHierAggDf.join(heliosPreviosPeriodsHierAggDf,["BRAND_SEG_TECH_CODE","G_HIERARCHY_ID","YEAR"],how="full")

forecastActualsDf=forecastActualsDf.fillna(0)
forecastActualsYeeYtdDf=forecastActualsDf.withColumn("YEE_LSV",col("sum(FORECAST_LSV)")+col("sum(ACTUAL_LSV)"))

finalDf=forecastActualsYeeYtdDf.select(col("YEAR"),col("BRAND_SEG_TECH_CODE"),col("G_HIERARCHY_ID"),col("sum(FORECAST_LSV)").alias("DMR_PLAN_LSV"),col("sum(ACTUAL_LSV)").alias("YTD_LSV"),col("YEE_LSV"))

finalDf.count()

# display(finalDf.where((col("G_HIERARCHY_ID")=="70015713") & (col("YEAR")=="2020")))

# display(finalDf.where((col("BRAND_SEG_TECH_CODE").isin("073-04-004"))&(col("G_HIERARCHY_ID").isin("70015717")) & (col("YEAR")=="2020")))

# display(finalDf.where((col("BRAND_SEG_TECH_CODE").isin("474-07-003"))&(col("G_HIERARCHY_ID").isin("70015718")) & (col("YEAR")=="2020")))

# display(finalDf.where((col("YEAR")=="2020") ).groupBy("YEAR").agg({"YTD_LSV":"sum"}).withColumn("sum(YTD_LSV)",col("sum(YTD_LSV)")/1000000))

try:
  subprocess.call(["hadoop", "fs", "-rm","-r", YEAR_END_ESTIMATE_FDM_ARCHIVE_OUTPUT_DIR])
  subprocess.call(["hadoop", "fs", "-cp", YEAR_END_ESTIMATE_FDM_OUTPUT_DIR, YEAR_END_ESTIMATE_FDM_ARCHIVE_OUTPUT_DIR])
except Exception as e:
  print(e)

# finalDf.write.mode("overwrite").parquet(YEAR_END_ESTIMATE_FDM_OUTPUT_DIR)
finalDf\
.repartition(1)\
.write.csv(YEAR_END_ESTIMATE_FDM_OUTPUT_DIR,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)