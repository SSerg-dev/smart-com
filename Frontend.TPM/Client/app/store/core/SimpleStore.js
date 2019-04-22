Ext.define('App.store.core.SimpleStore', {
    extend: 'Ext.ux.data.ExtendedStore',
    alias: 'store.simplestore',
    autoLoad: true,
    remoteFilter: true,
    remoteSort: true,
    pageSize: null,
    defaultPageSize: null,

    getById: function (id) {
        var result = (this.snapshot || this.data).findBy(function (record) {
            return record.get(record.idProperty) === id;
        });
        return result;
    }
});