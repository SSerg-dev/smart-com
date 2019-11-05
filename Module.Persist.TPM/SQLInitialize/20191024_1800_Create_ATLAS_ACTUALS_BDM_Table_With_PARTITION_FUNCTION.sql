IF NOT EXISTS (SELECT * FROM SYS.PARTITION_FUNCTIONS WHERE NAME = N'pfAtlasActualsRecordDateTime')
BEGIN
	DECLARE @DatePartitionFunction nvarchar(max) = 
		N'CREATE PARTITION FUNCTION pfAtlasActualsRecordDateTime (datetime) 
		AS RANGE RIGHT FOR VALUES (';  
	DECLARE @i datetime2 = '20150101';  
	WHILE @i < '20500101'  
	BEGIN  
	SET @DatePartitionFunction += '''' + CAST(@i as nvarchar(10)) + '''' + N', ';  
	SET @i = DATEADD(MM, 1, @i);  
	END  
	SET @DatePartitionFunction += '''' + CAST(@i as nvarchar(10))+ '''' + N');';  
	EXEC sp_executesql @DatePartitionFunction;  
END
GO

IF NOT EXISTS (SELECT * FROM SYS.PARTITION_SCHEMES WHERE NAME = N'schAtlasActualsRecordDateTime')
BEGIN
	CREATE PARTITION SCHEME schAtlasActualsRecordDateTime
	AS PARTITION pfAtlasActualsRecordDateTime 
	ALL TO ([Primary]);
END
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = N'ATLAS_ACTUALS_BDM')
CREATE TABLE [ATLAS_ACTUALS_BDM](
	[SHIP_TO_CODE] [nvarchar](40) NULL,
	[SOLD_TO_CODE] [nvarchar](40) NULL,
	[MATERIAL_CODE] [nvarchar](40) NULL,
	[PLANT_CODE] [nvarchar](40) NULL,
	[NET_WGT] [decimal](30, 8) NULL,
	[LSV] [decimal](30, 8) NULL,
	[BILL_QTY_CASE] [decimal](30, 8) NULL,
	[RECORD_DATETIME] [datetime] NULL,
	[PAYER_CODE] [nvarchar](40) NULL,
	[BILL_DATE] [datetime] NULL
) ON schAtlasActualsRecordDateTime ([RECORD_DATETIME])
GO