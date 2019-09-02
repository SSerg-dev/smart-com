Ext.define('App.model.tpm.incrementalpromo.HistoricalIncrementalPromo', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'IncrementalPromo',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true, isDefault: false },
        { name: 'ProductId', hidden: true, isDefault: false },
        { name: 'ProductZREP', type: 'string', hidden: false, isDefault: true },
        { name: 'ProductProductEN', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoClientHierarchy', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoNumber', type: 'int', hidden: false, isDefault: true },
        { name: 'PromoName', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanPromoIncrementalCases', type: 'float', hidden: false, isDefault: true },
        { name: 'CasePrice', type: 'float', hidden: false, isDefault: true },
        { name: 'PlanPromoIncrementalLSV', type: 'float', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalIncrementalPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
