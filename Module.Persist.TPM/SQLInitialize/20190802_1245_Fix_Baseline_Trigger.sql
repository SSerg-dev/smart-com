ALTER TRIGGER [BaseLine_ChangesIncident_Insert_Update_Trigger]
ON [BaseLine]
AFTER INSERT, UPDATE
AS 
DECLARE @count INT = 1
DECLARE @amount INT
DECLARE @id UNIQUEIDENTIFIER 
SELECT @amount = COUNT(*) FROM INSERTED
WHILE @count <> @amount
	BEGIN
		SET @id = (SELECT Id FROM ( SELECT ROW_NUMBER() OVER (ORDER BY Id ASC) AS rownumber, Id FROM INSERTED) AS t
				   WHERE rownumber = @count)
		INSERT INTO ChangesIncident ([DirectoryName], [ItemId], [CreateDate], [ProcessDate], [DeletedDate], [Disabled]) 
		VALUES ('BaseLine', @id, GETDATE(), NULL, NULL, 0)
		SET @count = @count + 1
	END;