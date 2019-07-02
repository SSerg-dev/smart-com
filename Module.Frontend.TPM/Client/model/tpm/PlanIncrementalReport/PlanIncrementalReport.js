Ext.define('App.model.tpm.planincrementalreport.PlanIncrementalReport', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PlanIncrementalReport',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ZREP', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoName', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoNameId', type: 'string', hidden: false, isDefault: true },
        { name: 'LocApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'TypeApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'ModelApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'WeekStartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'PlanProductCaseQty', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanUplift', type: 'number', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },
        { name: 'Status', type: 'string', hidden: false, isDefault: true },
        { name: 'InOut', useNull: true, type: 'bool', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PlanIncrementalReports',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
});