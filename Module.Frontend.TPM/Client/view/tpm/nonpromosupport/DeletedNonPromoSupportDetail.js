Ext.define('App.view.tpm.nonpromosupport.DeletedNonPromoSupportDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletednonpromosupportdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'Number',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('Number')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'NonPromoEquipmentEquipmentType',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('NonPromoEquipmentEquipmentType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanQuantity',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('PlanQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualQuantity',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('ActualQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanCostTE',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('PlanCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualCostTE',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('ActualCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'NonPromoSupport').value('EndDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }]
    }
})