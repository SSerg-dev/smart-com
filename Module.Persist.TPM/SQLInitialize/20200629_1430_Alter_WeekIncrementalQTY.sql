SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE OR ALTER FUNCTION [WeekIncrementalQTY](@PromoProductIncrementalQty FLOAT,
											@PromoDispatchesStart DATETIMEOFFSET(7),
											@PromoDispatchesEnd DATETIMEOFFSET(7),
											@PromoDuration INT,
											@DateWeekStart DATE,
											@DateWeekEnd DATE,
											@DeviationCoefficient FLOAT,
											@Day INT,
											@EndDay INT)
	RETURNS FLOAT
AS
BEGIN
	DECLARE
		@IncrementalQty FLOAT,
		@DaysBefore INT,
		@PromoDay INT,
		@DayQTY FLOAT,
		@Deviation FLOAT,
		@N FLOAT;

		IF @PromoDuration % 2 <> 0 AND @PromoDuration <> 1
			SET @N = (2 * @DeviationCoefficient) / (@PromoDuration - 1)
		ELSE
			SET @N = (2 * @DeviationCoefficient) / @PromoDuration

		SET @DayQTY = @PromoProductIncrementalQty / @PromoDuration;
		SET @IncrementalQty = 0;
		WHILE(@Day <= @EndDay)
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
				SET @PromoDay = 8 - @Day - DATEDIFF(DAY, @DateWeekStart, CAST(@PromoDispatchesStart AS DATE)); 

			IF @PromoDay <= 0 OR @PromoDay > @PromoDuration
			BEGIN
				SET @Day = @Day + 1;
				CONTINUE;
			END;
			
			IF (@PromoDuration % 2 <> 0 AND (@PromoDuration + 1) / 2 = @PromoDay)
				SET @Deviation = 0;
			ELSE IF @PromoDay > @PromoDuration / 2
				OR  @PromoDay > (@PromoDuration + 1) / 2
				SET @Deviation = (-1 * @DayQTY * @DeviationCoefficient + (@PromoDuration - @PromoDay) * @DayQTY * @N) * -1;
			ELSE	
				SET @Deviation = -1 * @DayQTY * @DeviationCoefficient + (@PromoDay - 1) * @DayQTY * @N;

			SET @IncrementalQty = @IncrementalQty + @DayQTY - @Deviation;

			SET @Day = @Day + 1;
		END;

	RETURN @IncrementalQty;
END;
GO
