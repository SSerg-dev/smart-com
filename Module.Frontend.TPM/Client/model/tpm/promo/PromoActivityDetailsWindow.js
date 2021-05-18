Ext.define('App.model.tpm.product.PromoProductDetailsWindow', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductDetailsWindow',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ZREP', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_Case', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'EAN_PC', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductEN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'BrandsegTech_code', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'Brandsegtech', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSV', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ActualProductBaselineLSV', useNull: true, type: 'string', hidden: false, isDefault: true/*, mapping:'PlanProductBaselineLSV'*/ },
        { name: 'PlanProductBaselineLSV', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ActualProductUpliftPercent', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'ProductEN', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'SumInvoiceProduct', useNull: true, type: 'float', hidden: false, isDefault: false }

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Promoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});