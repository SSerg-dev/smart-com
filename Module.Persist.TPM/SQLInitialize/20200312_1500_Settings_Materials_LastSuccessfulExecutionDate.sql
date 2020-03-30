DELETE [dbo].[Setting] WHERE [Name] = 'MATERIALS_LAST_SUCCESSFUL_EXECUTION_DATE'

GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'MATERIALS_LAST_SUCCESSFUL_EXECUTION_DATE'
           ,'String'
           ,'2020-03-12 16:39:35.422'
           ,'Last successful execution date of material sync with products')
GO