DECLARE @notificationName VARCHAR(255) = 'PROMO_ROI_REPORT_NOTIFICATION';
DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.Notifications.PromoROIReportNotificationHandler';

DELETE [dbo].[Recipient]  WHERE [MailNotificationSettingId] = (SELECT TOP(1) Id FROM [dbo].[MailNotificationSetting] WHERE [name] = @notificationname AND [Disabled] = 0);
DELETE [dbo].[MailNotificationSetting]  WHERE [Name] = @notificationName;
DELETE [dbo].[LoopHandler]  WHERE [Name] = @handlerName;

INSERT INTO [dbo].[MailNotificationSetting] ([Id] ,[Name] ,[Description] ,[Subject] ,[Body] ,[IsDisabled] ,[To] ,[CC] ,[BCC] ,[Disabled] ,[DeletedDate])
     VALUES (NEWID() ,@notificationName ,'Promo ROI Report notification.' ,'Promo ROI Report' ,'/' ,0 ,'' ,NULL ,NULL ,0 ,NULL);

INSERT INTO [dbo].[LoopHandler] ([Id] ,[Description] ,[Name] ,[ExecutionPeriod] ,[ExecutionMode] ,[CreateDate] ,[LastExecutionDate] ,[NextExecutionDate] ,[ConfigurationName] ,[Status] ,[RunGroup] ,[UserId] ,[RoleId])
    VALUES (NEWID() ,N'Sending notification promo ROI report' ,@handlerName ,86400000 ,'SCHEDULE' ,GETDATE() ,'2020-02-05 08:00:00.0000000 +03:00' ,'2020-02-06 08:00:00.0000000 +03:00' ,'PROCESSING' ,NULL ,NULL ,NULL ,NULL);

INSERT INTO [dbo].[Setting] ([Id], [Name], [Type], [Value], [Description])
     VALUES (NEWID(), 'PROMO_ROI_REPORT_PERIOD_NAME', 'string', 'P5', 'After this date, a letter for the previous year will not be sent.');

INSERT INTO [dbo].[Setting] ([Id], [Name], [Type], [Value], [Description])
     VALUES (NEWID(), 'PROMO_ROI_REPORT_MARS_WEEK_NAME', 'string', 'W4', 'Mars week name.');
