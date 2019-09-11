Ext.define('App.model.core.interface.Interface', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Interface',
    fields: [
		{ name: 'Id', hidden: true},
		{ name: 'Name', type: 'string', isDefault: true},
		{ name: 'Direction', type: 'string', isDefault: true },
		{ name: 'Description', type: 'string', isDefault: true}
	],
    proxy: {
        type: 'breeze',
        resourceName: 'Interfaces',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});