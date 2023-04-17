Ext.define('App.view.tpm.planpostpromoeffect.HistoricalPlanPostPromoEffectDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalplanpostpromoeffectdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanPostPromoEffect').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPlanPostPromoEffect', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('BrandTechName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanPostPromoEffect').value('Size')
        }, {
            xtype: 'singlelinedisplayfield',
            text: l10n.ns('tpm', 'PlanPostPromoEffect').value('DiscountRangeName'),
            name: 'DiscountRangeName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DurationRangeName',
            fieldLabel: l10n.ns('tpm', 'HistoricalPlanPostPromoEffect').value('DurationRangeName')
        }, {
            xtype: 'numberfield',
            name: 'PlanPostPromoEffectW1',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW1')
        }, {
            xtype: 'numberfield',
            name: 'PlanPostPromoEffectW2',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW2')
        }]
    }
});
