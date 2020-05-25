Ext.define('App.view.tpm.pricelist.PriceListEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.pricelisteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'ClientTreeGHierarchyCode',
            fieldLabel: l10n.ns('tpm', 'PriceList').value('ClientTreeGHierarchyCode'),
        }, {
            xtype: 'textfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PriceList').value('ClientTreeFullPathName'),
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
                to: 'ProductZREP'
            }]
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'PriceList').value('StartDate'),
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'PriceList').value('EndDate'),
        }, {
            xtype: 'numberfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'PriceList').value('Price'),
        }]
    }
});     