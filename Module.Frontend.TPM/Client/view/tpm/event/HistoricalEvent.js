Ext.define('App.view.tpm.event.HistoricalEvent', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalevent',
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
            model: 'App.model.tpm.event.HistoricalEvent',
            storeId: 'historicaleventstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.event.HistoricalEvent',
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
                text: l10n.ns('tpm', 'HistoricalEvent').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalEvent').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalEvent').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalEvent').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalEvent', 'OperationType'),
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
        model: 'App.model.tpm.event.HistoricalEvent',
        items: [
            {
                xtype: 'singlelinedisplayfield',
                name: '_User',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_User')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: '_Role',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_Role')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: '_EditDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_EditDate')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalEvent', 'OperationType'),
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('_Operation')
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'Name',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('Name'),
            },
            {
                xtype: 'singlelinedisplayfield',
                name: 'Description',
                fieldLabel: l10n.ns('tpm', 'HistoricalEvent').value('Description'),
            },
            {
                xtype: 'textfield',
                name: 'Type',
                fieldLabel: l10n.ns('tpm', 'Event').value('EventTypeName')
            },
            {
                xtype: 'textfield',
                name: 'MarketSegment',
                fieldLabel: l10n.ns('tpm', 'Event').value('MarketSegment')
            },
        ]
    }]
});
