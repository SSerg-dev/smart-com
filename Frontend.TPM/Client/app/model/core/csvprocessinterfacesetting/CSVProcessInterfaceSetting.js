Ext.define('App.model.core.csvprocessinterfacesetting.CSVProcessInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'CSVProcessInterfaceSetting',
    fields: [
		{ name: 'Id', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVProcessInterfaceSetting', 'Id') },
		{ name: 'InterfaceId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVProcessInterfaceSetting', 'InterfaceId') },
		{ name: 'InterfaceName', type: 'string', isDefault: true, mapping: 'Interface.Name', extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('Interface', 'Name', 'search') },
		{ name: 'Delimiter', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVProcessInterfaceSetting', 'Delimiter', 'string') },
		{ name: 'UseQuoting', type: 'boolean', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVProcessInterfaceSetting', 'UseQuoting', 'boolean') },
		{ name: 'QuoteChar', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVProcessInterfaceSetting', 'QuoteChar', 'string') },
		{ name: 'ProcessHandler', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('CSVProcessInterfaceSetting', 'ProcessHandler', 'string') }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'CSVProcessInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});