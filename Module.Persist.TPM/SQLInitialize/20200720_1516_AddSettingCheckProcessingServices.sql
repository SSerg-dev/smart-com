
INSERT INTO [dbo].[Setting]
           ([Id]
           ,[Name]
           ,[Type]
           ,[Value]
           ,[Description])
     VALUES
           (NEWID()
           ,'CHECK_PROCESSING_SERVICES'
           ,'Int'
           ,200000
           ,'Difference between the last run of the ScheduleHandler and the current time to test the processing service')
GO




