﻿Ext.define('App.view.tpm.tradeinvestment.DeletedTradeInvestmentDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedtradeinvestmentdetail',
    width: 500,
    minWidth: 500,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('StartDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('EndDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TIType',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('TIType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TISubType',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('TISubType')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SizePercent',
            fieldLabel: l10n.ns('tpm', 'TradeInvestment').value('SizePercent')
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
})