UPDATE BlockedPromo
SET [Disabled]=1
WHERE [Disabled]=0 AND HandlerId in (Select H.Id From LoopHandler H Where LOWER(H.[Name]) = 'module.host.tpm.handlers.baselineupgradehandler')

DELETE FROM LoopHandler WHERE LOWER([Name]) = 'module.host.tpm.handlers.baselineupgradehandler'
AND LOWER(ExecutionMode) = 'schedule';