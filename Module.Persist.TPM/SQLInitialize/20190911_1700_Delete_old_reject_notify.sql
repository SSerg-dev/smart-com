DELETE FROM [dbo].[LoopHandler]
      WHERE [Name] = 'Module.Host.TPM.Handlers.Notifications.RejectPromoNotificationHandler'
GO

DELETE FROM [dbo].[Recipient]
      WHERE [MailNotificationSettingId] IN (SELECT Id FROM MailNotificationSetting WHERE [Name] = 'REJECT_PROMO_NOTIFICATION')
GO

DELETE FROM [dbo].[MailNotificationSetting] 
	  WHERE [Name] = 'REJECT_PROMO_NOTIFICATION'
GO

DELETE FROM [dbo].[Setting]
      WHERE [Name] = 'PROMO_REJECT_NOTIFICATION_TEMPLATE_FILE'
GO
