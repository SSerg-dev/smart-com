Ext.define('App.controller.tpm.demand.HistoricalDemand', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaldemand directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaldemand directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaldemand #datatable': {
                    activate: this.onActivateCard
                },
                'historicaldemand #detailform': {
                    activate: this.onActivateCard
                },
                'historicaldemand #detail': {
                    click: this.onDetailButtonClick
                },
                'historicaldemand #table': {
                    click: this.onTableButtonClick
                },
                'historicaldemand #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaldemand #next': {
                    click: this.onNextButtonClick
                },
                'historicaldemand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaldemand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaldemand #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});