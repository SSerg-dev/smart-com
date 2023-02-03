Ext.define('App.model.tpm.rsmode.DeletedRSmode', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RollingScenario',
    fields: [
        { name: 'DeletedDate', type: 'date', isDefault: false },
        { name: 'Id', hidden: true },
        { name: 'RSId', type: 'int', hidden: false, isDefault: true, },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ExpirationDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoStatusSystemName', type: 'string', mapping: 'PromoStatus.SystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        { name: 'IsMLmodel', type: 'boolean', hidden: false, isDefault: true },
        { name: 'TaskStatus', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedRollingScenarios',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});