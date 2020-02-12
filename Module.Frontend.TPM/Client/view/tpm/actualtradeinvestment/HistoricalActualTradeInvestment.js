Ext.define('App.view.tpm.actualtradeinvestment.HistoricalActualTradeInvestment', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalactualtradeinvestment',
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
            model: 'App.model.tpm.actualtradeinvestment.HistoricalActualTradeInvestment',
            storeId: 'historicalactualtradeinvestmentstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.actualtradeinvestment.HistoricalActualTradeInvestment',
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
                text: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalActualTradeInvestment', 'OperationType'),
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
        model: 'App.model.tpm.actualtradeinvestment.HistoricalActualTradeInvestment',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalActualTradeInvestment', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('StartDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('EndDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TIType',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('TIType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TISubType',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('TISubType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SizePercent',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('SizePercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarcCalcROI',
            renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcROI')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarcCalcBudgets',
            renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcBudgets')
        }]
    }]
});
             
