Ext.define('App.model.tpm.budget.DeletedBudget', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Budget',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedBudgets',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
