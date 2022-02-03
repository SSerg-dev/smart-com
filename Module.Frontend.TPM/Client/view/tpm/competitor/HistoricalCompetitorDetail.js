Ext.define('App.view.tpm.competitor.HistoricalCompetitorDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcompetitordetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitor').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitor').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitor').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCompetitor', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitor').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'HistoricalCompetitor').value('Name'),
        }]
    }
});
