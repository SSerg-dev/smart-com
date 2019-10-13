Ext.define('App.view.tpm.demandpricelist.DemandPriceListEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.demandpricelisteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

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
            xtype: 'textfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('ClientTreeDemandCode')
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('StartDate'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            listeners: {
                afterrender: function (field) {
                    var minValue = new Date();
                    var currentTimeZoneOffsetInHours = minValue.getTimezoneOffset();
                    var minValueInt = minValue.getTime();
                    field.setMinValue(new Date(minValueInt + currentTimeZoneOffsetInHours * 60000 + 10800000));
                    field.getPicker().setValue(field.minValue);
                }
            }         
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
