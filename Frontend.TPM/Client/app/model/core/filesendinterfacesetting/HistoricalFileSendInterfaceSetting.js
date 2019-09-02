Ext.define('App.model.core.filesendinterfacesetting.HistoricalFileSendInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalFileSendInterfaceSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', '_Id', 'string') },
		{ name: '_ObjectId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', '_ObjectId') },
		{ name: '_User', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', '_User', 'string') },
		{ name: '_Role', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', '_Role', 'string') },
        { name: '_EditDate', type: 'date', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', '_EditDate', 'date'), timeZone: +3, convert: dateConvertTimeZone },
		{ name: '_Operation', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', '_Operation', 'string') },
		{ name: 'InterfaceId', hidden: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', 'InterfaceId') },
		{ name: 'InterfaceName', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('Interface', 'Name', 'search') },
		{ name: 'TargetPath', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', 'TargetPath', 'string') },
		{ name: 'TargetFileMask', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', 'TargetFileMask', 'string') },
		{ name: 'SendHandler', type: 'string', isDefault: true, extendedFilterEntry: App.extfilter.core.ConfigSource.getEntryConfig('HistoricalFileSendInterfaceSetting', 'SendHandler', 'string') }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalFileSendInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});