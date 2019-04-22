Ext.define('App.model.core.loophandler.SingleLoopHandler', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'LoopHandler',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'Description', type: 'string', isDefault: true },
		{ name: 'Name', type: 'string', isDefault: true },
		{ name: 'ExecutionPeriod', type: 'int', isDefault: true },
		{ name: 'ExecutionMode', type: 'string', isDefault: true },
		{ name: 'CreateDate', type: 'date', isDefault: true },
		{ name: 'LastExecutionDate', type: 'date', isDefault: true },
		{ name: 'NextExecutionDate', type: 'date', isDefault: true },
		{ name: 'ConfigurationName', type: 'string', isDefault: true },
		{ name: 'Status', type: 'string', isDefault: true },
		{ name: 'UserId', hidden: true },
		{ name: 'UserName', type: 'string', isDefault: true, mapping: 'User.Name' },
        { name: 'RoleId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('LoopHandler', 'RoleId') },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'SingleLoopHandlers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});