Ext.define('App.controller.tpm.baseline.HistoricalBaseLine', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalbaseline directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalbaseline directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalbaseline #datatable': {
                    activate: this.onActivateCard
                },
                'historicalbaseline #detailform': {
                    activate: this.onActivateCard
                },
                'historicalbaseline #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalbaseline #table': {
                    click: this.onTableButtonClick
                },
                'historicalbaseline #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalbaseline #next': {
                    click: this.onNextButtonClick
                },
                'historicalbaseline #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalbaseline #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalbaseline #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
