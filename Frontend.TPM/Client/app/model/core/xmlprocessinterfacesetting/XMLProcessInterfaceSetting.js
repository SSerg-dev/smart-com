Ext.define('App.model.core.xmlprocessinterfacesetting.XMLProcessInterfaceSetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'XMLProcessInterfaceSetting',
    fields: [
		{ name: 'Id', hidden: true},
		{ name: 'InterfaceId', hidden: true},
		{ name: 'InterfaceName', type: 'string', isDefault: true, mapping: 'Interface.Name'},
		{ name: 'RootElement', type: 'string', isDefault: true},
		{ name: 'ProcessHandler', type: 'string', isDefault: true}
	],
    proxy: {
        type: 'breeze',
        resourceName: 'XMLProcessInterfaceSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});