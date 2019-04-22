Ext.define('App.view.tpm.demandpricelist.DemandPriceListEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.demandpricelisteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'searchfield',
            name: 'ProductId',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('ProductZREP'),
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
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('ClientTreeDemandCode'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'DemandCode',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            mapping: [{
                from: 'DemandCode',
                to: 'ClientTreeDemandCode'
            }]
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('StartDate'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y'
            // minValue: new Date(),          
        }, {
            xtype: 'numberfield',
            name: 'Price',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('Price'),
        }]
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'canceledit',
        hidden: true
    }, {
        text: l10n.ns('core', 'buttons').value('edit'),
        itemId: 'edit',
        hidden: true
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'ok',
        hidden: true
    }]
});
