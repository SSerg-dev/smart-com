Ext.define('App.controller.tpm.budgetsubitem.DeletedBudgetSubItem', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedbudgetsubitem directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedbudgetsubitem directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedbudgetsubitem #datatable': {
                    activate: this.onActivateCard
                },
                'deletedbudgetsubitem #detailform': {
                    activate: this.onActivateCard
                },
                'deletedbudgetsubitem #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedbudgetsubitem #table': {
                    click: this.onTableButtonClick
                },
                'deletedbudgetsubitem #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedbudgetsubitem #next': {
                    click: this.onNextButtonClick
                },
                'deletedbudgetsubitem #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedbudgetsubitem #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedbudgetsubitem #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedbudgetsubitem #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
