####Notebook "JUPITER_ORDERS_DELIVERY_FDM". 
####*Get Delivery & Sales Orders data by ZREP CLIENT WEEK*.
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
from datetime import timedelta, datetime
import datetime as datetime
import pandas as pd
import os
import json
import subprocess

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": "/JUPITER/OUTPUT/#MAINTENANCE/2022-08-23_manual__2022-08-23T07%3A07%3A22%2B00%3A00_", "ProcessDate": "2022-08-23", "Schema": "Jupiter", "PipelineName": "jupiter_orders_delivery_fdm"}']
 
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

print(f'EXTRACT_ENTITIES_AUTO_PATH={EXTRACT_ENTITIES_AUTO_PATH}')

today = datetime.datetime.today().date()

today_year = str(today.year)
today_month = str(today.month)
today_day = str(today.day)

today_str = today_year.zfill(4) + '/' + today_month.zfill(2) + "/" + today_day.zfill(2)

####*Set paths*

#Inputs
DATESDIM_PATH = SETTING_RAW_DIR + '/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_CALENDAR.csv'
MATERIALS_PATH = SETTING_RAW_DIR + '/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_MATERIALS.csv'
HYDRATE_PATH = SETTING_RAW_DIR + '/SOURCES/HYDRATEATLAS/Z2LIS_11_VAITM.PARQUET'
HYDRATE_DELIVERY_PATH = SETTING_RAW_DIR + '/SOURCES/HYDRATEATLAS/Z2LIS_12_VCITM.PARQUET'
# SHIPTO_PATH = SETTING_RAW_DIR + '/SOURCES/APOLLODEMAND/UDT_CUST_SHIP_TO.PARQUET'
SHIPTO_PATH = SETTING_PROCESS_DIR + '/SHIPTO_TO_CLIENT_MAPPING/SHIPTO_TO_CLIENT_MAPPING.PARQUET'

#Outputs
OUTPUT_DIR = SETTING_OUTPUT_DIR + "/" + es.pipelineSubfolderName
ORDERS_DELIVERY_FDM_OUTPUT_PATH = OUTPUT_DIR + "/" + today_str + "/ORDERS_DELIVERY_FDM.parquet"
FULL_WEEK_ACTUAL_OUTPUT_PATH = SETTING_PROCESS_DIR + "/FULL_WEEK_ACTUAL.parquet"

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

datesDF = spark.read.format("csv").option("delimiter","|").option("header","true").schema(datesDimSchema).load(DATESDIM_PATH)
hydrateDF = spark.read.format("parquet").load(HYDRATE_PATH)
hydrateDeliveryDF = spark.read.format("parquet").load(HYDRATE_DELIVERY_PATH)
shipToDF = spark.read.format("parquet").load(SHIPTO_PATH)
materialDf = spark.read.format("csv").option("delimiter", "\u0001").option("header", "true").load(MATERIALS_PATH)

shipToDF=shipToDF\
.withColumn('CUST_NBR', regexp_replace('0CUST_SALES', r'^[0]*', ''))\
.withColumn('PET_DMD_GRP_DESC', col('GRP_DESC'))

####*Delivery filtering (step 1)*

windowFirstDeliveryRowNumber = (Window.partitionBy('VGBEL').rowsBetween(Window.unboundedPreceding, Window.unboundedFollowing))
windowFirstDeliverySeqRowNumber = (Window.partitionBy('VGBEL').orderBy(col('DI_SEQUENCE_NUMBER').desc()))

deliveryNumberDF = hydrateDeliveryDF\
  .where(\
           (col('SPARA') == 5)\
         & (col('VKORG') == 261)\
         & (col('PSTYV') != 'ZBCH')\
         & (col('PSTYV') != 'ZRBC')
        )\
  .select(\
         col('VGBEL')
         ,when(((to_date(hydrateDeliveryDF.WADAT_IST, 'yyyy.MM.dd') == '1900-01-01') | hydrateDeliveryDF.WADAT_IST.isNull()), to_date(hydrateDeliveryDF.WADAT, 'yyyy.MM.dd'))\
              .otherwise(to_date(hydrateDeliveryDF.WADAT_IST, 'yyyy.MM.dd')).alias('WADAT_IST')
         ,to_timestamp(col('LOAD_DATE'), 'yyyy.MM.dd HH:mm:ss').alias('LOAD_DATE')
         ,col('DI_SEQUENCE_NUMBER').cast(IntegerType()).alias('DI_SEQUENCE_NUMBER')
         ,length(regexp_replace("VGBEL", r'^[0]*', '')).alias('ORDER_LENGTH')
        )\
  .where(col('WADAT_IST').isNotNull())      

deliveryNumberDF = deliveryNumberDF\
  .withColumn('maxDate', max('LOAD_DATE').over(windowFirstDeliveryRowNumber))\
  .where(col('LOAD_DATE') == col('maxDate'))

deliveryNumberDF = deliveryNumberDF\
  .withColumn('Row_Number', row_number().over(windowFirstDeliverySeqRowNumber))\
  .where(col('Row_Number') == 1).drop('Row_Number')

deliveryNumberDF = deliveryNumberDF.select(col('VGBEL')).distinct()

####*Delivery filtering (step 2)*

windowDeliveryRowNumber = (Window.partitionBy('VGBEL', 'MATNR', 'VGPOS')\
  .rowsBetween(Window.unboundedPreceding, Window.unboundedFollowing))

windowDeliverySeqRowNumber = (Window.partitionBy(['VGBEL', 'MATNR', 'VGPOS']).orderBy(col('DI_SEQUENCE_NUMBER').desc()))

deliveryFilteredDF = hydrateDeliveryDF.join(deliveryNumberDF, 'VGBEL', 'inner')

deliveryFilteredDF = deliveryFilteredDF\
  .where( (col('SPARA') == 5)\
         & (col('VKORG') == 261)\
         & (col('PSTYV') != 'ZBCH')\
         & (col('PSTYV') != 'ZRBC')
        )\
  .withColumn('isWadatIstValid',when(((to_date(deliveryFilteredDF.WADAT_IST, 'yyyy.MM.dd') == '1900-01-01') | deliveryFilteredDF.WADAT_IST.isNull()), False).otherwise(True))\
  .select(\
         col('DI_OPERATION_TYPE')
        ,when(((to_date(deliveryFilteredDF.WADAT_IST, 'yyyy.MM.dd') == '1900-01-01') | deliveryFilteredDF.WADAT_IST.isNull()),\
               to_date(deliveryFilteredDF.WADAT, 'yyyy.MM.dd')).otherwise(to_date(hydrateDeliveryDF.WADAT_IST, 'yyyy.MM.dd')).alias('WADAT_IST')
        ,col('PSTYV')
        ,col('VBELN')
        ,col('VGBEL')
        ,col('MATNR')
        ,col('VGPOS')
        ,col('LFART')
        ,col('LGMNG')
        ,col('KCMENG')
        ,col('KUNNR')
        ,to_timestamp(col('LOAD_DATE'), 'yyyy.MM.dd HH:mm:ss').alias('LOAD_DATE')
        ,col('DI_SEQUENCE_NUMBER').cast(IntegerType()).alias('DI_SEQUENCE_NUMBER')
        ,length(regexp_replace("VGBEL", r'^[0]*', '')).alias('ORDER_LENGTH')
        ,col('isWadatIstValid')
         )\
  .withColumn('QTY_KCMENG_LGMNG', when(col('isWadatIstValid') == True, col('KCMENG')).otherwise(when(col('LFART') == 'ZLR', col('LGMNG')).otherwise(col('KCMENG'))))

deliveryFilteredDF = deliveryFilteredDF\
  .withColumn('maxDate', max('LOAD_DATE').over(windowDeliveryRowNumber))\
  .where(col('LOAD_DATE') == col('maxDate'))

deliveryFilteredDF = deliveryFilteredDF\
  .withColumn('Row_Number', row_number().over(windowDeliverySeqRowNumber))\
  .where(col('Row_Number') == 1).drop('Row_Number')

deliveryFilteredDF = deliveryFilteredDF\
  .where(col('WADAT_IST').isNotNull())

deliveryFilteredDF = deliveryFilteredDF\
  .where(col('DI_OPERATION_TYPE') != 'D')
     
deliveryActualsDF = deliveryFilteredDF\
  .where(col('isWadatIstValid') == True)\
  .groupBy(['KUNNR', 'MATNR', 'WADAT_IST'])\
  .agg(sum('QTY_KCMENG_LGMNG').alias('KCMENG'), first('VGBEL').alias('VGBEL'))

deliveryOpenOrdersDF = deliveryFilteredDF\
  .where(col('isWadatIstValid') == False)\
  .groupBy(['KUNNR', 'MATNR', 'WADAT_IST'])\
  .agg(sum('QTY_KCMENG_LGMNG').alias('KCMENG'), first('VGBEL').alias('VGBEL'))

####*Sales orders filtering (step 1)*

windowFirstOrdersRowNumber = (Window.partitionBy('VBELN').rowsBetween(Window.unboundedPreceding, Window.unboundedFollowing))
windowFirstOrdersSeqRowNumber = (Window.partitionBy('VBELN').orderBy(col('DI_SEQUENCE_NUMBER').desc()))

ordersNumberDF = hydrateDF\
  .where( (col('SPART') == 5)\
         & (col('VKORG') == 261)\
         & (col('AUART').isin(['ZBO','ZCS','ZKB','ZOR','ZTP','ZDTC','ZRE']))\
        )\
  .select(\
         col('VBELN')
         ,col('VGBEL')
         ,to_date(col('ZZWADAT'), 'yyyy.MM.dd').alias('ZZWADAT')
         ,to_date(col('KURSK_DAT'), 'yyyy.MM.dd').alias('KURSK_DAT')
         ,to_timestamp(col('LOAD_DATE'), 'yyyy.MM.dd HH:mm:ss').alias('LOAD_DATE')
         ,col('DI_SEQUENCE_NUMBER').cast(IntegerType()).alias('DI_SEQUENCE_NUMBER')
         ,length(regexp_replace("VBELN", r'^[0]*', '')).alias('ORDER_LENGTH')
        )\
  .withColumn('EARLIEST_DATE', date_add(col('ZZWADAT'), -180))\
  .where(col('ZZWADAT').isNotNull())  

splittedOrders = ordersNumberDF.where(col('VGBEL').isNotNull()).select(col('VGBEL').alias('VBELN')).distinct()
ordersNumberDF = ordersNumberDF.join(splittedOrders, 'VBELN', 'left_anti')

ordersNumberDF = ordersNumberDF\
  .withColumn('maxDate', max('LOAD_DATE').over(windowFirstOrdersRowNumber))\
  .where(col('LOAD_DATE') == col('maxDate'))

ordersNumberDF = ordersNumberDF\
  .withColumn('Row_Number', row_number().over(windowFirstOrdersSeqRowNumber))\
  .where(col('Row_Number') == 1).drop('Row_Number')

ordersNumberDF = ordersNumberDF.join(deliveryNumberDF, ordersNumberDF.VBELN == deliveryNumberDF.VGBEL, 'left_anti')
ordersNumberDF = ordersNumberDF.select(col('VBELN')).distinct()

####*Sales orders filtering (step 2)*

windowRowNumber = (Window.partitionBy('VBELN', 'MATNR', 'POSNR')\
  .rowsBetween(Window.unboundedPreceding, Window.unboundedFollowing))

windowSeqRowNumber = (Window.partitionBy(['VBELN', 'MATNR', 'POSNR']).orderBy(col('DI_SEQUENCE_NUMBER').desc()))

ordersFilteredDF = hydrateDF.join(ordersNumberDF, 'VBELN', 'inner')

ordersFilteredDF = ordersFilteredDF\
  .where((col('SPART') == 5)\
         & (col('VKORG') == 261)\
         & (col('AUART').isin(['ZBO','ZCS','ZKB','ZOR','ZTP','ZDTC','ZRE']))\
        )\
  .select(\
          col('DI_OPERATION_TYPE')
          ,to_date(col('ZZWADAT'), 'yyyy.MM.dd').alias('ZZWADAT')
          ,col('PSTYV')
          ,col('POSNR')
          ,col('ABGRU')
          ,col('VBELN')
          ,col('MATNR')
          ,col('PKUNWE')
          ,col('KLMENG')
          ,to_timestamp(col('LOAD_DATE'), 'yyyy.MM.dd HH:mm:ss').alias('LOAD_DATE')
          ,col('DI_SEQUENCE_NUMBER').cast(IntegerType()).alias('DI_SEQUENCE_NUMBER')
          ,length(regexp_replace("VBELN", r'^[0]*', '')).alias('ORDER_LENGTH')
         )

ordersFilteredDF = ordersFilteredDF\
  .withColumn('maxDate', max('LOAD_DATE').over(windowRowNumber))\
  .where(col('LOAD_DATE') == col('maxDate'))

ordersFilteredDF = ordersFilteredDF\
  .withColumn('Row_Number', row_number().over(windowSeqRowNumber))\
  .where(col('Row_Number') == 1).drop('Row_Number')

ordersFilteredDF = ordersFilteredDF\
  .where((col('ZZWADAT').isNotNull())
         & (col('PSTYV') != 'ZAPA')
       )

ordersFilteredDF = ordersFilteredDF\
  .where((col('ABGRU').isNull())
         & (col('DI_OPERATION_TYPE') != 'D')
       )

ordersFilteredDF = ordersFilteredDF\
  .groupBy(['PKUNWE', 'MATNR', 'ZZWADAT'])\
  .agg(sum('KLMENG').alias('KLMENG'), first('VBELN').alias('VBELN'))

####*Union orders*

deliveryActualsDF = deliveryActualsDF\
  .withColumn('TYPE', lit('ACTUAL'))

deliveryOpenOrdersDF = deliveryOpenOrdersDF\
  .withColumn('TYPE', lit('OPEN_ORDER'))

ordersFilteredDF = ordersFilteredDF\
  .withColumn('TYPE',lit('OPEN_ORDER'))

deliveryDF = deliveryActualsDF\
  .select(\
           col('KUNNR').alias('SHIPTO')
          ,col('MATNR')
          ,col('WADAT_IST').alias('DATE')
          ,col('KCMENG').alias('QTY')
          ,col('TYPE')
         )

deliveryOrdersDF = deliveryOpenOrdersDF\
  .select(\
           col('KUNNR').alias('SHIPTO')
          ,col('MATNR')
          ,col('WADAT_IST').alias('DATE')
          ,col('KCMENG').alias('QTY')
          ,col('TYPE')
         )

ordersDF = ordersFilteredDF\
  .select(\
           col('PKUNWE').alias('SHIPTO')
          ,col('MATNR')
          ,col('ZZWADAT').alias('DATE')
          ,col('KLMENG').alias('QTY')
          ,col('TYPE')
         )

ordersDeliveryDF = deliveryDF\
  .union(deliveryOrdersDF)\
  .union(ordersDF)

windowMatRowNumber = (Window.partitionBy('MATNR').orderBy(col('Date').desc()))
materialFilteredDF = materialDf\
  .select(\
          materialDf.MATNR,
          materialDf.ZREP
          ,to_date(col('VMSTD'), 'yyyy.MM.dd').alias('Date')
          ,materialDf.BrandTech
         )\
  .withColumn('Row_Number', row_number().over(windowMatRowNumber))\
  .where(col('Row_Number') == 1).drop('Row_Number')

####*Result*

resultOrdersDeliveryDF = ordersDeliveryDF\
  .select(\
          regexp_replace("SHIPTO", r'^[0]*', '').alias('ShipTo')
          ,regexp_replace("MATNR", r'^[0]*', '').alias('MATNR')
          ,to_date(col('DATE'), 'yyyy.MM.dd').alias('Date')
          ,col('QTY')
          ,col('TYPE')
         )

resultOrdersDeliveryShipToDF = resultOrdersDeliveryDF\
  .join(shipToDF, resultOrdersDeliveryDF.ShipTo == shipToDF.CUST_NBR, 'left')\
  .select(\
           when(shipToDF.PET_DMD_GRP_DESC.contains('Far East'), 'FAREAST').otherwise(upper(regexp_replace(shipToDF.PET_DMD_GRP_DESC, '\s?(\w+$)', '\_05'))).alias('DMDGROUP')\
          ,resultOrdersDeliveryDF.MATNR
          ,resultOrdersDeliveryDF.Date
          ,resultOrdersDeliveryDF.QTY
          ,resultOrdersDeliveryDF.TYPE
         )\
  .where(col('DMDGROUP') !='N')

resultOrdersDeliveryMaterialsDF = resultOrdersDeliveryShipToDF\
  .join(materialFilteredDF, 'MATNR', 'left')\
  .select(\
          resultOrdersDeliveryShipToDF.DMDGROUP
          ,resultOrdersDeliveryShipToDF.Date
          ,resultOrdersDeliveryShipToDF.QTY
          ,resultOrdersDeliveryShipToDF.TYPE
          ,regexp_replace(materialDf.ZREP, r'^[0]*', '').alias('ZREP') 
          ,materialDf.BrandTech
         )

currentMarsWeek = datesDF.select(col('MarsWeekFullName'), col('OriginalDate')).where(col('OriginalDate') == today).drop('OriginalDate').first()[0]
previousMarsWeek = datesDF.select(col('MarsWeekFullName'), col('OriginalDate')).where(col('OriginalDate') == (today - timedelta(7))).drop('OriginalDate').first()[0]

resultDF = resultOrdersDeliveryMaterialsDF\
  .join(datesDF, resultOrdersDeliveryMaterialsDF.Date == datesDF.OriginalDate, 'left')\
  .select(\
          resultOrdersDeliveryMaterialsDF.DMDGROUP
          ,resultOrdersDeliveryMaterialsDF.ZREP
          ,datesDF.MarsWeekFullName.alias('MARSDATE')
          ,resultOrdersDeliveryMaterialsDF.Date
          ,resultOrdersDeliveryMaterialsDF.BrandTech
          ,resultOrdersDeliveryMaterialsDF.QTY
          ,resultOrdersDeliveryMaterialsDF.TYPE
         )\
  .groupBy(['DMDGROUP', 'ZREP', 'MARSDATE', 'TYPE'])\
  .agg(sum('QTY').alias('QTY'), first('BrandTech').alias('BrandTech'), first('Date').alias('Date'))\
  .where(col('MARSDATE') == currentMarsWeek)

fullWeekActualsDF = resultOrdersDeliveryMaterialsDF\
  .join(datesDF, resultOrdersDeliveryMaterialsDF.Date == datesDF.OriginalDate, 'left')\
  .select(\
          resultOrdersDeliveryMaterialsDF.DMDGROUP
          ,resultOrdersDeliveryMaterialsDF.ZREP
          ,datesDF.MarsWeekFullName.alias('MARSDATE')
          ,resultOrdersDeliveryMaterialsDF.Date
          ,resultOrdersDeliveryMaterialsDF.BrandTech
          ,resultOrdersDeliveryMaterialsDF.QTY
          ,resultOrdersDeliveryMaterialsDF.TYPE
         )\
  .groupBy(['DMDGROUP', 'ZREP', 'MARSDATE', 'TYPE'])\
  .agg(sum('QTY').alias('QTY'), first('BrandTech').alias('BrandTech'), first('Date').alias('Date'))\
  .where((col('MARSDATE') == previousMarsWeek) & (col('TYPE') == 'ACTUAL'))

resultDF.write.mode("overwrite").parquet(ORDERS_DELIVERY_FDM_OUTPUT_PATH)
fullWeekActualsDF.write.mode("overwrite").parquet(FULL_WEEK_ACTUAL_OUTPUT_PATH)