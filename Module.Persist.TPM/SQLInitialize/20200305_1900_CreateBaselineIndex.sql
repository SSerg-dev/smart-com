IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = N'IX_BaseLine_NonClustered')
BEGIN
	  CREATE NONCLUSTERED INDEX IX_BaseLine_NonClustered ON BaseLine
	  (
			  ProductId ASC
	  )
	  INCLUDE (StartDate, DemandCode, NeedProcessing) 
END
GO