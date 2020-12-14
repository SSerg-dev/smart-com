CREATE OR ALTER FUNCTION [CountAverage]
(
	@startDate DATETIMEOFFSET(7),
	@productid UNIQUEIDENTIFIER,
	@demandcode NVARCHAR(255)
)
	RETURNS FLOAT
AS
BEGIN
	DECLARE @result FLOAT;
	DECLARE @minStartDate DATETIMEOFFSET(7);
	DECLARE @maxStartDate DATETIMEOFFSET(7);
	DECLARE @diffWithMinDate INT;
	DECLARE @diffWithMaxDate INT;

	SELECT @minStartDate = MIN(StartDate) FROM BaseLine WHERE Disabled = 0 AND ProductId = @productid AND DemandCode = @demandcode;
	SELECT @maxStartDate = MAX(StartDate) FROM BaseLine WHERE Disabled = 0 AND ProductId = @productid AND DemandCode = @demandcode;
	SELECT @diffWithMinDate = ABS(DATEDIFF(WEEK, @startDate, @minStartDate)); 
	SELECT @diffWithMaxDate = ABS(DATEDIFF(WEEK, @startDate, @maxStartDate)); 

	IF(@diffWithMinDate <= 1) BEGIN
		SELECT @result = AVG(InputBaselineQTY) FROM BaseLine 
						 WHERE STARTDATE BETWEEN DATEADD(DAY, -(@diffWithMinDate * 7), @startDate) AND DATEADD(DAY, (4 - @diffWithMinDate) * 7, @startDate)
							   AND ProductId = @productid
							   AND DemandCode = @demandcode
							   AND Disabled = 0;
	END
	ELSE BEGIN
		IF(@diffWithMaxDate <= 1) BEGIN
			SELECT @result = AVG(InputBaselineQTY) FROM BaseLine 
							 WHERE STARTDATE BETWEEN DATEADD(DAY, -(4 - @diffWithMaxDate) * 7, @startDate) AND DATEADD(DAY, @diffWithMaxDate * 7, @startDate)
							   AND ProductId = @productid
							   AND DemandCode = @demandcode
							   AND Disabled = 0;
		END
		ELSE BEGIN
			SELECT @result = AVG(InputBaselineQTY) FROM BaseLine 
							 WHERE STARTDATE BETWEEN DATEADD(DAY,-14,@startDate) AND DATEADD(DAY,14,@startDate)
							   AND ProductId = @productid
							   AND DemandCode = @demandcode
							   AND Disabled = 0;
		END
	END

	RETURN @result
END