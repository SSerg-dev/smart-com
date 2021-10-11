Ext.define('App.view.tpm.calendarcompetitorbrandtechcolor.HistoricalCalendarCompetitorBrandTechColorDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcalendarcompetitorbrandtechcolordetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorBrandTechColor').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorBrandTechColor').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorBrandTechColor').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalCalendarCompetitorBrandTechColor', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalCalendarCompetitorBrandTechColor').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Color',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('Color'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CalendarCompetitorCompanyCompanyName',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('CompanyName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('BrandTech'),
        }]
    }
});