Ext.define('App.controller.tpm.btl.HistoricalBTL', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalbtl directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalbtl directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalbtl #datatable': {
                    activate: this.onActivateCard
                },
                'historicalbtl #detailform': {
                    activate: this.onActivateCard
                },
                'historicalbtl #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalbtl #table': {
                    click: this.onTableButtonClick
                },
                'historicalbtl #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalbtl #next': {
                    click: this.onNextButtonClick
                },
                'historicalbtl #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalbtl #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalbtl #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
