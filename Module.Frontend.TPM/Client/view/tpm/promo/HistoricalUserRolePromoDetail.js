Ext.define('App.view.tpm.promo.HistoricalUserRolePromoDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicaluserrolepromodetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalUserRolePromo').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalUserRolePromo').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalUserRolePromo').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalUserRolePromo', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalUserRolePromo').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreatorLogin',
            fieldLabel: l10n.ns('tpm', 'UserRolePromo').value('Name'),
        }]
    }
});
