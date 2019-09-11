Ext.define('App.model.core.filesendinterfacesetting.FileSendInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'FileSendInterfaceSetting',
    fields: [
		{ name: 'Id', hidden: true},
		{ name: 'InterfaceId', hidden: true},
		{ name: 'InterfaceName', type: 'string', isDefault: true, mapping: 'Interface.Name'},
		{ name: 'TargetPath', type: 'string', isDefault: true},
		{ name: 'TargetFileMask', type: 'string', isDefault: true},
		{ name: 'SendHandler', type: 'string', isDefault: true}
	],
    proxy: {
        type: 'breeze',
        resourceName: 'FileSendInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});