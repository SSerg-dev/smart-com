BEGIN TRANSACTION;

DELETE [dbo].[Setting] WHERE [Name] = 'PROMO_ROI_REPORT_PERIOD_NAME';

INSERT INTO [dbo].[Setting] ([Id], [Name], [Type], [Value], [Description])
     VALUES (NEWID(), 'PROMO_ROI_REPORT_MARS_DAY_FULL_NAME_WITHOUT_YEAR', 'string', 'P5 W1 D1', 'After this date, a letter for the previous year will not be sent.');

COMMIT;