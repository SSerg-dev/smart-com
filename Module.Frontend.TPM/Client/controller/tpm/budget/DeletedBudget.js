Ext.define('App.controller.tpm.budget.DeletedBudget', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedbudget directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedbudget directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedbudget #datatable': {
                    activate: this.onActivateCard
                },
                'deletedbudget #detailform': {
                    activate: this.onActivateCard
                },
                'deletedbudget #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedbudget #table': {
                    click: this.onTableButtonClick
                },
                'deletedbudget #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedbudget #next': {
                    click: this.onNextButtonClick
                },
                'deletedbudget #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedbudget #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedbudget #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedbudget #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
