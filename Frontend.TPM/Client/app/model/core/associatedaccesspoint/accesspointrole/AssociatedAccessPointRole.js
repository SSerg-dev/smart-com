Ext.define('App.model.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'AccessPointRole',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'RoleId', hidden: true },
		{ name: 'AccessPointId', hidden: true },
		{ name: 'SystemName', type: 'string', isDefault: true, mapping: 'Role.SystemName' },
		{ name: 'DisplayName', type: 'string', isDefault: true, mapping: 'Role.DisplayName' },
		{ name: 'IsAllow', type: 'boolean', isDefault: true, mapping: 'Role.IsAllow' }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'AccessPointRoles',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});