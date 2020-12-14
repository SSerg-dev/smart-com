DECLARE @notificationName VARCHAR(255) = 'PROMO_ROI_REPORT_NOTIFICATION';
DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.Notifications.PromoROIReportNotificationHandler';
DECLARE @promoRoiReportMarsDayFullNameWithoutYear VARCHAR(255) = 'PROMO_ROI_REPORT_MARS_DAY_FULL_NAME_WITHOUT_YEAR'
DECLARE @promoRoiReportMarsWeekName VARCHAR(255) = 'PROMO_ROI_REPORT_MARS_WEEK_NAME'

DELETE [Recipient]  WHERE [MailNotificationSettingId] = (SELECT TOP(1) Id FROM [MailNotificationSetting] WHERE [name] = @notificationname AND [Disabled] = 0);
DELETE [MailNotificationSetting]  WHERE [Name] = @notificationName;
DELETE [LoopHandler]  WHERE [Name] = @handlerName;
DELETE [Setting]  WHERE [Name] = @promoRoiReportMarsDayFullNameWithoutYear;
DELETE [Setting]  WHERE [Name] = @promoRoiReportMarsWeekName;

INSERT INTO [MailNotificationSetting] ([Id] ,[Name] ,[Description] ,[Subject] ,[Body] ,[IsDisabled] ,[To] ,[CC] ,[BCC] ,[Disabled] ,[DeletedDate])
     VALUES (NEWID() ,@notificationName ,'Promo ROI Report notification.' ,'Promo ROI Report' ,'/' ,0 ,'' ,NULL ,NULL ,0 ,NULL);

INSERT INTO [LoopHandler] ([Id] ,[Description] ,[Name] ,[ExecutionPeriod] ,[ExecutionMode] ,[CreateDate] ,[LastExecutionDate] ,[NextExecutionDate] ,[ConfigurationName] ,[Status] ,[RunGroup] ,[UserId] ,[RoleId])
    VALUES (NEWID() ,N'Sending notification promo ROI report' ,@handlerName ,86400000 ,'SCHEDULE' ,GETDATE() ,'2020-02-05 08:00:00.0000000 +03:00' ,'2020-02-06 08:00:00.0000000 +03:00' ,'PROCESSING' ,NULL ,NULL ,NULL ,NULL);

INSERT INTO [Setting] ([Id], [Name], [Type], [Value], [Description])
     VALUES (NEWID(), @promoRoiReportMarsDayFullNameWithoutYear, 'string', 'P5 W1 D1', 'After this date, a letter for the previous year will not be sent.');

INSERT INTO [Setting] ([Id], [Name], [Type], [Value], [Description])
     VALUES (NEWID(), @promoRoiReportMarsWeekName, 'string', 'W4', 'Mars week name.');
