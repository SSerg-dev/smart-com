Ext.define('App.model.tpm.assortmentmatrix.HistoricalAssortmentMatrix', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'AssortmentMatrix',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ClientTreeName', type: 'string', hidden: false, isDefault: true },
        { name: 'ProductId', hidden: true, isDefault: true },
        { name: 'EAN_PC', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalAssortmentMatrices',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
