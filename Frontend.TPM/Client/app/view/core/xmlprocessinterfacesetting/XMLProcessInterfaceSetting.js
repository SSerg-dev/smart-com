Ext.define('App.view.core.xmlprocessinterfacesetting.XMLProcessInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.xmlprocessinterfacesetting',
    title: l10n.ns('core', 'compositePanelTitles').value('XMLProcessInterfaceSettingTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.xmlprocessinterfacesetting.XMLProcessInterfaceSetting',
            storeId: 'xmlprocessinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.xmlprocessinterfacesetting.XMLProcessInterfaceSetting',
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
                flex: 1
            },
            items: [{ 
				text: l10n.ns('core', 'XMLProcessInterfaceSetting').value('InterfaceName'),
				dataIndex: 'InterfaceName',
				filter: {
					type: 'search',
					selectorWidget: 'interface',
					valueField: 'Name',
					store: {
						type: 'directorystore',
						model: 'App.model.core.interface.Interface',
						extendedFilter: {
							xclass: 'App.ExtFilterContext',
							supportedModels: [{
								xclass: 'App.ExtSelectionFilterModel',
								model: 'App.model.core.interface.Interface',
								modelId: 'efselectionmodel'
							}, {
								xclass: 'App.ExtTextFilterModel',
								modelId: 'eftextmodel'
							}]
						}
					}
				}
			}, { 
				text: l10n.ns('core', 'XMLProcessInterfaceSetting').value('RootElement'),
				dataIndex: 'RootElement'
			}, { 
				text: l10n.ns('core', 'XMLProcessInterfaceSetting').value('ProcessHandler'),
				dataIndex: 'ProcessHandler'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('InterfaceName'),
            name: 'InterfaceName',
            type: 'search',
            selectorWidget: 'interface',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.core.interface.Interface',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.core.interface.Interface',
                        modelId: 'efselectionmodel'
                    }, {
                        xclass: 'App.ExtTextFilterModel',
                        modelId: 'eftextmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'InterfaceName'
            }]
        }, {
            xtype: 'textfield',
			name: 'RootElement',
			fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('RootElement')
        }, {
            xtype: 'textfield',
			name: 'ProcessHandler',
			fieldLabel: l10n.ns('core', 'XMLProcessInterfaceSetting').value('ProcessHandler')
		}]
    }]

});