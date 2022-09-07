Ext.define('App.model.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AccessPoint',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'Resource', type: 'string', isDefault: true },
		{ name: 'Action', type: 'string', isDefault: true },
		{ name: 'Description', type: 'string', isDefault: true },
        { name: 'TPMmode', type: 'boolean', hidden: false, isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'AccessPoints',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});