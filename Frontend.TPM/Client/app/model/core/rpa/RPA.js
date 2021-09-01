Ext.define('App.model.core.rpa.RPA', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RPA',
    fields: [
        {name: 'Id', hidden: true},
        {name: 'HandlerName', type: 'string', isDefault: true},
        {name: 'Constraint', type: 'string', isDefault: false},
        {name: 'Parametr', type: 'string', isDefault: false},
        {name: 'Status', type: 'string', isDefault: false},
        {name: 'FileURL', type: 'string', isDefault: false},
        {name: 'LogURL', type: 'string', isDefault: false}
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