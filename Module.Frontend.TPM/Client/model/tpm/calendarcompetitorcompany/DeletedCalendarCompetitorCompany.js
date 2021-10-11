Ext.define('App.model.tpm.calendarcompetitorcompany.DeletedCalendarCompetitorCompany', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CalendarCompetitorCompany',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedCalendarCompetitorCompanies',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
