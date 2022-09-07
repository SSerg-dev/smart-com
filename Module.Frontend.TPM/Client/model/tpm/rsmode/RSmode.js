Ext.define('App.model.tpm.rsmode.RSmode', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RollingScenario',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'RSId', type: 'int', hidden: false, isDefault: true,},
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
        //{
        //    name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
        //    defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true
        //},
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