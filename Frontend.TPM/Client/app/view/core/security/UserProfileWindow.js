Ext.define('App.view.core.security.UserProfileWindow', {
    extend: 'App.view.core.base.BaseReviewWindow',
    alias: 'widget.userprofilewindow',
    title: l10n.ns('core').value('passwordChangingWindowTitle'),
    width: 250,
    height: 300,
    minWidth: 250,
    minHeight: 300,
    listeners: {
        afterrender: function (window) {
            var roleText = l10n.ns('core', 'userwindow').value('role') + App.UserInfo.getCurrentRole().DisplayName;
            var loginText = l10n.ns('core', 'userwindow').value('login') + App.UserInfo.getUserName();
            window.down('label[name=login]').setText(loginText);
            window.down('label[name=role]').setText(roleText);
        }
    },
    items: [{
        xtype: 'panel',
        frame: true,
        ui: 'light-gray-panel',
        bodyPadding: '10 0 0 10',
        glyph: 0xf004,
        layout: {
            type: 'vbox',
            align: 'center'
        },
        items: [
            {
                xtype: 'securitybutton',
                glyph: 0xf004,
                cls: 'user-window-icon',
            }, {
                xtype: 'label',
                name: 'role',
                cls: 'user-window-text',
            }, {
                xtype: 'label',
                name: 'login',
                cls: 'user-window-text',
            }, {
                xtype: 'button',
                text: l10n.ns('core', 'userwindow').value('passwordButton'),
                ui: 'white-button-footer-toolbar',
                itemId: 'changepassword',
                width: 135,
                margin: 10
            }]
    }]
});