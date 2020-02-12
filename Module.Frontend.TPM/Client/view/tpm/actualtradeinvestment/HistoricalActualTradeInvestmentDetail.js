Ext.define('App.view.tpm.actualtradeinvestment.HistoricalActualTradeInvestmentDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalactualtradeinvestmentdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalActualTradeInvestment', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('StartDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('EndDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TIType',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('TIType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TISubType',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('TISubType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SizePercent',
            fieldLabel: l10n.ns('tpm', 'HistoricalActualTradeInvestment').value('SizePercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarcCalcROI',
            renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcROI')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarcCalcBudgets',
            renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcBudgets')
        }]
    }
});