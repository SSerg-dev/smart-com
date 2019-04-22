Ext.define('App.view.tpm.sale.Sale', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.sale',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Sale'),

    inPromoForm: false,
    promoId: null,

    initComponent: function () {
        this.items[0].store.type = (this.autoLoadStore ? 'directorystore' : 'associateddirectorystore');
        this.callParent(arguments);
    },

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            model: 'App.model.tpm.sale.Sale',
            storeId: 'salestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.sale.Sale',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            }
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
                text: l10n.ns('tpm', 'Sale').value('BudgetItemBudgetName'),
                dataIndex: 'BudgetItemBudgetName',
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
                text: l10n.ns('tpm', 'Sale').value('BudgetItemName'),
                dataIndex: 'BudgetItemName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budgetitem',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budgetitem.BudgetItem',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budgetitem.BudgetItem',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Sale').value('Plan'),
                dataIndex: 'Plan'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Sale').value('Fact'),
                dataIndex: 'Fact'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.sale.Sale',
        afterFormShow: function (scope, isCreating) {
            scope.down('searchfield[name=BudgetItemId]').focus(true, 10);
        },
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Sale').value('BudgetItemBudgetName'),
            name: 'BudgetItemBudgetName'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Sale').value('BudgetItemName'),
            name: 'BudgetItemId',
            selectorWidget: 'budgetitem',
            valueField: 'Id',
            displayField: 'Name',
            needUpdateMappings: true,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budgetitem.BudgetItem',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.budgetitem.BudgetItem',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BudgetItemName'
            }, {
                from: 'BudgetName',
                to: 'BudgetItemBudgetName'
            }]
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'Plan',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Sale').value('Plan'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'Fact',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Sale').value('Fact'),
        }]
    }]
});
