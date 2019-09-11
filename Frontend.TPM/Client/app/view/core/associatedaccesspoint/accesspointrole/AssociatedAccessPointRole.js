Ext.define('App.view.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.associatedaccesspointaccesspointrole',
    title: l10n.ns('core', 'compositePanelTitles').value('AssociatedAccessPointRoleTitle'),

    dockedItems: [{
        xtype: 'addonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'associateddirectorystore',
            model: 'App.model.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole',
            storeId: 'associatedaccesspointaccesspointrolestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole',
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
				text: l10n.ns('core', 'AssociatedAccessPointRole').value('SystemName'),
				dataIndex: 'SystemName',
				filter: {
				    type: 'search',
				    selectorWidget: 'role',
				    valueField: 'SystemName',
				    store: {
				        type: 'directorystore',
				        model: 'App.model.core.role.Role',
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
				    }
				}
			}, { 
				text: l10n.ns('core', 'AssociatedAccessPointRole').value('DisplayName'),
				dataIndex: 'DisplayName',
				dataIndex: 'DisplayName',
				filter: {
				    type: 'search',
				    selectorWidget: 'role',
				    valueField: 'DisplayName',
				    store: {
				        type: 'directorystore',
				        model: 'App.model.core.role.Role',
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
				    }
				}
			}, { 
				text: l10n.ns('core', 'AssociatedAccessPointRole').value('IsAllow'),
				dataIndex: 'IsAllow',
				xtype: 'booleancolumn',
				trueText: l10n.ns('core', 'booleanValues').value('true'),
				falseText: l10n.ns('core', 'booleanValues').value('false')
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.associatedaccesspoint.accesspointrole.AssociatedAccessPointRole',
        items: [{ 
			xtype: 'singlelinedisplayfield',
			name: 'SystemName',
			fieldLabel: l10n.ns('core', 'AssociatedAccessPointRole').value('SystemName')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'DisplayName',
			fieldLabel: l10n.ns('core', 'AssociatedAccessPointRole').value('DisplayName')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'IsAllow',
			renderer: App.RenderHelper.getBooleanRenderer(),
			fieldLabel: l10n.ns('core', 'AssociatedAccessPointRole').value('IsAllow')		
		}]
    }]

});