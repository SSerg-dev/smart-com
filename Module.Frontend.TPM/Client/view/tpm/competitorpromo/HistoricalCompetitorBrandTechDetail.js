Ext.define('App.view.tpm.competitorbrandtech.HistoricalCompetitorBrandTechDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcompetitorbrandtechdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCompetitorBrandTech', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitorBrandTech').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorName',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('CompetitorName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('BrandTech'),
        }]
    }
});