﻿Ext.define('App.view.tpm.increasebaseline.HistoricalIncreaseBaseLineDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalincreasebaselinedetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalIncreaseBaseLine').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalIncreaseBaseLine').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalIncreaseBaseLine').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalIncreaseBaseLine', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalIncreaseBaseLine').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('ClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InputBaselineQTY',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('InputBaselineQTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SellInBaselineQTY',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('SellInBaselineQTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SellOutBaselineQTY',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('SellOutBaselineQTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('Type'),
        }]
    }
});
