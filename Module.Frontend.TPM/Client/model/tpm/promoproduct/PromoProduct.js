﻿Ext.define('App.model.tpm.promoproduct.PromoProduct', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProduct',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true },
        { name: 'ZREP', type: 'string', hidden: true, isDefault: false },
        { name: 'EAN_Case', type: 'string', hidden: true, isDefault: false },        
        { name: 'EAN_PC', type: 'string', hidden: false, isDefault: true },
        {
            name: 'PluCode', type: 'string', mapping: 'Plu.PluCode',
            defaultFilterConfig: { valueField: 'PluCode' }, breezeEntityType: 'PromoProduct2Plu', hidden: false, isDefault: true
        },
        { name: 'PlanProductCaseQty', type: 'float', hidden: true, isDefault: false, useNull: true },        
        { name: 'PlanProductPCQty', type: 'int', hidden: true, isDefault: false, useNull: true },        
        { name: 'PlanProductCaseLSV', type: 'float', hidden: true, isDefault: false, useNull: true },        
        { name: 'PlanProductPCLSV', type: 'float', hidden: true, isDefault: false, useNull: true },  
        { name: 'PlanProductBaselineLSV', type: 'float', hidden: true, isDefault: false, useNull: true },  
        { name: 'PlanProductBaselineCaseQty', type: 'float', hidden: true, isDefault: false, useNull: true },  
        { name: 'ProductBaselinePrice', type: 'float', hidden: true, isDefault: false, useNull: true },       
        { name: 'PlanProductPCPrice', type: 'float', hidden: true, isDefault: false, useNull: true },     
        { name: 'ActualProductPCQty', type: 'int', hidden: false, isDefault: true, useNull: true },         
        { name: 'ActualProductCaseQty', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductUOM', type: 'string', hidden: true, isDefault: false, useNull: true },        
        { name: 'ActualProductSellInPrice', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductShelfDiscount', type: 'float', hidden: true, isDefault: false, useNull: true },        
        { name: 'ActualProductPCLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
		{ name: 'ActualProductUpliftPercent', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalPCQty', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalPCLSV', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductIncrementalLSV', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSVW1', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSVW2', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectLSV', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectLSV', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductIncrementalCaseQty', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductUpliftPercent', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductLSV', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectQty', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectQty', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductIncrementalLSV', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductLSV', type: 'float', hidden: true, isDefault: false, useNull: true },

        { name: 'UOM_PC2Case', type: 'int', hidden: true, mapping: 'Product.UOM_PC2Case'},
        //
        { name: 'PlanProductPostPromoEffectQtyW1', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'PlanProductPostPromoEffectQtyW2', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectQtyW1', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductPostPromoEffectQtyW2', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'ActualProductLSVByCompensation', type: 'float', hidden: true, isDefault: false, useNull: true },
        { name: 'SumInvoiceProduct', useNull: true, type: 'float', hidden: true, isDefault: false },

        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoProducts',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            updateActualsMode: false,
            promoIdInUpdateActualsMode: null
        }
    }
});
