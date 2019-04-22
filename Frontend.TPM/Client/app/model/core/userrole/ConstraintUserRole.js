Ext.define('App.model.core.userrole.ConstraintUserRole', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'UserRole',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'UserId', hidden: true },
		{ name: 'RoleId', hidden: true },
		{ name: 'IsDefault', type: 'boolean', isDefault: true },
		{ name: 'Login', type: 'string', isDefault: true, mapping: 'User.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'User' },
		{ name: 'RoleSystemName', type: 'string', isDefault: true, mapping: 'Role.SystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'Role' },
		{ name: 'RoleDisplayName', type: 'string', isDefault: true, mapping: 'Role.DisplayName', defaultFilterConfig: { valueField: 'DisplayName' }, breezeEntityType: 'Role' },
		{ name: 'RoleIsAllow', type: 'boolean', isDefault: true, mapping: 'Role.IsAllow', defaultFilterConfig: { valueField: 'IsAllow' }, breezeEntityType: 'Role' }
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