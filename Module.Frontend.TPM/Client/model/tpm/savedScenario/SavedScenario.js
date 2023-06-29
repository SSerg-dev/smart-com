Ext.define('App.model.tpm.savedScenario.SavedScenario', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'SavedScenario',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'CreateDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ScenarioName', type: 'string', hidden: false, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'RollingScenario.ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'SavedScenarios',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});