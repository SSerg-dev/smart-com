IF NOT EXISTS (SELECT * FROM SYS.PARTITION_FUNCTIONS WHERE NAME = N'pfPromoProductDifferenceSection')
BEGIN
	DECLARE @DatePartitionFunction nvarchar(max) = 
		N'CREATE PARTITION FUNCTION pfPromoProductDifferenceSection (datetime) 
		AS RANGE RIGHT FOR VALUES (';  
	DECLARE @i datetime2 = '20200301'; 
	WHILE @i < '20500101'
	BEGIN  
	SET @DatePartitionFunction += '''' + CAST(@i as nvarchar(10)) + '''' + N', ';  
	SET @i = DATEADD(MM, 1, @i);  
	END  
	SET @DatePartitionFunction += '''' + CAST(@i as nvarchar(10))+ '''' + N');';  
	EXEC sp_executesql @DatePartitionFunction;  
END
GO

IF NOT EXISTS (SELECT * FROM SYS.PARTITION_SCHEMES WHERE NAME = N'schPromoProductDifferenceSection')
BEGIN
	CREATE PARTITION SCHEME schPromoProductDifferenceSection
	AS PARTITION pfPromoProductDifferenceSection 
	ALL TO ([Primary]);
END
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = N'PromoProductDifference_Log')
CREATE TABLE [PromoProductDifference_Log](
	[Id] [uniqueidentifier] NOT NULL,
	[DemandUnit] [nvarchar](max) NULL,
	[DMDGROUP] [nvarchar](max) NULL,
	[LOC] [nvarchar](max) NULL,
	[StartDate] [nvarchar](max) NULL,
	[DURInMinutes] [int] NULL,
	[Type] [nvarchar](max) NULL,
	[ForecastID] [nvarchar](max) NULL,
	[QTY] [decimal](30, 6) NULL,
	[MOE] [nvarchar](max) NULL,
	[Source] [nvarchar](max) NULL,
	[SALES_ORG] [nvarchar](max) NULL,
	[SALES_DIST_CHANNEL] [nvarchar](max) NULL,
	[SALES_DIVISON] [nvarchar](max) NULL,
	[BUS_SEG] [nvarchar](max) NULL,
	[MKT_SEG] [nvarchar](max) NULL,
	[DELETION_FLAG] [nvarchar](max) NULL,
	[DELETION_DATE] [nvarchar](max) NULL,
	[INTEGRATION_STAMP] [nvarchar](max) NULL,
	[Roll_FC_Flag] [nvarchar](max) NULL,
	[Promotion_Start_Date] [nvarchar](max) NULL,
	[Promotion_Duration] [int] NULL,
	[Promotion_Status] [nvarchar](max) NULL,
	[Promotion_Campaign] [nvarchar](max) NULL,
	[Create_Date] [datetime] not null
)ON schPromoProductDifferenceSection ([Create_Date]);
GO