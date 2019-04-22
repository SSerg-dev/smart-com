Ext.define('App.model.tpm.promodemand.HistoricalPromoDemand', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'PromoDemand',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'MechanicId', hidden: true, isDefault: true },
        { name: 'MechanicTypeId', hidden: true, isDefault: true },
        { name: 'BrandTechBrandName', type: 'string', isDefault: true },
        { name: 'BrandTechName', type: 'string', isDefault: true },
        { name: 'Account', type: 'string', hidden: false, isDefault: true },
        { name: 'MechanicName', type: 'string', isDefault: true },
        { name: 'MechanicTypeName', type: 'string', isDefault: true },
        { name: 'Discount', type: 'int', hidden: false, isDefault: true },
        { name: 'Week', type: 'string', hidden: false, isDefault: true },
        { name: 'MarsStartDate', type: 'date', hidden: false, isDefault: false },
        { name: 'MarsEndDate', type: 'date', hidden: false, isDefault: false },
        { name: 'Baseline', type: 'float', hidden: false, isDefault: true },
        { name: 'Uplift', type: 'float', hidden: false, isDefault: true },
        { name: 'Incremental', type: 'float', hidden: false, isDefault: true },
        { name: 'Activity', type: 'float', hidden: false, isDefault: true },
        { name: 'BaseClientObjectId', type: 'int', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPromoDemands',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
