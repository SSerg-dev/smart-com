Ext.define('App.model.core.filecollectinterfacesetting.HistoricalFileCollectInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalFileCollectInterfaceSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', '_Id', 'string') },
		{ name: '_ObjectId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', '_ObjectId') },
		{ name: '_User', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', '_User', 'string') },
		{ name: '_Role', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', '_Role', 'string') },
		{ name: '_EditDate', type: 'date', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', '_EditDate', 'date') },
		{ name: '_Operation', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', '_Operation', 'string') },
		{ name: 'InterfaceId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', 'InterfaceId') },
		{ name: 'InterfaceName', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('Interface', 'Name', 'search') },
		{ name: 'SourcePath', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', 'SourcePath', 'string') },
		{ name: 'SourceFileMask', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', 'SourceFileMask', 'string') },
		{ name: 'CollectHandler', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileCollectInterfaceSetting', 'CollectHandler', 'string') }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalFileCollectInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});