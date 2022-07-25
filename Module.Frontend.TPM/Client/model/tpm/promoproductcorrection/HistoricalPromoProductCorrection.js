Ext.define('App.model.tpm.promoproductcorrection.HistoricalPromoProductCorrection', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'PromoProductsCorrection',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'PromoProductId', hidden: true, isDefault: true },
        { name: 'PlanProductUpliftPercentCorrected', type: 'float', hidden: false, isDefault: true },
        { name: 'UserId', hidden: true, isDefault: true, defaultValue: null },
        { name: 'UserName', type: 'string', hidden: false, isDefault: false },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ChangeDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },

        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPromoProductsCorrections',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
