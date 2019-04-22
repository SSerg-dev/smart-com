Ext.define('App.model.core.role.DeletedRole', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Role',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'DeletedDate', type: 'date', isDefault: true },
		{ name: 'SystemName', type: 'string', isDefault: true },
		{ name: 'DisplayName', type: 'string', isDefault: true },
		{ name: 'IsAllow', type: 'boolean', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedRoles',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});