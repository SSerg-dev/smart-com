--Скрипт отключения таксов для загрузки прайслистов, материалов и клиентов
--Плюс замена путей для получения бейслайнов и отправки инкременталов
--Применять при копировании данных с прода на среду, где нужно сохранить данные на момент среза (например сценарирование)
UPDATE 
	[Scenario].LoopHandler
	SET
		NextExecutionDate = '2100-01-01'
	WHERE
		Name IN('Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckStarterHandler',
				'Module.Host.TPM.Handlers.PriceListMergeHandler',
				'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsCustomersCheckHandler')
		AND ExecutionMode = 'SCHEDULE'

UPDATE 
	[Scenario].LoopHandler
	SET
		NextExecutionDate = '2100-01-01'
	WHERE
		Name IN('Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsProductsCheckStarterHandler',
				'Module.Host.TPM.Handlers.PriceListMergeHandler',
				'Module.Host.TPM.Handlers.DataLakeIntegrationHandlers.MarsCustomersCheckHandler')
		AND ExecutionMode = 'SCHEDULE'

UPDATE 
	[Scenario].[FileCollectInterfaceSetting]
	SET
		SourcePath = 'in\Apollo\baselineScenario'

UPDATE 
	[Scenario].[FileSendInterfaceSetting]
	SET
		TargetPath = 'out\Apollo\upliftScenario'

		