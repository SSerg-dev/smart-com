Ext.define('App.model.core.filebuffer.FileBuffer', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'FileBuffer',
    getId: function () {
        return this.get('Key');
    },
    fields: [
        { name: 'Key', hidden: true, convert: function (v, record) { return [record.get('Id'), record.get('CreateDate')]; } },
		{ name: 'Id', hidden: true },
        { name: 'InterfaceId', hidden: true },
        { name: 'InterfaceName', type: 'string', isDefault: true, mapping: 'Interface.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Interface', hidden: false },
        { name: 'InterfaceDirection', type: 'string', isDefault: true, mapping: 'Interface.Direction', defaultFilterConfig: { valueField: 'Direction' }, breezeEntityType: 'Interface', hidden: false },
        { name: 'CreateDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
		{ name: 'UserId', hidden: true },
		{ name: 'HandlerId', hidden: true },
		{ name: 'FileName', type: 'string', isDefault: true },
		{ name: 'Status', type: 'string', isDefault: true },
        { name: 'ProcessDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'FileBuffers',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});