UPDATE [User] SET [Disabled] = 1 WHERE [Name]='MSNBH-POLYAART\artem polyanin'
OR [User].[Name]='msnbh-filyuand\andrey filyushkin'
OR [User].[Name]='msnbh-zinkeale\alexander zinkevich'
OR [User].[Name]='msnbh-kondrale\alexey kondratiev'
OR [User].[Name]='msnbh-vologand\andrey vologdin'
OR [User].[Name]='msnbh-belyamar\maria belyakova'
OR [User].[Name]='msnbh-spivaale\alexander spivak'
GO

INSERT INTO [User] ([Id], [DeletedDate], [Disabled], [Email], [Name], [Password], [PasswordSalt], [Sid])
VALUES 
(NEWID(), NULL, 0, NULL, 'MSNBH-POLYAART\artem polyanin', NULL, NULL, 'S-1-5-21-1833980595-3398159026-1082415255-3122'),
(NEWID(), NULL, 0, NULL, 'msnbh-filyuand\andrey filyushkin', NULL, NULL, 'S-1-5-21-1711696212-265188269-1646879335-1002'),
(NEWID(), NULL, 0, NULL, 'msnbh-zinkeale\alexander zinkevich', NULL, NULL, 'S-1-5-21-2960532321-970095943-1536476367-1002'),
(NEWID(), NULL, 0, NULL, 'msnbh-kondrale\alexey kondratiev', NULL, NULL, 'S-1-5-21-2626165454-1941722167-3939239512-1002'),
(NEWID(), NULL, 0, NULL, 'msnbh-vologand\andrey vologdin', NULL, NULL, ' S-1-5-21-1563142702-718819802-2742959173-1005'),
(NEWID(), NULL, 0, NULL, 'msnbh-belyamar\maria belyakova', NULL, NULL, 'S-1-5-21-943814875-2088804584-4038503024-1002'),
(NEWID(), NULL, 0, NULL, 'msnbh-spivaale\alexander spivak', NULL, NULL, 'S-1-5-21-2614690131-2606777446-1620217832-1005')

GO