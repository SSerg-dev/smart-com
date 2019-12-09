Ext.define('App.model.tpm.mechanic.Mechanic', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Mechanic',
    fields: [
        { name: 'Id', hidden: true },

        { name: 'PromoTypesId', hidden: true, isDefault: true }, 
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'SystemName',  type: 'string', hidden: false, isDefault: true },
        {
            name: 'PromoTypes.Name', type: 'string', mapping: 'PromoTypes.Name', defaultFilterConfig: { valueField: 'Name' },
              breezeEntityType: 'PromoTypes', hidden: false, isDefault: true
        },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Mechanics',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
