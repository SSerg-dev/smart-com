Ext.define('App.model.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSetting', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'MailNotificationSetting',
    fields: [
		{ name: 'Id', hidden: true },
		{ name: 'Name', type: 'string', isDefault: true },
		{ name: 'Description', type: 'string', isDefault: true },
		{ name: 'Subject', type: 'string', isDefault: true },
		{ name: 'Body', type: 'string', isDefault: true },
		{ name: 'IsDisabled', type: 'boolean', isDefault: true },
		{ name: 'To', type: 'string', isDefault: true },
		{ name: 'CC', type: 'string', isDefault: true },
		{ name: 'BCC', type: 'string', isDefault: true }
	],
    proxy: {
        type: 'breeze',
        resourceName: 'MailNotificationSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});