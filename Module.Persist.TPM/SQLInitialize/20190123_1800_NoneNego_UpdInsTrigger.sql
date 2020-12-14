CREATE TRIGGER nonenego_UpdateInsertTrigger ON [NoneNego]
AFTER INSERT, UPDATE
AS
UPDATE [NoneNego]
SET [ToDate] = '9999-12-28T23:59:59.999+00:00'
WHERE Id = (SELECT Id FROM inserted) AND ToDate IS NULL