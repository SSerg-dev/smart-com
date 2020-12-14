DELETE [Setting] WHERE [Name] = 'PRODUCT_SYNC_FAIL_NOTIFICATION_TEMPLATE_FILE'

GO

INSERT INTO [Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'PRODUCT_SYNC_FAIL_NOTIFICATION_TEMPLATE_FILE'
           ,'String'
           ,'ProductSyncFailTemplate.txt'
           ,'Product synchronization fail notifications template')
GO