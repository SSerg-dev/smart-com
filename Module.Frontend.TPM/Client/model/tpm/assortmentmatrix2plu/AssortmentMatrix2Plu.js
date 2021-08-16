Ext.define('App.model.tpm.assortmentmatrix2plu.AssortmentMatrix2Plu', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AssortmentMatrix2Plu',
    fields: [
        { name: 'Id', hidden: true, isKey: true },
        { name: 'PluCode', type: 'string', hidden: false, isDefault: true }],
    proxy: {
        type: 'breeze',
        resourceName: 'AssortmentMatrix2Plus',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            needActualAssortmentMatrix: false
        }
    }
});