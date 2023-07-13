# Databricks notebook source
from pyspark import SparkConf, SparkContext, SparkFiles
from pyspark.sql.types import StructType
import json

sc = SparkContext.getOrCreate();
'''
Recurively list catalogs and return files name with full path 
'''
def getFilesList(path):
  files = []
  
  def getFileSizeRecursive(path):
    list = dbutils.fs.ls(path)
    for x in list: 
      if x.isFile():
        files.append(x.path)
    for path in [x.path.replace('dbfs:/','') for x in list if x.isDir()]:
      getFileSizeRecursive(path)
    
  try:  
    getFileSizeRecursive(path)
    return files
  except:
    return files
  

def getShowString(df, n=20, truncate=True, vertical=False):
    if isinstance(truncate, bool) and truncate:
        return(df._jdf.showString(n, 20, vertical))
    else:
        return(df._jdf.showString(n, int(truncate), vertical))
"""
returns dictionary, where key - file name without extension, and value - Spark Schema Object
"""
def getSchemasMap(schemas_dir):
    schema_files=['ActualCOGS.json','ActualCOGSTn.json','ActualTradeInvestment.json','AssortmentMatrix.json','BTL.json','BTLPromo.json','BaseLine.json','BrandTech.json','BudgetItem.json','BudgetSubItem.json','COGS.json','ChangesIncident.json','ClientTree.json','ClientTreeBrandTech.json','ClientTreeHierarchyView.json','IncrementalPromo.json','PlanCOGSTn.json','PlanIncrementalReport.json','PriceList.json','Product.json','ProductChangeIncident.json','ProductTree.json','Promo.json','PromoPriceIncrease.json','PromoProduct.json','PromoProductPriceIncrease.json','PromoProductTree.json','PromoProductsCorrection.json','PromoProductCorrectionPriceIncrease.json','PromoStatus.json','PromoSupport.json','PromoSupportPromo.json','RATIShopper.json','RollingVolume.json','ServiceInfo.json','Setting.json','TradeInvestment.json','PlanPostPromoEffect.json']

    schemas_map = {}
    for f in schema_files:
        sc.addFile(f'{schemas_dir}{f}')

        with open(SparkFiles.get(f), 'rb') as handle:
            schemas_map[f.replace('.json','')]=StructType.fromJson(json.load(handle))
    return schemas_map  

"""
Get schemas for Scenario
returns dictionary, where key - file name without extension, and value - Spark Schema Object
"""
def getSchemasMapScenario(schemas_dir):
    schema_files=['BTL.json','BTLPromo.json','Brand.json','BrandTech.json','Budget.json','BudgetItem.json','BudgetSubItem.json','ClientTree.json','Color.json','Event.json','Mechanic.json','MechanicType.json','Product.json','ProductTree.json','Promo.json','PromoProduct.json','PromoProductTree.json','PromoProductsCorrection.json','PromoStatus.json','PromoSupport.json','PromoSupportPromo.json','PromoTypes.json','RejectReason.json','ServiceInfo.json','Technology.json','User.json']
    schemas_map = {}
    for f in schema_files:
        sc.addFile(f'{schemas_dir}{f}')

        with open(SparkFiles.get(f), 'rb') as handle:
            schemas_map[f.replace('.json','')]=StructType.fromJson(json.load(handle))
    return schemas_map

print('Defined functions:')
print('getFilesList(path) - list files recursively')
print('    path - root path')
print('getShowString(df, n=20, truncate=True, vertical=False) - prints Spark DataFrame')
print('getSchemasMap(schemas_dir) - creates dictionanry with Spark Schemas, schemas_dir - directory with json files with schemas')
