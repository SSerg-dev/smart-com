UPDATE [User] SET [Disabled] = 1, [DeletedDate] = GETDATE()
WHERE [Name]='artem polyanin@smartcom.software'
OR [User].[Name]='andrey.filyushkin@smartcom.software'
OR [User].[Name]='alexander.zinkevich@smartcom.software'
OR [User].[Name]='alexey.kondratiev@smartcom.software'
OR [User].[Name]='andrey.vologdin@smartcom.software'
OR [User].[Name]='alexander.spivak@smartcom.software'
OR [User].[Name]='artem.morozov@smartcom.software'
OR [User].[Name]='natalia.aleksandrova@smartcom.software'
OR [User].[Name]='marina.kryuchkova@smartcom.software'
OR [User].[Name]='evgeny.suglobov@smartcom.software'
OR [User].[Name]='evgeny.bondarenko@smartcom.software'
OR [User].[Name]='oleg.zazaev@smartcom.software'
OR [User].[Name]='oleg.borisov@smartcom.software'
OR [User].[Name]='ekaterina.kalugina@smartcom.software'
OR [User].[Name]='ekaterina.burlak@smartcom.software'
OR [User].[Name]='mikhail.volovich@smartcom.software'
OR [User].[Name]='vadim.kosarev@smartcom.software'
OR [User].[Name]='ilia.fedorov@smartcom.software'
OR [User].[Name]='denis.moskvitin@smartcom.software'
OR [User].[Name]='anatoliy.soldatov@smartcom.software'
OR [User].[Name]='dmitry.puzikov@smartcom.software'
GO

INSERT INTO [User] ([Id], [DeletedDate], [Disabled], [Email], [Name], [Password], [PasswordSalt], [Sid])
VALUES 
(NEWID(), NULL, 0, NULL, 'artem polyanin@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'andrey.filyushkin@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'alexander.zinkevich@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'alexey.kondratiev@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'andrey.vologdin@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'alexander.spivak@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'artem.morozov@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'natalia.aleksandrova@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'marina.kryuchkova@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'evgeny.suglobov@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'evgeny.bondarenko@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'oleg.zazaev@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'oleg.borisov@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'ekaterina.kalugina@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'ekaterina.burlak@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'mikhail.volovich@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'vadim.kosarev@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'ilia.fedorov@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'denis.moskvitin@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'anatoliy.soldatov@smartcom.software', NULL, NULL, ''),
(NEWID(), NULL, 0, NULL, 'dmitry.puzikov@smartcom.software', NULL, NULL, '')
GO