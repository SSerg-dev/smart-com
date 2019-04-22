Ext.define('App.model.tpm.product.HistoricalProduct', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Product',
    fields: [
         { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'ZREP', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'EAN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductRU', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductEN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandFlagAbbr', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandFlag', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'SubmarkFlag', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'IngredientVariety', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductCategory', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductType', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'MarketSegment', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'SupplySegment', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'FunctionalVariety', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'Size', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandEssence', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'PackType', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'GroupSize', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'TradedUnitFormat', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ConsumerPackFormat', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'UOM_PC2Case', useNull: true, type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalProducts',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
