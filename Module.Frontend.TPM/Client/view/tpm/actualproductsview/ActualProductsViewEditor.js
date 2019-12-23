Ext.define('App.view.tpm.actualproductsview.ActualProductsViewEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.actualproducteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ZREP'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'EAN_Case',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('EAN_Case'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'EAN_PC',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('EAN_PC'),
            maxLength: 255,
        },{
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ProductEN'),
            maxLength: 255,
            }, {
                xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
                name: 'BrandsegTech_code',
                fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('BrandsegTech_code'),
                maxLength: 255,
            }, {
                xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
                name: 'Brandsegtech',
                fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('Brandsegtech'),
                maxLength: 255,
            }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandFlagAbbr',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('BrandFlagAbbr'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandFlag',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('BrandFlag'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SubmarkFlag',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('SubmarkFlag'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'IngredientVariety',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('IngredientVariety'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductCategory',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ProductCategory'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductType',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ProductType'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'MarketSegment',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('MarketSegment'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SupplySegment',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('SupplySegment'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'FunctionalVariety',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('FunctionalVariety'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('Size'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandEssence',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('BrandEssence'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'PackType',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('PackType'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'GroupSize',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('GroupSize'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'TradedUnitFormat',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('TradedUnitFormat'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ConsumerPackFormat',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('ConsumerPackFormat'),
            maxLength: 255,
        }, {
            xtype: 'numberfield',
            name: 'UOM_PC2Case',
            fieldLabel: l10n.ns('tpm', 'ActualProductsView').value('UOM_PC2Case'),
            allowDecimals: true,
            allowExponential: false,
            minValue: 0,
            maxValue: 100000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
        }]
    },

    //Скрывать кнопки редактирования
    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'canceledit'
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
