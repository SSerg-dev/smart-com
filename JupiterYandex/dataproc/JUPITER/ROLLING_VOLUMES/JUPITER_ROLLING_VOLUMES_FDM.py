####Notebook "JUPITER_ROLLING_VOLUMES_FDM". 
####*Get rolling volumes by ZREP CLIENT WEEK*.
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
from datetime import timedelta, datetime, date
import datetime as datetime
import os

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": "/JUPITER/OUTPUT/#MAINTENANCE/2022-08-23_manual__2022-08-23T09%3A27%3A30%2B00%3A00_", "ProcessDate": "2022-08-23", "Schema": "Jupiter", "PipelineName": "jupiter_rolling_volumes_fdm"}']
 
 sc.addPyFile("hdfs:///SRC/SHARED/EXTRACT_SETTING.py")
 sc.addPyFile("hdfs:///SRC/SHARED/SUPPORT_FUNCTIONS.py") 
 os.environ["HADOOP_USER_NAME"] = "airflow"

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

incrementalSchema = StructType([
  StructField("PromoNumber", IntegerType(), False),
  StructField("ZREP", StringType(), False),
  StructField("DemandCode", StringType(), False),
  StructField("WeekStartDate", StringType(), False),
  StructField("DispatchesStart", StringType(), False),
  StructField("DispatchesEnd", StringType(), False),
  StructField("PlanProductIncrementalCaseQty",  DoubleType(), False),
  StructField("Status", StringType(), False)
])

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

startRollingDate = es.systemParametersDF.select(to_date(col("Value"), 'yyyy-MM-dd').alias("Value")).where(col('Key')=='StartRollingDate').collect()[0].Value
print(startRollingDate)

today = datetime.datetime.today().date()
seven_days = timedelta(7)
week_ago = today - seven_days

week_day = today.isoweekday()
start_mars_week = today
if week_day != 7:
  days_to_start_week = timedelta(week_day)
  start_mars_week = today - days_to_start_week
  
year_ago = today - timedelta(365)
week_day_year_ago = year_ago.isoweekday()
start_mars_week_year_ago = year_ago
if week_day_year_ago != 7:
  days_to_start_week = timedelta(week_day_year_ago)
  start_mars_week_year_ago = year_ago - days_to_start_week
  
if startRollingDate < start_mars_week_year_ago:
  startRollingDate = start_mars_week_year_ago
  
today_year = str(today.year)
today_month = str(today.month)
today_day = str(today.day)

week_ago_year = str(week_ago.year)
week_ago_month = str(week_ago.month)
week_ago_day = str(week_ago.day)

today_str = today_year.zfill(4) + '/' + today_month.zfill(2) + "/" + today_day.zfill(2)
week_ago_str = week_ago_year.zfill(4) + '/' + week_ago_month.zfill(2) + "/" + week_ago_day.zfill(2)

print(start_mars_week)
print(start_mars_week_year_ago)
print(startRollingDate)

####*Set paths*

#Inputs
ORDERS_PATH = SETTING_OUTPUT_DIR + '/JUPITER_ORDERS_DELIVERY_FDM/' + today_str + '/ORDERS_DELIVERY_FDM.parquet'
DATESDIM_PATH = SETTING_RAW_DIR + '/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_CALENDAR.csv'

BASELINE_PATH = SETTING_RAW_DIR + '/SOURCES/JUPITER/BaseLine'
PRODUCT_PATH = SETTING_RAW_DIR + '/SOURCES/JUPITER/Product'
PROMO_STATUS_PATH = SETTING_RAW_DIR + '/SOURCES/JUPITER/PromoStatus'
INCREMENTAL_PATH = SETTING_RAW_DIR + '/SOURCES/JUPITER/PlanIncrementalReport'
ROLLINGVOLUMES_PATH = SETTING_RAW_DIR + '/SOURCES/JUPITER/RollingVolume'

PREV_INCREMENTAL_PATH = SETTING_PROCESS_DIR + '/PREVIOUS_INCREMENTAL.PARQUET'
FULL_WEEK_ACTUAL_PATH = SETTING_PROCESS_DIR + "/FULL_WEEK_ACTUAL.parquet"

#Outputs
OUTPUT_DIR=SETTING_OUTPUT_DIR + "/" + es.pipelineSubfolderName
ROLLINGVOLUMES_FDM_OUTPUT_PATH = OUTPUT_DIR + "/" + today_str + "/ROLLINGVOLUMES_FDM.CSV"

SCHEMAS_DIR=SETTING_RAW_DIR + '/SCHEMAS/'
schemas_map = sp.getSchemasMap(SCHEMAS_DIR)

datesDF = spark.read.format("csv").option("delimiter","|").option("header","true").schema(datesDimSchema).load(DATESDIM_PATH)
ordersDF = spark.read.parquet(ORDERS_PATH)
baselineDF = spark.read.csv(BASELINE_PATH,sep="\u0001",header=True,schema=schemas_map["BaseLine"])
productDF = spark.read.csv(PRODUCT_PATH,sep="\u0001",header=True,schema=schemas_map["Product"])
promoStatusDF = spark.read.csv(PROMO_STATUS_PATH,sep="\u0001",header=True,schema=schemas_map["PromoStatus"])
incrementalDF = spark.read.csv(INCREMENTAL_PATH,sep="\u0001",header=True,schema=schemas_map["PlanIncrementalReport"])
fullWeekActualsDF = spark.read.format("parquet").load(FULL_WEEK_ACTUAL_PATH)
rollingVolumeDF = spark.read.csv(ROLLINGVOLUMES_PATH,sep="\u0001",header=True,schema=schemas_map["RollingVolume"])

try:
  previousPlanIncrementalDF = spark.read.format("parquet").load(PREV_INCREMENTAL_PATH)
except:
  print('Previous incremental plan not exists. Creating new one.')     
  previousPlanIncrementalDF = sqlContext.createDataFrame(sc.emptyRDD(), incrementalSchema)

####*Calculate Promo Difference*

currentPlanIncrementalDF = incrementalDF\
  .select(\
          col('PromoNumber')
          ,split(col("ZREP"), "\_0125")[0].alias("ZREP")\
          ,split(col("DemandCode"), "\_05_0125")[0].alias("DMDGROUP")\
          ,to_date(col("WeekStartDate"), 'yyyy-MM-dd').alias("WeekStartDate")\
          ,to_date(col("DispatchesStart"), 'yyyy-MM-dd').alias("DispatchesStart")\
          ,to_date(col("DispatchesEnd"), 'yyyy-MM-dd').alias("DispatchesEnd")\
          ,col("PlanProductIncrementalCaseQty").cast(DoubleType()).alias("CurrentIncrementalQty")\
          ,col("Status").alias("CurrentStatus")\
         )\
  .withColumn('CurrentDispatchesStart', date_add(col('DispatchesStart'), 1))\
  .withColumn('CurrentDispatchesEnd', date_add(col('DispatchesEnd'), 1))\
  .where((col("WeekStartDate") < start_mars_week) & (col("WeekStartDate") >= startRollingDate))

previousPlanIncrementalDF = previousPlanIncrementalDF\
  .select(\
          col('PromoNumber')
          ,split(col("ZREP"), "\_0125")[0].alias("ZREP")\
          ,split(col("DemandCode"), "\_05_0125")[0].alias("DMDGROUP")\
          ,to_date(col("WeekStartDate"), 'yyyy-MM-dd').alias("WeekStartDate")\
          ,to_date(col("DispatchesStart"), 'yyyy-MM-dd').alias("DispatchesStart")\
          ,to_date(col("DispatchesEnd"), 'yyyy-MM-dd').alias("DispatchesEnd")\
          ,col("PlanProductIncrementalCaseQty").cast(DoubleType()).alias("PreviousIncrementalQty")\
          ,col("Status").alias("PreviousStatus")\
         )\
  .withColumn('PreviousDispatchesStart', date_add(col('DispatchesStart'), 1))\
  .withColumn('PreviousDispatchesEnd', date_add(col('DispatchesEnd'), 1))\
  .where((col("WeekStartDate") < start_mars_week) & (col("WeekStartDate") >= startRollingDate))

promoDifferenceDF = previousPlanIncrementalDF\
  .join(currentPlanIncrementalDF, ['PromoNumber', 'ZREP', 'DMDGROUP', 'WeekStartDate'], 'left')\
  .select(\
          previousPlanIncrementalDF.PromoNumber
          ,previousPlanIncrementalDF.ZREP
          ,previousPlanIncrementalDF.DMDGROUP
          ,previousPlanIncrementalDF.WeekStartDate
          
          ,currentPlanIncrementalDF.CurrentDispatchesStart
          ,previousPlanIncrementalDF.PreviousDispatchesStart
          
          ,currentPlanIncrementalDF.CurrentDispatchesEnd
          ,previousPlanIncrementalDF.PreviousDispatchesEnd
          
          ,currentPlanIncrementalDF.CurrentStatus
          ,previousPlanIncrementalDF.PreviousStatus
          
          ,currentPlanIncrementalDF.CurrentIncrementalQty
          ,previousPlanIncrementalDF.PreviousIncrementalQty
         )\
  .fillna(0, ['CurrentIncrementalQty', 'PreviousIncrementalQty'])\
  .withColumn('DifferenceIncrementalQty', col('CurrentIncrementalQty') - col('PreviousIncrementalQty'))

####*Promo Difference, Baseline and Incremental preparation*

promoDifferenceDF = promoDifferenceDF\
  .select(\
          col("ZREP")
          ,col("DMDGROUP")
          ,to_date(col("WeekStartDate"), 'yyyy-MM-dd').alias("WeekStartDate")\
          ,col("DifferenceIncrementalQty").cast(DecimalType(30,6)).alias("QTY")\
         )\
  .groupBy(['ZREP', 'DMDGROUP'])\
  .agg(sum('QTY').alias('PromoDifferenceQty'))\
  .withColumn('WeekStartDate', to_date(lit(start_mars_week), 'yyyy-MM-dd'))   

goodStatuses = ['OnApproval', 'Planned', 'Started', 'Approved']

currIncrementalWeekDF = incrementalDF\
  .select(\
          split(col("ZREP"), "\_0125")[0].alias("ZREP")\
          ,split(col("DemandCode"), "\_05_0125")[0].alias("DMDGROUP")\
          ,to_date(col("WeekStartDate"), 'yyyy-MM-dd').alias("WeekStartDate")\
          ,col("PlanProductIncrementalCaseQty").cast(DecimalType(30,6)).alias("QTY")\
          ,col("Status")\
  )\
  .where((col('WeekStartDate') == start_mars_week) & (col("Status").isin(*goodStatuses)))\
  .groupBy(['ZREP', 'DMDGROUP', 'WeekStartDate'])\
  .agg(sum('QTY').alias('PlanProductIncrementalQty'))

baselineWeekDF = baselineDF\
  .where(col('Disabled') == 'False')\
  .join(productDF, baselineDF.ProductId == productDF.Id, 'inner')\
  .select(\
          col('ZREP')
          ,split(col("DemandCode"), "\_05_0125")[0].alias("DMDGROUP")\
          ,to_date(col("StartDate"), 'yyyy-MM-dd').alias("StartDate")\
          ,col("SellInBaselineQTY").cast(DecimalType(30,6))\
  )\
  .withColumn('WeekStartDate', date_add(col('StartDate'), 1))\
  .where(col('WeekStartDate') == start_mars_week)

####*Actual and Open Orders separation*

ordersWeekDF = ordersDF\
  .join(datesDF, ordersDF.MARSDATE == datesDF.MarsWeekFullName, 'inner')\
  .select(\
           ordersDF.DMDGROUP
          ,ordersDF.ZREP
          ,ordersDF.MARSDATE
          ,datesDF.OriginalDate.alias('WeekStartDate')
          ,ordersDF.Date
          ,ordersDF.BrandTech
          ,ordersDF.QTY.cast(DecimalType(30,6))
          ,ordersDF.TYPE
         )\
  .groupBy(['DMDGROUP', 'ZREP', 'MARSDATE', 'Date', 'BrandTech', 'QTY', 'TYPE'])\
  .agg(min('WeekStartDate').alias('WeekStartDate'))\
  .where(col('WeekStartDate') == start_mars_week)

previousWeekActualsDF = fullWeekActualsDF\
  .join(datesDF, fullWeekActualsDF.MARSDATE == datesDF.MarsWeekFullName, 'inner')\
  .select(\
           fullWeekActualsDF.DMDGROUP
          ,fullWeekActualsDF.ZREP
          ,fullWeekActualsDF.MARSDATE
          ,datesDF.OriginalDate.alias('WeekStartDate')
          ,fullWeekActualsDF.Date
          ,fullWeekActualsDF.BrandTech
          ,fullWeekActualsDF.QTY.cast(DecimalType(30,6))
          ,fullWeekActualsDF.TYPE
         )\
  .groupBy(['DMDGROUP', 'ZREP', 'MARSDATE', 'Date', 'BrandTech', 'QTY', 'TYPE'])\
  .agg(min('WeekStartDate').alias('WeekStartDate'))\
  .where(col('WeekStartDate') == (start_mars_week - timedelta(7)))

previousWeekActualsDF = previousWeekActualsDF\
  .groupBy(['DMDGROUP', 'ZREP', 'MARSDATE'])\
  .agg(sum('QTY').alias('QTY'), first('BrandTech').alias('BrandTech'), first('TYPE').alias('TYPE'))\
  .withColumn('WeekStartDate', lit(start_mars_week))

actualsDF = ordersWeekDF\
  .where(col('TYPE') == 'ACTUAL')\
  .groupBy(['DMDGROUP', 'ZREP', 'MARSDATE'])\
  .agg(first('WeekStartDate').alias('WeekStartDate'), sum('QTY').alias('QTY'), first('BrandTech').alias('BrandTech'), first('TYPE').alias('TYPE'))

openOrdersDF = ordersWeekDF\
  .where(col('TYPE') == 'OPEN_ORDER')\
  .groupBy(['DMDGROUP', 'ZREP', 'MARSDATE'])\
  .agg(first('WeekStartDate').alias('WeekStartDate'), sum('QTY').alias('QTY'), first('BrandTech').alias('BrandTech'), first('TYPE').alias('TYPE'))

####*Previous week rolling*

formattedDatesDF = datesDF\
  .withColumn('FormattedWeek', concat(col('MarsYear'), lit('P'), lpad(col('MarsPeriod'), 2, '0'), col('MarsWeekName')))

prevRollingVolumeDF = rollingVolumeDF\
  .join(productDF, ((rollingVolumeDF.ProductId == productDF.Id) & (col('Disabled') == 'false')), 'inner')\
  .join(formattedDatesDF, formattedDatesDF.FormattedWeek == rollingVolumeDF.Week, 'inner')\
  .select(\
           rollingVolumeDF.ManualRollingTotalVolumes
          ,rollingVolumeDF.ActualOO.alias('ActualOpenOrdersQty')
          ,split(col("DemandGroup"), "\_05_0125")[0].alias("DMDGROUP")\
          ,productDF.ZREP
          ,rollingVolumeDF.Week
          ,formattedDatesDF.OriginalDate
         )

prevRollingVolumeDF = prevRollingVolumeDF\
  .groupBy(['ManualRollingTotalVolumes','ActualOpenOrdersQty','DMDGROUP','ZREP','Week'])\
  .agg(date_add(min(col('OriginalDate')), 7).alias('WeekStartDate'))

####*Result*

rollingVolumesDF = currIncrementalWeekDF\
  .join(actualsDF, ['ZREP', 'DMDGROUP', 'WeekStartDate'], 'left')\
  .join(openOrdersDF, ['ZREP', 'DMDGROUP', 'WeekStartDate'], 'left')\
  .join(previousWeekActualsDF, ['ZREP', 'DMDGROUP', 'WeekStartDate'], 'left')\
  .join(prevRollingVolumeDF, ['ZREP', 'DMDGROUP', 'WeekStartDate'], 'left')\
  .join(baselineWeekDF, ['ZREP', 'DMDGROUP', 'WeekStartDate'], 'left')\
  .join(promoDifferenceDF, ['ZREP', 'DMDGROUP', 'WeekStartDate'], 'left')\
  .select(\
           currIncrementalWeekDF.ZREP\
          ,currIncrementalWeekDF.DMDGROUP\
          ,currIncrementalWeekDF.WeekStartDate\
          ,currIncrementalWeekDF.PlanProductIncrementalQty.alias('PlanProductIncrementalQty')\
          ,prevRollingVolumeDF.ManualRollingTotalVolumes.alias('PreviousRollingVolumesQty')\
          ,prevRollingVolumeDF.ActualOpenOrdersQty.alias('PreviousActualOpenOrdersQty')\
          ,actualsDF.QTY.alias('ActualsQty')\
          ,openOrdersDF.QTY.alias('OpenOrdersQty')\
          ,previousWeekActualsDF.QTY.alias('FullWeekActualsQty')\
          ,baselineWeekDF.SellInBaselineQTY.alias('BaselineQty')
          ,promoDifferenceDF.PromoDifferenceQty.alias('PromoDifferenceQty')
         )\
  .fillna(0, ['PreviousRollingVolumesQty', 'ActualsQty', 'OpenOrdersQty', 'FullWeekActualsQty', 'BaselineQty', 'PromoDifferenceQty'])\
  .withColumn('ActualIncrementalQty', col('ActualsQty') + col('OpenOrdersQty') - col('BaselineQty'))\
  .withColumn('PreliminaryRollingVolumesQty', col('PlanProductIncrementalQty') - col('ActualIncrementalQty'))\
  .withColumn('ActualOpenOrdersQty', col('ActualsQty') + col('OpenOrdersQty'))\
  .withColumn('FullWeekDiffQty', col('FullWeekActualsQty') - col('PreviousActualOpenOrdersQty'))\
  .withColumn('RollingVolumesQty',\
              when((col('PlanProductIncrementalQty') + col('PreviousRollingVolumesQty') - col('ActualIncrementalQty') + col('PromoDifferenceQty')) < 0, 0)\
         .otherwise(col('PlanProductIncrementalQty') + col('PreviousRollingVolumesQty') - col('ActualIncrementalQty') + col('PromoDifferenceQty')))

rollingVolumesDF = rollingVolumesDF\
  .select(\
           rollingVolumesDF.ZREP
          ,rollingVolumesDF.DMDGROUP
          ,to_date(date_add(col('WeekStartDate'), 7), 'yyyy.MM.dd').alias('WeekStartDate')
          ,rollingVolumesDF.PlanProductIncrementalQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.PreviousRollingVolumesQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.ActualsQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.OpenOrdersQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.ActualOpenOrdersQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.BaselineQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.PreliminaryRollingVolumesQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.PromoDifferenceQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.ActualIncrementalQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.FullWeekDiffQty.cast(DecimalType(30,6))
          ,rollingVolumesDF.RollingVolumesQty.cast(DecimalType(30,6))
         )

# rollingVolumesDF.write.mode("overwrite").parquet(ROLLINGVOLUMES_FDM_OUTPUT_PATH)

rollingVolumesDF\
.repartition(1)\
.write.csv(ROLLINGVOLUMES_FDM_OUTPUT_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)

incrementalDF.write.mode("overwrite").parquet(PREV_INCREMENTAL_PATH)