Ext.define('App.model.tpm.promoproductcorrection.PromoProductCorrection', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductsCorrection',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoProductId', hidden: true, isDefault: true },
        { name: 'PlanProductUpliftPercentCorrected', type: 'float', hidden: false, isDefault: true },
        { name: 'UserId', hidden: true, isDefault: true, defaultValue: null },
        { name: 'UserName', type: 'string', hidden: false, isDefault: false },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ChangeDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        {
            name: 'ZREP', type: 'string', mapping: 'PromoProduct.Product.ZREP', breezeEntityType: 'Product', hidden: false, isDefault: true
        },
        {
            name: 'Number', type: 'int', mapping: 'PromoProduct.Promo.Number', hidden: false, isDefault: true, defaultFilterConfig: { valueField: 'Number' }, isKey: true,
        },
        { name: 'ClientHierarchy', type: 'string', mapping: 'PromoProduct.Promo.ClientHierarchy', hidden: false, isDefault: true },
        {
            name: 'BrandTech',
            type: 'string',
            mapping: 'PromoProduct.Promo.BrandTech.Name',
            hidden: false,
            isDefault: true,
            useNull: true,
            convert: function (value) {
                if (value === "")
                    return null;
                return value;
            }
        },
        { name: 'Event', type: 'string', mapping: 'PromoProduct.Promo.Event.Name', hidden: false, isDefault: true },
        { name: 'Mechanic', type: 'string', mapping: 'PromoProduct.Promo.Mechanic', hidden: false, isDefault: true },
        { name: 'MarsStartDate', type: 'string', mapping: 'PromoProduct.Promo.MarsStartDate', hidden: false, isDefault: true },
        { name: 'MarsEndDate', type: 'string', mapping: 'PromoProduct.Promo.MarsEndDate', hidden: false, isDefault: true },
        { name: 'Status', type: 'string', mapping: 'PromoProduct.Promo.PromoStatus.Name', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSV', type: 'float', mapping: 'PromoProduct.PlanProductBaselineLSV', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductIncrementalLSV', type: 'float', mapping: 'PromoProduct.PlanProductIncrementalLSV', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductLSV', type: 'float', mapping: 'PromoProduct.PlanProductLSV', hidden: false, isDefault: true, useNull: true },
        { name: 'ProductSubrangesList', type: 'string', mapping: 'PromoProduct.Promo.ProductSubrangesList', hidden: false, isDefault: true },
        { name: 'PromoDispatchStartDate', type:'date', hidden: true, mapping:'PromoProduct.Promo.DispatchesStart', timeZone: +3, convert: dateConvertTimeZone}
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoProductsCorrections',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            TPMmode: 'Current'
        }
    }
});
