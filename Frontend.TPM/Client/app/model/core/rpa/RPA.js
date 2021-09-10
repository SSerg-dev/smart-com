Ext.define('App.model.core.rpa.RPA', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RPA',
    fields: [
        { name: 'Id', hidden: true},
        { name: 'HandlerName', type: 'string', isDefault: true },
        { name: 'CreateDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'UserName', type: 'string', isDefault: true },
        { name: 'Constraint', type: 'string', isDefault: true},
        { name: 'Parametr', type: 'string', isDefault: true},
        { name: 'Status', type: 'string', isDefault: true},
        { name: 'FileURL', type: 'string', isDefault: true},
        { name: 'LogURL', type: 'string', isDefault: true}
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'RPAs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
})