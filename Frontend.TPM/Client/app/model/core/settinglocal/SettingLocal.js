Ext.define('App.model.core.settinglocal.SettingLocal', {
    extend: 'Ext.data.Model',

    idProperty: 'id',
    fields: ['id', 'name', 'value'],
    proxy: {
        type: 'localstorage',
        id: 'settinglocal'
    }

});