INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'AUTO_RESET_PROMO_PERIOD_WEEKS'
           ,N'Int'
           ,N'2'
           ,N'If the difference between promo dispatch start and current date equals this value, promo will automatically reset to Draft from Draft(published).')
GO

INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'CLOSED_PROMO_PERIOD_YEARS'
           ,N'Int'
           ,N'2'
           ,N'Promo which was closed in period [current date - <value> ; current date], will be chosen for uplift finding')
GO
