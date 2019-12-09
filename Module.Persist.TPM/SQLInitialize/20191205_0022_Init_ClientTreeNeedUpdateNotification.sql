DELETE [dbo].[Setting] WHERE [Name] = 'CLIENTTREE_NEED_UPDATE_NOTIFICATION_TEMPLATE_FILE'
GO

INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'CLIENTTREE_NEED_UPDATE_NOTIFICATION_TEMPLATE_FILE'
           ,'String'
           ,'ClientTreeNeedUpdateTemplate.txt'
           ,'On ClientTreeNeedUpdate notifications template')
GO

DELETE [dbo].[MailNotificationSetting] WHERE [Name] = 'CLIENTTREE_NEED_UPDATE_NOTIFICATION'
GO

INSERT INTO [dbo].[MailNotificationSetting]
		   ([Name]
           ,[Description]
           ,[Subject]
           ,[Body]
           ,[IsDisabled]
           ,[To]
           ,[CC]
           ,[BCC]
           ,[Disabled]
           ,[DeletedDate])
     VALUES
		   (N'CLIENTTREE_NEED_UPDATE_NOTIFICATION'
           ,N'Notification of the updating client tree'
           ,N'Nodes might to be added'
           ,N'#HTML_SCAFFOLD#'
           ,0
           ,''
           ,NULL
           ,NULL
           ,0
           ,NULL)
GO