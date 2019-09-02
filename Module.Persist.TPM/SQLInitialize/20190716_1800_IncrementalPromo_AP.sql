-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('Post', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX'));
Delete FROM AccessPoint WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('Post', 'Delete', 'FullImportXLSX', 'DownloadTemplateXLSX') AND [Disabled] = 'false';

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='HistoricalIncrementalPromoes' AND [Action] IN ('GetHistoricalIncrementalPromoes'));
Delete FROM AccessPoint WHERE [Resource]='HistoricalIncrementalPromoes' AND [Action] IN ('GetHistoricalIncrementalPromoes') AND [Disabled] = 'false';

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='DeletedIncrementalPromoes' AND [Action] IN ('GetDeletedIncrementalPromo', 'GetDeletedIncrementalPromoes'));
Delete FROM AccessPoint WHERE [Resource]='DeletedIncrementalPromoes' AND [Action] IN ('GetDeletedIncrementalPromo', 'GetDeletedIncrementalPromoes') AND [Disabled] = 'false';
