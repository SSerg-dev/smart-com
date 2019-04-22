Ext.define('App.util.core.NotifyService', {
    alternateClassName: 'App.Notify',
    singleton: true,

    pushError: function (message) {
        Ext.Msg.show({
            title: l10n.ns('core').value('errorTitle'),
            msg: message,
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.ERROR
        });
    },

    pushInfo: function (message) {
        Ext.Msg.show({
            title: l10n.ns('core').value('alertTitle'),
            msg: message,
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO
        });
    },

    pushMessage: function (_title, message) {
        Ext.Msg.show({
            title: _title,
            msg: message,
            buttons: Ext.MessageBox.OK,
            icon: Ext.Msg.INFO
        });
    }

});