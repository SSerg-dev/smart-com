Ext.define('App.view.tpm.nonpromosupport.HistoricalNonPromoSupportDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalnonpromosupportdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBudgetSubItem', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetSubItem').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanQuantity',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('PlanQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualQuantity',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('ActualQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanCostTE',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('PlanCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualCostTE',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('ActualCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'AttachFileName',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('AttachFileName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InvoiceNumber',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('InvoiceNumber'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechName',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('BrandTechName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'NonPromoEquipmentEquipmentType',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoSupport').value('NonPromoEquipmentEquipmentType'),
        }]
    }
});
