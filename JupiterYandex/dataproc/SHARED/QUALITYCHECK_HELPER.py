#Quality check helper functions
from pyspark import SparkConf, SparkContext
from pyspark.sql import SparkSession
from pyspark.sql.types import *
from pyspark.sql import Row

sc = SparkContext.getOrCreate();
spark = SparkSession(sc)

HDFS_PREFIX = 'hdfs://'
QC_STATUS_COMPLETE='COMPLETE'
QC_STATUS_FAILURE='FAILURE'

# constant for row count quality check code
QC_ROW_COUNT_CHECK_CODE = 10000

# constant for field convert consolidation 
QC_FIELD_CONVERT_CHECK_CODE = 30001
QC_SCHEMA_CHECK_CODE = 30101

# constant for file quality check code
QC_FILE_EXISTS_CHECK_CODE = 50000
QC_FILE_NOT_EMPTY_CHECK_CODE = 50001

def appendQualityCheckDetail(
   maintenancePathPrefix
  ,dateStamp  
  ,pipelineRunId
  ,schema
  ,entityName
  ,entityPath
  ,entityFormat
  ,status
):
 df_schema = StructType(
         [StructField('PipelineRunId', StringType()),
         StructField('Schema', StringType()),
         StructField('EntityName', StringType()),
         StructField('EntityPath', StringType()),
         StructField('EntityFormat', StringType()),
         StructField('Status', StringType()),
         ])
 rows = [Row(PipelineRunId=pipelineRunId,Schema=schema,EntityName=entityName,EntityPath=entityPath,EntityFormat=entityFormat,Status=status)]
 df = spark.createDataFrame(rows, df_schema)
 
 QC_DETAIL_DIR = f'{HDFS_PREFIX}{maintenancePathPrefix}QC_DETAIL.CSV'
 print(QC_DETAIL_DIR)
 QC_DETAIL_PATH = f'{QC_DETAIL_DIR}/{schema}.{entityName}.csv'
 df.repartition(1).write.csv(path=QC_DETAIL_PATH,header="true", mode="overwrite", sep="\u0001")
 
 
def appendQualityCheckLog(
   maintenancePathPrefix
  ,dateStamp  
  ,pipelineRunId
  ,schema
  ,entityName
  ,checkCode
  ,checkValue
  ,checkValueType
  ,orderNo
  ,startDate
  ,duration
  ,description
  ,errorDescription
  ,status
):
   df_schema = StructType(
         [StructField('PipelineRunId', StringType()),
         StructField('Schema', StringType()),
         StructField('EntityName', StringType()),
         StructField('CheckCode', StringType()),
         StructField('CheckValue', StringType()),
         StructField('CheckValueType', StringType()),
         StructField('OrderNo', StringType()),
         StructField('StartDate', StringType()),
         StructField('Duration', StringType()),
         StructField('Status', StringType()),
         StructField('Description', StringType()),
         StructField('ErrorDescription', StringType()),
         ])                             

   rows = [Row(PipelineRunId=pipelineRunId,
  Schema=schema,
  EntityName=entityName,
  CheckCode=checkCode,
  CheckValue=checkValue,
  CheckValueType=checkValueType,
  OrderNo=orderNo,
  StartDate=startDate,
  Duration=duration,
  Status=status,
  Description=description,
  ErrorDescription=errorDescription
  )]
  
   df = spark.createDataFrame(rows, df_schema)
 
   QC_LOG_DIR = f'{HDFS_PREFIX}{maintenancePathPrefix}QC_LOG.CSV'
   print(QC_LOG_DIR)
   QC_LOG_PATH = f'{QC_LOG_DIR}/{schema}.{entityName}.csv'
   df.repartition(1).write.csv(path=QC_LOG_PATH,header="true", mode="overwrite", sep="\u0001")
 
