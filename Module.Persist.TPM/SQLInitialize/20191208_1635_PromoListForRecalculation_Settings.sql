DELETE [dbo].[Setting] WHERE [Name] = 'PROMO_LIST_FOR_RECALCULATION'
GO

INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_LIST_FOR_RECALCULATION'
           ,N'String'
           ,N''
           ,N'Promoes for recalculation (it will go to DraftPublished state(by PromoListPlanRecalculationHandler) and then to Finished state(by PromoListActualRecalculationHandler))')
GO
