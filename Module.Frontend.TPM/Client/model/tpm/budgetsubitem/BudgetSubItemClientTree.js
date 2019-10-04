Ext.define('App.model.tpm.budgetsubitem.BudgetSubItemClientTree', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BudgetSubItemClientTree',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ClientTreeId', hidden: true },
        { name: 'BudgetSubItemId', hidden: true },
        { name: 'ClientTreeFullPathName', type: 'string', isDefault: true, mapping: 'ClientTree.FullPathName', defaultFilterConfig: { valueField: 'FullPathName' } },
        { name: 'ClientTreeObjectId', type: 'int', isDefault: true, mapping: 'ClientTree.ObjectId', defaultFilterConfig: { valueField: 'ObjectId' } }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BudgetSubItemClientTrees',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
