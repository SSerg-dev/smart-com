Ext.define('App.model.core.associateduser.userrole.AssociatedUserRole', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'UserRole',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'UserId', hidden: true },
		{ name: 'RoleId', hidden: true },
		{ name: 'IsDefault', type: 'boolean', isDefault: true, defaultValue: false },
		{ name: 'SystemName', type: 'string', isDefault: true, mapping: 'Role.SystemName' },
		{ name: 'DisplayName', type: 'string', isDefault: true, mapping: 'Role.DisplayName' },
		{ name: 'IsAllow', type: 'boolean', isDefault: false, mapping: 'Role.IsAllow' }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'UserRoles',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});