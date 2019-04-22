----------------------------------------------------------
--оставляем только X-sites, Catalog и POSM для MARKETING--
----------------------------------------------------------
DECLARE @budgetMarketingId UNIQUEIDENTIFIER;
SELECT @budgetMarketingId = Id FROM Budget WHERE LOWER([Name]) = 'marketing' and [Disabled] = 'FALSE';

IF (@budgetMarketingId IS NULL) BEGIN
	SET @budgetMarketingId = NEWID();
	INSERT INTO Budget VALUES (@budgetMarketingId, 'FALSE', NULL, 'Marketing');
END

DECLARE @budgetMarketingXsitesId UNIQUEIDENTIFIER;
SELECT @budgetMarketingXsitesId = Id FROM BudgetItem WHERE CHARINDEX('x-sites', LOWER([Name]), 0) > 0
	AND BudgetId = @budgetMarketingId
	AND [Disabled] = 'FALSE';

IF (@budgetMarketingXsitesId IS NULL) BEGIN
	SET @budgetMarketingXsitesId = NEWID();
	INSERT INTO BudgetItem VALUES (@budgetMarketingXsitesId, 'FALSE', NULL, 'X-sites', @budgetMarketingId, NULL);
END

DECLARE @budgetMarketingCatalogId UNIQUEIDENTIFIER;
SELECT @budgetMarketingCatalogId = Id FROM BudgetItem WHERE CHARINDEX('catalog', LOWER([Name]), 0) > 0
	AND BudgetId = @budgetMarketingId
	AND [Disabled] = 'FALSE';

IF (@budgetMarketingCatalogId IS NULL) BEGIN
	SET @budgetMarketingCatalogId = NEWID();
	INSERT INTO BudgetItem VALUES (@budgetMarketingCatalogId, 'FALSE', NULL, 'Catalog', @budgetMarketingId, NULL);
END

DECLARE @budgetMarketingPOSMId UNIQUEIDENTIFIER;
SELECT @budgetMarketingPOSMId = Id FROM BudgetItem WHERE CHARINDEX('posm', LOWER([Name]), 0) > 0
	AND BudgetId = @budgetMarketingId
	AND [Disabled] = 'FALSE';

IF (@budgetMarketingPOSMId IS NULL) BEGIN
	SET @budgetMarketingPOSMId = NEWID();
	INSERT INTO BudgetItem VALUES (@budgetMarketingPOSMId, 'FALSE', NULL, 'POSM', @budgetMarketingId, NULL);
END

----------------------------------------------------------------
--оставляем только X-sites, Catalog и POSM для Cost Production--
----------------------------------------------------------------

DECLARE @budgetCostProdId UNIQUEIDENTIFIER;
SELECT @budgetCostProdId = Id FROM Budget WHERE LOWER([Name]) = 'cost production' and [Disabled] = 'FALSE';

IF (@budgetCostProdId IS NULL) BEGIN
	SET @budgetCostProdId = NEWID();
	INSERT INTO Budget VALUES (@budgetCostProdId, 'FALSE', NULL, 'Cost production');
END

DECLARE @budgetCostProdXsitesId UNIQUEIDENTIFIER;
SELECT @budgetCostProdXsitesId = Id FROM BudgetItem WHERE CHARINDEX('x-sites', LOWER([Name]), 0) > 0
	AND BudgetId = @budgetCostProdId
	AND [Disabled] = 'FALSE';

IF (@budgetCostProdXsitesId IS NULL) BEGIN
	SET @budgetCostProdXsitesId = NEWID();
	INSERT INTO BudgetItem VALUES (@budgetCostProdXsitesId, 'FALSE', NULL, 'X-sites', @budgetCostProdId, NULL);
END

DECLARE @budgetCostProdCatalogId UNIQUEIDENTIFIER;
SELECT @budgetCostProdCatalogId = Id FROM BudgetItem WHERE CHARINDEX('catalog', LOWER([Name]), 0) > 0
	AND BudgetId = @budgetCostProdId
	AND [Disabled] = 'FALSE';

IF (@budgetCostProdCatalogId IS NULL) BEGIN
	SET @budgetCostProdCatalogId = NEWID();
	INSERT INTO BudgetItem VALUES (@budgetCostProdCatalogId, 'FALSE', NULL, 'Catalog', @budgetCostProdId, NULL);
END

DECLARE @budgetCostProdPOSMId UNIQUEIDENTIFIER;
SELECT @budgetCostProdPOSMId = Id FROM BudgetItem WHERE CHARINDEX('posm', LOWER([Name]), 0) > 0
	AND BudgetId = @budgetCostProdId
	AND [Disabled] = 'FALSE';

IF (@budgetCostProdPOSMId IS NULL) BEGIN
	SET @budgetCostProdPOSMId = NEWID();
	INSERT INTO BudgetItem VALUES (@budgetCostProdPOSMId, 'FALSE', NULL, 'POSM', @budgetCostProdId, NULL);
END

-----------------------------------------------------------
--выравниваем подстатьи между Marketing и Cost Production--
-----------------------------------------------------------

-- X-Sites
DECLARE xsitesSubItemsMarketing CURSOR LOCAL READ_ONLY  
	FOR SELECT [NAME] FROM BudgetSubItem WHERE [Disabled] = 'FALSE' AND BudgetItemId = @budgetMarketingXsitesId
  	  
	DECLARE @NameSubItem  NVARCHAR (255)
    DECLARE @subItemCostProductionId UNIQUEIDENTIFIER;

  	OPEN xsitesSubItemsMarketing 
  	FETCH NEXT FROM xsitesSubItemsMarketing INTO @NameSubItem
  	WHILE @@FETCH_STATUS = 0   
   	BEGIN
  		SET @subItemCostProductionId = NULL;
		SELECT @subItemCostProductionId = Id FROM BudgetSubItem WHERE [Name] = @NameSubItem	AND BudgetItemId = @budgetCostProdXsitesId AND [Disabled] = 'FALSE';

		IF (@subItemCostProductionId IS NULL) BEGIN			
			SET @subItemCostProductionId = NEWID();
			INSERT INTO BudgetSubItem VALUES (@subItemCostProductionId, 'FALSE', NULL, @NameSubItem, @budgetCostProdXsitesId);
		END
		FETCH NEXT FROM xsitesSubItemsMarketing INTO @NameSubItem
  	END  
  	CLOSE xsitesSubItemsMarketing 

-- Catalog
DECLARE catalogSubItemsMarketing CURSOR LOCAL READ_ONLY  
	FOR SELECT [NAME] FROM BudgetSubItem WHERE [Disabled] = 'FALSE' AND BudgetItemId = @budgetMarketingCatalogId

  	OPEN catalogSubItemsMarketing 
  	FETCH NEXT FROM catalogSubItemsMarketing INTO @NameSubItem
  	WHILE @@FETCH_STATUS = 0   
   	BEGIN
  		SET @subItemCostProductionId = NULL;
		SELECT @subItemCostProductionId = Id FROM BudgetSubItem WHERE [Name] = @NameSubItem	AND BudgetItemId = @budgetCostProdCatalogId AND [Disabled] = 'FALSE';

		IF (@subItemCostProductionId IS NULL) BEGIN			
			SET @subItemCostProductionId = NEWID();
			INSERT INTO BudgetSubItem VALUES (@subItemCostProductionId, 'FALSE', NULL, @NameSubItem, @budgetCostProdCatalogId);
		END
		FETCH NEXT FROM catalogSubItemsMarketing INTO @NameSubItem
  	END  
  	CLOSE catalogSubItemsMarketing

-- POSM
DECLARE posmSubItemsMarketing CURSOR LOCAL READ_ONLY  
	FOR SELECT [NAME] FROM BudgetSubItem WHERE [Disabled] = 'FALSE' AND BudgetItemId = @budgetMarketingPOSMId

  	OPEN posmSubItemsMarketing 
  	FETCH NEXT FROM posmSubItemsMarketing INTO @NameSubItem
  	WHILE @@FETCH_STATUS = 0   
   	BEGIN
  		SET @subItemCostProductionId = NULL;
		SELECT @subItemCostProductionId = Id FROM BudgetSubItem WHERE [Name] = @NameSubItem	AND BudgetItemId = @budgetCostProdPOSMId AND [Disabled] = 'FALSE';

		IF (@subItemCostProductionId IS NULL) BEGIN			
			SET @subItemCostProductionId = NEWID();
			INSERT INTO BudgetSubItem VALUES (@subItemCostProductionId, 'FALSE', NULL, @NameSubItem, @budgetCostProdPOSMId);
		END
		FETCH NEXT FROM posmSubItemsMarketing INTO @NameSubItem
  	END  
  	CLOSE posmSubItemsMarketing 