UPDATE [MailNotificationSetting]
   SET [Description] = N'Notification that a new product has been found is suitable for promotional conditions.',
	   [Subject] = N'New products relevant for the following promotions were found'
 WHERE [Name] = 'PROMO_PRODUCT_CREATE_NOTIFICATION'

 UPDATE [MailNotificationSetting]
   SET [Description] = N'Notification that a new product has been found is suitable for promotional conditions.',
	   [Subject] = N'Products used in the following promotions no longer match the conditions'
 WHERE [Name] = 'PROMO_PRODUCT_DELETE_NOTIFICATION'

 UPDATE [Setting]
   SET [Name] = N'CHECK_PRODUCT_CHANGE_PROMO_STATUS_LIST'
 WHERE [Name] = N'CHECK_NEW_PRODUCT_STATUS_LIST'

 UPDATE [LoopHandler]
   SET [Name] = N'Module.Host.TPM.Handlers.PromoProductChangeNotificationHandler',
	   [Description] = N'Distribution of product change notifications'
  where ExecutionMode = 'SCHEDULE'
  and [Name] = N'Module.Host.TPM.Handlers.NewProductsNotificationHandler'