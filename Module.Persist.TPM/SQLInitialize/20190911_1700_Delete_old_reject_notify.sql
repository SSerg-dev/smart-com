DELETE FROM [LoopHandler]
      WHERE [Name] = 'Module.Host.TPM.Handlers.Notifications.RejectPromoNotificationHandler'
GO

DELETE FROM [Recipient]
      WHERE [MailNotificationSettingId] IN (SELECT Id FROM MailNotificationSetting WHERE [Name] = 'REJECT_PROMO_NOTIFICATION')
GO

DELETE FROM [MailNotificationSetting] 
	  WHERE [Name] = 'REJECT_PROMO_NOTIFICATION'
GO

DELETE FROM [Setting]
      WHERE [Name] = 'PROMO_REJECT_NOTIFICATION_TEMPLATE_FILE'
GO
