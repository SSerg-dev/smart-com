#JUPITER raw data quality check
import sys
from pyspark import SparkConf, SparkContext
from pyspark.sql import SparkSession
from pyspark.sql.types import *
from pyspark.sql.functions import *
import pyspark.sql.functions as F
import pandas as pd
from datetime import timedelta, datetime, date
import datetime as datetime
import os
import subprocess
import math

conf = SparkConf().setAppName("Jupiter - PySpark")
sc = SparkContext(conf=conf)
spark = SparkSession(sc)

import EXTRACT_SETTING as es
import QUALITYCHECK_HELPER as qh

RAW_DIR = es.SETTING_RAW_DIR
DATE_DIR=es.DATE_DIR

EXTRACT_ENTITIES_AUTO_PATH = f'{es.HDFS_PREFIX}{es.MAINTENANCE_PATH_PREFIX}EXTRACT_ENTITIES_AUTO.csv'
processDate=es.processDate
pipelineRunId=es.pipelineRunId

print(f'EXTRACT_ENTITIES_AUTO_PATH={EXTRACT_ENTITIES_AUTO_PATH}')


extractEntities = spark.read.csv(EXTRACT_ENTITIES_AUTO_PATH, header= True, sep='\u0001').select('Schema','Method','EntityName').collect()

for extEnt in extractEntities:
  if extEnt.EntityName=='__MigrationHistory': continue
  
  startDate = datetime.datetime.now()

  RAW_FILE_PATH = RAW_DIR+'/'+DATE_DIR+'/'+extEnt.Schema+'/'+extEnt.EntityName+'/'+extEnt.Method+'/'+extEnt.EntityName+'.csv'


  try:
    
    endDate = datetime.datetime.now()
    duration = math.ceil((endDate-startDate).total_seconds())

    # read raw file
    rawDF = spark.read.csv(RAW_FILE_PATH, header=True, sep='\u0001')
    estimateCount = rawDF.count()
    QCCount = rawDF.select('#QCCount').first()

    if not QCCount is None:
      QCCount = int(QCCount[0])
    else:
      QCCount = 0

    # Row count quality check. Compare fact uploaded rows with estimated rows
    print(f'es={estimateCount},up={QCCount}')
    if int(estimateCount)!=int(QCCount):
      print('Estimate count', estimateCount,' actual ',QCCount)
      errorDescription = 'Estimate count = '+str(estimateCount)+' actual = '+str(QCCount)
      
      qh.appendQualityCheckDetail(
      es.MAINTENANCE_PATH_PREFIX
     ,processDate
     ,pipelineRunId
     ,extEnt.Schema
     ,extEnt.EntityName
     ,RAW_FILE_PATH
     ,'csv'
     ,qh.QC_STATUS_FAILURE
   )
      qh.appendFailureQualityCheckLog(
          es.MAINTENANCE_PATH_PREFIX
         ,processDate
         ,pipelineRunId
         ,extEnt.Schema     
         ,extEnt.EntityName
         ,qh.QC_ROW_COUNT_CHECK_CODE
         ,'False'
         ,'boolean'
         ,1
         ,startDate.strftime('%Y-%m-%d %H:%M:%S')
         ,duration
         ,'Row count quality check between estimate rows and fact uploaded rows'
         ,errorDescription
         ,qh.QC_STATUS_FAILURE
       )            
      
    else:
      qh.appendQualityCheckDetail(
      es.MAINTENANCE_PATH_PREFIX
     ,processDate
     ,pipelineRunId
     ,extEnt.Schema
     ,extEnt.EntityName
     ,RAW_FILE_PATH
     ,'csv'
     ,qh.QC_STATUS_COMPLETE
   )
      qh.appendQualityCheckLog(
          es.MAINTENANCE_PATH_PREFIX
         ,processDate
         ,pipelineRunId
         ,extEnt.Schema     
         ,extEnt.EntityName
         ,qh.QC_ROW_COUNT_CHECK_CODE
         ,'True'
         ,'boolean'
         ,1
         ,startDate.strftime('%Y-%m-%d %H:%M:%S')
         ,duration
         ,'Row count quality check between estimate rows and fact uploaded rows'
         ,''
         ,qh.QC_STATUS_COMPLETE
       )            
      
  except Exception as e:
      print(f'ERROR={str(e)}')
      qh.appendQualityCheckDetail(
      MAINTENANCE_PATH_PREFIX
     ,processDate
     ,pipelineRunId
     ,extEnt.Schema
     ,extEnt.EntityName
     ,RAW_FILE_PATH
     ,'csv'
     ,qh.QC_STATUS_FAILURE
   )
      qh.appendFailureQualityCheckLog(
          MAINTENANCE_PATH_PREFIX
         ,processDate
         ,pipelineRunId
         ,extEnt.Schema     
         ,extEnt.EntityName
         ,qh.QC_ROW_COUNT_CHECK_CODE
         ,'False'
         ,'boolean'
         ,1
         ,startDate.strftime('%Y-%m-%d %H:%M:%S')
         ,-1
         ,'Row count quality check between estimate rows and fact uploaded rows'
         ,'Error during execute notebook'
         ,qh.QC_STATUS_FAILURE
     )  

QC_DETAIL_DIR = f'{es.HDFS_PREFIX}{es.MAINTENANCE_PATH_PREFIX}QC_DETAIL.CSV/*/'
check_df=spark.read.csv(QC_DETAIL_DIR, header= True, sep='\u0001')
if check_df.where(col("Status") == qh.QC_STATUS_FAILURE).count() > 0:
 raise Exception("RAW QC ERROR! Check QC detail files.")
