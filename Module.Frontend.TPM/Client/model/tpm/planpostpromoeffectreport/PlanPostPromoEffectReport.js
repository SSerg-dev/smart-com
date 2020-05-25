Ext.define('App.model.tpm.planpostpromoeffectreport.PlanPostPromoEffectReport', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PlanPostPromoEffectReportWeekView',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ZREP', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoNameId', type: 'string', hidden: false, isDefault: true },
        { name: 'LocApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'TypeApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'ModelApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'WeekStartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PlanUplift', type: 'number', hidden: false, isDefault: true },
		{ name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
		{ name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
		{ name: 'Status', type: 'string', hidden: false, isDefault: true, defaultFilterConfig: getPlanPostPromoEffectReportDefaultFilterStatus() },
        { name: 'Week', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanPostPromoEffectQtyW1', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineCaseQtyW1', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanProductPostPromoEffectLSVW1', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSVW1', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanPostPromoEffectQtyW2', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineCaseQtyW2', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanProductPostPromoEffectLSVW2', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSVW2', type: 'number', hidden: false, isDefault: true },

        { name: 'InOut', type: 'bool', hidden: false, isDefault: true },
        { name: 'IsOnInvoice', type: 'bool', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PlanPostPromoEffectReports',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    },
});

function getPlanPostPromoEffectReportDefaultFilterStatus() {
    var statuses = ['Approved', 'Planned'];
    var value = Ext.create('App.extfilter.core.ValueList', statuses);

    var result = {
        value: value,
        operation: 'In'
    };
    return result;
}