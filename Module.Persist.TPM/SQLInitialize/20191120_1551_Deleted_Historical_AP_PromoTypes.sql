 
DECLARE @ResourceName VARCHAR(MAX) = 'HistoricalPromoTypes';
DECLARE @Action VARCHAR(MAX) = 'GetHistoricalPromoTypes';

DELETE AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = @ResourceName);

DELETE AccessPoint WHERE Resource = @ResourceName;