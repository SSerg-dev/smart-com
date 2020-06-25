
INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'DAY_OF_WEEK_Rolling_Volume'
           ,'Int'
           ,6
           ,'Day of week to add rolling volumes (0 equals Sunday)')
GO


