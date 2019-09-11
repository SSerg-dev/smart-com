Ext.define('App.view.core.csvextractinterfacesetting.CSVExtractInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.csvextractinterfacesetting',
    title: l10n.ns('core', 'compositePanelTitles').value('CSVExtractInterfaceSettingTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.csvextractinterfacesetting.CSVExtractInterfaceSetting',
            storeId: 'csvextractinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.csvextractinterfacesetting.CSVExtractInterfaceSetting',
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
				text: l10n.ns('core', 'CSVExtractInterfaceSetting').value('InterfaceName'),
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
				text: l10n.ns('core', 'CSVExtractInterfaceSetting').value('FileNameMask'),
				dataIndex: 'FileNameMask'
			}, { 
				text: l10n.ns('core', 'CSVExtractInterfaceSetting').value('ExtractHandler'),
				dataIndex: 'ExtractHandler'
			}]
        }
    }, {
        xtype: 'detailform',
        itemId: 'detailform',
        items: [{
			name: 'InterfaceName',
			fieldLabel: l10n.ns('core', 'CSVExtractInterfaceSetting').value('InterfaceName')
		}, {
			name: 'FileNameMask',
			fieldLabel: l10n.ns('core', 'CSVExtractInterfaceSetting').value('FileNameMask')
		}, {
			name: 'ExtractHandler',
			fieldLabel: l10n.ns('core', 'CSVExtractInterfaceSetting').value('ExtractHandler')
		}]
    }]

});