Ext.define('App.view.core.common.PasswordChangingWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.passwordchangingwindow',
    title: l10n.ns('core').value('passwordChangingWindowTitle'),
    resizable: false,
    width: 350,

    items: [{
        xtype: 'editorform',
        items: [{
            xtype: 'passwordfield',
            name: 'Password',
            allowBlank: false,
            allowOnlyWhitespace: false
        }]
    }]

});