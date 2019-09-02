Ext.define('App.model.tpm.promosales.PromoSales', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoSales',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ClientId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        { name: 'BrandId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        { name: 'BrandTechId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        { name: 'PromoStatusId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        { name: 'MechanicId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        { name: 'MechanicTypeId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        { name: 'BudgetItemId', useNull: true, hidden: true, isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'Plan', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'Fact', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'MechanicDiscount', type: 'int', hidden: false, isDefault: true },
        {
            name: 'ClientCommercialSubnetCommercialNetName', type: 'string', mapping: 'Client.CommercialSubnet.CommercialNet.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'CommercialNet', hidden: false, isDefault: true
        },
        { name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
        { name: 'PromoStatusSystemName', type: 'string', mapping: 'PromoStatus.SystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'MechanicName', type: 'string', mapping: 'Mechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: true },
        { name: 'MechanicTypeName', type: 'string', mapping: 'MechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: true },
        {
            name: 'BudgetItemBudgetName', type: 'string', mapping: 'BudgetItem.Budget.Name',
            defaultFilterConfig: { valueField: 'BudgetName' }, breezeEntityType: 'Budget', hidden: false, isDefault: true
        },
        {
            name: 'BudgetItemName', type: 'string', mapping: 'BudgetItem.Name',
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BudgetItem', hidden: false, isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoSaleses',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});