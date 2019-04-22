Ext.define('App.model.tpm.product.PromoProductDetailsWindow', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductDetailsWindow',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ZREP', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'EAN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductRU', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductEN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSV', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ActualProductBaselineLSV', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSV', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductEN', useNull: true, type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Promoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});