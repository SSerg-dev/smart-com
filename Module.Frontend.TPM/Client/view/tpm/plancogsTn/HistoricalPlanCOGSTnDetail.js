Ext.define('App.view.tpm.plancogsTn.HistoricalPlanCOGSTnDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalplancogstndetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCOGS', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('StartDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('EndDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Volume',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanCOGSTn').value('Volume')
        }]
    }
});
