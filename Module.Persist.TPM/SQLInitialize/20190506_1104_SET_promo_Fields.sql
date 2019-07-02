--Установка полей для отчёта
CREATE FUNCTION ConcatItems(@GroupId uniqueidentifier)
   RETURNS NVARCHAR(500)
AS
BEGIN
    DECLARE @ItemList varchar(500)
    SET @ItemList = ''

    SELECT @ItemList = @ItemList + ';' + Name
    FROM ProductTree
    WHERE ObjectId IN (SELECT ProductTreeObjectId from PromoProductTree WHERE PromoId = @GroupId AND Disabled = 0) AND (EndDate is NULL OR EndDate > GETDATE())

    RETURN SUBSTRING(@ItemList, 2, 500)
END
GO
CREATE FUNCTION GetL1ClientName(@ObjectId int)
   RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @L1Name varchar(100)
	DECLARE @Depth int
	DECLARE @ParentId int

    SELECT @L1Name = [Name], @Depth = depth, @ParentId = parentId
    FROM ClientTree WHERE ObjectId = @ObjectId AND (EndDate is NULL OR EndDate > GETDATE());
	IF @Depth=1
		RETURN @L1Name
	ELSE IF @Depth=0
		RETURN NULL;
	ELSE IF @ParentId IS NOT NULL
		RETURN dbo.GetL1ClientName(@ParentId)
	ELSE 
		RETURN NULL;
	RETURN NULL;
END
GO
CREATE FUNCTION GetL2ClientName(@ObjectId int)
   RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @L2Name varchar(100)
	DECLARE @Depth int
	DECLARE @ParentId int

    SELECT @L2Name = [Name], @Depth = depth, @ParentId = parentId
    FROM ClientTree WHERE ObjectId = @ObjectId AND (EndDate is NULL OR EndDate > GETDATE());
	IF @Depth=2
		RETURN @L2Name
	ELSE IF @Depth=0
		RETURN NULL;
	ELSE IF @ParentId IS NOT NULL
		RETURN dbo.GetL2ClientName(@ParentId)
	ELSE 
		RETURN NULL;
	RETURN NULL;
END
GO

UPDATE promo SET ProductSubrangesList = dbo.ConcatItems(Id);
UPDATE promo SET EventName = (SELECT [Name] from Event WHERE id = EventId);
UPDATE promo SET ClientName = (SELECT [Name] from ClientTree WHERE ClientTree.ObjectId = ClientTreeId  AND (EndDate is NULL OR EndDate > GETDATE()));
UPDATE promo SET Client1LevelName = dbo.GetL1ClientName(ClientTreeId);
UPDATE promo SET Client2LevelName = dbo.GetL2ClientName(ClientTreeId);
UPDATE promo SET PromoDuration = DATEDIFF(DAY, StartDate, EndDate);
UPDATE promo SET DispatchDuration = DATEDIFF(DAY, DispatchesStart, DispatchesEnd);

DROP FUNCTION ConcatItems;
DROP FUNCTION GetL1ClientName;
DROP FUNCTION GetL2ClientName;