
DELETE [dbo].[Setting] WHERE [Name] = 'TIME_TO_RESTART_MAIN_NIGHT_PROCESS'

GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'TIME_TO_RESTART_MAIN_NIGHT_PROCESS'
           ,'INT'
           ,'172800000'
           ,'time to check restar main night process in milliseconds')
GO
