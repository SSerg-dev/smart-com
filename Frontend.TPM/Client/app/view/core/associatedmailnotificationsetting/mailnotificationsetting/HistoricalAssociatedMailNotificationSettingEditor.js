Ext.define('App.view.core.associatedmailnotificationsetting.mailnotificationsetting.HistoricalAssociatedMailNotificationSettingEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalassociatedmailnotificationsettingeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalAssociatedMailNotificationSetting', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('_Operation')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'Name',
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Name')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'Description',
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Description')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'Subject',
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Subject')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'Body',
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('Body')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'IsDisabled',
                renderer: App.RenderHelper.getBooleanRenderer(),
                trueText: l10n.ns('core', 'booleanValues').value('true'),
                falseText: l10n.ns('core', 'booleanValues').value('false'),
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('IsDisabled')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'To',
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('To')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'CC',
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('CC')
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'BCC',
                fieldLabel: l10n.ns('core', 'HistoricalAssociatedMailNotificationSetting').value('BCC')
            }]
    }
});
