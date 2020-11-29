Ext.define('App.view.tpm.budgetitem.DeletedBudgetItem', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedbudgetitem',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.budgetitem.DeletedBudgetItem',
            storeId: 'deletedbudgetitemstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.budgetitem.DeletedBudgetItem',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
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
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                dataIndex: 'DeletedDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'BudgetItem').value('BudgetName'),
                dataIndex: 'BudgetName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budget',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budget.Budget',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budget.Budget',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'BudgetItem').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'BudgetItem').value('Description_ru'),
                dataIndex: 'Description_ru'
            }, {
                text: l10n.ns('tpm', 'BudgetItem').value('ButtonColor'),
                dataIndex: 'ButtonColor',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div>', record.get('ButtonColor'));
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.budgetitem.DeletedBudgetItem',
        items: [
            {
                xtype: 'singlelinedisplayfield',
                name: 'DeletedDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'BudgetName',
                fieldLabel: l10n.ns('tpm', 'BudgetItem').value('BudgetName'),
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'Name',
                fieldLabel: l10n.ns('tpm', 'BudgetItem').value('Name'),
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'ButtonColor',
                fieldLabel: l10n.ns('tpm', 'BudgetItem').value('ButtonColor'),
            }]
    }]
});
