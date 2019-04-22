Ext.define('App.controller.tpm.budgetitem.HistoricalBudgetItem', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalbudgetitem directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalbudgetitem directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalbudgetitem #datatable': {
                    activate: this.onActivateCard
                },
                'historicalbudgetitem #detailform': {
                    activate: this.onActivateCard
                },
                'historicalbudgetitem #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalbudgetitem #table': {
                    click: this.onTableButtonClick
                },
                'historicalbudgetitem #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalbudgetitem #next': {
                    click: this.onNextButtonClick
                },
                'historicalbudgetitem #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalbudgetitem #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalbudgetitem #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
