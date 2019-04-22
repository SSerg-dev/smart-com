Ext.define('App.model.core.filecollectinterfacesetting.FileCollectInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'FileCollectInterfaceSetting',
    fields: [
		{ name: 'Id', hidden: true},
		{ name: 'InterfaceId', hidden: true},
		{ name: 'InterfaceName', type: 'string', isDefault: true, mapping: 'Interface.Name'},
		{ name: 'SourcePath', type: 'string', isDefault: true},
		{ name: 'SourceFileMask', type: 'string', isDefault: true},
		{ name: 'CollectHandler', type: 'string', isDefault: true}
	],
    proxy: {
        type: 'breeze',
        resourceName: 'FileCollectInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});