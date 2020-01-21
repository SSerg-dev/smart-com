Ext.define('App.model.tpm.promo.HistoricalUserRolePromo', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Promo',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'CreatorLogin', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            //Id промо для метода GetById в истории
            promoIdHistory: null,
            onlyCreator: true
        }
    }
});
