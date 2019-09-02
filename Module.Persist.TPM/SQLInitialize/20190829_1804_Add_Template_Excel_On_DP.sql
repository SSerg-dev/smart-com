/****** Script for SelectTopNRows command from SSMS  ******/

  INSERT INTO AccessPointRole(RoleId,AccessPointId) VALUES((SELECT  [Id]
  FROM [Role]
  WHERE SystemName = 'DemandPlanning'),
  (SELECT TOP 1 ID  FROM [AccessPoint] 
  
	WHERE [Resource] = 'Products' AND [Action] = 'DownloadTemplateXLSX' AND [Disabled] = 'false'  )  )