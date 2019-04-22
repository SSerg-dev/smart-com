Ext.define('App.view.core.associatedmailnotificationsetting.AssociatedMailNotificationSettingContainer', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedmailnotificationsettingcontainer',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'associatedmailnotificationsettingmailnotificationsetting',
        margin: '10 0 0 20',
        minHeight: 150,
		linkConfig: {
            'associatedmailnotificationsettingrecipient': {
                masterField: 'Id',
                detailField: 'MailNotificationSettingId'
            }
		}
    }, {
        flex: 0,
        xtype: 'splitter',
        cls: 'associated-splitter'
    }, {
        xtype: 'associatedmailnotificationsettingrecipient',
        margin: '0 0 20 20',
        minHeight: 150,
        suppressSelection: true
	}]

});