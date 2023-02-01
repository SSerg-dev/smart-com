####Notebook "COGS_TI_CALCULATION". 
####*COGS and TI Base calculation functions*.
###### *Developer: [LLC Smart-Com](http://smartcom.software/), andrey.philushkin@effem.com*

from pyspark.sql.functions import *

logText = ['COGS was not found','COGSTn was not found','COGS duplicate record error','COGSTn duplicate record error','TI Base was not found']
logTnText = ['COGSTn was not found','COGSTn duplicate record error','TI Base was not found']
actualLogText = ['Actual COGS was not found','Actual COGSTn was not found','Actual COGS duplicate record error','Actual COGSTn duplicate record error','Actual TI Base was not found']
actualTnLogText = ['Actual COGSTn was not found','Actual COGSTn duplicate record error','Actual TI Base was not found']

def getCogsPercent(activeClientTreeList,cogsClientList):
    def f(objectId, brandTechName, dispatchesStart):
      c = [x for x in activeClientTreeList if x.ObjectId == objectId]
      cogs = []
      while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
        cogs = [x for x in cogsClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName == brandTechName\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
        c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(cogs) == 0:
        while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
          cogs = [x for x in cogsClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName is None\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
          c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(cogs) == 0:
        return "COGS was not found"
      elif len(cogs) > 1:
        return "COGS duplicate record error"
      else:
        return cogs[0].LSVpercent
    return udf(f)    

def getCogsTnPercent(activeClientTreeList,cogsTnClientList):
    def f(objectId, brandTechName, dispatchesStart):
      c = [x for x in activeClientTreeList if x.ObjectId == objectId]
      cogs = []
      while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
        cogs = [x for x in cogsTnClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName == brandTechName\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
        c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(cogs) == 0:
        while (len(cogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
          cogs = [x for x in cogsTnClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName is None\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
          c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(cogs) == 0:
        return "COGSTn was not found"
      elif len(cogs) > 1:
        return "COGSTn duplicate record error"
      else:
        return cogs[0].TonCost
    return udf(f)          

def getActualCogsPercent(activeClientTreeList,actualCogsClientList):
    def f(objectId, brandTechName, dispatchesStart):
      c = [x for x in activeClientTreeList if x.ObjectId == objectId]
      actualcogs = []
      while (len(actualcogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
        actualcogs = [x for x in actualCogsClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName == brandTechName\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
        c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(actualcogs) == 0:
        while (len(actualcogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
          actualcogs = [x for x in actualCogsClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName is None\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
          c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(actualcogs) == 0:
        return "Actual COGS was not found"
      elif len(actualcogs) > 1:
        return "Actual COGS duplicate record error"
      else:
        return actualcogs[0].LSVpercent
    return udf(f)          

def getActualCogsTnPercent(activeClientTreeList,actualCogsTnClientList):
    def f(objectId, brandTechName, dispatchesStart):
      c = [x for x in activeClientTreeList if x.ObjectId == objectId]
      actualcogs = []
      while (len(actualcogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
        actualcogs = [x for x in actualCogsTnClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName == brandTechName\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
        c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(actualcogs) == 0:
        while (len(actualcogs) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
          actualcogs = [x for x in actualCogsTnClientList if x.cogsClientTreeObjectId == c[0].ObjectId and x.cbtName is None\
                                             and x.cogsStartDate <= dispatchesStart and x.cogsEndDate >= dispatchesStart]
          c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
      
      if len(actualcogs) == 0:
        return "Actual COGSTn was not found"
      elif len(actualcogs) > 1:
        return "Actual COGSTn duplicate record error"
      else:
        return actualcogs[0].TonCost
    return udf(f)          

def getTiPercent(activeClientTreeList,tiClientList):
    def f(objectId, brandTechName, startDate):
      c = [x for x in activeClientTreeList if x.ObjectId == objectId]
      ti = []
      while (len(ti) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
        ti = [x for x in tiClientList if x.tiClientTreeObjectId == c[0].ObjectId and (x.tibtName == brandTechName or x.tibtName is None)\
                                         and x.tiStartDate <= startDate and x.tiEndDate >= startDate]
        c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
        
      if len(ti) == 0:
        return "TI Base was not found"
      else:
        sizePercent = [x.SizePercent for x in ti]
        sumSizePercent = sizePercent[0]
        for el in sizePercent[1:]:
          sumSizePercent = sumSizePercent + el
        return sumSizePercent
    return udf(f)          

def getActualTiPercent(activeClientTreeList,actualTiClientList):
    def f(objectId, brandTechName, startDate):
      c = [x for x in activeClientTreeList if x.ObjectId == objectId]
      actualti = []
      while (len(actualti) == 0) & (len(c) != 0) & (c[0].Type != 'root'):
        actualti = [x for x in actualTiClientList if x.tiClientTreeObjectId == c[0].ObjectId and (x.tibtName == brandTechName or x.tibtName is None)\
                                         and x.tiStartDate <= startDate and x.tiEndDate >= startDate]
        c = [x for x in activeClientTreeList if x.ObjectId == c[0].parentId]
        
      if len(actualti) == 0:
        return "Actual TI Base was not found"
      else:
        sizePercent = [x.SizePercent for x in actualti]
        sumSizePercent = sizePercent[0]
        for el in sizePercent[1:]:
          sumSizePercent = sumSizePercent + el
        return sumSizePercent 
    return udf(f)          