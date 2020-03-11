Ext.define('App.controller.core.interface.HistoricalInterface', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalinterface directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalinterface directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalinterface #datatable': {
                    activate: this.onActivateCard
                },
                'historicalinterface #detailform': {
                    activate: this.onActivateCard
                },
                'historicalinterface #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalinterface #table': {
                    click: this.onTableButtonClick
                },
                'historicalinterface #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalinterface #next': {
                    click: this.onNextButtonClick
                },
                'historicalinterface #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalinterface #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalinterface #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
