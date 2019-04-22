Ext.define('App.model.core.user.DeletedUser', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'User',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'DeletedDate', type: 'date', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('DeletedUser', 'DeletedDate', 'date') },
		{ name: 'Sid', type: 'string', isDefault: true },
		{ name: 'Name', type: 'string', isDefault: true },
        { name: 'Email', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedUsers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});