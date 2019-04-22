Ext.define('App.view.core.filebuffer.HistoricalFileBuffer', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalfilebuffer',
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
            model: 'App.model.core.filebuffer.HistoricalFileBuffer',
            storeId: 'historicalfilebufferstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.filebuffer.HistoricalFileBuffer',
                    modelId: 'efselectionmodel'
				}]
            },
			sorters: [{
				property: '_EditDate',
				direction: 'DESC'
			}]        
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
				text: l10n.ns('core', 'HistoricalFileBuffer').value('_User'),
				dataIndex: '_User',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalFileBuffer').value('_Role'),
				dataIndex: '_Role',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalFileBuffer').value('_EditDate'),
				dataIndex: '_EditDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, { 
				text: l10n.ns('core', 'HistoricalFileBuffer').value('_Operation'),
				dataIndex: '_Operation',
				renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileBuffer', 'OperationType'),
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
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.filebuffer.HistoricalFileBuffer',
        items: [{ 
			xtype: 'singlelinedisplayfield',
			name: '_User',
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_User')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: '_Role',
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_Role')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: '_EditDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_EditDate')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: '_Operation',
			renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalFileBuffer', 'OperationType'),
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('_Operation')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'InterfaceName',
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('InterfaceName')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'InterfaceDirection',
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('InterfaceDirection')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'CreateDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('CreateDate')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'HandlerId',
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('HandlerId')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'FileName',
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('FileName')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'Status',
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('Status')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'ProcessDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'HistoricalFileBuffer').value('ProcessDate')		
		}]
    }]

});