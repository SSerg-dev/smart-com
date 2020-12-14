INSERT INTO [Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'CHECK_PROMO_STATUS_LIST'
           ,N'String'
           ,N'DraftPublished,OnApproval,Approved,Planned,Started,Finished'
           ,N'Promotional statuses for which it is necessary to check the compliance of a new product with a filter')
GO

INSERT INTO [Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'CHECK_PROMO_PERIOD_DAYS'
           ,N'Int'
           ,N'196'
           ,N'If before the start of the promo there are fewer days than the specified number, you need to include it in the DataFlow recalculation.')
GO