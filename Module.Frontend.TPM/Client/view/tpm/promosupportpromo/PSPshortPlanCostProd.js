Ext.define('App.view.tpm.promosupportpromo.PSPshortPlanCostProd', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.pspshortplancostprod',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoSupportPromo'),

    customHeaderItems: [],

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promosupportpromo.PSPshortPlanCostProd',
            storeId: 'promosupportpromoshortplanstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupportpromo.PSPshortPlanCostProd',
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
                minWidth: 110
            },
            items: [{
                text: l10n.ns('tpm', 'PSPshortPlanCostProd').value('BudgetSubItemName'),
                dataIndex: 'BudgetSubItemName'
            }, {
                xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PSPshortPlanCostProd').value('PlanCostProd'),
                dataIndex: 'PlanCostProd'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosupportpromo.PSPshortPlanCostProd',
        items: []
    }]
});
