UPDATE Jupiter.[User] SET [Disabled] = 1, [DeletedDate] = GETDATE()
WHERE [User].[Name]='kondratiev.aleksey@effem.com'
OR [User].[Name]='eugeny.suglobov@effem.com'
OR [User].[Name]='artem.x.morozov@effem.com'
OR [User].[Name]='andrey.philushkin@effem.com'
OR [User].[Name]='mikhail.volovich@effem.com'
OR [User].[Name]='bondarenko.eugeny@effem.com'
OR [User].[Name]='marina.kruchkova@effem.com'
OR [User].[Name]='alexander.spivak@effem.com'
OR [User].[Name]='ekaterina.kalugina@effem.com'
GO

INSERT INTO Jupiter.[User] ([Id], [DeletedDate], [Disabled], [Email], [Name], [Password], [PasswordSalt], [Sid])
VALUES 
(NEWID(), NULL, 0, NULL, 'kondratiev.aleksey@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'eugeny.suglobov@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'artem.x.morozov@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'andrey.philushkin@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'bondarenko.eugeny@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'mikhail.volovich@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'ekaterina.kalugina@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'alexander.spivak@effem.com', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'marina.kruchkova@effem.com', NULL, NULL, '')
GO