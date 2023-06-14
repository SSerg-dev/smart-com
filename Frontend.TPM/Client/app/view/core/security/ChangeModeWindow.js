Ext.define('App.view.core.security.ChangeModeWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.changemodewindow',
    title: 'Change Mode',

    width: 250,
    height: 450,
    minWidth: 250,
    minHeight: 350,

    items: [{
        xtype: 'modesview',
        autoScroll: true,
        store: {
            type: 'modestore',
            storeId: 'modestore',
            autoLoad: true
        }
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'ok',
        disabled: true
    }]

});