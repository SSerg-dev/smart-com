# Get material data from RDF. Filters by date, and leaves only active materials.

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

INPUT_MATERIALS_PATH = SETTING_RAW_DIR + '/SOURCES/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_MATERIALS.csv'
OUTPUT_MATERIALS_PARQUET_PATH = SETTING_OUTPUT_DIR + '/UNIVERSALCATALOG/MARS_UNIVERSAL_PETCARE_MATERIALS.CSV'

SKU_libraryDF = spark.read.format("csv").option("header", "true").option("delimiter", "\u0001").load(INPUT_MATERIALS_PATH)

SKU_libraryDF.count()

SKU_libraryDF=SKU_libraryDF\
              .withColumn("START_DATE",to_date(col("START_DATE")))\
              .withColumn("END_DATE",to_date(col("END_DATE")))\
              .where((col("START_DATE") <= today )&(col("END_DATE") >=today)&(col("0DIVISION")==5))

SKU_libraryDF=SKU_libraryDF.select('VKORG','MATNR','VMSTD','0CREATEDON','0DIVISION','0DIVISION___T','MATERIAL','SKU','0MATL_TYPE___T','0UNIT_OF_WT','GGROSS_WT','GNET_WT','Brand','Segmen','Technology','BrandTech','BrandTech_code','Brand_code','Tech_code','ZREP','EAN_Case','EAN_PC','Brand_Flag_abbr','Brand_Flag','Submark_Flag','Ingredient_variety','Product_Category','Product_Type','Supply_Segment','Functional_variety','Size','Brand_essence','Pack_Type','Traded_unit_format','Consumer_pack_format','UOM_PC2Case','Segmen_code','BrandsegTech_code','count','Brandsegtech','SubBrand','SubBrand_code','BrandSegTechSub','BrandSegTechSub_code','START_DATE','END_DATE')
SKU_libraryDF.count()

SKU_libraryDF=SKU_libraryDF.fillna({"Technology":"Not Applicable","BrandTech":"Not Applicable","BrandTech_code":" ","Tech_code":" ","BrandsegTech_code":" ","BrandSegTech":"Not Applicable"})

# display(SKU_libraryDF.select("BrandSegTech").distinct())

SKU_libraryDF.where(col("Technology").isNull()).count()

SKU_libraryDF.count()

# SKU_libraryDF.write.mode("overwrite").parquet(OUTPUT_MATERIALS_PARQUET_PATH)

SKU_libraryDF\
.withColumn("VMSTD",to_date(col("VMSTD"),"yyyy.MM.dd"))\
.repartition(1)\
.write.csv(OUTPUT_MATERIALS_PARQUET_PATH,
sep="\u0001",
header=True,
mode="overwrite",
emptyValue="",
timestampFormat="yyyy-MM-dd HH:mm:ss"
)