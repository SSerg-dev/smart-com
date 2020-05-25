SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[FillPromoProductDifference]
AS
BEGIN

INSERT INTO [dbo].[PromoProductDifference_Log] ([Id], [DemandUnit], [DMDGROUP], [LOC], [StartDate], [DURInMinutes], [Type], [ForecastID], [QTY], [MOE], [Source], [SALES_ORG],[SALES_DIST_CHANNEL], [SALES_DIVISON],[BUS_SEG], [MKT_SEG], [DELETION_FLAG], [DELETION_DATE], [INTEGRATION_STAMP], [Roll_FC_Flag], [Promotion_Start_Date], [Promotion_Duration], [Promotion_Status],[Promotion_Campaign], [Create_Date])
SELECT [Id], [DemandUnit], [DMDGROUP], [LOC], [StartDate], [DURInMinutes], [Type], [ForecastID], [QTY], [MOE], [Source], [SALES_ORG],[SALES_DIST_CHANNEL], [SALES_DIVISON],[BUS_SEG], [MKT_SEG], [DELETION_FLAG], [DELETION_DATE], [INTEGRATION_STAMP], [Roll_FC_Flag], [Promotion_Start_Date], [Promotion_Duration], [Promotion_Status],[Promotion_Campaign], GETDATE() FROM [dbo].[PromoProductDifference]

	TRUNCATE TABLE [dbo].[PromoProductDifference]

	DECLARE PromoProductDifferenceCursor CURSOR FAST_FORWARD
	FOR
		SELECT
			d.OriginalDate AS WeekStartDate,
			p.StartDate AS PromoStart,
			p.EndDate AS PromoEnd,
			cdi.WEEK,
			p.Number AS PromoId, 
			pp.ZREP,
			pr.Segmen_code AS MKT_SEG,
			cdi.DMDGroup,
			cdi.IncrementalQty AS CDI_QTY,
			cdi.LastChangeDate AS CDI_LCD,
			pdi.IncrementalQty AS PDI_QTY,
			pdi.LastChangeDate AS PDI_LCD,
			pt.SystemName AS PromoType,
			p.DeletedDate,
			ct.GHierarchyCode,
			ct.ObjectId,
			ct.parentId,
			p.Disabled AS PromoDisabled,
			pp.Disabled AS PromoProductDisabled,
			ps.SystemName AS PromoSysStatus
		FROM [dbo].[CurrentDayIncremental] AS cdi (NOLOCK)
			LEFT JOIN [dbo].[PreviousDayIncremental] pdi (NOLOCK) 
				ON cdi.PromoId = pdi.PromoId 
				AND cdi .ProductId = pdi.ProductId 
				AND cdi.DemandCode = pdi.DemandCode
				AND cdi.DMDGroup = pdi.DMDGroup
				AND cdi.WEEK = pdi.Week
			INNER JOIN [dbo].[Dates] d (NOLOCK)
				ON cdi.WEEK = d.MarsWeekFullName 
					AND d.MarsDay = 1
			INNER JOIN [dbo].[Promo] p (NOLOCK)
				ON cdi.PromoId = p.Id 
			INNER JOIN [dbo].[PromoProduct] pp (NOLOCK)
				ON (cdi.ProductId = pp.ProductId 
					AND cdi.PromoId = pp.PromoId)
			INNER JOIN [dbo].[Product] pr (NOLOCK)
				ON pp.ProductId = pr.Id
			INNER JOIN [dbo].[PromoTypes] pt (NOLOCK)
				ON p.PromoTypesId = pt.Id
			INNER JOIN [dbo].[ClientTree] AS ct (NOLOCK)
				ON p.ClientTreeKeyId = ct.Id
			INNER JOIN [dbo].[PromoStatus] ps (NOLOCK)
				ON p.PromoStatusId = ps.Id
		UNION 
		SELECT
			d.OriginalDate AS WeekStartDate,
			p.StartDate AS PromoStart,
			p.EndDate AS PromoEnd,
			cdi.WEEK,
			p.Number AS PromoId, 
			pp.ZREP,
			pr.Segmen_code AS MKT_SEG,
			pdi.DMDGroup,
			cdi.IncrementalQty AS CDI_QTY,
			cdi.LastChangeDate AS CDI_LCD,
			pdi.IncrementalQty AS PDI_QTY,
			pdi.LastChangeDate AS PDI_LCD,
			pt.SystemName AS PromoType,
			p.DeletedDate,
			ct.GHierarchyCode,
			ct.ObjectId,
			ct.parentId,
			p.Disabled AS PromoDisabled,
			pp.Disabled AS PromoProductDisabled,
			ps.SystemName AS PromoSysStatus
		FROM [dbo].[PreviousDayIncremental] AS pdi (NOLOCK)
			LEFT JOIN [dbo].[CurrentDayIncremental] cdi (NOLOCK) 
				ON cdi.PromoId = pdi.PromoId 
				AND cdi .ProductId = pdi.ProductId 
				AND cdi.DemandCode = pdi.DemandCode
				AND cdi.DMDGroup = pdi.DMDGroup
				AND cdi.WEEK = pdi.Week
			INNER JOIN [dbo].[Dates] d (NOLOCK)
				ON pdi.WEEK = d.MarsWeekFullName 
					AND d.MarsDay = 1
			INNER JOIN [dbo].[Promo] p (NOLOCK)
				ON pdi.PromoId = p.Id 
			INNER JOIN [dbo].[PromoProduct] pp (NOLOCK)
				ON (pdi.ProductId = pp.ProductId 
					AND pdi.PromoId = pp.PromoId)
			INNER JOIN [dbo].[Product] pr (NOLOCK)
				ON pp.ProductId = pr.Id
			INNER JOIN [dbo].[PromoTypes] pt (NOLOCK)
				ON p.PromoTypesId = pt.Id
			INNER JOIN [dbo].[ClientTree] AS ct (NOLOCK)
				ON p.ClientTreeKeyId = ct.Id
			INNER JOIN [dbo].[PromoStatus] ps (NOLOCK)
				ON p.PromoStatusId = ps.Id

	DECLARE
		@WeekStartDate DATETIMEOFFSET(7),
		@PromoStart DATETIMEOFFSET(7),
		@PromoEnd DATETIMEOFFSET(7),
		@MarsWeekFullName NVARCHAR(12),
		@PromoId INT,
		@ZREP NVARCHAR(MAX),
		@DMDGroup NVARCHAR(MAX),
		@CDI_QTY FLOAT,
		@CDI_LCD VARCHAR(MAX),
		@PDI_QTY FLOAT,
		@PDI_LCD VARCHAR(MAX),
		@MKT_SEG NVARCHAR(MAX),
		@PromoType NVARCHAR(255),
		@SALAES_DIST_CHANEL INT,
		@PromoDeletedDate DATETIMEOFFSET(7),
		@GHierarchyCode NVARCHAR(MAX),
		@ObjectId INT,
		@ParentId INT,
		@PromoDisabled BIT,
		@PromoStatusSysName NVARCHAR(MAX),
		@PromoProductDisabled BIT,
	
		@DemandUnit NVARCHAR(MAX),
		@ForecastID NVARCHAR(MAX),
		@DELETION_FLAG NVARCHAR(MAX),
		@INTEGRATION_STAMP NVARCHAR(MAX),
		@Roll_FC_Flag INT,
		@DateFormat NVARCHAR(17),
		@QTY FLOAT;

		SET @DateFormat = 'yyyyMMdd HH:mm:ss';
		SET @INTEGRATION_STAMP = FORMAT(GETDATE(), @DateFormat);

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
			@MKT_SEG,
			@DMDGroup,
			@CDI_QTY,
			@CDI_LCD,
			@PDI_QTY,
			@PDI_LCD,
			@PromoType,
			@PromoDeletedDate,
			@GHierarchyCode,
			@ObjectId,
			@ParentId,
			@PromoDisabled,
			@PromoProductDisabled,
			@PromoStatusSysName;

		IF @@FETCH_STATUS <> 0
			BREAK;

		IF (@CDI_QTY <> @PDI_QTY) OR (@CDI_QTY IS NOT NULL AND @PDI_QTY IS NULL) OR (@PDI_QTY IS NOT NULL AND @CDI_QTY IS NULL)
		BEGIN			
			IF @PromoType = 'Regular'
				SET @ForecastID = 'RG_' + CAST(@PromoId AS NVARCHAR(MAX));
			ELSE IF @PromoType = 'InOut'
				SET @ForecastID = 'IO_' + CAST(@PromoId AS NVARCHAR(MAX));
			ELSE IF @PromoType = 'Loyalty'
				SET @ForecastID = 'LY_' + CAST(@PromoId AS NVARCHAR(MAX));
			ELSE IF @PromoType = 'Dynamic'
				SET @ForecastID = 'DY_' + CAST(@PromoId AS NVARCHAR(MAX));
			ELSE CONTINUE;
			
			SET @DemandUnit = @ZREP + '_0125';

			--IF @PromoDeletedDate IS NULL
				SET @DELETION_FLAG = 'N'
			--ELSE 
				--SET @DELETION_FLAG = 'Y'

			WHILE @GHierarchyCode IS NULL OR @GHierarchyCode = ''
			BEGIN
				SET @ObjectId = (
					SELECT TOP(1)
						ct.ObjectId
					FROM ClientTree ct
					WHERE ct.ObjectId = @ParentId
				);
				SET @ParentId = (
					SELECT TOP(1)
						ct.parentId 
					FROM ClientTree ct
					WHERE ct.ObjectId = @ObjectId
					);

				SET @GHierarchyCode = (
					SELECT TOP(1)
						ct.GHierarchyCode
					FROM ClientTree ct
					WHERE ct.ObjectId = @ObjectId
				);

				IF @ObjectId = 5000000
					BREAK;
			END;

			SET @SALAES_DIST_CHANEL = (
				SELECT TOP(1)
					[0DISTR_CHAN] 
				FROM [dbo].[MARS_UNIVERSAL_PETCARE_CUSTOMERS]
				WHERE 
					ZCUSTHG04 = @GHierarchyCode
			);

			IF @SALAES_DIST_CHANEL IS NULL
			BEGIN
				IF SUBSTRING(@GHierarchyCode, 1, 2) = '00'
					SET @GHierarchyCode = SUBSTRING(@GHierarchyCode, 3, LEN(@GHierarchyCode))
				ELSE 
					SET @GHierarchyCode = '00' + @GHierarchyCode

				SET @SALAES_DIST_CHANEL = (
					SELECT TOP(1)
							[0DISTR_CHAN] 
						FROM [dbo].[MARS_UNIVERSAL_PETCARE_CUSTOMERS]
						WHERE 
							ZCUSTHG04 = @GHierarchyCode
				);
			END;
			
			IF (@PromoDisabled = 1
				OR @PromoProductDisabled = 1
				OR @PromoStatusSysName NOT IN ('OnApproval', 'Planned', 'Started', 'Approved')
				OR @CDI_QTY IS NULL)
				SET @QTY = 0;
			ELSE
				SET @QTY = @CDI_QTY;

			INSERT INTO [dbo].[PromoProductDifference]
			(
				[Id],
				[DemandUnit],
				[DMDGROUP],
				[LOC],
				[StartDate],
				[DURInMinutes],
				[Type],
				[ForecastID],
				[QTY],
				[MOE],
				[Source],
				[SALES_ORG],
				[SALES_DIST_CHANNEL],
				[SALES_DIVISON],
				[BUS_SEG],
				[MKT_SEG],
				[DELETION_FLAG],
				[DELETION_DATE],
				[INTEGRATION_STAMP],
				[Roll_FC_Flag],
				[Promotion_Start_Date],
				[Promotion_Duration],
				[Promotion_Status],
				[Promotion_Campaign]
			)
			VALUES(
				NEWID(),
				CONCAT('000000000000',@DemandUnit),
				CONCAT('00',@DMDGroup),
				'RU',
				FORMAT(@WeekStartDate, @DateFormat),
				10080,
				7,
				@ForecastID,
				CONVERT(DECIMAL(30,6), @QTY),
				'0125',
				'Jupiter',
				261,
				@SALAES_DIST_CHANEL,
				51,
				'05',
				CONVERT(INT, @MKT_SEG),
				@DELETION_FLAG,
				'99991231 23:59:59',
				@INTEGRATION_STAMP,
				0,
				FORMAT(@PromoStart, @DateFormat),
				DATEDIFF(DAY, @PromoStart, @PromoEnd) + 1,
				'Auto Approved',
				'JPT'
			);
		END;
	END;

	CLOSE PromoProductDifferenceCursor;
	DEALLOCATE PromoProductDifferenceCursor;
END;