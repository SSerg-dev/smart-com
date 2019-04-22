﻿Ext.define('App.view.tpm.demand.HistoricalDemandDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicaldemanddetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalDemand', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalDemand').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Number'),
            name: 'Number'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientCommercialSubnetCommercialNetName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesStart',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesStart'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesEnd'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanBaseline',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanBaseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanDuration',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanDuration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanUplift',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanUplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanIncremental',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanIncremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanActivity',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanActivity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanSteal',
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanSteal'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactBaseline',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactBaseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactDuration',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactDuration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactUplift',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactUplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactIncremental',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactIncremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactActivity',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactActivity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FactSteal',
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactSteal'),
        }]
    }
});
