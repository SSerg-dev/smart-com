Ext.define('App.controller.tpm.budget.HistoricalBudget', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalbudget directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalbudget directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalbudget #datatable': {
                    activate: this.onActivateCard
                },
                'historicalbudget #detailform': {
                    activate: this.onActivateCard
                },
                'historicalbudget #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalbudget #table': {
                    click: this.onTableButtonClick
                },
                'historicalbudget #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalbudget #next': {
                    click: this.onNextButtonClick
                },
                'historicalbudget #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalbudget #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalbudget #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
