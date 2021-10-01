Ext.define('App.model.tpm.calendarcompetitorcompany.CalendarCompetitorCompany', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CalendarCompetitorCompany',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'CompanyName', type: 'string', defaultFilterConfig: { valueField: 'CompanyName' }, hidden: false, isDefault: true },
        { name: 'CalendarCompetitorId', hidden: true, isDefault: true },
        {
            name: 'CompetitorName',
            type: 'string',
            mapping: 'CalendarСompetitor.Name',
            defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'CalendarСompetitor',
            hidden: false,
            isDefault: true
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'CalendarCompetitorCompany',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
