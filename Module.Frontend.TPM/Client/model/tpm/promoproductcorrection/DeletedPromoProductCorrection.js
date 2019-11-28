Ext.define('App.model.tpm.promoproductcorrection.DeletedPromoProductCorrection', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductsCorrection',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'PromoProductId', hidden: true, isDefault: true },
        { name: 'PlanProductUpliftPercentCorrected', type: 'float', hidden: false, isDefault: true },
        { name: 'UserId', hidden: true, isDefault: true },
        { name: 'UserName', type: 'string', hidden: false, isDefault: false },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ChangeDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        {
            name: 'ZREP', type: 'string', mapping: 'PromoProduct.ZREP', breezeEntityType: 'PromoProduct', hidden: false, isDefault: true
        },
        {
            name: 'Number', type: 'int', mapping: 'PromoProduct.Promo.Number', breezeEntityType: 'Promo', hidden: false, isDefault: true, defaultFilterConfig: { valueField: 'Number' },
        },


    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedPromoProductsCorrections',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
