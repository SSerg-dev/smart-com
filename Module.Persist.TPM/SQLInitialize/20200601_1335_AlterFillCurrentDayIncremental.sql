SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[FillCurrentDayIncremental]
AS   
BEGIN
	EXEC DropIncrementalIndex 1;
	TRUNCATE TABLE CurrentDayIncremental;;

	DECLARE PromoProductCursor CURSOR FAST_FORWARD
		FOR 
			SELECT
				p.Id AS PromoId,
				pp.ProductId AS ProductId,
				p.DispatchesStart AS PromoDispatchesStart,
				p.DispatchesEnd AS PromoDispatchesEnd,
				pp.PlanProductIncrementalCaseQty AS IncrementalQty,
				p.DispatchDuration AS PromoDuration,
				p.LastChangedDate,
				ct.DemandCode AS DemandCode,
				ct.DMDGroup AS DMDGroup,
				ct.ObjectId,
				ct.parentId AS ParentId,
				p.DeviationCoefficient
			FROM [dbo].[Promo] p (NOLOCK)
			INNER JOIN [dbo].[PromoProduct] AS pp (NOLOCK)
				ON p.Id = pp.PromoId
			INNER JOIN [dbo].[ClientTree] AS ct (NOLOCK)
				ON p.ClientTreeKeyId = ct.Id
			INNER JOIN [dbo].[PromoStatus] AS ps (NOLOCK)
				ON p.PromoStatusId = ps.Id
			WHERE
				ps.SystemName IN ('OnApproval', 'Planned', 'Started', 'Approved')
				AND ct.EndDate IS NULL
				AND p.IsApolloExport = 1
				AND pp.Disabled = 0 AND p.Disabled = 0;

	DECLARE 
		@GlobalDispatchesStart DATE,
		@GlobalDispatchesEnd DATE;

	SET @GlobalDispatchesStart = CAST(GETDATE() AS DATE);
	SET @GlobalDispatchesEnd = (
		SELECT
			MAX(CAST(p.DispatchesEnd AS DATE))
		FROM [dbo].[Promo] p (NOLOCK)
		INNER JOIN [dbo].[PromoProduct] AS pp (NOLOCK)
			ON p.Id = pp.PromoId
		INNER JOIN [dbo].[ClientTree] AS ct (NOLOCK)
			ON p.ClientTreeKeyId = ct.Id
		INNER JOIN [dbo].[PromoStatus] ps (NOLOCK)
			ON p.PromoStatusId = ps.Id
		WHERE
			ps.SystemName IN ('OnApproval', 'Planned', 'Started', 'Approved')
			AND ct.EndDate IS NULL
			AND p.IsApolloExport = 1
			AND pp.Disabled = 0 AND p.Disabled = 0
	);
	DECLARE MarsWeekCursor CURSOR FAST_FORWARD
		FOR 
			SELECT
				e.OriginalDateStart AS DateStart,
				d.OriginalDate AS DateEnd,
				d.MarsWeekFullName
			FROM Dates AS d (NOLOCK)
			JOIN (
				SELECT 
					OriginalDate AS OriginalDateStart,
					MarsWeekFullName AS MarsWeekFullNameEnd
				FROM Dates  (NOLOCK)
				WHERE	
				(
					OriginalDate >= @GlobalDispatchesStart
					OR DATEDIFF(DAY, @GlobalDispatchesStart, OriginalDate) < 0 AND DATEDIFF(DAY, @GlobalDispatchesStart, OriginalDate) > -7
				)
				AND MarsDay = 1
			) AS e
			ON d.MarsWeekFullName = e.MarsWeekFullNameEnd
			WHERE
				(
					d.OriginalDate <= @GlobalDispatchesEnd
					OR DATEDIFF(DAY, d.OriginalDate, @GlobalDispatchesEnd) < 0 AND DATEDIFF(DAY, d.OriginalDate, @GlobalDispatchesEnd) > -7
				)
				AND d.MarsDay = 7;
	DECLARE 
			@PromoId UNIQUEIDENTIFIER,
			@ProductId UNIQUEIDENTIFIER,
			@PromoDispatchesStart DATETIMEOFFSET(7),
			@PromoDispatchesEnd DATETIMEOFFSET(7),
			@PromoProductIncrementalQty FLOAT,
			@PromoDuration INT,
			@IncrementalQty FLOAT,
			@MarsWeekFullName NVARCHAR(12),
			@DateWeekStart DATE,
			@DateWeekEnd DATE,
			@LastChangedDate DATETIMEOFFSET(7),
			@DemandCode NVARCHAR(255),
			@DMDGroup NVARCHAR(MAX),
			@ObjectId INT,
			@ParentId INT,
			@DeviationCoefficient FLOAT,
			
			@Day INT,
			@DaysBefore INT,
			@PromoDay INT,
			@DayQTY FLOAT,
			@Deviation FLOAT;


	OPEN PromoProductCursor;
	WHILE 1 = 1
	BEGIN
		FETCH NEXT 
			FROM PromoProductCursor 
			INTO 
				@PromoId,
				@ProductId,
				@PromoDispatchesStart,
				@PromoDispatchesEnd,
				@PromoProductIncrementalQty,
				@PromoDuration,
				@LastChangedDate,
				@DemandCode,
				@DMDGroup,
				@ObjectId,
				@ParentId,
				@DeviationCoefficient;

		IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'PromoProductCursor') <> 0
			BREAK;

		SET @DeviationCoefficient = ISNULL(@DeviationCoefficient, 0);
		SET @DayQTY = @PromoProductIncrementalQty / @PromoDuration;
		OPEN MarsWeekCursor;
		WHILE 1 = 1
		BEGIN
			FETCH NEXT 
				FROM MarsWeekCursor 
				INTO 
					@DateWeekStart,
					@DateWeekEnd,
					@MarsWeekFullName;
			
			IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'MarsWeekCursor') <> 0
				BREAK;

			SET @IncrementalQty = 0;
			SET @Day = 1;
			WHILE(@Day <= 7)
			BEGIN
				IF (@DateWeekStart < CAST(@PromoDispatchesStart AS DATE) AND @DateWeekEnd < CAST(@PromoDispatchesStart AS DATE))
					OR (@DateWeekStart > CAST(@PromoDispatchesEnd AS DATE) AND @DateWeekEnd > CAST(@PromoDispatchesEnd AS DATE))
					BREAK;

				IF (CAST(@PromoDispatchesStart AS DATE) >= @DateWeekStart AND CAST(@PromoDispatchesStart AS DATE) <= @DateWeekEnd)
					SET @DaysBefore = 0;
				ELSE
					SET @DaysBefore = DATEDIFF(DAY, CAST(@PromoDispatchesStart AS DATE), @DateWeekStart);
				
				IF @DaysBefore <> 0
					SET @PromoDay = @DaysBefore + @Day;
				ELSE 
					SET @PromoDay = @Day - DATEDIFF(DAY, @DateWeekStart, CAST(@PromoDispatchesStart AS DATE)); 
				IF @PromoDay <= 0 OR @PromoDay > @PromoDuration
				BEGIN
					SET @Day = @Day + 1;
					CONTINUE;
				END;

				IF (@PromoDuration % 2 <> 0 AND (@PromoDuration + 1) / 2 = @PromoDay)
					SET @Deviation = 0;
				ELSE IF @PromoDuration / 2 < @PromoDay
					OR (@PromoDuration + 1) / 2 < @PromoDay
					SET @Deviation = (-1 * @DayQTY * @DeviationCoefficient + (@PromoDuration - @PromoDay) * @DayQTY * 2 * @DeviationCoefficient / @PromoDuration) * -1;
				ELSE	
					SET @Deviation = -1 * @DayQTY * @DeviationCoefficient + (@PromoDay - 1) * @DayQTY * 2 * @DeviationCoefficient / @PromoDuration;

				SET @IncrementalQty = @IncrementalQty + @DayQTY - @Deviation;

				SET @Day = @Day + 1;
			END;

			WHILE (@DemandCode IS NULL OR @DemandCode = '') OR (@DMDGroup IS NULL OR @DMDGroup = '')
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

				SET @DemandCode = (
					SELECT TOP(1)
						ct.DemandCode
					FROM ClientTree ct
					WHERE ct.ObjectId = @ObjectId
				);
				SET @DMDGroup = (
					SELECT TOP(1)
						ct.DMDGroup
					FROM ClientTree ct
					WHERE ct.ObjectId = @ObjectId
				);				

				IF @ObjectId = 5000000
					BREAK;
			END;

			INSERT INTO CurrentDayIncremental
				VALUES (
					NEWID(),
					@MarsWeekFullName,
					@PromoId,
					@ProductId,
					ISNULL(@IncrementalQty, 0),
					@LastChangedDate,
					@DemandCode,
					@DMDGroup
				);
		END;
		CLOSE MarsWeekCursor;

	END;
	
	EXEC CreateIncrementalIndex 1;

	CLOSE PromoProductCursor;
	DEALLOCATE PromoProductCursor;
	DEALLOCATE MarsWeekCursor;
END;