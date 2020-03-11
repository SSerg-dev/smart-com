Ext.define('App.store.core.AssociatedDirectoryStore', {
    extend: 'App.store.core.DirectoryStore',
    alias: 'store.associateddirectorystore',
    autoLoad: false,

    listeners: {
        beforeload: function (store) {
            if (Ext.isEmpty(store.fixedFilters)) { //TODO: добавить проверку на конкретное имя фильтра?
                return false;
            }
        }
    }

});