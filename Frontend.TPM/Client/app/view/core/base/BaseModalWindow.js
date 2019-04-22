Ext.define('App.view.core.base.BaseModalWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.basewindow',
    constrain: true,
    ghost: false,
    modal: true,
    layout: 'fit',

    defaults: {
        margin: '10 15 15 15'
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        //itemId: 'cancel'
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'ok'
    }]
});