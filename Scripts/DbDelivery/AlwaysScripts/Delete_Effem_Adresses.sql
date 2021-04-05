--удаление марс-адресов из настроек рассылки 
DECLARE @value NVARCHAR(100);
SET @value ='%@effem.com'

DELETE FROM Recipient WHERE 
Value LIKE @value OR MailNotificationSettingId IN 
(SELECT Id FROM MailNotificationSetting WHERE
[To] LIKE @value OR CC LIKE @value OR BCC LIKE @value)

DELETE FROM MailNotificationSetting WHERE
[To] LIKE @value OR CC LIKE @value OR BCC LIKE @value


--удаление марс-пользователей
SET @value = 'mars-%'
UPDATE [User] SET Disabled = 1, DeletedDate = GETDATE(), Email = NULL
WHERE [Name] LIKE @value AND Disabled = 0