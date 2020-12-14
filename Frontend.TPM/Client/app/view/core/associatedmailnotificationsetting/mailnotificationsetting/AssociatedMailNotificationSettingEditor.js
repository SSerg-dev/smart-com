Ext.define('App.view.core.associatedmailnotificationsetting.mailnotificationsetting.AssociatedMailNotificationSettingEditor', {
	extend: 'App.view.core.common.EditorDetailWindow',
	alias: 'widget.associatedmailnotificationsettingeditor',
	width: 800,
	minWidth: 800,
	maxHeight: 500,
	cls: 'readOnlyFields',

	items: {
		xtype: 'editorform',
		columnsCount: 2,
		items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Description')
        }, {
            xtype: 'textfield',
            name: 'Subject',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Subject')
        }, {
            xtype: 'textfield',
            name: 'Body',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('Body')
        }, {
            xtype: 'checkboxfield',
            name: 'IsDisabled',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('IsDisabled')
        }, {
            xtype: 'textfield',
            name: 'To',
            vtype: 'extendEmail',
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('To')
        }, {
            xtype: 'textfield',
            name: 'CC',
            vtype: 'extendEmail',
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('CC')
        }, {
            xtype: 'textfield',
            name: 'BCC',
            vtype: 'extendEmail',
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('core', 'AssociatedMailNotificationSetting').value('BCC')
        }]
	}
});