Ext.define('App.view.tpm.budgetitem.HistoricalBudgetItemDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalbudgetitemdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetItem').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetItem').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetItem').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBudgetItem', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBudgetItem').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetName',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('BudgetName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ButtonColor',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('ButtonColor'),
        }]
    }
});
