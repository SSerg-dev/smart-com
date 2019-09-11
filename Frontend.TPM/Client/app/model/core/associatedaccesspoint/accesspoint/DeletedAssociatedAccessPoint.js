Ext.define('App.model.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AccessPoint',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'DeletedDate', type: 'date', isDefault: true },
		{ name: 'Resource', type: 'string', isDefault: true },
		{ name: 'Action', type: 'string', isDefault: true },
		{ name: 'Description', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedAccessPoints',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});