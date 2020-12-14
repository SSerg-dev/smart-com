Ext.define('App.view.core.associateduser.dbuser.HistoricalAssociatedUserEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalassociatedusereditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalAssociatedUser', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUser').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Email',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Email'),
        }]
    }
});
