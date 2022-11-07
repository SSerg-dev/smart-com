# Get CUSTOMERS data from RDF

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
import datetime as datetime
from datetime import timedelta
from pyspark.sql.functions import *
import pyspark.sql.functions as sf
from pyspark.sql.types import LongType, StringType, StructField, StructType, BooleanType, ArrayType, IntegerType
from pyspark.sql import Row
import os

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

#Getting date string
datestring = datetime.datetime.now()
datestring = datestring.strftime('%Y/%m/%d')
print(datestring)

today = datetime.datetime.today()
print(today)

INPUT_CUSTOMERS_PATH = SETTING_RAW_DIR + '/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_CUSTOMERS.csv'
OUTPUT_CUSTOMERS_PARQUET_PATH = SETTING_OUTPUT_DIR + '/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_CUSTOMERS.CSV'

customersDF = spark.read.format("csv").option("header", "true").option("delimiter", "\u0001").load(INPUT_CUSTOMERS_PATH)

customersDF = customersDF\
  .withColumn('SP_Description', col('SP Description'))\
  .drop('SP Description', 'Division')

customersDF.count()

# customersDF.write.mode("overwrite").parquet(OUTPUT_CUSTOMERS_PARQUET_PATH)

customersDF\
.withColumn("Active_From",to_date(col("Active_From"),"yyyy/MM/dd"))\
.withColumn("Active_Till",to_date(col("Active_Till"),"yyyy/MM/dd"))\
.repartition(1)\
.write.csv(OUTPUT_CUSTOMERS_PARQUET_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss",
escape="",
quote="",
)