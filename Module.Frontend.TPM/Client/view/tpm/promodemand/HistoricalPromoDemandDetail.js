Ext.define('App.view.tpm.promodemand.HistoricalPromoDemandDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalpromodemanddetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoDemand').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoDemand').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoDemand').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromoDemand', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoDemand').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechBrandName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Account',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Account'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BaseClientObjectId',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('BaseClientObjectId'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicTypeName',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('MechanicTypeName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Discount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Week',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Week'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Baseline',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Baseline'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Uplift',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Uplift'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Incremental',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Incremental'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Activity',
            fieldLabel: l10n.ns('tpm', 'PromoDemand').value('Activity'),
        }]
    }
});
