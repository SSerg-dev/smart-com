Ext.define('App.view.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint', {
	extend: 'App.view.core.common.CombinedDirectoryPanel',
	alias: 'widget.historicalassociatedaccesspointaccesspoint',
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
			model: 'App.model.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint',
			storeId: 'historicalassociatedaccesspointaccesspointstore',
			extendedFilter: {
				xclass: 'App.ExtFilterContext',
				supportedModels: [{
					xclass: 'App.ExtSelectionFilterModel',
					model: 'App.model.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint',
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
				text: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_User'),
				dataIndex: '_User',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, {
				text: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_Role'),
				dataIndex: '_Role',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, {
				text: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_EditDate'),
				dataIndex: '_EditDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, {
				text: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_Operation'),
				dataIndex: '_Operation',
				renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalAccessPoint', 'OperationType'),
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
		model: 'App.model.core.associatedaccesspoint.accesspoint.HistoricalAssociatedAccessPoint',
		items: [{
			xtype: 'singlelinedisplayfield',
			name: '_User',
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_User')
		}, {
			xtype: 'singlelinedisplayfield',
			name: '_Role',
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_Role')
		}, {
			xtype: 'singlelinedisplayfield',
			name: '_EditDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_EditDate')
		}, {
			xtype: 'singlelinedisplayfield',
			name: '_Operation',
			renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalAccessPoint', 'OperationType'),
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('_Operation')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Resource',
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('Resource')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Action',
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('Action')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Description',
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedAccessPoint').value('Description')
		}]
	}]

});