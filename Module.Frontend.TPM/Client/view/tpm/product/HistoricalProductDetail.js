Ext.define('App.view.tpm.product.HistoricalProductDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalproductdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalProduct', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalProduct').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'Product').value('ZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EAN',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductRU',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductRU'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductEN'),
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
            xtype: 'singlelinedisplayfield',            name: 'UOM_PC2Case',            fieldLabel: l10n.ns('tpm', 'Product').value('UOM_PC2Case'),
        }]
    }
});
