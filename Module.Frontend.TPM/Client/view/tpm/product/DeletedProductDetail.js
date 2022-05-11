Ext.define('App.view.tpm.product.DeletedProductDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedproductdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'Product').value('ZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN_Case',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_Case'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN_PC',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_PC'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductEN'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Brand',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Brand_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Technology',
            fieldLabel: l10n.ns('tpm', 'Product').value('Technology'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Tech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Tech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Segmen_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Segmen_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Brandsegtech',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brandsegtech'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTechsub',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTechsub'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubBrand',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrand'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubBrand_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrand_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandFlagAbbr',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlagAbbr'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlag'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubmarkFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubmarkFlag'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IngredientVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('IngredientVariety'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductCategory',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductCategory'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductType',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarketSegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('MarketSegment'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SupplySegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('SupplySegment'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FunctionalVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('FunctionalVariety'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'Product').value('Size'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandEssence',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandEssence'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PackType',
            fieldLabel: l10n.ns('tpm', 'Product').value('PackType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'GroupSize',
            fieldLabel: l10n.ns('tpm', 'Product').value('GroupSize'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TradedUnitFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('TradedUnitFormat'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ConsumerPackFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('ConsumerPackFormat'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'UOM_PC2Case',
            fieldLabel: l10n.ns('tpm', 'Product').value('UOM_PC2Case'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Division',
            fieldLabel: l10n.ns('tpm', 'Product').value('Division'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'UOM',
            fieldLabel: l10n.ns('tpm', 'Product').value('UOM'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'NetWeight',
            fieldLabel: l10n.ns('tpm', 'Product').value('NetWeight'),
        }]
    }
})