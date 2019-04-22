Ext.define('App.controller.tpm.color.HistoricalColor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcolor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcolor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcolor #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcolor #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcolor #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcolor #table': {
                    click: this.onTableButtonClick
                },
                'historicalcolor #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcolor #next': {
                    click: this.onNextButtonClick
                },
                'historicalcolor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcolor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcolor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
