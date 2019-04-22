Ext.define('App.view.tpm.promodemand.DeletedPromoDemandDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedpromodemanddetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandName',
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
})
