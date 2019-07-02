﻿DECLARE @CustomerMarketingRoleId UNIQUEIDENTIFIER;
SELECT @CustomerMarketingRoleId = Id FROM [Role] WHERE LOWER([SystemName]) = 'CustomerMarketing' AND [Disabled] = 0;

IF (@CustomerMarketingRoleId IS NULL) BEGIN
	SET @CustomerMarketingRoleId ='DEF978B2-EC28-45DE-BD92-AF48ACCA1052';
	INSERT INTO [Role] (Id, [Disabled], DeletedDate, SystemName, DisplayName, IsAllow) VALUES (@CustomerMarketingRoleId, 0, NULL, 'CustomerMarketing', 'Customer Marketing', 1);
END

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(),  @CustomerMarketingRoleId, Id FROM ACCESSPOINT WHERE Resource + ':' + [Action] in ('Categories:GetCategory',
'Categories:GetCategories',
'DeletedCategories:GetDeletedCategory',
'DeletedCategories:GetDeletedCategories',
'HistoricalCategories:GetHistoricalCategories',
'Brands:GetBrand',
'Brands:GetBrands',
'DeletedBrands:GetDeletedBrand',
'DeletedBrands:GetDeletedBrands',
'HistoricalBrands:GetHistoricalBrands',
'Brands:ExportXLSX',
'Segments:GetSegment',
'Segments:GetSegments',
'DeletedSegments:GetDeletedSegment',
'DeletedSegments:GetDeletedSegments',
'HistoricalSegments:GetHistoricalSegments',
'Technologies:GetTechnology',
'Technologies:GetTechnologies',
'DeletedTechnologies:GetDeletedTechnology',
'DeletedTechnologies:GetDeletedTechnologies',
'HistoricalTechnologies:GetHistoricalTechnologies',
'Technologies:ExportXLSX',
'TechHighLevels:GetTechHighLevel',
'TechHighLevels:GetTechHighLevels',
'DeletedTechHighLevels:GetDeletedTechHighLevel',
'DeletedTechHighLevels:GetDeletedTechHighLevels',
'HistoricalTechHighLevels:GetHistoricalTechHighLevels',
'Programs:GetProgram',
'Programs:GetPrograms',
'DeletedPrograms:GetDeletedProgram',
'DeletedPrograms:GetDeletedPrograms',
'HistoricalPrograms:GetHistoricalPrograms',
'Formats:GetFormat',
'Formats:GetFormats',
'DeletedFormats:GetDeletedFormat',
'DeletedFormats:GetDeletedFormats',
'HistoricalFormats:GetHistoricalFormats',
'BrandTeches:GetBrandTech',
'BrandTeches:GetBrandTeches',
'DeletedBrandTeches:GetDeletedBrandTech',
'DeletedBrandTeches:GetDeletedBrandTeches',
'HistoricalBrandTeches:GetHistoricalBrandTeches',
'BrandTeches:ExportXLSX',
'Subranges:GetSubrange',
'Subranges:GetSubranges',
'DeletedSubranges:GetDeletedSubrange',
'DeletedSubranges:GetDeletedSubranges',
'HistoricalSubranges:GetHistoricalSubranges',
'AgeGroups:GetAgeGroup',
'AgeGroups:GetAgeGroups',
'DeletedAgeGroups:GetDeletedAgeGroup',
'DeletedAgeGroups:GetDeletedAgeGroups',
'HistoricalAgeGroups:GetHistoricalAgeGroups',
'Varieties:GetVariety',
'Varieties:GetVarieties',
'DeletedVarieties:GetDeletedVariety',
'DeletedVarieties:GetDeletedVarieties',
'HistoricalVarieties:GetHistoricalVarieties',
'Products:GetProduct',
'Products:GetProducts',
'DeletedProducts:GetDeletedProduct',
'DeletedProducts:GetDeletedProducts',
'HistoricalProducts:GetHistoricalProducts',
'Products:GetCategory',
'DeletedProducts:GetCategory',
'Products:GetBrand',
'DeletedProducts:GetBrand',
'Products:GetSegment',
'DeletedProducts:GetSegment',
'Products:GetTechnology',
'DeletedProducts:GetTechnology',
'Products:GetTechHighLevel',
'DeletedProducts:GetTechHighLevel',
'Products:GetProgram',
'DeletedProducts:GetProgram',
'Products:GetFormat',
'DeletedProducts:GetFormat',
'Products:GetBrandTech',
'DeletedProducts:GetBrandTech',
'Products:GetSubrange',
'DeletedProducts:GetSubrange',
'Products:GetAgeGroup',
'DeletedProducts:GetAgeGroup',
'Products:GetVariety',
'DeletedProducts:GetVariety',
'Products:ExportXLSX',
'Regions:GetRegion',
'Regions:GetRegions',
'DeletedRegions:GetDeletedRegion',
'DeletedRegions:GetDeletedRegions',
'HistoricalRegions:GetHistoricalRegions',
'CommercialNets:GetCommercialNet',
'CommercialNets:GetCommercialNets',
'DeletedCommercialNets:GetDeletedCommercialNet',
'DeletedCommercialNets:GetDeletedCommercialNets',
'HistoricalCommercialNets:GetHistoricalCommercialNets',
'CommercialSubnets:GetCommercialSubnet',
'CommercialSubnets:GetCommercialSubnets',
'DeletedCommercialSubnets:GetDeletedCommercialSubnet',
'DeletedCommercialSubnets:GetDeletedCommercialSubnets',
'HistoricalCommercialSubnets:GetHistoricalCommercialSubnets',
'CommercialSubnets:GetCommercialNet',
'DeletedCommercialSubnets:GetCommercialNet',
'Distributors:GetDistributor',
'Distributors:GetDistributors',
'DeletedDistributors:GetDeletedDistributor',
'DeletedDistributors:GetDeletedDistributors',
'HistoricalDistributors:GetHistoricalDistributors',
'StoreTypes:GetStoreType',
'StoreTypes:GetStoreTypes',
'DeletedStoreTypes:GetDeletedStoreType',
'DeletedStoreTypes:GetDeletedStoreTypes',
'HistoricalStoreTypes:GetHistoricalStoreTypes',
'Clients:GetClient',
'Clients:GetClients',
'DeletedClients:GetDeletedClient',
'DeletedClients:GetDeletedClients',
'HistoricalClients:GetHistoricalClients',
'Clients:GetRegion',
'DeletedClients:GetRegion',
'Clients:GetCommercialSubnet',
'DeletedClients:GetCommercialSubnet',
'Clients:GetDistributor',
'DeletedClients:GetDistributor',
'Clients:GetStoreType',
'DeletedClients:GetStoreType',
'HistoricalSettings:GetHistoricalSettings',
'HistoricalCSVExtractInterfaceSettings:GetHistoricalCSVExtractInterfaceSettings',
'Security:Get',
'Security:SaveGridSettings',
'Interfaces:GetInterfaces',
'Interfaces:GetInterface',
'HistoricalUsers:GetHistoricalUsers',
'HistoricalRoles:GetHistoricalRoles',
'File:ExportDownload',
'File:ImportResultSuccessDownload',
'File:ImportResultWarningDownload',
'File:ImportResultErrorDownload',
'AdUsers:GetAdUsers',
'HistoricalMailNotificationSettings:GetHistoricalMailNotificationSettings',
'UserLoopHandlers:GetUserLoopHandlers',
'UserLoopHandlers:GetUserLoopHandler',
'UserLoopHandlers:GetUser',
'HistoricalXMLProcessInterfaceSettings:GetHistoricalXMLProcessInterfaceSettings',
'HistoricalFileSendInterfaceSettings:GetHistoricalFileSendInterfaceSettings',
'CSVProcessInterfaceSettings:GetCSVProcessInterfaceSettings',
'CSVProcessInterfaceSettings:GetCSVProcessInterfaceSetting',
'CSVProcessInterfaceSettings:GetInterface',
'SingleLoopHandlers:GetSingleLoopHandlers',
'SingleLoopHandlers:GetSingleLoopHandler',
'SingleLoopHandlers:GetUser',
'LoopHandlers:Parameters',
'LoopHandlers:ReadLogFile',
'HistoricalInterfaces:GetHistoricalInterfaces',
'FileBuffers:GetFileBuffers',
'FileBuffers:GetFileBuffer',
'FileBuffers:GetInterface',
'FileSendInterfaceSettings:GetFileSendInterfaceSettings',
'FileSendInterfaceSettings:GetFileSendInterfaceSetting',
'FileSendInterfaceSettings:GetInterface',
'CSVExtractInterfaceSettings:GetCSVExtractInterfaceSettings',
'CSVExtractInterfaceSettings:GetCSVExtractInterfaceSetting',
'CSVExtractInterfaceSettings:GetInterface',
'FileCollectInterfaceSettings:GetFileCollectInterfaceSettings',
'FileCollectInterfaceSettings:GetFileCollectInterfaceSetting',
'FileCollectInterfaceSettings:GetInterface',
'DeletedMailNotificationSettings:GetDeletedMailNotificationSettings',
'DeletedMailNotificationSettings:GetDeletedMailNotificationSetting',
'HistoricalConstraints:GetHistoricalConstraints',
'HistoricalFileCollectInterfaceSettings:GetHistoricalFileCollectInterfaceSettings',
'HistoricalCSVProcessInterfaceSettings:GetHistoricalCSVProcessInterfaceSettings',
'XMLProcessInterfaceSettings:GetXMLProcessInterfaceSettings',
'XMLProcessInterfaceSettings:GetXMLProcessInterfaceSetting',
'XMLProcessInterfaceSettings:GetInterface',
'HistoricalFileBuffers:GetHistoricalFileBuffers',
'DeletedUsers:GetDeletedUsers',
'DeletedUsers:GetDeletedUser',
'Recipients:GetRecipients',
'Recipients:GetRecipient',
'Recipients:GetMailNotificationSetting',
'HistoricalRecipients:GetHistoricalRecipients',
'Mechanics:GetMechanic',
'Mechanics:GetMechanics',
'DeletedMechanics:GetDeletedMechanic',
'DeletedMechanics:GetDeletedMechanics',
'HistoricalMechanics:GetHistoricalMechanics',
'Mechanics:ExportXLSX',
'PromoStatuss:GetPromoStatus',
'PromoStatuss:GetPromoStatuss',
'DeletedPromoStatuss:GetDeletedPromoStatus',
'DeletedPromoStatuss:GetDeletedPromoStatuss',
'HistoricalPromoStatuss:GetHistoricalPromoStatuss',
'PromoStatuss:ExportXLSX',
'Budgets:GetBudget',
'Budgets:GetBudgets',
'DeletedBudgets:GetDeletedBudget',
'DeletedBudgets:GetDeletedBudgets',
'HistoricalBudgets:GetHistoricalBudgets',
'Budgets:ExportXLSX',
'BudgetItems:GetBudgetItem',
'BudgetItems:GetBudgetItems',
'DeletedBudgetItems:GetDeletedBudgetItem',
'DeletedBudgetItems:GetDeletedBudgetItems',
'HistoricalBudgetItems:GetHistoricalBudgetItems',
'BudgetItems:GetBudget',
'DeletedBudgetItems:GetBudget',
'BudgetItems:ExportXLSX',
'Promoes:GetPromo',
'Promoes:GetPromoes',
'DeletedPromoes:GetDeletedPromo',
'DeletedPromoes:GetDeletedPromoes',
'HistoricalPromoes:GetHistoricalPromoes',
'Promoes:Put',
'ImportPromoes:Put',
'Promoes:Post',
'ImportPromoes:Post',
'Promoes:Patch',
'ImportPromoes:Patch',
'Promoes:Delete',
'ImportPromoes:Delete',
'Promoes:GetClient',
'DeletedPromoes:GetClient',
'Promoes:GetBrand',
'DeletedPromoes:GetBrand',
'Promoes:GetBrandTech',
'DeletedPromoes:GetBrandTech',
'Promoes:GetProduct',
'DeletedPromoes:GetProduct',
'Promoes:GetPromoStatus',
'DeletedPromoes:GetPromoStatus',
'Promoes:GetMechanic',
'DeletedPromoes:GetMechanic',
'Promoes:ExportXLSX',
'Sales:GetSale',
'Sales:GetSales',
'DeletedSales:GetDeletedSale',
'DeletedSales:GetDeletedSales',
'HistoricalSales:GetHistoricalSales',
'Sales:GetPromo',
'DeletedSales:GetPromo',
'Sales:GetBudget',
'DeletedSales:GetBudget',
'Sales:GetBudgetItem',
'DeletedSales:GetBudgetItem',
'Colors:GetColor',
'Colors:GetColors',
'Colors:GetSuitable',
'Promoes:FullImportXLSX',
'PromoSaleses:GetPromoSale',
'PromoSaleses:GetPromoSaleses',
'DeletedPromoSaleses:GetDeletedPromoSaleses',
'DeletedPromoSaleses:GetDeletedPromoSales',
'HistoricalPromoSaleses:GetHistoricalPromoSaleses',
'Promoes:DeclinePromo',
'Demands:GetDemand',
'Demands:GetDemands',
'DeletedDemands:GetDeletedDemands',
'DeletedDemands:GetDeletedDemand',
'HistoricalDemands:GetHistoricalDemands',
'RejectReasons:GetRejectReason',
'RejectReasons:GetRejectReasons',
'DeletedRejectReasons:GetDeletedRejectReason',
'DeletedRejectReasons:GetDeletedRejectReasons',
'HistoricalRejectReasons:GetHistoricalRejectReasons',
'RejectReasons:ExportXLSX',
'ClientTrees:GetClientTree',
'ClientTrees:GetClientTrees',
'ProductTrees:GetProductTree',
'ProductTrees:GetProductTrees',
'Events:GetEvent',
'Events:GetEvents',
'DeletedEvents:GetDeletedEvent',
'DeletedEvents:GetDeletedEvents',
'HistoricalEvents:GetHistoricalEvents',
'Events:Put',
'Events:Post',
'Events:Patch',
'Events:Delete',
'Events:ExportXLSX',
'MechanicTypes:GetMechanicType',
'MechanicTypes:GetMechanicTypes',
'DeletedMechanicTypes:GetDeletedMechanicType',
'DeletedMechanicTypes:GetDeletedMechanicTypes',
'HistoricalMechanicTypes:GetHistoricalMechanicTypes',
'MechanicTypes:Put',
'MechanicTypes:Post',
'MechanicTypes:Patch',
'MechanicTypes:Delete',
'MechanicTypes:ExportXLSX',
'NodeTypes:GetNodeType',
'NodeTypes:GetNodeTypes',
'DeletedNodeTypes:GetDeletedNodeType',
'DeletedNodeTypes:GetDeletedNodeTypes',
'HistoricalNodeTypes:GetHistoricalNodeTypes',
'ProductTrees:GetHierarchyDetail',
'ClientTrees:GetHierarchyDetail',
'DeletedColors:GetDeletedColors',
'DeletedColors:GetDeletedColor',
'HistoricalColors:GetHistoricalColors',
'BaseClients:GetBaseClients',
'BaseClients:GetBaseClients',
'BaseClients:GetBaseClient',
'BaseClients:Post',
'PromoDemands:GetPromoDemand',
'PromoDemands:GetPromoDemands',
'DeletedPromoDemands:GetDeletedPromoDemand',
'DeletedPromoDemands:GetDeletedPromoDemands',
'HistoricalPromoDemands:GetHistoricalPromoDemands',
'PromoDemands:GetBrandTech',
'DeletedPromoDemands:GetBrandTech',
'PromoDemands:GetBrandTech',
'DeletedPromoDemands:GetBrandTech',
'PromoDemands:GetMechanic',
'DeletedPromoDemands:GetMechanic',
'PromoStatusChanges:PromoStatusChangesByPromo',
'BaseClientTreeViews:GetBaseClientTreeViews',
'RetailTypes:GetRetailType',
'RetailTypes:GetRetailTypes',
'DeletedRetailTypes:GetDeletedRetailType',
'DeletedRetailTypes:GetDeletedRetailTypes',
'HistoricalRetailTypes:GetHistoricalRetailTypes',
'RetailTypes:ExportXLSX',
'NoneNegoes:GetNoneNego',
'NoneNegoes:GetNoneNegoes',
'DeletedNoneNegoes:GetDeletedNoneNego',
'DeletedNoneNegoes:GetDeletedNoneNegoes',
'HistoricalNoneNegoes:GetHistoricalNoneNegoes',
'NoneNegoes:Put',
'NoneNegoes:Post',
'NoneNegoes:Patch',
'NoneNegoes:Delete',
'NoneNegoes:ExportXLSX',
'Promoes:ExportSchedule',
'NoneNegoes:IsValidPeriod',
'Actuals:GetActual',
'Actuals:GetActuals',
'DeletedActuals:GetDeletedActual',
'DeletedActuals:GetDeletedActuals',
'HistoricalActuals:GetHistoricalActuals',
'BaseLines:GetBaseLine',
'BaseLines:GetBaseLines',
'DeletedBaseLines:GetDeletedBaseLine',
'DeletedBaseLines:GetDeletedBaseLines',
'HistoricalBaseLines:GetHistoricalBaseLines',
'ClientTreeSharesViews:GetClientTreeSharesViews',
'ClientTreeSharesViews:ExportXLSX',
'ClientTrees:CanCreateBaseClient',
'BudgetSubItems:GetBudgetSubItem',
'BudgetSubItems:GetBudgetSubItems',
'DeletedBudgetSubItems:GetDeletedBudgetSubItem',
'DeletedBudgetSubItems:GetDeletedBudgetSubItems',
'HistoricalBudgetSubItems:GetHistoricalBudgetSubItems',
'BudgetSubItems:ExportXLSX',
'PostPromoEffects:GetPostPromoEffect',
'PostPromoEffects:GetPostPromoEffects',
'DeletedPostPromoEffects:GetDeletedPostPromoEffect',
'DeletedPostPromoEffects:GetDeletedPostPromoEffects',
'HistoricalPostPromoEffects:GetHistoricalPostPromoEffects',
'PostPromoEffects:ExportXLSX',
'PromoSupportPromoes:GetPromoSupportPromo',
'PromoSupportPromoes:GetPromoSupportPromoes',
'Promoes:CalculateMarketingTI',
'PromoSupportPromoes:Post',
'PromoSupportPromoes:Delete',
'Promoes:ChangeStatus',
'PromoSupportPromoes:GetValuesForItems',
'PromoSupportPromoes:PromoSuportPromoPost',
'Promoes:ReadPromoCalculatingLog',
'Promoes:CheckPromoCalculatingStatus',
'PromoProducts:GetPromoProduct',
'PromoProducts:GetPromoProducts',
'PromoProducts:Put',
'PromoProducts:Post',
'PromoProducts:Patch',
'PromoProducts:Delete',
'PromoProducts:ExportXLSX',
'PromoProducts:FullImportXLSX',
'DeletedPromoProducts:GetDeletedPromoProduct',
'DeletedPromoProducts:GetDeletedPromoProducts',
'HistoricalPromoProducts:GetHistoricalPromoProducts',
'PromoSupportPromoes:Patch',
'PromoSupportPromoes:ExportXLSX',
'PromoSupportPromoes:GetLinkedSubItems',
'PromoSupportPromoes:ManageSubItems',
'BaseLines:ExportDemandPriceListXLSX',
'PromoSupports:Put',
'PromoSupports:Post',
'PromoSupports:Patch',
'PromoSupports:Delete',
'PromoSupports:GetPromoSupportGroup',
'PromoSupports:SetUserTimestamp',
'PromoSupports:UploadFile',
'PromoSupports:DownloadFile',
'PromoSupports:GetUserTimestamp',
'PromoSupports:ExportXLSX',
'PromoSupports:GetPromoSupport',
'PromoSupports:GetPromoSupports',
'PromoSupports:PatchCostProduction',
'PromoROIReports:GetPromoROIReports',
'PromoROIReports:ExportXLSX',
'Events:FullImportXLSX',
'MechanicTypes:FullImportXLSX',
'PromoProducts:DownloadTemplateXLSX',
'Events:DownloadTemplateXLSX',
'MechanicTypes:DownloadTemplateXLSX',
'PromoViews:GetPromoView',
'PromoViews:GetPromoViews',
'PromoSupportPromoes:ChangeListPSP',
'Promoes:ExportPromoROIReportXLSX',
'NoneNegoes:DownloadTemplateXLSX',
'NoneNegoes:FullImportXLSX',
'Promoes:GetCanChangeStatePromoes',
'NodeTypes:ExportXLSX',
'BaseClientTreeViews:ExportXLSX',
'Colors:ExportXLSX',
'PromoGridViews:GetPromoGridViews',
'PromoGridViews:GetPromoGridView',
'PromoGridViews:Put',
'PromoGridViews:Post',
'PromoGridViews:Patch',
'PromoGridViews:Delete',
'PromoGridViews:ExportXLSX',
'PromoGridViews:GetCanChangeStatePromoGridViews',
'DeletedPromoGridViews:GetDeletedPromoGridViews',
'DeletedPromoGridViews:GetDeletedPromoGridView',
'HistoricalPromoGridViews:GetHistoricalPromoGridViews',
'ClientTrees:DownloadLogoFile',
'ProductTrees:DownloadLogoFile',
'AssortmentMatrices:GetAssortmentMatrices',
'AssortmentMatrices:GetAssortmentMatrix',
'DeletedAssortmentMatrices:GetDeletedAssortmentMatrix',
'DeletedAssortmentMatrices:GetDeletedAssortmentMatrices',
'HistoricalAssortmentMatrices:GetHistoricalAssortmentMatrices',
'AssortmentMatrices:Put',
'AssortmentMatrices:Post',
'AssortmentMatrices:Patch',
'AssortmentMatrices:Delete',
'AssortmentMatrices:ExportXLSX',
'IncrementalPromoes:GetIncrementalPromo',
'IncrementalPromoes:GetIncrementalPromoes',
'DeletedIncrementalPromoes:GetDeletedIncrementalPromo',
'DeletedIncrementalPromoes:GetDeletedIncrementalPromoes',
'HistoricalIncrementalPromoes:GetHistoricalIncrementalPromoes',
'IncrementalPromoes:Put',
'IncrementalPromoes:Post',
'IncrementalPromoes:Patch',
'IncrementalPromoes:Delete',
'IncrementalPromoes:ExportXLSX',
'IncrementalPromoes:FullImportXLSX',
'IncrementalPromoes:DownloadTemplateXLSX');