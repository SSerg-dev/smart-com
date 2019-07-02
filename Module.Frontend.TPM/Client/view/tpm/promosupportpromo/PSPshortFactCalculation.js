Ext.define('App.view.tpm.promosupportpromo.PSPshortFactCalculation', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.pspshortfactcalculation',
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
            model: 'App.model.tpm.promosupportpromo.PSPshortFactCalculation',
            storeId: 'promosupportpromoshortfactstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupportpromo.PSPshortFactCalculation',
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
                text: l10n.ns('tpm', 'PSPshortFactCalculation').value('BudgetSubItemName'),
                dataIndex: 'BudgetSubItemName'
            }, {
                xtype: 'numbercolumn',
				renderer: function (value) {
                    if (value !== null && value !== undefined) {
                        valueToDisplay = value / 1000000.0;
                        return Ext.util.Format.number(valueToDisplay, '0.00');
                    } else {
                        return null;
                    }
                },
                text: l10n.ns('tpm', 'PSPshortFactCalculation').value('FactCalculation'),
                dataIndex: 'FactCalculation'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosupportpromo.PSPshortFactCalculation',
        items: []
    }]
});
