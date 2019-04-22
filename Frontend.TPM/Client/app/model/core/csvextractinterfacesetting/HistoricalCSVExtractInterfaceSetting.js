Ext.define('App.model.core.csvextractinterfacesetting.HistoricalCSVExtractInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalCSVExtractInterfaceSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', '_Id', 'string') },
		{ name: '_ObjectId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', '_ObjectId') },
		{ name: '_User', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', '_User', 'string') },
		{ name: '_Role', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', '_Role', 'string') },
		{ name: '_EditDate', type: 'date', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', '_EditDate', 'date') },
		{ name: '_Operation', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', '_Operation', 'string') },
		{ name: 'InterfaceId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', 'InterfaceId') },
		{ name: 'InterfaceName', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('Interface', 'Name', 'search') },
		{ name: 'FileNameMask', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', 'FileNameMask', 'string') },
		{ name: 'ExtractHandler', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalCSVExtractInterfaceSetting', 'ExtractHandler', 'string') }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalCSVExtractInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});