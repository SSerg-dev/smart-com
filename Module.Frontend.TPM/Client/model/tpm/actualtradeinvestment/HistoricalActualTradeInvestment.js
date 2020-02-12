Ext.define('App.model.tpm.actualtradeinvestment.HistoricalActualTradeInvestment', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'TradeInvestment',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true },
        { name: '_Operation', type: 'string', isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'BrandTechName', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'ClientTreeFullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'TIType', type: 'string', hidden: false, isDefault: true },
        { name: 'TISubType', type: 'string', hidden: false, isDefault: true },
        { name: 'SizePercent', type: 'float', hidden: false, isDefault: true },
        { name: 'MarcCalcROI', type: 'bool', hidden: false, isDefault: true },
        { name: 'MarcCalcBudgets', type: 'bool', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalActualTradeInvestments',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            Id: null
        }
    }
});
