Ext.define('App.model.tpm.promoproductcorrection.PromoProductCorrection', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductsCorrection',
    fields: [
        { name: 'Id', hidden: true },
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
        { name: 'TempId', type: 'string', hidden: true, isDefault: true, defaultValue: null },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoProductsCorrections',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
