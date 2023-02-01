####Notebook "RA_TI_SHOPPER_CALCULATION". 
####*RA TI Shopper calculation functions*.
###### *Developer: [LLC Smart-Com](http://smartcom.software/), andrey.philushkin@effem.com*

from pyspark.sql.functions import *

raLogText = ['RA TI Shopper was not found', 'RA TI Shopper duplicate record error']

def getRaTiShopperPercent(activeClientTreeList,ratiShopperList):
  def f(clientId, budgetYear):
      c = [x for x in activeClientTreeList if x.Id == clientId]
      rati = []
      while (len(rati) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
        rati = [x for x in ratiShopperList if x.ClientTreeId == c[0].Id and x.Year == budgetYear]
        c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
  
      if len(rati) == 0:
        return "RA TI Shopper was not found"
      elif len(rati) > 1:
        return "RA TI Shopper duplicate record error"
      else:
        return rati[0].RATIShopperPercent
  
  return udf(f)      