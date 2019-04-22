INSERT INTO AccessPoint
(Id, Disabled, DeletedDate, Resource, [Action], Description)
VALUES('F45896BF-F91A-E911-8BBF-08606E18DF3F', 0, NULL, 'ClientTreeSharesViews', 'ExportXLSX', 'ExportXLSX');
INSERT INTO AccessPoint
(Id, Disabled, DeletedDate, Resource, [Action], Description)
VALUES('6A8654CB-8F1D-E911-8BBF-08606E18DF3F', 0, NULL, 'ClientTreeSharesViews', 'FullImportXLSX', 'FullImportXLSX');


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('2E0FFCE3-F91A-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'F45896BF-F91A-E911-8BBF-08606E18DF3F');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('263FEC7F-BB1F-E911-8BBF-08606E18DF3F', '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', 'F45896BF-F91A-E911-8BBF-08606E18DF3F');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('273FEC7F-BB1F-E911-8BBF-08606E18DF3F', '3F0B3688-2983-48D8-974C-3B541D1C12A4', 'F45896BF-F91A-E911-8BBF-08606E18DF3F');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('AFD11F89-BB1F-E911-8BBF-08606E18DF3F', '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', 'F45896BF-F91A-E911-8BBF-08606E18DF3F');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('3B6525AF-BB1F-E911-8BBF-08606E18DF3F', '60009B10-D398-4686-B5C4-6D0032152AF4', 'F45896BF-F91A-E911-8BBF-08606E18DF3F');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('3C6525AF-BB1F-E911-8BBF-08606E18DF3F', 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853', 'F45896BF-F91A-E911-8BBF-08606E18DF3F');


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('0E9F89D8-8F1D-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '6A8654CB-8F1D-E911-8BBF-08606E18DF3F');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('3B51D11D-BC1F-E911-8BBF-08606E18DF3F', '3F0B3688-2983-48D8-974C-3B541D1C12A4', '6A8654CB-8F1D-E911-8BBF-08606E18DF3F');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES('3C51D11D-BC1F-E911-8BBF-08606E18DF3F', '60009B10-D398-4686-B5C4-6D0032152AF4', '6A8654CB-8F1D-E911-8BBF-08606E18DF3F');
