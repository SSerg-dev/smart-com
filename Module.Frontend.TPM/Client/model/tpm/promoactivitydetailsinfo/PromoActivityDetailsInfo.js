﻿Ext.define('App.model.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProduct',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true },
        { name: 'ZREP', type: 'string', hidden: false, isDefault: false },
        { name: 'ProductEN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_Case', type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_PC', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanProductCaseQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPCQty', type: 'int', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductCaseLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPCLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductBaselineLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductBaselineLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductBaselineCaseQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ProductBaselinePrice', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPCPrice', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPCQty', type: 'int', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualProductCaseQty', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualProductUOM', type: 'string', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualProductSellInPrice', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductShelfDiscount', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPCLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
		{ name: 'ActualProductUpliftPercent', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalPCQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalPCLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSVW1', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSVW2', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductIncrementalCaseQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductIncrementalLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductUpliftPercent', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductLSVByCompensation', type: 'float', hidden: false, isDefault: false, useNull: true },

        { name: 'UOM_PC2Case', type: 'int', hidden: true, mapping: 'Product.UOM_PC2Case' }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoProducts',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
