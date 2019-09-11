Ext.define('App.model.core.user.HistoricalUser', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalUser',
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
		{ name: 'Sid', type: 'string', isDefault: true },
		{ name: 'Name', type: 'string', isDefault: true },
        { name: 'Email', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalUsers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});