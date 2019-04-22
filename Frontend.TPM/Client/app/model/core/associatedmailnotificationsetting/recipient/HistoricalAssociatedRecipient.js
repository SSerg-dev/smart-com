Ext.define('App.model.core.associatedmailnotificationsetting.recipient.HistoricalAssociatedRecipient', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalRecipient',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
		{ name: '_EditDate', type: 'date', isDefault: true },
		{ name: '_Operation', type: 'string', isDefault: true },
		{ name: 'MailNotificationSettingId', hidden: true },
		{ name: 'Type', type: 'string', isDefault: true },
		{ name: 'Value', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalRecipients',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});