UPDATE [dbo].[Setting]
   SET [Name] = N'NOT_CHECK_PROMO_STATUS_LIST'
      ,[Value] = N'Draft,Cancelled,Deleted,Closed'
      ,[Description] = N'Promo statuses for which it is not necessary to night recalculation'
 WHERE Name = N'CHECK_PROMO_STATUS_LIST'
GO

DELETE FROM [dbo].[Setting]
      WHERE [Name] = N'CHECK_PROMO_PERIOD_DAYS'
GO