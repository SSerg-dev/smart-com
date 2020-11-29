Ext.define('App.view.tpm.budgetsubitem.HistoricalBudgetSubItem', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.historicalbudgetsubitem',
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
            model: 'App.model.tpm.budgetsubitem.HistoricalBudgetSubItem',
            storeId: 'historicalbudgetsubitemstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.budgetsubitem.HistoricalBudgetSubItem',
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
                    text: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_User'),
                    dataIndex: '_User',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Role'),
                    dataIndex: '_Role',
                    filter: {
                        type: 'string',
                        operator: 'eq'
                    }
                }, {
                    text: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_EditDate'),
                    dataIndex: '_EditDate',
                    xtype: 'datecolumn',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                }, {
                    text: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Operation'),
                    dataIndex: '_Operation',
                    renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBudgetSubItem', 'OperationType'),
                    filter: {
                        type: 'combo',
                        valueField: 'id',
                        store: {
                            type: 'operationtypestore'
                        },
                        operator: 'eq'
                    }
                }
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.budgetsubitem.HistoricalBudgetSubItem',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBudgetSubItem', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetItemBudgetName',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetItemName',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Description_ru'),
        }]
    }]
});
