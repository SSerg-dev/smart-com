Ext.define('App.view.tpm.baseline.BaseLineEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.baselineeditor',
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
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ProductZREP'),
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
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode')
        }, {
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('StartDate'),
            minValue: new Date(),
            allowBlank: false,
            editable: false,
            format: 'd.m.Y',
            listeners: {
                afterrender: function (field) {
                    var minValue = new Date();
                    var currentTimeZoneOffsetInHours = minValue.getTimezoneOffset();
                    var minValueInt = minValue.getTime();
                    var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                    if (currentRole !== 'SupportAdministrator') {
                        field.setMinValue(new Date(minValueInt + currentTimeZoneOffsetInHours * 60000 + 10800000));
                        field.getPicker().setValue(field.minValue);
                    } else {
                        field.setMinValue(null);
                    }
                }
            }          
        }, {
            xtype: 'numberfield',
            name: 'InputBaselineQTY',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('InputBaselineQTY'),
        }, {
            xtype: 'numberfield',
            name: 'SellInBaselineQTY',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('SellInBaselineQTY'),
        }, {
            xtype: 'numberfield',
            name: 'SellOutBaselineQTY',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('SellOutBaselineQTY'),
        }, {
            xtype: 'numberfield',
            name: 'Type',
            allowDecimals: false,
            allowExponential: false,
            minValue: 0,
            maxValue: 1000000000,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Type'),
        }]
    }
});
