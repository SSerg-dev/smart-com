Ext.define('App.model.core.user.AdUser', {
    extend: 'Ext.data.Model',
    idProperty: 'Sid',
    breezeEntityType: 'AdUser',
    fields: [
		{ name: 'Sid', type: 'string', isDefault: true },
		{ name: 'Name', type: 'string', isDefault: true },
        { name: 'Email', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'AdUsers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});