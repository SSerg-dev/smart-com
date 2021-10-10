Ext.define('App.model.tpm.calendarcompetitorcompany.CalendarCompetitorCompany', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CalendarCompetitorCompany',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'CompanyName', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'CalendarCompetitorCompanies',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
