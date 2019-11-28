DELETE FROM Setting WHERE [Name] = N'PROMO_PRODUCT_CORRECTION_PROMO_STATUS_LIST'

INSERT INTO [dbo].[Setting]
           ([Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (N'PROMO_PRODUCT_CORRECTION_PROMO_STATUS_LIST'
           ,N'String'
           ,N'Draft,Deleted,Cancelled,Started,Finished,Closed'
           ,N'Stutuses promo product correction')