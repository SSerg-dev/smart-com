Ext.define('App.view.tpm.budgetitem.DeletedBudgetItemDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedbudgetitemdetail',
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
            name: 'BudgetName',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('BudgetName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('Description_ru'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ButtonColor',
            fieldLabel: l10n.ns('tpm', 'BudgetItem').value('ButtonColor'),
        }]
    }
})