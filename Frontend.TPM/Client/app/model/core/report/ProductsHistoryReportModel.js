Ext.define('App.model.core.report.ProductsHistoryReportModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Test',
    fields: [
				{
				    name: 'TargetStartDate', type: 'date', isDefault: true, filterOperationsConfig: {
				        allowedOperations: { date: ['Between'] }
				        }
				    
				},
	],
    proxy: {
        type: 'breeze',
        resourceName: 'Tests',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});