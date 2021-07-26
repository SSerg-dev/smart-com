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
            name: 'EAN_PC',
            maxLength: 255,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('EAN_PC'),
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
                    var record = field.up('editorform').getRecord();
                    var actualProductQty = record.get('ActualProductCaseQty'); //field.up('editorform').down('numberfield[name=ActualProductCaseQty]');
                    var valueCase = newValue && actualProductQty ? newValue / (record.get('UOM_PC2Case') * 1.0) : null;
                    record.set('ActualProductCaseQty', valueCase);

                    var valueActualProductPCLSV = newValue * record.get('ActualProductSellInPrice') * (record.get('ActualProductShelfDiscount')/100);
                    //record.set('ActualProductPCLSV', valueActualProductPCLSV);
                    field.up('editorform').down('singlelinedisplayfield[name=ActualProductPCLSV]').setValue(valueActualProductPCLSV);
                }
            }
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualProductPCLSV',
            allowDecimals: true,
            allowExponential: false,
            //minValue: 0,
            //maxValue: 10000000000,
            //allowBlank: true,
            //allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCLSV'),
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'PluCode',
                fieldLabel: l10n.ns('tpm', 'PromoProduct').value('PluCode'),
            }]
    }
});
