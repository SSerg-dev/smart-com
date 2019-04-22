Ext.define('App.view.tpm.tradeinvestment.HistoricalTradeInvestmentDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicaltradeinvestmentdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalTradeInvestment', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('StartDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('EndDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TIType',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('TIType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TISubType',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('TISubType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SizePercent',
            fieldLabel: l10n.ns('tpm', 'HistoricalTradeInvestment').value('SizePercent')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarcCalcROI',
            renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('MarcCalcROI')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarcCalcBudgets',
            renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('MarcCalcBudgets')
        }]
    }
});