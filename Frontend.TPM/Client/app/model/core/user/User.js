Ext.define('App.model.core.user.User', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'User',
    fields: [
		{ name: 'Id', hidden: true, },       
		{ name: 'Name', type: 'string', isDefault: true, filter: { model: 'User', field: 'Name', type: 'string' } },
        { name: 'Email', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'UserDTOs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});