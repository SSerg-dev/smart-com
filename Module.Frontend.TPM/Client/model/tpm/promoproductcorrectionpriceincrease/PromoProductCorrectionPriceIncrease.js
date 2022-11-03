Ext.define('App.model.tpm.promoproductcorrectionpriceincrease.PromoProductCorrectionPriceIncrease', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductCorrectionPriceIncrease',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoProductPriceIncreaseId', hidden: true, isDefault: true },
        { name: 'PlanProductUpliftPercentCorrected', type: 'float', hidden: false, isDefault: true },
        { name: 'UserId', hidden: true, isDefault: true, defaultValue: null },
        { name: 'UserName', type: 'string', hidden: false, isDefault: false },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ChangeDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoProductCorrectionPriceIncreases',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
