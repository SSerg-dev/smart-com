Ext.define('App.model.tpm.planincrementalreport.PlanIncrementalReport', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PlanIncrementalReport',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ZREP', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoNameId', type: 'string', hidden: false, isDefault: true },
        { name: 'LocApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'TypeApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'ModelApollo', type: 'string', hidden: false, isDefault: true },
        { name: 'WeekStartDate', type: 'date', hidden: false, isDefault: true, timeZone: 0, convert: dateConvertTimeZone }, // timeZone: 0, т.к. расширенный фильтр этого поля работает при таком значении
        { name: 'PlanProductIncrementalCaseQty', type: 'number', hidden: false, isDefault: true },
        { name: 'PlanUplift', type: 'number', hidden: false, isDefault: true },
		{ name: 'DispatchesStart', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
		{ name: 'DispatchesEnd', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
		{ name: 'Week', type: 'string', hidden: false, isDefault: true },
		{ name: 'Status', type: 'string', hidden: false, isDefault: true, defaultFilterConfig: getPlanIncrementalReportDefaultFilterStatus() },
		{ name: 'PlanProductBaselineCaseQty', type: 'number', hidden: false, isDefault: true },
		{ name: 'PlanProductIncrementalLSV', type: 'number', hidden: false, isDefault: true },
		{ name: 'PlanProductBaselineLSV', type: 'number', hidden: false, isDefault: true },
        { name: 'InOut', type: 'bool', hidden: false, isDefault: true },
        { name: 'IsGrowthAcceleration', type: 'bool', hidden: false, isDefault: true },
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

function getPlanIncrementalReportDefaultFilterStatus() {
	var statuses = ['Approved', 'Planned', 'Started'];
	var value = Ext.create('App.extfilter.core.ValueList', statuses);

	var result = {
		value: value,
		operation: 'In'
	};
	return result;
}