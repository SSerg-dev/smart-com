﻿Ext.define('App.model.tpm.promoproduct.DeletedPromoProduct', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProduct',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'PromoId', hidden: true },
        { name: 'ZREP', type: 'string', hidden: false, isDefault: false },
        { name: 'EAN_Case', type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_PC', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanProductCaseQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPCQty', type: 'int', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductCaseLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPCLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductBaselineLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductBaselineCaseQty', type: 'float', hidden: false, isDefault: false, useNull: true },  
        { name: 'ProductBaselinePrice', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPCPrice', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductUplift', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPCQty', type: 'int', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualProductCaseQty', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualProductUOM', type: 'string', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualProductSellInPrice', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductSellInDiscount', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductShelfPrice', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualProductShelfDiscount', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPCLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualPromoShare', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductUplift', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalPCQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalPCLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSVW1', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSVW2', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectLSVW1', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectLSVW2', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductIncrementalCaseQty', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductUpliftPercent', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductLSV', type: 'float', hidden: false, isDefault: false, useNull: true },
        //
        { name: 'PlanProductPostPromoEffectQtyW1', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectQtyW2', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectQtyW1', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectQtyW2', type: 'float', hidden: false, isDefault: false, useNull: true },
        { name: 'ActualProductLSVByCompensation', type: 'float', hidden: false, isDefault: false, useNull: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedPromoProducts',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
