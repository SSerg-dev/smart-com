Ext.define('App.view.tpm.ratishopper.RATIShopperEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.ratishoppereditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('ClientTreeFullPathName'),
            selectorWidget: 'clienttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            clientTreeIdValid: true,
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.clienttree.ClientTree',
                autoLoad: false,
                root: {}
            },
            listeners:
            {
                change: function (field, newValue, oldValue) {
                    if (field && field.record && field.record.data.ObjectId === 5000000) {
                        this.clientTreeIdValid = false;
                    } else {
                        this.clientTreeIdValid = true;
                    }
                }
            },
            validator: function () {
                if (!this.clientTreeIdValid) {
                    return l10n.ns('core', 'customValidators').value('clientTreeSelectRoot')
                }
                return true;
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ClientTreeFullPathName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'Year',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('Year'),
            readOnly: false,
            allowBlank: false,
            listeners: {
                change: function (newValue, oldValue) {
                },
            }
        }, {
            xtype: 'numberfield',
            name: 'RATIShopperPercent',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('RATIShopperPercent'),
            minValue: 0,
            maxValue: 100,
            readOnly: false,
            allowBlank: false,
            listeners: {
                change: function (newValue, oldValue) {
                    if (newValue > 100) {
                        this.setValue(oldValue);
                    }
                },
            }
        }
        ]
    }
});


