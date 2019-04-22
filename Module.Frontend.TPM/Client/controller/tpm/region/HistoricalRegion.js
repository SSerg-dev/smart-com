Ext.define('App.controller.tpm.region.HistoricalRegion', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalregion directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalregion directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalregion #datatable': {
                    activate: this.onActivateCard
                },
                'historicalregion #detailform': {
                    activate: this.onActivateCard
                },
                'historicalregion #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalregion #table': {
                    click: this.onTableButtonClick
                },
                'historicalregion #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalregion #next': {
                    click: this.onNextButtonClick
                },
                'historicalregion #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalregion #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalregion #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
