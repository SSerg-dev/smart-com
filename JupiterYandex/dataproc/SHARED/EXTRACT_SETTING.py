#Shared parameters extraction
import os
import datetime
import json
import sys
from pyspark import SparkConf, SparkContext
from pyspark.sql import SparkSession
from pyspark.sql.types import *
from pyspark.sql.functions import *

try:
 sc = SparkContext.getOrCreate();
 spark = SparkSession(sc)
 print(f'INPUT_PARAMS={sys.argv[1]}')

 input_params=json.loads(sys.argv[1])

 PARAMETERS_FILE = 'PARAMETERS.csv'
 HDFS_PREFIX = 'hdfs://'


 processDate = input_params.get("ProcessDate")
 pipelineName = input_params.get("PipelineName")
 processDateParam = datetime.datetime.strptime(processDate, "%Y-%m-%d")
 DATE_DIR = processDateParam.strftime("%Y/%m/%d")

 SETTINGS_PATH = f'{HDFS_PREFIX}{input_params["MaintenancePathPrefix"]}{PARAMETERS_FILE}'
 print(f'SETTINGS_PATH={SETTINGS_PATH}')

 systemParametersDF = spark.read.csv(SETTINGS_PATH, header= True, sep='\u0001')
 MAINTENANCE_PATH_PREFIX=input_params["MaintenancePathPrefix"]

                                       
 pipelineRunId = systemParametersDF.filter(col("Key")=="RunId").collect()[0].Value
 SETTING_RAW_DIR = f'{HDFS_PREFIX}{systemParametersDF.filter(col("Key")=="RawPath").collect()[0].Value}'
 SETTING_PROCESS_DIR =  f'{HDFS_PREFIX}{systemParametersDF.filter(col("Key")=="ProcessPath").collect()[0].Value}'
 SETTING_OUTPUT_DIR = f'{HDFS_PREFIX}{systemParametersDF.filter(col("Key")=="OutputPath").collect()[0].Value}'
 SYSTEM_NAME = systemParametersDF.filter(col("Key")=='SystemName').collect()[0].Value
 pipelineSubfolderName = pipelineName

 temp = systemParametersDF.filter(col("Key")=='Branch').collect()
 if temp:
  SETTING_BRANCH_NAME = temp[0].Value
 else:
  SETTING_BRANCH_NAME = ''


 print(SETTINGS_PATH)  
 print('')  
 print('processDate:',processDate)  
 print('pipelineRunId:',pipelineRunId)
 print('pipelineSubfolderName:',pipelineSubfolderName)
 print('')
 print('SETTING_RAW_DIR:',SETTING_RAW_DIR)
 print('SETTING_PROCESS_DIR:',SETTING_PROCESS_DIR)
 print('SETTING_OUTPUT_DIR:',SETTING_OUTPUT_DIR)
 print('MAINTENANCE_PATH_PREFIX:',MAINTENANCE_PATH_PREFIX)
 
 print('SYSTEM_NAME:',SYSTEM_NAME)
 print('SETTING_BRANCH_NAME:',SETTING_BRANCH_NAME)
 print('Other settings - list "systemParametersDF" data frame')
except Exception as e:
 print(f'EXTRACT_SETTING_ERROR={str(e)}') 
