Ext.define('App.controller.tpm.variety.HistoricalVariety', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalvariety directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalvariety directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalvariety #datatable': {
                    activate: this.onActivateCard
                },
                'historicalvariety #detailform': {
                    activate: this.onActivateCard
                },
                'historicalvariety #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalvariety #table': {
                    click: this.onTableButtonClick
                },
                'historicalvariety #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalvariety #next': {
                    click: this.onNextButtonClick
                },
                'historicalvariety #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalvariety #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalvariety #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
