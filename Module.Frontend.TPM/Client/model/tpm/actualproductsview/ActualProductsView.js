﻿Ext.define('App.model.tpm.actualproductsview.ActualProductsView', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProduct',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ZREP', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_Case', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_PC', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductRU', mapping: 'Product.ProductRU', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductEN', mapping: 'Product.ProductEN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandFlagAbbr', mapping: 'Product.BrandFlagAbbr', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandFlag', mapping: 'Product.BrandFlag', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'SubmarkFlag', mapping: 'Product.SubmarkFlag', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'IngredientVariety', mapping: 'Product.IngredientVariety', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductCategory', mapping: 'Product.ProductCategory', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductType', mapping: 'Product.ProductType', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'MarketSegment', mapping: 'Product.MarketSegment', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'SupplySegment', mapping: 'Product.SupplySegment', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'FunctionalVariety', mapping: 'Product.FunctionalVariety', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'Size', mapping: 'Product.Size', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandEssence', mapping: 'Product.BrandEssence', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'PackType', mapping: 'Product.PackType', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'GroupSize', mapping: 'Product.GroupSize', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'TradedUnitFormat', mapping: 'Product.TradedUnitFormat', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ConsumerPackFormat', mapping: 'Product.ConsumerPackFormat', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'UOM_PC2Case', mapping: 'Product.UOM_PC2Case', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'ProductId', hidden: true },
        { name: 'PromoId', useNull: true, type: 'int', hidden: true},

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoProducts',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
});