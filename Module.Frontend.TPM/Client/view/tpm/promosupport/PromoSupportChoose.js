Ext.define('App.view.tpm.promosupport.PromoSupportChoose', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promosupportchoose',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoSupport'),

    customHeaderItems: [],

    dockedItems: [{
        xtype: 'custompromosupportbigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promosupport.PromoSupportChoose',
            storeId: 'promosupportchoosestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupport.PromoSupportChoose',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
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
                text: l10n.ns('tpm', 'PromoSupport').value('Number'),
                dataIndex: 'Number'
            }, {
                text: l10n.ns('tpm', 'PromoSupportChoose').value('BudgetSubItemBudgetItemName'),
                dataIndex: 'BudgetSubItemBudgetItemName',
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
                text: l10n.ns('tpm', 'PromoSupportChoose').value('BudgetSubItemName'),
                dataIndex: 'BudgetSubItemName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budgetsubitem',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budgetsubitem.BudgetSubItem',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoSupportChoose').value('PlanQuantity'),
                dataIndex: 'PlanQuantity'
            }, {
                text: l10n.ns('tpm', 'PromoSupportChoose').value('PlanCostTE'),
                dataIndex: 'PlanCostTE'
            }, {
                text: l10n.ns('tpm', 'PromoSupportChoose').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PromoSupportChoose').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosupport.PromoSupportChoose',
        items: []
    }]
});