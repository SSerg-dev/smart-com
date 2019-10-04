Ext.define('App.view.tpm.budgetsubitem.AssociatedBudgetSubItemClientTree', {
    extend: 'App.view.core.common.AssociatedDirectoryView',
    alias: 'widget.associatedbudgetsubitemclienttree',

    defaults: {
        flex: 1,
        margin: '0 0 0 20'
    },

    items: [{
        xtype: 'budgetsubitem',
        margin: '10 0 0 20',
        minHeight: 150,
        suppressSelection: true,
		linkConfig: {
            'budgetsubitemclienttree': {
                masterField: 'Id',
                detailField: 'BudgetSubItemId'
            }
		}
    }, {
        flex: 0,
        xtype: 'splitter',
        cls: 'associated-splitter'
    }, {
        xtype: 'budgetsubitemclienttree',
        margin: '0 0 20 20',
        minHeight: 150,
        autoLoad: false
	}]
});