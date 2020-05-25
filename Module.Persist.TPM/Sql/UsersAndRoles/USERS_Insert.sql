UPDATE [dbo].[User] SET [Disabled] = 1 WHERE [Name]='smartcom\marina.kryuchkova'
OR [Name]='smartcom\artem.morozov'
OR [User].[Name]='smartcom\mikhail.volovich'
OR [User].[Name]='smartcom\denis.moskvitin'
OR [User].[Name]='smartcom\alexander.streltsov'
OR [User].[Name]='smartcom\anatoliy.soldatov'
OR [User].[Name]='smartcom\vadim.kosarev'
OR [User].[Name]='smartcom\ilia.fedorov'
OR [User].[Name]='smartcom\natalia.aleksandrova'
OR [User].[Name]='smartcom\dmitry.puzikov'
OR [User].[Name]='smartcom\artem.polyanin'
OR [User].[Name]='smartcom\alexey.morozkin'
OR [User].[Name]='SMARTCOM\alexandr.butyrskiy'
OR [User].[Name]='smartcom\ekaterina.kalugina'
OR [User].[Name]='smartcom\andrey.filyushkin'
OR [User].[Name]='smartcom\evgeny.suglobov'
OR [User].[Name]='smartcom\alexey.kondratiev'
OR [User].[Name]='smartcom\andrey.samborsky'
OR [User].[Name]='smartcom\alexander.pereponov' 
OR [User].[Name]='smartcom\alexander.zinkevich' 
OR [User].[Name]='smartcom\maria.belyakova' 
OR [User].[Name]='smartcom\andrey.vologdin' 
OR [User].[Name]='smartcom\alexander.spivak' 
OR [User].[Name]='smartcom\evgeny.bondarenko' 
GO

INSERT INTO [dbo].[User] ([Id], [DeletedDate], [Disabled], [Email], [Name], [Password], [PasswordSalt], [Sid])
VALUES 
(NEWID(), NULL, 0, NULL, 'smartcom\marina.kryuchkova', NULL, NULL, 'S-1-5-21-3086326434-3727772798-3018203464-1002'),
(NEWID(), NULL, 0, NULL, 'smartcom\artem.morozov', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1303'),
(NEWID(), NULL, 0, NULL, 'smartcom\mikhail.volovich', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1127'),
(NEWID(), NULL, 0, NULL, 'smartcom\denis.moskvitin', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1320'),
(NEWID(), NULL, 0, NULL, 'smartcom\alexander.streltsov', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1298'),
(NEWID(), NULL, 0, NULL, 'smartcom\anatoliy.soldatov', NULL, NULL, 'S-1-5-21-2262509386-4168703749-3092966379-1001'),
(NEWID(), NULL, 0, NULL, 'smartcom\vadim.kosarev', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1224'),
(NEWID(), NULL, 0, NULL, 'smartcom\ilia.fedorov', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1309'),
(NEWID(), NULL, 0, NULL, 'smartcom\natalia.aleksandrova', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1300'),
(NEWID(), NULL, 0, NULL, 'smartcom\dmitry.puzikov', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1306'),
(NEWID(), NULL, 0, NULL, 'smartcom\artem.polyanin', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-3122'),
(NEWID(), NULL, 0, NULL, 'smartcom\alexey.morozkin', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1282'),
(NEWID(), NULL, 0, NULL, 'SMARTCOM\alexandr.butyrskiy', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1239'),
(NEWID(), NULL, 0, NULL, 'smartcom\ekaterina.kalugina', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1402'),
(NEWID(), NULL, 0, NULL, 'smartcom\andrey.filyushkin', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1396'),
(NEWID(), NULL, 0, NULL, 'smartcom\evgeny.suglobov', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1313'),
(NEWID(), NULL, 0, NULL, 'smartcom\alexey.kondratiev', NULL, NULL, 'S-1-5-21-1185832031-822170871-3086717415-1003'),
(NEWID(), NULL, 0, NULL, 'smartcom\andrey.samborsky', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-3478'),
(NEWID(), NULL, 0, NULL, 'smartcom\alexander.pereponov', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-1492'),
(NEWID(), NULL, 0, NULL, 'smartcom\alexander.zinkevich', NULL, NULL, 'S-1-5-21-2960532321-970095943-1536476367-1002'),
(NEWID(), NULL, 0, NULL, 'smartcom\maria.belyakova', NULL, NULL, 'S-1-5-21-943814875-2088804584-4038503024-1002'),
(NEWID(), NULL, 0, NULL, 'smartcom\andrey.vologdin', NULL, NULL, 'S-1-5-21-1563142702-718819802-2742959173-1005'),
(NEWID(), NULL, 0, NULL, 'smartcom\alexander.spivak', NULL, NULL, 'S-1-5-21-2614690131-2606777446-1620217832-1005'),
(NEWID(), NULL, 0, NULL, 'smartcom\evgeny.bondarenko', NULL, NULL, 'S-1-5-21-3416733516-166020062-2842090717-1002')
GO