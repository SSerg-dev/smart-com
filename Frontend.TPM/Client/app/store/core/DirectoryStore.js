Ext.define('App.store.core.DirectoryStore', {
    extend: 'Ext.ux.data.ExtendedStore',
    alias: 'store.directorystore',
    autoLoad: true,
    buffered: true,
    trailingBufferZone: 50,
    leadingBufferZone: 50,
    remoteFilter: true,
    remoteSort: true,
    pageSize: 50,

    getById: function (id) {
        var result = (this.snapshot || this.data).findBy(function (record) {
            return record.get(record.idProperty) === id;
        });
        return result;
    }
});