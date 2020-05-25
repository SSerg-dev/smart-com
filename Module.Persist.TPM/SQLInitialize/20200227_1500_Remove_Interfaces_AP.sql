DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('FileBuffers', 'HistoricalFileBuffers') AND [Action] IN ('GetHistoricalFileBuffers', 'Put', 'Patch'));
DELETE FROM AccessPoint WHERE [Resource] IN ('FileBuffers', 'HistoricalFileBuffers') AND [Action] IN ('GetHistoricalFileBuffers', 'Put', 'Patch') AND [Disabled] = 'false';

DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('FileSendInterfaceSettings', 'CSVExtractInterfaceSettings') AND [Action] IN ('Post', 'Delete'));
DELETE FROM AccessPoint WHERE [Resource] IN ('FileSendInterfaceSettings', 'CSVExtractInterfaceSettings') AND [Action] IN ('Post', 'Delete') 	AND [Disabled] = 'false';