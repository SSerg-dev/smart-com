Ext.define('App.model.tpm.budget.Budget', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Budget',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Budgets',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
