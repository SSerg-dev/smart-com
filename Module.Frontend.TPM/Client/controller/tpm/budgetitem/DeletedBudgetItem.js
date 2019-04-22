Ext.define('App.controller.tpm.budgetitem.DeletedBudgetItem', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedbudgetitem directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedbudgetitem directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedbudgetitem #datatable': {
                    activate: this.onActivateCard
                },
                'deletedbudgetitem #detailform': {
                    activate: this.onActivateCard
                },
                'deletedbudgetitem #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedbudgetitem #table': {
                    click: this.onTableButtonClick
                },
                'deletedbudgetitem #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedbudgetitem #next': {
                    click: this.onNextButtonClick
                },
                'deletedbudgetitem #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedbudgetitem #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedbudgetitem #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedbudgetitem #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
