CREATE OR ALTER PROCEDURE FillPromoProductDifference
AS
BEGIN
	TRUNCATE TABLE [dbo].[PromoProductDifference]

	DECLARE PromoProductDifferenceCursor CURSOR FAST_FORWARD
	FOR
		SELECT 
			d.OriginalDate AS WeekStartDate,
			p.DispatchesStart AS PromoStart,
			p.DispatchesEnd AS PromoEnd,
			cdi.WEEK,
			p.Number AS PromoId, 
			pp.ZREP,
			cdi.DMDGroup,
			cdi.IncrementalQty AS CDI_QTY,
			cdi.LastChangeDate AS CDI_LCD,
			pdi.IncrementalQty AS PDI_QTY,
			pdi.LastChangeDate AS PDI_LCD,
			pt.SystemName AS PromoType,
			pr.Segmen_code AS MKT_SEG,
			p.DeletedDate,
			ct.GHierarchyCode
		FROM [dbo].[CurrentDayIncremental] AS cdi (NOLOCK)
			LEFT JOIN [dbo].[PreviousDayIncremental] pdi (NOLOCK) 
				ON cdi.Id = pdi.Id
			INNER JOIN [dbo].[Dates] d (NOLOCK)
				ON cdi.WEEK = d.MarsWeekFullName 
					AND d.MarsDay = 1
			INNER JOIN [dbo].[Promo] p (NOLOCK)
				ON cdi.PromoId = p.Id
			INNER JOIN [dbo].[PromoProduct] pp (NOLOCK)
				ON cdi.ProductId = pp.ProductId 
					AND cdi.PromoId = pp.PromoId
			INNER JOIN [dbo].[Product] pr (NOLOCK)
				ON pp.ProductId = pr.Id
			INNER JOIN [dbo].[PromoTypes] pt (NOLOCK)
				ON p.PromoTypesId = pt.Id
			INNER JOIN [dbo].[ClientTree] AS ct (NOLOCK)
				ON p.ClientTreeKeyId = ct.Id;

	DECLARE
		@WeekStartDate DATETIMEOFFSET(7),
		@PromoStart DATETIMEOFFSET(7),
		@PromoEnd DATETIMEOFFSET(7),
		@MarsWeekFullName NVARCHAR(12),
		@PromoId INT,
		@ZREP NVARCHAR(max),
		@DMDGroup NVARCHAR(max),
		@CDI_QTY FLOAT,
		@CDI_LCD VARCHAR(max),
		@PDI_QTY FLOAT,
		@PDI_LCD VARCHAR(max),
		@PromoType NVARCHAR(255),
		@SALAES_DIST_CHANEL INT,
		@MKT_SEG NVARCHAR(255),
		@PromoDeletedDate DATETIMEOFFSET(7),
		@GHierarchyCode NVARCHAR(MAX),
	
		@DemandUnit NVARCHAR(MAX),
		@ForecastID NVARCHAR(MAX),
		@DELETION_FLAG NVARCHAR(MAX),
		@INTEGRATION_STAMP NVARCHAR(MAX),
		@Roll_FC_Flag INT;

	OPEN PromoProductDifferenceCursor;
	WHILE 1 = 1
	BEGIN
		FETCH NEXT FROM PromoProductDifferenceCursor
		INTO
			@WeekStartDate,
			@PromoStart,
			@PromoEnd,
			@MarsWeekFullName,
			@PromoId,
			@ZREP,
			@DMDGroup,
			@CDI_QTY,
			@CDI_LCD,
			@PDI_QTY,
			@PDI_LCD,
			@PromoType,
			@MKT_SEG,
			@PromoDeletedDate,
			@GHierarchyCode;

		IF @@FETCH_STATUS <> 0
			BREAK;

		IF (@CDI_QTY <> @PDI_QTY) OR (@CDI_QTY IS NOT NULL AND @PDI_QTY IS NULL)
		BEGIN
		
			SET @DemandUnit = @ZREP + '_0125';
			IF @PromoType = 'Regular'
				SET @ForecastID = 'PR_' + @ZREP;
			ELSE IF @PromoType = 'InOut'
				SET @PromoType = 'IO_' + @ZREP;
			ELSE CONTINUE;
			IF @PromoDeletedDate IS NULL
				SET @DELETION_FLAG = 'N';
			ELSE 
				SET @DELETION_FLAG = 'Y';

			SET @SALAES_DIST_CHANEL = (
				SELECT TOP(1)
					[0DISTR_CHAN] 
				FROM [dbo].[MARS_UNIVERSAL_PETCARE_CUSTOMERS]
				WHERE 
					ZCUSTHG04 = @GHierarchyCode
			);

			INSERT INTO [dbo].[PromoProductDifference] VALUES(
				NEWID(),
				@DemandUnit,
				CONVERT(INT, @DMDGroup),
				'RU_0125',
				CONVERT(NVARCHAR(MAX), CAST(@WeekStartDate AS DATE)),
				10080,
				7,
				@ForecastID,
				@CDI_QTY,
				125,
				'Jupiter',
				261,
				@SALAES_DIST_CHANEL,
				51,
				5,
				CONVERT(INT, @MKT_SEG),
				@DELETION_FLAG,
				'99991231 23:59:59',
				CONVERT(NVARCHAR(MAX), CAST(GETDATE() AS DATE)),
				0,
				CONVERT(NVARCHAR(MAX), @PromoStart),
				DATEDIFF(DAY, @PromoStart, @PromoEnd),
				'Auto Approved',
				'Jupiter'
			);
		END;
	
	END;

	CLOSE PromoProductDifferenceCursor;
	DEALLOCATE PromoProductDifferenceCursor;
END;

