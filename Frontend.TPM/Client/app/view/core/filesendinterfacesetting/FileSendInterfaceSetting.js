Ext.define('App.view.core.filesendinterfacesetting.FileSendInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.filesendinterfacesetting',
    title: l10n.ns('core', 'compositePanelTitles').value('FileSendInterfaceSettingTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.filesendinterfacesetting.FileSendInterfaceSetting',
            storeId: 'filesendinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.filesendinterfacesetting.FileSendInterfaceSetting',
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
				text: l10n.ns('core', 'FileSendInterfaceSetting').value('InterfaceName'),
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
				text: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetPath'),
				dataIndex: 'TargetPath'
			}, { 
				text: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetFileMask'),
				dataIndex: 'TargetFileMask'
			}, { 
				text: l10n.ns('core', 'FileSendInterfaceSetting').value('SendHandler'),
				dataIndex: 'SendHandler'
			}]
        }
    }, {
        xtype: 'detailform',
        itemId: 'detailform',
        items: [{
			name: 'InterfaceName',
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('InterfaceName')
		}, {
			name: 'TargetPath',
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetPath')
		}, {
			name: 'TargetFileMask',
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('TargetFileMask')
		}, {
			name: 'SendHandler',
			fieldLabel: l10n.ns('core', 'FileSendInterfaceSetting').value('SendHandler')
		}]
    }]

});