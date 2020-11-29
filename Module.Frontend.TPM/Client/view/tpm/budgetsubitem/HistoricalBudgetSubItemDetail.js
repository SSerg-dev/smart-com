Ext.define('App.view.tpm.budgetsubitem.HistoricalBudgetSubItemDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalbudgetsubitemdetail',
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
            name: 'BudgetItemBudgetName',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetItemName',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('BudgetItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'BudgetSubItem').value('Description_ru'),
        }]
    }
});
