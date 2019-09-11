Ext.define('App.view.core.constraint.HistoricalConstraint', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalconstraint',
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
            model: 'App.model.core.constraint.HistoricalConstraint',
            storeId: 'historicalconstraintstore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.constraint.HistoricalConstraint',
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
                flex: 1,
                minWidth: 100
            },
            items: [{ 
				text: l10n.ns('core', 'HistoricalConstraint').value('_User'),
				dataIndex: '_User',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalConstraint').value('_Role'),
				dataIndex: '_Role',
				filter: {
					type: 'string',
					operator: 'eq'
				}
			}, { 
				text: l10n.ns('core', 'HistoricalConstraint').value('_EditDate'),
				dataIndex: '_EditDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			}, { 
				text: l10n.ns('core', 'HistoricalConstraint').value('_Operation'),
				dataIndex: '_Operation',
				renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalConstraint', 'OperationType'),
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
			fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_User')
		}, {
			name: '_Role',
			fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_Role')
		}, {
			name: '_EditDate',
			fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_EditDate'),
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
		}, {
			name: '_Operation',
			fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('_Operation'),
			renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalConstraint', 'OperationType')
		}, {
			name: 'Prefix',
			fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('Prefix')
		}, {
			name: 'Value',
			fieldLabel: l10n.ns('core', 'HistoricalConstraint').value('Value')
		}]
    }]

});