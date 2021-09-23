﻿Ext.define('App.model.tpm.calendarcompetitorcompany.CalendarCompetitorCompany', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CalendarCompetitorCompany',
    fields: [
        { name: 'Id', type: 'string', hidden: true, isDefault: true, isKey: true, mapping: 'Id' },
        {
            name: 'ClientTreeName', type: 'string', mapping: 'ClientTreeName',
            defaultFilterConfig: { valueField: 'Name' }, tree: true, hidden: false, isDefault: true
        },
        {
            name: 'ObjectId', type: 'int', mapping: 'ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true,
        },
        { name: 'PluCode', type: 'string', hidden: false, isDefault: true, mapping: 'PluCode' },
        { name: 'EAN_PC', type: 'string', hidden: false, isDefault: true, mapping: 'EAN_PC' },
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
