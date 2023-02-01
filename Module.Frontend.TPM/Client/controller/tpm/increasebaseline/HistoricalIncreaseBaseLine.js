Ext.define('App.controller.tpm.increasebaseline.HistoricalIncreaseBaseLine', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalincreasebaseline directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalincreasebaseline directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalincreasebaseline #datatable': {
                    activate: this.onActivateCard
                },
                'historicalincreasebaseline #detailform': {
                    activate: this.onActivateCard
                },
                'historicalincreasebaseline #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalincreasebaseline #table': {
                    click: this.onTableButtonClick
                },
                'historicalincreasebaseline #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalincreasebaseline #next': {
                    click: this.onNextButtonClick
                },
                'historicalincreasebaseline #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalincreasebaseline #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalincreasebaseline #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
