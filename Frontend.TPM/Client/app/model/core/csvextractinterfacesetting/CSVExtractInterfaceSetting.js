Ext.define('App.model.core.csvextractinterfacesetting.CSVExtractInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CSVExtractInterfaceSetting',
    fields: [
		{ name: 'Id', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVExtractInterfaceSetting', 'Id') },
		{ name: 'InterfaceId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVExtractInterfaceSetting', 'InterfaceId') },
		{ name: 'InterfaceName', type: 'string', isDefault: true, mapping: 'Interface.Name', extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('Interface', 'Name', 'search') },
		{ name: 'FileNameMask', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVExtractInterfaceSetting', 'FileNameMask', 'string') },
		{ name: 'ExtractHandler', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVExtractInterfaceSetting', 'ExtractHandler', 'string') }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'CSVExtractInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});