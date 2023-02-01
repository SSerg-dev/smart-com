from pyspark.sql import SQLContext, DataFrame, Row, Window
from pyspark.sql import SparkSession
from pyspark.sql.types import *
from pyspark.sql.functions import *
import pyspark.sql.functions as F
import pandas as pd
import datetime, time
import os
import json

spark = SparkSession.builder.appName('Jupiter - Promo').getOrCreate()
print("PROMO_PREPARE")

