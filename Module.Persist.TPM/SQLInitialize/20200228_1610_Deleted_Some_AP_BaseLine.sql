 
  DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='BaseLines' AND [Action] = 'Put') 

  DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='BaseLines' AND [Action] = 'Patch') 

  DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='BaseLines' AND [Action] = 'Post') 

  DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='BaseLines' AND [Action] = 'Delete') 

  DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='BaseLines' AND [Action] = 'FullImportXLSX') 

  DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='BaseLines' AND [Action] = 'DownloadTemplateXLSX') 