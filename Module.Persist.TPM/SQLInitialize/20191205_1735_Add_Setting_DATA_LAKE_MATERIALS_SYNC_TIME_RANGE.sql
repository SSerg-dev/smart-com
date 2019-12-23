DELETE FROM [Setting] WHERE [Name] = 'DATA_LAKE_MATERIALS_SYNC_TIME_RANGE'
GO
DELETE FROM [Setting] WHERE [Name] = 'APP_MIX_EXCEPTED_ZREPS'
GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'DATA_LAKE_MATERIALS_SYNC_TIME_RANGE'
           ,'Int'
           ,24
           ,'Time range from now in hours to check material table.')
GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'APP_MIX_EXCEPTED_ZREPS'
           ,'String'
           ,''
           ,'List of ZREP excepted from proccessing in materials sync mechanic')
GO