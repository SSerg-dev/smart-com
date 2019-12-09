Ext.define('App.model.tpm.promotypes.PromoTypes', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoTypes',
    fields: [
        { name: 'Id', hidden: true }, 
        { name: 'Name', type: 'string', hidden: false, isDefault: true  },
        { name: 'SystemName', type: 'string', hidden: true, isDefault: true  },
        { name: 'Glyph', type: 'string', hidden: true, isDefault: true  },
     
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoTypes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
