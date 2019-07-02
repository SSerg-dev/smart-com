Ext.define('App.model.tpm.incrementalpromo.HistoricalIncrementalPromo', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'IncrementalPromo',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true, isDefault: false },
        { name: 'PromoNumber', type: 'int', hidden: false, isDefault: true },
        { name: 'PromoName', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoBrandTechName', type: 'string' },
        { name: 'PromoStartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'PromoEndDate', type: 'date', hidden: false, isDefault: true },
        { name: 'PromoDispatchesStart', type: 'date', hidden: false, isDefault: true },
        { name: 'PromoDispatchesEnd', type: 'date', hidden: false, isDefault: true },
        { name: 'ProductId', hidden: true, isDefault: false },
        { name: 'ProductZREP', type: 'string', hidden: false, isDefault: true },
        { name: 'IncrementalCaseAmount', type: 'double', hidden: false, isDefault: true },
        { name: 'IncrementalLSV', type: 'double', hidden: false, isDefault: true },
        { name: 'IncrementalPrice', type: 'double', hidden: false, isDefault: true },
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
