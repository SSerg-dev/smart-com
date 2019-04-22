Ext.define('App.model.tpm.product.DeletedProduct', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Product',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
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
        resourceName: 'DeletedProducts',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
