INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'NOT_PLAN_RECALCULATE_PROMO_STATUS_LIST'
           ,N'String'
           ,N'Started,Finished,Closed'
           ,N'Plan parameters for promo in this statuses will not be recalculated')
GO
