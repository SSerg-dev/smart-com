Ext.define('App.view.tpm.clienttreebrandtech.HistoricalClientTreeBrandTechDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalclienttreebrandtechdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalClientTreeBrandTech', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalClientTreeBrandTech').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'ParentClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ParentClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeObjectId'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'ClientTreeName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeName'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'CurrentBrandTechName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('CurrentBrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'Share',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('Share'),
        }]
    }
});
