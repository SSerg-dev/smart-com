Ext.define('App.controller.tpm.storetype.HistoricalStoreType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalstoretype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalstoretype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalstoretype #datatable': {
                    activate: this.onActivateCard
                },
                'historicalstoretype #detailform': {
                    activate: this.onActivateCard
                },
                'historicalstoretype #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalstoretype #table': {
                    click: this.onTableButtonClick
                },
                'historicalstoretype #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalstoretype #next': {
                    click: this.onNextButtonClick
                },
                'historicalstoretype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalstoretype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalstoretype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
