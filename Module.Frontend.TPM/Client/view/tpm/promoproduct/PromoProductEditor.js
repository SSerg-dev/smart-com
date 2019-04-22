Ext.define('App.view.tpm.promoproduct.PromoProductEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promoproducteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    afterWindowShow: function (scope, isCreating) {
        scope.down('numberfield[name=ActualProductPCQty]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'EAN',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('EAN'),
        }, {
            xtype: 'numberfield',
            name: 'ActualProductPCQty',
            allowDecimals: false,
            allowExponential: false,
            minValue: 0,
            maxValue: 1000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCQty'),
            listeners: {
                change: function (field, newValue, oldValue) {                    
                    var actualProductQty = field.up('editorform').down('numberfield[name=ActualProductQty]');
                    var record = field.up('editorform').getRecord();
                    var valueCase = newValue && actualProductQty ? newValue / (record.get('UOM_PC2Case') * 1.0) : null;

                    actualProductQty.setValue(valueCase);
                }
            },
        }, {
            xtype: 'numberfield',
            name: 'ActualProductQty',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductQty'),
            listeners: {
                change: function (field, newValue, oldValue) {                    
                    var actualProductPCQty = field.up('editorform').down('numberfield[name=ActualProductPCQty]');
                    var record = field.up('editorform').getRecord();
                    var valuePC = newValue && actualProductPCQty ? newValue * record.get('UOM_PC2Case') : null;

                    actualProductPCQty.setValue(valuePC);
                }
            },
        }, {
            xtype: 'combobox',
            name: 'ActualProductUOM',
            //maxLength: 255,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductUOM'),
            store: {
                type: 'simplestore',
                id: 'pccase',
                fields: ['value'],
                data: [{ value:'PC' }, { value:'Case' }]
            },
            valueField:'value',
            displayField:'value',
            queryMode: 'local',
            listeners: {
                change: function (field, newValue, oldValue) {
                    var actualProductQty = field.up('editorform').down('numberfield[name=ActualProductQty]');
                    var actualProductPCQty = field.up('editorform').down('numberfield[name=ActualProductPCQty]');

                    if (newValue && actualProductQty && actualProductPCQty) {
                        if (newValue == 'PC') {
                            actualProductQty.setReadOnly(true);
                            actualProductQty.addCls('field-for-read-only');

                            actualProductPCQty.setReadOnly(false);
                            actualProductPCQty.removeCls('field-for-read-only');
                        }
                        else if (newValue == 'Case') {
                            actualProductPCQty.setReadOnly(true);
                            actualProductPCQty.addCls('field-for-read-only');

                            actualProductQty.setReadOnly(false);
                            actualProductQty.removeCls('field-for-read-only');
                        }     
                    }
                }
            },
        }, {
            xtype: 'numberfield',
            name: 'ActualProductShelfPrice',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductShelfPrice'),
        }, {
            xtype: 'numberfield',
            name: 'ActualProductPCLSV',
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCLSV'),
        }]
    }
});
