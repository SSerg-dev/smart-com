Ext.define('App.view.core.associateduser.aduser.HistoricalAssociatedAdUser', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalassociatedaduseruser',
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
            model: 'App.model.core.associateduser.user.HistoricalAssociatedUser',
            storeId: 'historicalassociateduseruserstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associateduser.user.HistoricalAssociatedUser',
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
                text: l10n.ns('core', 'HistoricalAssociatedUser').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalAssociatedUser').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'HistoricalAssociatedUser').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'HistoricalAssociatedUser').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalUser', 'OperationType'),
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
		model: 'App.model.core.associateduser.user.HistoricalAssociatedUser',
		items: [{
		    xtype: 'singlelinedisplayfield',
		    name: '_User',
		    fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_User')
		}, {
		    xtype: 'singlelinedisplayfield',
		    name: '_Role',
		    fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_Role')
		}, {
		    xtype: 'singlelinedisplayfield',
		    name: '_EditDate',
		    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
		    fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_EditDate')
		}, {
		    xtype: 'singlelinedisplayfield',
		    name: '_Operation',
		    fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_Operation'),
		    renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalAssociatedUser', 'OperationType')
		}, {
		    xtype: 'singlelinedisplayfield',
		    name: 'Sid',
		    fieldLabel: l10n.ns('core', 'AssociatedUser').value('Sid')
		}, {
			xtype: 'singlelinedisplayfield',
			name: 'Name',
			fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('Name')		
		}]
    }]

});