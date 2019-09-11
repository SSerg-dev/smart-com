Ext.define('App.view.core.role.Role', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.role',
    title: l10n.ns('core', 'compositePanelTitles').value('RoleTitle'),

    dockedItems: [{
        xtype: 'standarddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.role.Role',
            storeId: 'rolestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.role.Role',
                    modelId: 'efselectionmodel'
				}, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
				}]
            }        
		},

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
				minWidth: 100
            },
            items: [{ 
				text: l10n.ns('core', 'Role').value('SystemName'),
				dataIndex: 'SystemName'
			}, { 
				text: l10n.ns('core', 'Role').value('DisplayName'),
				dataIndex: 'DisplayName'
			}, { 
				xtype: 'booleancolumn',
				text: l10n.ns('core', 'Role').value('IsAllow'),
				dataIndex: 'IsAllow',
				trueText: l10n.ns('core', 'booleanValues').value('true'),
				falseText: l10n.ns('core', 'booleanValues').value('false')
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.role.Role',
        items: [{ 
			xtype: 'textfield',
			name: 'SystemName',
			fieldLabel: l10n.ns('core', 'Role').value('SystemName')		
		}, { 
			xtype: 'textfield',
			name: 'DisplayName',
			fieldLabel: l10n.ns('core', 'Role').value('DisplayName')		
		}, { 
		    xtype: 'booleancombobox',
		    store: {
		        type: 'booleannonemptystore'
		    },
			name: 'IsAllow',
			fieldLabel: l10n.ns('core', 'Role').value('IsAllow')		
		}]
    }]

});