Ext.define('App.model.tpm.budgetsubitem.BudgetSubItemWithFilter', {
    extend: 'App.model.tpm.budgetsubitem.BudgetSubItem',
    proxy: {
        type: 'breeze',
        resourceName: 'BudgetSubItems',
        action: 'BudgetSubItems',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            ClientTreeId: 0,
            BudgetItemId: null
        }
    }
});
