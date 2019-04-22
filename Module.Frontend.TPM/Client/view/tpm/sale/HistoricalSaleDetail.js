Ext.define('App.view.tpm.sale.HistoricalSaleDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalsaledetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalSale').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalSale').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalSale').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalSale', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalSale').value('_Operation')
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
});
