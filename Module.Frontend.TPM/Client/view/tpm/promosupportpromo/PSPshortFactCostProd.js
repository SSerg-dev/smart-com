Ext.define('App.view.tpm.promosupportpromo.PSPshortFactCostProd', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.pspshortfactcostprod',
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
            model: 'App.model.tpm.promosupportpromo.PSPshortFactCostProd',
            storeId: 'promosupportpromoshortfactstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupportpromo.PSPshortFactCostProd',
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
                text: l10n.ns('tpm', 'PSPshortFactCostProd').value('BudgetSubItemName'),
                dataIndex: 'BudgetSubItemName'
            }, {
                xtype: 'numbercolumn',
                extraOperator: 'gte_lt',
                additionalDivision: 1000000.0,
                renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        valueToDisplay = value / 1000000.0;
                        return Ext.util.Format.number(valueToDisplay, '0.00');
                    } else {
                        return null;
                    }
                },
                text: l10n.ns('tpm', 'PSPshortFactCostProd').value('FactCostProd'),
                dataIndex: 'FactCostProd'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosupportpromo.PSPshortFactCostProd',
        items: []
    }]
});
