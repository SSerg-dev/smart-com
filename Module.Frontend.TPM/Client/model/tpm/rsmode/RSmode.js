Ext.define('App.model.tpm.rsmode.RSmode', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RollingScenario',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'RSId', type: 'int', hidden: false, isDefault: true,},
        { name: 'ScenarioType', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ExpirationDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'RSstatus', type: 'string', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        { name: 'IsMLmodel', type: 'boolean', hidden: false, isDefault: true },
        { name: 'TaskStatus', type: 'string', hidden: false, isDefault: true },
        { name: 'HandlerId', hidden: true}
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'RollingScenarios',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});