Ext.define('App.model.tpm.nonpromoequipment.HistoricalNonPromoEquipment', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
	breezeEntityType: 'HistoricalNonPromoEquipment',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
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
        resourceName: 'HistoricalNonPromoEquipments',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
