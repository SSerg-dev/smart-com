Ext.define('App.view.core.security.SecurityChangeRoleWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.changerolewindow',
    title: l10n.ns('core').value('changeRoleWindowTitle'),

    width: 250,
    height: 450,
    minWidth: 250,
    minHeight: 350,

    items: [{
        xtype: 'rolesview',
        autoScroll: true,
        store: {
            type: 'currentuserrolesstore',
            storeId: 'currentuserrolesstore',
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