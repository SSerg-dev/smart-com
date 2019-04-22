Ext.define('App.view.tpm.main.notification.Notification', {
    alias: 'widget.notification',
    extend: 'Ext.container.Container',
    cls: 'notification-container',

    items: [{
        xtype: 'panel',
        itemId: 'panelForNotifications',
        hidden: false,
        cls: 'panel-for-notifications',

        tools: [{
            type: 'close',
            tooltop: 'Close the notification panel.',
            cls: 'panel-for-notifications-tools-close',

            handler: function (panel, tool, event) {
                var panelForNotifications = Ext.ComponentQuery.query('#panelForNotifications')[0];
                panelForNotifications.hide();
            }
        }],

        items: [{
            xtype: 'container',
            cls: 'notification-node',
            html: '<a>' + l10n.ns('tpm', 'Notification').value('SchedulerCreatePromoMessageFirstPart') + '</a>' +
                '<span class="mdi mdi-calendar-plus mdi-18px scheduler-create-promo-notification-icon"></span>' + '<strong>Create Promo</strong>' +
                '<a>' + l10n.ns('tpm', 'Notification').value('SchedulerCreatePromoMessageSecondPart') + '</a>'
        }]
    }, {
        xtype: 'button',
        itemId: 'notificationButton',
        text: 'Notifications',
        glyph: 0xf09f,
        scale: 'small',
        cls: 'notification-button',

        listeners: {
            click: function (button) {
                var panelForNotifications = button.up('notification').down('#panelForNotifications');
                panelForNotifications.hidden ? panelForNotifications.show() : panelForNotifications.hide();
            }
        }
    }]
});