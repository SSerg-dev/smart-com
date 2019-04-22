Ext.define('App.controller.tpm.budgetsubitem.HistoricalBudgetSubItem', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalbudgetsubitem directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalbudgetsubitem directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalbudgetsubitem #datatable': {
                    activate: this.onActivateCard
                },
                'historicalbudgetsubitem #detailform': {
                    activate: this.onActivateCard
                },
                'historicalbudgetsubitem #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalbudgetsubitem #table': {
                    click: this.onTableButtonClick
                },
                'historicalbudgetsubitem #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalbudgetsubitem #next': {
                    click: this.onNextButtonClick
                },
                'historicalbudgetsubitem #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalbudgetsubitem #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalbudgetsubitem #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
