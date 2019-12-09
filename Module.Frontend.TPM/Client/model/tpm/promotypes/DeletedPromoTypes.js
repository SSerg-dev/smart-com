Ext.define('App.model.tpm.promotypes.DeletedPromoTypes', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoTypes',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },


        { name: 'Name', type: 'string', hidden: false, isDefault: true },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedPromoTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
