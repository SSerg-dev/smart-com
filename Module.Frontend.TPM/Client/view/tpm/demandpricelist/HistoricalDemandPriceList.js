Ext.define('App.view.tpm.demandpricelist.HistoricalDemandPriceList', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicaldemandpricelist',
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
            model: 'App.model.tpm.demandpricelist.HistoricalDemandPriceList',
            storeId: 'historicaldemandpriceliststore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.demandpricelist.HistoricalDemandPriceList',
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
                    text: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalDemandPriceList', 'OperationType'),
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
        model: 'App.model.tpm.demandpricelist.HistoricalDemandPriceList',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('ClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('Price'),
        }]
    }]

});
