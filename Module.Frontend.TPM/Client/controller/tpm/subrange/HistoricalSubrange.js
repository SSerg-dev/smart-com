Ext.define('App.controller.tpm.subrange.HistoricalSubrange', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalsubrange directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalsubrange directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalsubrange #datatable': {
                    activate: this.onActivateCard
                },
                'historicalsubrange #detailform': {
                    activate: this.onActivateCard
                },
                'historicalsubrange #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalsubrange #table': {
                    click: this.onTableButtonClick
                },
                'historicalsubrange #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalsubrange #next': {
                    click: this.onNextButtonClick
                },
                'historicalsubrange #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalsubrange #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalsubrange #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
