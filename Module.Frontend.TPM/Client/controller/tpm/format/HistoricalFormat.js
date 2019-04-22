Ext.define('App.controller.tpm.format.HistoricalFormat', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalformat directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalformat directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalformat #datatable': {
                    activate: this.onActivateCard
                },
                'historicalformat #detailform': {
                    activate: this.onActivateCard
                },
                'historicalformat #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalformat #table': {
                    click: this.onTableButtonClick
                },
                'historicalformat #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalformat #next': {
                    click: this.onNextButtonClick
                },
                'historicalformat #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalformat #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalformat #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
