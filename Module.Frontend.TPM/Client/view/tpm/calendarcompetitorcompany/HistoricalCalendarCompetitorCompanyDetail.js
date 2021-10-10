Ext.define('App.view.tpm.calendarcompetitorcompany.HistoricalCalendarCompetitorCompanyDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcalendarcompetitorcompanydetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorCompany').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorCompany').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorCompany').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCalendarCompetitorCompany', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorCompany').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompanyName',
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorCompany').value('CompanyName'),
        }]
    }
});
