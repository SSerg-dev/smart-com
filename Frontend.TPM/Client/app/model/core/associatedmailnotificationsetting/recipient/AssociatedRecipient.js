Ext.define('App.model.core.associatedmailnotificationsetting.recipient.AssociatedRecipient', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Recipient',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'MailNotificationSettingId', hidden: true },
		{ name: 'Type', type: 'string', isDefault: true },
		{ name: 'Value', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'Recipients',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});