Ext.define('App.model.tpm.promosales.HistoricalPromoSales', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalPromoSales',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'ClientId', useNull: true, hidden: true, isDefault: true },
        { name: 'BrandId', useNull: true, hidden: true, isDefault: true },
        { name: 'BrandTechId', useNull: true, hidden: true, isDefault: true },
        { name: 'PromoStatusId', useNull: true, hidden: true, isDefault: true },
        { name: 'MechanicId', useNull: true, hidden: true, isDefault: true },
        { name: 'BudgetItemId', useNull: true, hidden: true, isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ClientCommercialSubnetCommercialNetName', type: 'string' },
        { name: 'BrandName', type: 'string' },
        { name: 'BrandTechName', type: 'string' },
        { name: 'PromoStatusName', type: 'string' },
        { name: 'PromoStatusSystemName', type: 'string' },
        { name: 'MechanicName', type: 'string' },
        { name: 'MechanicDiscount', type: 'int' },
        { name: 'MechanicTypeName', type: 'string' },
        { name: 'Plan', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'Fact', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'BudgetItemBudgetName', type: 'string' },
        { name: 'BudgetItemName', type: 'string' }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPromoSaleses',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
