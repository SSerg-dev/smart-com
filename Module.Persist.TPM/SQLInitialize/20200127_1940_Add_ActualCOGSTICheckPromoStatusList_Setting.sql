DELETE [Setting] WHERE [Name] = 'ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST'
GO

INSERT INTO [Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST'
           ,'String'
           ,'Finished,Closed'
           ,'Statuses for Actual COGS/TI checking')
GO