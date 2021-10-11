Ext.define('App.model.tpm.calendarcompetitorbrandtechcolor.CalendarCompetitorBrandTechColor', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CalendarCompetitorBrandTechColor',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Color', useNull: false, type: 'string', hidden: false, isDefault: true },
        { name: 'CalendarCompetitorCompanyId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        {
            name: 'CompanyName', type: 'string', mapping: 'CalendarCompetitorCompany.CompanyName', defaultFilterConfig: { valueField: 'CompanyName' },
            breezeEntityType: 'Brand', hidden: false, isDefault: true
        },
        { name: 'BrandTech', useNull: false, type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'CalendarCompetitorBrandTechColors',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
