Ext.define('App.view.core.csvprocessinterfacesetting.HistoricalCSVProcessInterfaceSetting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalcsvprocessinterfacesetting',
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
            model: 'App.model.core.csvprocessinterfacesetting.HistoricalCSVProcessInterfaceSetting',
            storeId: 'historicalcsvprocessinterfacesettingstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.csvprocessinterfacesetting.HistoricalCSVProcessInterfaceSetting',
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
				text: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_User'),
				dataIndex: '_User',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_Role'),
				dataIndex: '_Role',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_EditDate'),
				dataIndex: '_EditDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, { 
				text: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_Operation'),
				dataIndex: '_Operation',
				renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalCSVProcessInterfaceSetting', 'OperationType'),
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
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_User')
		}, {
			name: '_Role',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_Role')
		}, {
			name: '_EditDate',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_EditDate'),
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
		}, {
			name: '_Operation',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('_Operation'),
			renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalCSVProcessInterfaceSetting', 'OperationType')
		}, {
			name: 'InterfaceName',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('InterfaceName')
		}, {
			name: 'Delimiter',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('Delimiter')
		}, {
			name: 'UseQuoting',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('UseQuoting')
		}, {
			name: 'QuoteChar',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('QuoteChar')
		}, {
			name: 'ProcessHandler',
			fieldLabel: l10n.ns('core', 'HistoricalCSVProcessInterfaceSetting').value('ProcessHandler')
		}]
    }]

});