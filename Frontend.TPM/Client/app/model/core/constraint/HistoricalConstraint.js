Ext.define('App.model.core.constraint.HistoricalConstraint', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalConstraint',
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
		{ name: 'UserRoleId', hidden: true },
		{ name: 'Prefix', type: 'string', isDefault: true },
		{ name: 'Value', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalConstraints',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});