Ext.define('App.view.tpm.baseline.HistoricalBaseLine', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalbaseline',
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
            model: 'App.model.tpm.baseline.HistoricalBaseLine',
            storeId: 'historicalbaselinestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.baseline.HistoricalBaseLine',
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
            items: [
                {
                    text: l10n.ns('tpm', 'HistoricalBaseLine').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalBaseLine').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalBaseLine').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalBaseLine').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBaseLine', 'OperationType'),
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
        model: 'App.model.tpm.baseline.HistoricalBaseLine',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'QTY',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('QTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Price'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BaselineLSV',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('BaselineLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Type'),
        }]
    }]

});
