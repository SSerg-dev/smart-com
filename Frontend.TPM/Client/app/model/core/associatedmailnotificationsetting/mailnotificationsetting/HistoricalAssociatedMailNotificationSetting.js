Ext.define('App.model.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSetting', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'HistoricalMailNotificationSetting',
    fields: [
		{ name: '_Id', type: 'string', hidden: true },
		{ name: '_ObjectId', hidden: true },
		{ name: '_User', type: 'string', isDefault: true },
		{ name: '_Role', type: 'string', isDefault: true },
		{ name: '_EditDate', type: 'date', isDefault: true },
		{ name: '_Operation', type: 'string', isDefault: true },
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
        resourceName: 'HistoricalMailNotificationSettings',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});