-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('CoefficientSI2SOs','HistoricalCoefficientSI2SOs','DeletedCoefficientSI2SOs','PriceLists') AND [Action] IN ('GetFilteredData'));
DELETE FROM AccessPoint WHERE [Resource] IN ('CoefficientSI2SOs','HistoricalCoefficientSI2SOs','DeletedCoefficientSI2SOs','PriceLists') AND [Action] IN ('GetFilteredData') AND [Disabled] = 'false';

declare @list varchar(MAX), @i int
select @i=0, @list ='CoefficientSI2SOs,HistoricalCoefficientSI2SOs,DeletedCoefficientSI2SOs,PriceLists,'

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