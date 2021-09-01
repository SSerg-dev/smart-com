Ext.define('App.model.core.rpasetting.RPASetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RPASetting',
    fields: [
        {name: 'Id', hidden: true},
        {name: 'Json', type: 'string', isDefault: true},
        {name: 'Name', type: 'string', isDefault: true}
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'RPASettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
})