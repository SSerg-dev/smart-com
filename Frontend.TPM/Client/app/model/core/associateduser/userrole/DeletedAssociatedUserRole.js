Ext.define('App.model.core.associateduser.userrole.DeletedAssociatedUserRole', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalUserRole',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{
		    name: '_User', type: 'string', isDefault: true,
		    extendedFilterEntry: {
		        defaultOperations: {
		            string: 'Equals'
		        },
		        allowedOperations: {
		            string: ['Equals', 'NotEqual', 'In', 'IsNull', 'NotNull']
		        }
		    }
		},
		{
		    name: '_Role', type: 'string', isDefault: true,
		    extendedFilterEntry: {
		        defaultOperations: {
		            string: 'Equals'
		        },
		        allowedOperations: {
		            string: ['Equals', 'NotEqual', 'In', 'IsNull', 'NotNull']
		        }
		    }
		},
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
		{
		    name: '_Operation', type: 'string', isDefault: true,
		    extendedFilterEntry: {
		        defaultOperations: {
		            string: 'Equals'
		        },
		        allowedOperations: {
		            string: ['Equals', 'NotEqual', 'In', 'IsNull', 'NotNull']
		        },
		        editors: {
		            string: {
		                xtype: 'combobox',
		                valueField: 'id',
		                store: {
		                    type: 'operationtypestore'
		                }
		            }
		        }
		    }
		},
        { name: 'Id', hidden: true },
        { name: 'UserId', hidden: true },
        { name: 'RoleId', hidden: true },
        { name: 'IsDefault', type: 'boolean', isDefault: true },
        { name: 'RoleSystemName', type: 'string', isDefault: true },
        { name: 'RoleDisplayName', type: 'string', isDefault: true },
        { name: 'RoleIsAllow', type: 'boolean', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedUserRoles',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            userId: null
        }
    }
});