Ext.define('App.model.tpm.mechanic.DeletedMechanic', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Mechanic',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'SystemName', useNull: true, type: 'string', hidden: false, isDefault: true },
          { name: 'PromoTypesId', hidden: true, isDefault: true },

        {
            name: 'PromoTypeName', type: 'string', mapping: 'PromoTypes.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'PromoTypes', hidden: false, isDefault: true
        },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedMechanics',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
