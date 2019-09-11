Ext.define('App.model.core.csvprocessinterfacesetting.HistoricalCSVProcessInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalCSVProcessInterfaceSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', '_Id', 'string') },
		{ name: '_ObjectId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', '_ObjectId') },
		{ name: '_User', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', '_User', 'string') },
		{ name: '_Role', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', '_Role', 'string') },
        { name: '_EditDate', type: 'date', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', '_EditDate', 'date'), timeZone: +3, convert: dateConvertTimeZone },
		{ name: '_Operation', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', '_Operation', 'string') },
		{ name: 'InterfaceId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', 'InterfaceId') },
		{ name: 'InterfaceName', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('Interface', 'Name', 'search') },
		{ name: 'Delimiter', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', 'Delimiter', 'string') },
		{ name: 'UseQuoting', type: 'boolean', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', 'UseQuoting', 'boolean') },
		{ name: 'QuoteChar', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', 'QuoteChar', 'string') },
		{ name: 'ProcessHandler', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVProcessInterfaceSetting', 'ProcessHandler', 'string') }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalCSVProcessInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});