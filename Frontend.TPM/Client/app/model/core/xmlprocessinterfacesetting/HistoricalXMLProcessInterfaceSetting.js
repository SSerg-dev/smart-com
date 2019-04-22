Ext.define('App.model.core.xmlprocessinterfacesetting.HistoricalXMLProcessInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalXMLProcessInterfaceSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', '_Id', 'string') },
		{ name: '_ObjectId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', '_ObjectId') },
		{ name: '_User', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', '_User', 'string') },
		{ name: '_Role', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', '_Role', 'string') },
		{ name: '_EditDate', type: 'date', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', '_EditDate', 'date') },
		{ name: '_Operation', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', '_Operation', 'string') },
		{ name: 'InterfaceId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', 'InterfaceId') },
		{ name: 'InterfaceName', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('Interface', 'Name', 'search') },
		{ name: 'RootElement', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', 'RootElement', 'string') },
		{ name: 'ProcessHandler', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalXMLProcessInterfaceSetting', 'ProcessHandler', 'string') }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalXMLProcessInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});