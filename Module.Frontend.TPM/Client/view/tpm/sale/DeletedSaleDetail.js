Ext.define('App.view.tpm.sale.DeletedSaleDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedsaledetail',
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
            name: 'BudgetItemBudgetName',
            fieldLabel: l10n.ns('tpm', 'Sale').value('BudgetItemBudgetName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetItemName',
            fieldLabel: l10n.ns('tpm', 'Sale').value('BudgetItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Plan',
            fieldLabel: l10n.ns('tpm', 'Sale').value('Plan'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Fact',
            fieldLabel: l10n.ns('tpm', 'Sale').value('Fact'),
        }]
    }
})