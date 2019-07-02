Ext.define('App.controller.tpm.incrementalpromo.HistoricalIncrementalPromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalincrementalpromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalincrementalpromo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalincrementalpromo #datatable': {
                    activate: this.onActivateCard
                },
                'historicalincrementalpromo #detailform': {
                    activate: this.onActivateCard
                },
                'historicalincrementalpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalincrementalpromo #table': {
                    click: this.onTableButtonClick
                },
                'historicalincrementalpromo #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalincrementalpromo #next': {
                    click: this.onNextButtonClick
                },
                'historicalincrementalpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalincrementalpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalincrementalpromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
