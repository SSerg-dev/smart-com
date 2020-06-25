 
INSERT INTO [dbo].[ServiceInfo]
           ([Id]
           ,[Name]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'ROLLING_PROMO_START_DATE'
           ,CONVERT(datetimeoffset, GETDATE())
		   ,'start date promo for first integrating')
GO


