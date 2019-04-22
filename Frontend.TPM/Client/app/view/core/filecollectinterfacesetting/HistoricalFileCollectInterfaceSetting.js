Ext.define('App.view.core.filecollectinterfacesetting.HistoricalFileCollectInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalfilecollectinterfacesetting',
    title: l10n.ns('core', 'compositePanelTitles').value('historyPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.filecollectinterfacesetting.HistoricalFileCollectInterfaceSetting',
            storeId: 'historicalfilecollectinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.filecollectinterfacesetting.HistoricalFileCollectInterfaceSetting',
                    modelId: 'efselectionmodel'
				}]
            },
			sorters: [{
				property: '_EditDate',
				direction: 'DESC'
			}],        
		},

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1
            },
            items: [{ 
				text: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_User'),
				dataIndex: '_User',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_Role'),
				dataIndex: '_Role',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_EditDate'),
				dataIndex: '_EditDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, { 
				text: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_Operation'),
				dataIndex: '_Operation',
				renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileCollectInterfaceSetting', 'OperationType'),
				filter: {
					type: 'combo',
					valueField: 'id',
				    store: {
				        type: 'operationtypestore'
				    },
					operator: 'eq'
				}
			}]
        }
    }, {
        xtype: 'detailform',
        itemId: 'detailform',
        items: [{
			name: '_User',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_User')
		}, {
			name: '_Role',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_Role')
		}, {
			name: '_EditDate',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_EditDate'),
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
		}, {
			name: '_Operation',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('_Operation'),
			renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileCollectInterfaceSetting', 'OperationType')
		}, {
			name: 'InterfaceName',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('InterfaceName')
		}, {
			name: 'SourcePath',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('SourcePath')
		}, {
			name: 'SourceFileMask',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('SourceFileMask')
		}, {
			name: 'CollectHandler',
			fieldLabel: l10n.ns('core', 'HistoricalFileCollectInterfaceSetting').value('CollectHandler')
		}]
    }]

});