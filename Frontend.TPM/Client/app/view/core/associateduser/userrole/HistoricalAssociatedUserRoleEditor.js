Ext.define('App.view.core.associateduser.userrole.HistoricalAssociatedUserRoleEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalassociateduserroleeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUserRole').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUserRole').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUserRole').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('core.HistoricalAssociatedUserRole', 'OperationType'),
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUserRole').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'RoleDisplayName',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUserRole').value('RoleDisplayName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IsDefault',
            fieldLabel: l10n.ns('core', 'HistoricalAssociatedUserRole').value('IsDefault'),
        }]
    }
});
