Ext.define('App.model.core.associateduser.userrole.HistoricalAssociatedUserRole', {
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
        { name: 'RoleDisplayName', type: 'string', isDefault: true },
        { name: 'RoleIsAllow', type: 'boolean', isDefault: false }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalUserRoles',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            Id: null
        }
    }
});