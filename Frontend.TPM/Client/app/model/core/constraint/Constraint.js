Ext.define('App.model.core.constraint.Constraint', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Constraint',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'UserRoleId', hidden: true },
		{ name: 'Prefix', type: 'string', isDefault: true },
		{ name: 'Value', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'Constraints',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});