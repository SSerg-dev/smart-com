Ext.define('App.model.tpm.promoproduct2plu.PromoProduct2Plu', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AssortmentMatrix2Plu',
    fields: [
        { name: 'Id', hidden: true, isKey: true },
        { name: 'PluCode', type: 'string', hidden: false, isDefault: true }],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoProduct2Plus',
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