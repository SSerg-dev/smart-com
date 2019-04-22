Ext.define('App.controller.tpm.product.HistoricalProduct', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalproduct directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalproduct directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalproduct #datatable': {
                    activate: this.onActivateCard
                },
                'historicalproduct #detailform': {
                    activate: this.onActivateCard
                },
                'historicalproduct #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalproduct #table': {
                    click: this.onTableButtonClick
                },
                'historicalproduct #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalproduct #next': {
                    click: this.onNextButtonClick
                },
                'historicalproduct #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalproduct #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalproduct #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
