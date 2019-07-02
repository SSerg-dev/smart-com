Ext.define('App.view.tpm.incrementalpromo.HistoricalIncrementalPromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalincrementalpromo',
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
            model: 'App.model.tpm.incrementalpromo.HistoricalIncrementalPromo',
            storeId: 'historicalincrementalpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.incrementalpromo.HistoricalIncrementalPromo',
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
                    text: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalIncrementalPromo').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalIncrementalPromo', 'OperationType'),
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
        model: 'App.model.tpm.incrementalpromo.HistoricalIncrementalPromo',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'PromoNumber',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoNumber'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('BrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TechnologyName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('TechnologyName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('EndDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesStartDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('DispatchesStartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesEndDate',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('DispatchesEndDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalCaseAmount',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalCaseAmount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IncrementalPrice',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('IncrementalPrice'),
        }]
    }]
});
