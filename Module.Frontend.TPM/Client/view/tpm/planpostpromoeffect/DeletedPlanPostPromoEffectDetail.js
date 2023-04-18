Ext.define('App.view.tpm.cogs.DeletedCOGSDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedcogsdetail',
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
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'COGS').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'COGS').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'COGS').value('BrandTechName')
        }, {
            text: l10n.ns('tpm', 'PlanPostPromoEffect').value('Size'),
            dataIndex: 'Size'
        }, {
            text: l10n.ns('tpm', 'PlanPostPromoEffect').value('DiscountRangeName'),
            dataIndex: 'DiscountRangeName'
        }, {
            text: l10n.ns('tpm', 'PlanPostPromoEffect').value('DurationRangeName'),
            dataIndex: 'DurationRangeName'
        }, {
            text: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW1'),
            dataIndex: 'PlanPostPromoEffectW1'
        }, {
            text: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW2'),
            dataIndex: 'PlanPostPromoEffectW2'
        }]
    }
})
