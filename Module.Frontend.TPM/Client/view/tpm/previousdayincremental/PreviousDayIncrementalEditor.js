Ext.define('App.view.tpm.previousdayincremental.PreviousDayIncrementalEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.previousdayincrementaleditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    cls: 'readOnlyFields',
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Week',
            fieldLabel: l10n.ns('tpm', 'PreviousDayIncremental').value('Week'),
        }, {
            xtype: 'textfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'PreviousDayIncremental').value('DemandCode'),
        }, {
            xtype: 'numberfield',
            name: 'IncrementalQty',
            fieldLabel: l10n.ns('tpm', 'PreviousDayIncremental').value('IncrementalQty'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('ZREP'),
            name: 'ProductId',
            selectorWidget: 'product',
            valueField: 'Id',
            displayField: 'ZREP',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.product.Product',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.product.Product',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'ZREP',
                to: 'ZREP'
            }]
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('Number'),
            name: 'PromoId',
            selectorWidget: 'promo',
            valueField: 'Id',
            displayField: 'Number',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.promo.Promo',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.promo.Promo',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Number',
                to: 'Number'
            }]
        }, {
            xtype: 'datefield',
            name: 'LastChangeDate',
            fieldLabel: l10n.ns('tpm', 'PreviousDayIncremental').value('LastChangeDate'),
        },
        ]
    
    }
         
    
});     