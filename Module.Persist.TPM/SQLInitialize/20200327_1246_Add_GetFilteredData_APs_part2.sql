-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('HistoricalFileCollectInterfaceSettings','FileCollectInterfaceSettings','FileSendInterfaceSettings','HistoricalFileSendInterfaceSettings','HistoricalInterfaces','Interfaces','CSVProcessInterfaceSettings','HistoricalCSVProcessInterfaceSettings','XMLProcessInterfaceSettings','HistoricalXMLProcessInterfaceSettings','CSVExtractInterfaceSettings','HistoricalCSVExtractInterfaceSettings','DeletedActualCOGSs','DeletedActualTradeInvestments','DeletedAgeGroups','DeletedAssortmentMatrices','DeletedBaseLine','DeletedBrands','DeletedBrandTeches','DeletedBTLs','DeletedBudgetItems','DeletedBudgets','DeletedBudgetSubItems','DeletedCategories','DeletedClients','DeletedCOGSs','DeletedColors','DeletedCommercialNets','DeletedCommercialSubnets','DeletedDemands','DeletedDistributors','DeletedEvents','DeletedFormats','DeletedIncrementalPromoes','DeletedMechanics','DeletedMechanicTypes','DeletedNodeTypes','DeletedNoneNegoes','DeletedNonPromoEquipment','DeletedNonPromoSupports','DeletedPostPromoEffects','DeletedProducts','DeletedPrograms','DeletedPromoDemands','DeletedPromoes','DeletedPromoProducts','DeletedPromoProductsCorrections','DeletedPromoSaleses','DeletedPromoStatuss','DeletedPromoSupports','DeletedPromoTypes','DeletedRegions','DeletedRejectReasons','DeletedRetailTypes','DeletedSales','DeletedSegments','DeletedStoreTypes','DeletedSubranges','DeletedTechHighLevels','DeletedTechnologies','DeletedTradeInvestments','DeletedVarieties','HistoricalActualCOGSs','HistoricalActualTradeInvestments','HistoricalAgeGroups','HistoricalAssortmentMatrices','HistoricalBaseLines','HistoricalBrands','HistoricalBrandTeches','HistoricalBTLs') AND [Action] IN ('GetFilteredData'));
DELETE FROM AccessPoint WHERE [Resource] IN ('HistoricalFileCollectInterfaceSettings','FileCollectInterfaceSettings','FileSendInterfaceSettings','HistoricalFileSendInterfaceSettings','HistoricalInterfaces','Interfaces','CSVProcessInterfaceSettings','HistoricalCSVProcessInterfaceSettings','XMLProcessInterfaceSettings','HistoricalXMLProcessInterfaceSettings','CSVExtractInterfaceSettings','HistoricalCSVExtractInterfaceSettings','DeletedActualCOGSs','DeletedActualTradeInvestments','DeletedAgeGroups','DeletedAssortmentMatrices','DeletedBaseLine','DeletedBrands','DeletedBrandTeches','DeletedBTLs','DeletedBudgetItems','DeletedBudgets','DeletedBudgetSubItems','DeletedCategories','DeletedClients','DeletedCOGSs','DeletedColors','DeletedCommercialNets','DeletedCommercialSubnets','DeletedDemands','DeletedDistributors','DeletedEvents','DeletedFormats','DeletedIncrementalPromoes','DeletedMechanics','DeletedMechanicTypes','DeletedNodeTypes','DeletedNoneNegoes','DeletedNonPromoEquipment','DeletedNonPromoSupports','DeletedPostPromoEffects','DeletedProducts','DeletedPrograms','DeletedPromoDemands','DeletedPromoes','DeletedPromoProducts','DeletedPromoProductsCorrections','DeletedPromoSaleses','DeletedPromoStatuss','DeletedPromoSupports','DeletedPromoTypes','DeletedRegions','DeletedRejectReasons','DeletedRetailTypes','DeletedSales','DeletedSegments','DeletedStoreTypes','DeletedSubranges','DeletedTechHighLevels','DeletedTechnologies','DeletedTradeInvestments','DeletedVarieties','HistoricalActualCOGSs','HistoricalActualTradeInvestments','HistoricalAgeGroups','HistoricalAssortmentMatrices','HistoricalBaseLines','HistoricalBrands','HistoricalBrandTeches','HistoricalBTLs') AND [Action] IN ('GetFilteredData') AND [Disabled] = 'false';

declare @list varchar(MAX), @i int
select @i=0, @list ='HistoricalFileCollectInterfaceSettings,FileCollectInterfaceSettings,FileSendInterfaceSettings,HistoricalFileSendInterfaceSettings,HistoricalInterfaces,Interfaces,CSVProcessInterfaceSettings,HistoricalCSVProcessInterfaceSettings,XMLProcessInterfaceSettings,HistoricalXMLProcessInterfaceSettings,CSVExtractInterfaceSettings,HistoricalCSVExtractInterfaceSettings,DeletedActualCOGSs,DeletedActualTradeInvestments,DeletedAgeGroups,DeletedAssortmentMatrices,DeletedBaseLine,DeletedBrands,DeletedBrandTeches,DeletedBTLs,DeletedBudgetItems,DeletedBudgets,DeletedBudgetSubItems,DeletedCategories,DeletedClients,DeletedCOGSs,DeletedColors,DeletedCommercialNets,DeletedCommercialSubnets,DeletedDemands,DeletedDistributors,DeletedEvents,DeletedFormats,DeletedIncrementalPromoes,DeletedMechanics,DeletedMechanicTypes,DeletedNodeTypes,DeletedNoneNegoes,DeletedNonPromoEquipment,DeletedNonPromoSupports,DeletedPostPromoEffects,DeletedProducts,DeletedPrograms,DeletedPromoDemands,DeletedPromoes,DeletedPromoProducts,DeletedPromoProductsCorrections,DeletedPromoSaleses,DeletedPromoStatuss,DeletedPromoSupports,DeletedPromoTypes,DeletedRegions,DeletedRejectReasons,DeletedRetailTypes,DeletedSales,DeletedSegments,DeletedStoreTypes,DeletedSubranges,DeletedTechHighLevels,DeletedTechnologies,DeletedTradeInvestments,DeletedVarieties,HistoricalActualCOGSs,HistoricalActualTradeInvestments,HistoricalAgeGroups,HistoricalAssortmentMatrices,HistoricalBaseLines,HistoricalBrands,HistoricalBrandTeches,HistoricalBTLs,'

while( @i < LEN(@list))
begin
    declare @item varchar(MAX)
    SELECT  @item = SUBSTRING(@list,  @i,CHARINDEX(',',@list,@i)-@i)

     --Точка доступа
	INSERT INTO AccessPoint 
	(Resource, Action, Disabled, DeletedDate) VALUES 
	(@item, 'GetFilteredData', 0, NULL);

	--Роли
	--Administrator
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	--FunctionalExpert
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	-- CustomerMarketing
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	-- KeyAccountManager
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	-- DemandFinance
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	-- DemandPlanning
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	-- SuperReader
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	-- CustomerMarketing
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 
	
	-- SupportAdministrator
	INSERT INTO AccessPointRole
	(Id, RoleId, AccessPointId)
	SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN (@item) AND [Action] = 'GetFilteredData'; 

    set @i = CHARINDEX(',',@list,@i)+1
    if(@i = 0) set @i = LEN(@list) 
end