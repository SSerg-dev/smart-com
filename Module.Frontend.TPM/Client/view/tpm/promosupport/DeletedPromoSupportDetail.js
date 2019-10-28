Ext.define('App.view.tpm.promosupport.DeletedPromoSupportDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedpromosupportdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'Number', 
            fieldLabel: l10n.ns('tpm', 'PromoSupport').value('Number')
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'ClientTreeFullPathName',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('ClientTreeFullPathName'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'BudgetSubItemBudgetItemName',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('BudgetSubItemBudgetItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'BudgetSubItemName',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('BudgetSubItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'PlanQuantity',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('PlanQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'ActualQuantity',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('ActualQuantity'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'PlanCostTE',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('PlanCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'ActualCostTE',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('ActualCostTE'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'StartDate',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('StartDate'),

                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
                name: 'EndDate',
                fieldLabel: l10n.ns('tpm', 'PromoSupport').value('EndDate'),

                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }]
    }
})