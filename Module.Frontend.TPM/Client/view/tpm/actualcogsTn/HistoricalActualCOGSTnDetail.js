Ext.define('App.view.tpm.actualcogsTn.HistoricalActualCOGSTnDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalactualcogstndetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalActualCOGS', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('StartDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('EndDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Volume',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualCOGSTn').value('Volume')
        }]
    }
});
