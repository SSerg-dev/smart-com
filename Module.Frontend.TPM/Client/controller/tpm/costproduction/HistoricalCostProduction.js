Ext.define('App.controller.tpm.costproduction.HistoricalCostProduction', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcostproduction directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcostproduction directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcostproduction #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcostproduction #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcostproduction #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcostproduction #table': {
                    click: this.onTableButtonClick
                },
                'historicalcostproduction #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcostproduction #next': {
                    click: this.onNextButtonClick
                },
                'historicalcostproduction #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcostproduction #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcostproduction #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
