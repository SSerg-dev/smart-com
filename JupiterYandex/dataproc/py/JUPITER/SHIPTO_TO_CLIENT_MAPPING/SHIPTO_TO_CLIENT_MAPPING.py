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
from datetime import timedelta, date, datetime
from pyspark.sql.types import *
from pyspark.sql.functions import *

if is_notebook():
 sys.argv=['','{"MaintenancePathPrefix": "/JUPITER/OUTPUT/#MAINTENANCE/2022-09-09_manual__2022-09-09T11%3A15%3A40%2B00%3A00_", "ProcessDate": "2022-09-12", "Schema": "Jupiter", "PipelineName": "jupiter_shipto_client_mapping"}']
 
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


CUSTOMER_ATTR_DATA = SETTING_RAW_DIR + "/SOURCES/HYDRATEATLAS/0CUSTOMER_ATTR/DATA/"
CUSTOMER_TEXT_DATA = SETTING_RAW_DIR + "/SOURCES/HYDRATEATLAS/0CUSTOMER_TEXT/DATA/"
CUST_SALES_ATTR_DATA = SETTING_RAW_DIR + "/SOURCES/HYDRATEATLAS/0CUST_SALES_ATTR/DATA/"
CUST_SALES_TEXT_DATA = SETTING_RAW_DIR + "/SOURCES/HYDRATEATLAS/0CUST_SALES_TEXT/DATA/"
CUST_HIER = SETTING_RAW_DIR + "/SOURCES/HYDRATEATLAS/0CUST_SALES_LKDH_HIER_T_ELEMENTS/DATA/"
ZCUSTOPF_ATTR = SETTING_RAW_DIR + "/SOURCES/HYDRATEATLAS/ZCUSTOPF_ATTR/DATA/"

RESULT_MAPPING_PARQ = SETTING_PROCESS_DIR + "/SHIPTO_TO_CLIENT_MAPPING/SHIPTO_TO_CLIENT_MAPPING.PARQUET"


custHierSchema = StructType([StructField("HIENM", StringType(), False),
                              StructField("HCLASS", StringType(), False),
                              StructField("DATEFROM", StringType(), False),
                              StructField("DATETO", StringType(), False),
                              StructField("HEADERID", StringType(), False),
                              StructField("NODEID", StringType(), False),
                              StructField("IOBJNM", StringType(), False),
                              StructField("NODENAME", StringType(), False),
                              StructField("TLEVEL", StringType(), False),
                              StructField("LINK", StringType(), False),
                              StructField("PARENTID", StringType(), False),
                              StructField("CHILDID", StringType(), False),
                              StructField("NEXTID", StringType(), False),
                              StructField("ODQ_CHANGEMODE", StringType(), False),
                              StructField("ODQ_ENTITYCNTR", StringType(), False),
                              StructField("LOAD_DATE", StringType(), False),
                              StructField("LOAD_DATETIME", StringType(), False),
                              StructField("INPUT_FILE_NAME", StringType(), False)])

custopfSchema = StructType([StructField('KUNNR_PK', StringType(), False),
                            StructField('VKORG_PK', StringType(), False),
                            StructField('VTWEG_PK', StringType(), False),
                            StructField('SPART_PK', StringType(), False),
                            StructField('PARVW_PK', StringType(), False),
                            StructField('PARZA_PK', StringType(), False),
                            StructField('KUNN2_PK', StringType(), False),
                            StructField('KTOKD_PK', StringType(), False),
                            StructField('KNREF_PK', StringType(), False),
                            StructField('ODQ_CHANGEMODE', StringType(), False),
                            StructField('ODQ_ENTITYCNTR', StringType(), False),
                            StructField('LOAD_DATE', StringType(), False),
                            StructField('LOAD_DATETIME', StringType(), False),
                            StructField('INPUT_FILE_NAME', StringType(), False)])

customerTextSchema = StructType([StructField('KUNNR_PK' , StringType(), False),
                              StructField('TXTMD' , StringType(), False),
                              StructField('TXTLG' , StringType(), False),
                              StructField('ODQ_CHANGEMODE' , StringType(), False),
                              StructField('ODQ_ENTITYCNTR' , StringType(), False),
                              StructField('LOAD_DATE' , StringType(), False),
                              StructField('LOAD_DATETIME' , StringType(), False),
                              StructField('INPUT_FILE_NAME' , StringType(), False)])

custHierDf = spark.read.schema(custHierSchema).parquet(CUST_HIER)
customerTextDf = spark.read.schema(customerTextSchema).parquet(CUSTOMER_TEXT_DATA)
custopfDf = spark.read.schema(custopfSchema).parquet(ZCUSTOPF_ATTR)

custHierDf = custHierDf.\
withColumn('DATEFROM',to_date(col('DATEFROM'),'yyyy.MM.dd')).\
withColumn('DATETO',to_date(col('DATETO'),'yyyy.MM.dd')).\
withColumn('TLEVEL',col('TLEVEL').cast(IntegerType()))


custHierDf = custHierDf.select(col("NODEID").alias("NODEID"),\
    col("IOBJNM").alias("InfoObject(IOBJNM)"),\
    col("NODENAME").alias("NODENAME"),\
    col("TLEVEL").alias("Le"),\
    col("LINK").alias("L"),\
    col("PARENTID").alias("Parent ID"),\
    col("CHILDID").alias("Child ID"),\
    col("NEXTID").alias("Next ID"),\
    col("DATEFROM").alias("Active_From"),\
    col("DATETO").alias("Active_Till")).where(col("HIENM")=="G")

custHierDf = custHierDf.\
    withColumn('H_DIV',regexp_extract('NODENAME', '(^.{0,2})(.{2,2})(.{2,3})', 1)).\
    withColumn('H_CH',regexp_extract('NODENAME', '(^.{0,2})(.{2,2})(.{2,3})', 2)).\
    withColumn('H_SO',regexp_extract('NODENAME', '(^.{0,2})(.{2,2})(.{2,3})', 3)).\
    withColumn('H_PTR',regexp_extract('NODENAME', '(.{10})\\s*$', 1))
custHierDf=custHierDf.where( (col('H_DIV') == '51') & (col('H_SO') == '261') & (col('H_CH').isin({'33','22','11'})))

childParentDf = custHierDf.select(col('NODEID').alias('NODEID'),col('Parent ID').alias('PARENTID'),col('H_PTR').alias('NODEADDR'))

custHierLvlPrevDf = custHierDf.join(childParentDf,(custHierDf['NODEID']==childParentDf.NODEID) & (custHierDf['Le']==5) | (custHierDf['NODEID']==childParentDf.NODEID) & (custHierDf['Le']==4), how='left').drop(childParentDf['NODEID'])

custHierLvlPrevDf = custHierLvlPrevDf\
  .withColumn('Lvl5',when(col('Le')==5,col('NODEID')))\
  .withColumn('Lvl4',when(col('Le')==5,col('PARENTID'))\
  .otherwise(when(col('Le')==4,col('NODEID'))\
  .otherwise(None))).drop('PARENTID','NODEADDR')

custHierLvlPrevDf = custHierLvlPrevDf.join(childParentDf\
                                         ,custHierLvlPrevDf['Lvl4']==childParentDf.NODEID,how='left').drop(childParentDf['NODEID'])

custHierLvlPrevDf = custHierLvlPrevDf.withColumnRenamed('PARENTID','lvl4parent').drop('NODEADDR')

for l in range(4,2,-1):
    custHierLvlCurDf = custHierLvlPrevDf.join(childParentDf\
        ,((custHierLvlPrevDf['NODEID']==childParentDf.NODEID) & (custHierLvlPrevDf['Le']>=(l))\
          |(custHierLvlPrevDf['NODEID']==childParentDf.NODEID) & (custHierDf['Le']==(l-1))),how='left').drop(childParentDf['NODEID'])
    custHierLvlCurDf = custHierLvlCurDf.withColumn('Lvl'+str(l-1)\
                                                 ,when(col('Le')>=l,col('lvl'+str(l)+'parent'))\
                                                 .otherwise(when(col('Le')==(l-1),col('NODEID'))\
                                                 .otherwise(None))).drop('PARENTID','NODEADDR')
    
    custHierLvlCurDf = custHierLvlCurDf.join(childParentDf,custHierLvlCurDf['Lvl'+str(l-1)]==childParentDf.NODEID,how='left').drop(childParentDf['NODEID'])
    custHierLvlCurDf = custHierLvlCurDf.withColumnRenamed('PARENTID','Lvl'+str(l-1)+'parent').drop('NODEADDR')
    custHierLvlPrevDf = custHierLvlCurDf
    
custHierLvlCurDf = custHierLvlCurDf.drop('lvl4parent','lvl3parent','lvl2parent')    
  
for l in range(2,6):  
  custHierLvlCurDf = custHierLvlCurDf.join(childParentDf,custHierLvlCurDf['Lvl'+str(l)]==childParentDf['NODEID'],how='left')
  custHierLvlCurDf = custHierLvlCurDf.withColumnRenamed('NODEADDR','ZCUSTHG0'+str(l-1)).drop('NODEID').drop('PARENTID').drop('Lvl'+str(l))


custHierDupDf = custHierDf.select(col('NODEID').alias('NODE_ID'),col('H_PTR').alias('G_H_ParentID'))
custHierLvlCurDf = custHierLvlCurDf.join(custHierDupDf,custHierLvlCurDf['Parent ID']==custHierDupDf['NODE_ID'],how='left')
custHierLevels1to5Df = custHierLvlCurDf\
.select(col('H_PTR').alias('0CUST_SALES'),\
        col('H_DIV').alias('Division'),\
        col('H_SO').alias('0SALESORG'),\
        col('H_CH').alias('0DISTR_CHAN'),\
        'ZCUSTHG01','ZCUSTHG02','ZCUSTHG03','ZCUSTHG04',\
        col('Active_From'),col('Active_Till'),'G_H_ParentID',col('Le').alias('G_H_level'))

custHierLevels1to5Df = custHierLevels1to5Df.withColumn('G_H_level',col('G_H_level') - 1)


custopfZaDf = custopfDf.where( (col('VKORG_PK') == '261') \
                            & (col('VTWEG_PK').isin({'33','22','11'})) \
                            & col('PARVW_PK').isin({'ZA'}) \
                            & (col('PARZA_PK') == '0') \
                            & (col('KTOKD_PK').isin('0001', '0002')) \
                            & (col('SPART_PK') == '51' ))

custHierLevels1to5PlusShiptoDf = custHierLevels1to5Df.join(custopfZaDf,\
                                                         ((custopfZaDf.KUNN2_PK == custHierLevels1to5Df['0CUST_SALES']) \
                                                          & (custopfZaDf.VKORG_PK == custHierLevels1to5Df['0SALESORG']) \
                                                          & (custopfZaDf.VTWEG_PK == custHierLevels1to5Df['0DISTR_CHAN']) \
                                                          & (custopfZaDf.SPART_PK == custHierLevels1to5Df.Division)),how='inner')

custHierLevels1to5PlusShiptoDf = custHierLevels1to5PlusShiptoDf.withColumn('G_H_level',col('G_H_level')+1)\
    .withColumn('G_H_ParentID',col('0CUST_SALES'))\
    .withColumn('0CUST_SALES',col('KUNNR_PK'))

custHierLevels1to5PlusShiptoDf = custHierLevels1to5PlusShiptoDf.\
select('0CUST_SALES','Division','0SALESORG','0DISTR_CHAN','ZCUSTHG01',\
       'ZCUSTHG02','ZCUSTHG03','ZCUSTHG04','Active_From','Active_Till','G_H_ParentID','G_H_level')


hydratePlusShiptoDf = custHierLevels1to5Df.union(custHierLevels1to5PlusShiptoDf)
customerFilteredDf = customerTextDf.select('KUNNR_PK','TXTMD')\
        .withColumn('TXTMD',regexp_replace('TXTMD', 'RU$', ''))\
        .withColumn('TXTMD',regexp_replace('TXTMD', ' $', ''))

hydratePlusShiptoDf = hydratePlusShiptoDf.join(customerFilteredDf,hydratePlusShiptoDf.ZCUSTHG01==customerFilteredDf.KUNNR_PK,how='left')\
.withColumnRenamed('TXTMD','ZCUSTHG01___T').drop('KUNNR_PK')
hydratePlusShiptoDf = hydratePlusShiptoDf.join(customerFilteredDf,hydratePlusShiptoDf.ZCUSTHG02==customerFilteredDf.KUNNR_PK,how='left')\
.withColumnRenamed('TXTMD','ZCUSTHG02___T').drop('KUNNR_PK')
hydratePlusShiptoDf = hydratePlusShiptoDf.join(customerFilteredDf,hydratePlusShiptoDf.ZCUSTHG03==customerFilteredDf.KUNNR_PK,how='left')\
.withColumnRenamed('TXTMD','ZCUSTHG03___T').drop('KUNNR_PK')
hydratePlusShiptoDf = hydratePlusShiptoDf.join(customerFilteredDf,hydratePlusShiptoDf.ZCUSTHG04==customerFilteredDf.KUNNR_PK,how='left')\
.withColumnRenamed('TXTMD','ZCUSTHG04___T').drop('KUNNR_PK')

hydratePlusShiptoDf = hydratePlusShiptoDf.join(customerFilteredDf,\
                                             hydratePlusShiptoDf['0CUST_SALES']==customerFilteredDf.KUNNR_PK,how='left')\
.withColumnRenamed('TXTMD','0CUST_SALES___T').drop('KUNNR_PK')

hydratePlusShiptoDf = hydratePlusShiptoDf.\
select('0CUST_SALES','0CUST_SALES___T','Division','0SALESORG','0DISTR_CHAN','ZCUSTHG01','ZCUSTHG01___T','ZCUSTHG02','ZCUSTHG02___T','ZCUSTHG03','ZCUSTHG03___T','ZCUSTHG04','ZCUSTHG04___T','Active_From','Active_Till','G_H_ParentID','G_H_level')

hydratePlusShiptoDf = hydratePlusShiptoDf.where(col('G_H_level') == 5)


custopfSpShDf = custopfDf.where( (col('VKORG_PK') == '261')\
                              & (col('VTWEG_PK').isin({'33','22','11'})) \
                              & (col('PARVW_PK') == 'SH') \
                              & (col('SPART_PK') == '51') \
                              & (col('KTOKD_PK').isin('0001', '0002')))

custopfSpShDf = custopfSpShDf.select(col('KUNNR_PK').alias('SoldToPoint'),'KUNN2_PK','PARVW_PK')

soldToCountDf = custopfSpShDf.groupBy("SoldToPoint","KUNN2_PK").agg(countDistinct("PARVW_PK").alias('count'))\
.select(col('SoldToPoint').alias('SoldToPoint2'),col('KUNN2_PK').alias('KUNN2_PK2'),'count')

soldToCount1Df = custopfSpShDf.join(soldToCountDf,\
                                  (custopfSpShDf.SoldToPoint == soldToCountDf.SoldToPoint2) \
                                  & (custopfSpShDf.KUNN2_PK == soldToCountDf.KUNN2_PK2) & (soldToCountDf['count']==1))

soldToCount2Df = custopfSpShDf.join(soldToCountDf,\
                                  (custopfSpShDf.SoldToPoint == soldToCountDf.SoldToPoint2) & (custopfSpShDf.KUNN2_PK == soldToCountDf.KUNN2_PK2)\
                                  & (soldToCountDf['count']==2) & (custopfSpShDf.PARVW_PK=='SH'))

soldToDf = soldToCount1Df.union(soldToCount2Df).drop('SoldToPoint2','KUNN2_PK2','PARVW_PK','count')

soldToTextDf = soldToDf.join(customerFilteredDf,\
                           soldToDf.SoldToPoint==customerFilteredDf['KUNNR_PK'],how='left').withColumnRenamed('TXTMD','SP_Description')


hydratePlusShiptoAndSoldToDf = hydratePlusShiptoDf.join(soldToTextDf,\
                                                      hydratePlusShiptoDf['0CUST_SALES']==soldToTextDf.KUNN2_PK,how='left').drop('KUNN2_PK')
                                                      
hydratePlusShiptoAndSoldToDf = hydratePlusShiptoAndSoldToDf.withColumn("0CUST_SALES",\
                                                                       hydratePlusShiptoAndSoldToDf["0CUST_SALES"].cast(IntegerType()))

hydratePlusShiptoAndSoldToDf = hydratePlusShiptoAndSoldToDf.\
    select('0CUST_SALES','0DISTR_CHAN','SoldToPoint','ZCUSTHG03___T','SP_Description','Active_From','Active_Till')


currentDate = datetime.today()
hydratePlusShiptoAndSoldToDf = hydratePlusShiptoAndSoldToDf.where( col('SoldToPoint').isNotNull() ).dropDuplicates()

shipToMappingDf = hydratePlusShiptoAndSoldToDf.where( (col('Active_From') <= currentDate) & (col('Active_Till') >= currentDate) ).\
    withColumnRenamed('ZCUSTHG03___T','GRP_DESC').\
    withColumnRenamed('0DISTR_CHAN','Channel')


shipToMappingDf\
.write.parquet(RESULT_MAPPING_PARQ,
mode="overwrite"
)