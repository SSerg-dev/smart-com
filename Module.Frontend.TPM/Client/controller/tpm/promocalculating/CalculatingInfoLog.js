Ext.define('App.controller.tpm.promocalculating.CalculatingInfoLog', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'calculatinginfolog[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick
                },
                'calculatinginfologeditor': {
                    afterrender: this.onCalculatingInfoLogEditorAfterRender
                },
                'calculatinginfolog button[name=logError]': {
                    toggle: this.onLogErrorToggle
                },
                'calculatinginfolog button[name=logWarning]': {
                    toggle: this.onLogWarningToggle
                },
                'calculatinginfolog button[name=logMessage]': {
                    toggle: this.onLogMessageToggle
                }
            }
        });
    },

    onCalculatingInfoLogEditorAfterRender: function (window) {
        var record = Ext.ComponentQuery.query('calculatinginfolog')[0].down('grid').getSelectionModel().getSelection()[0];

        if (record.data.Type == 'WARNING') {
            window.down('[name=Type]').addCls('warningraw');
        } else if (record.data.Type == 'ERROR') {
            window.down('[name=Type]').addCls('errorraw');
        }
    },

    onLogErrorToggle: function (button) {
        this.filterLog(button);
    },

    onLogWarningToggle: function (button) {
        this.filterLog(button);
    },

    onLogMessageToggle: function (button) {
        this.filterLog(button);
    },


    filterLog: function (button) {
        var FilterList = [];
        if (button.pressed) {
            button.addCls('messageclicked');
            button.removeCls('messagenotclicked');
        }
        else {
            button.addCls('messagenotclicked');
            button.removeCls('messageclicked');
        }
        Ext.ComponentQuery.query('calculatinginfolog button[itemId!=collapse]').forEach(function (button) {
            if (button.pressed) {
                FilterList.push(button.filterText);
            }
        });
        var logFilter = new Ext.util.Filter({
            filterFn: function (item) { var i = FilterList.includes(item.data.Type); return i }
        })
        var store = Ext.ComponentQuery.query('calculatinginfolog grid')[0].getStore();
        store.clearFilter();
        store.filter(logFilter);
    }
});
