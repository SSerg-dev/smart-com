Ext.define('App.view.core.setting.HistoricalSetting', {
	extend: 'App.view.core.common.CombinedDirectoryPanel',
	alias: 'widget.historicalsetting',
	title: l10n.ns('core', 'compositePanelTitles').value('historyPanelTitle'),

	dockedItems: [{
		xtype: 'readonlydirectorytoolbar',
		dock: 'right'
	}],

	items: [{
		xtype: 'directorygrid',
		itemId: 'datatable',
		editorModel: 'Core.form.EditorDetailWindowModel',

		store: {
			type: 'directorystore',
			model: 'App.model.core.setting.HistoricalSetting',
			storeId: 'historicalsettingstore',
			extendedFilter: {
				xclass: 'App.ExtFilterContext',
				supportedModels: [{
					xclass: 'App.ExtSelectionFilterModel',
					model: 'App.model.core.setting.HistoricalSetting',
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
				text: l10n.ns('core', 'HistoricalSetting').value('_User'),
				dataIndex: '_User',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, {
				text: l10n.ns('core', 'HistoricalSetting').value('_Role'),
				dataIndex: '_Role',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, {
				text: l10n.ns('core', 'HistoricalSetting').value('_EditDate'),
				dataIndex: '_EditDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, {
				text: l10n.ns('core', 'HistoricalSetting').value('_Operation'),
				dataIndex: '_Operation',
				renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalSetting', 'OperationType'),
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
		model: 'App.model.core.setting.HistoricalSetting',
		items: [{
			xtype: 'singlelinedisplayfield',
			name: '_User',
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_User')
		}, {
			xtype: 'singlelinedisplayfield',
			name: '_Role',
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_Role')
		}, {
			xtype: 'singlelinedisplayfield',
			name: '_EditDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_EditDate')
		}, {
			xtype: 'singlelinedisplayfield',
			name: '_Operation',
			renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalSetting', 'OperationType'),
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('_Operation')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Name',
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Name')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Type',
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Type')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Value',
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Value')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Description',
			fieldLabel: l10n.ns('core', 'HistoricalSetting').value('Description')
		}]
	}]

});