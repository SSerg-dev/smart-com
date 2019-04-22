Ext.define('App.view.core.security.LogInWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.loginwindow',
    title: l10n.ns('core', 'logIn').value('title'),
    layout: 'fit',
    closable: false,
    minWidth: 400,
    width: 550,
    resizeHandles: 'w e',

    items: [{
        xtype: 'form',
        frame: true,
        ui: 'light-gray-panel',
        bodyPadding: '10 10 0 10',

        layout: {
            type: 'vbox',
            align: 'stretch'
        },

        defaults: {
            xtype: 'textfield',
            labelAlign: 'left',
            labelWidth: 120,
            labelSeparator: '',
            allowBlank: false
        },

        items: [{
            fieldLabel: l10n.ns('core', 'logIn').value('login'),
            name: 'login'
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('core', 'logIn').value('password'),
            name: 'password',
            inputType: 'password'
        }]
    }],

    buttons: [{
        text: l10n.ns('core', 'logIn').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'ok'
    }]
});