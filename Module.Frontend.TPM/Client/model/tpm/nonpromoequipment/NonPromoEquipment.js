Ext.define('App.model.tpm.nonpromoequipment.NonPromoEquipment', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'NonPromoEquipment',
    fields: [
        { name: 'Id', hidden: true },
		{ name: 'EquipmentType', type: 'string', hidden: false, isDefault: true },
        { name: 'Description_ru', type: 'string', hidden: false, isDefault: true },
        {
            name: 'BudgetItemName', type: 'string', mapping: 'BudgetItem.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'BudgetItem', hidden: false, isDefault: true
        },
        { name: 'BudgetItemId', hidden: true, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'NonPromoEquipments',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
