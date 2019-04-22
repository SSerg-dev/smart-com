Ext.define('App.controller.tpm.rejectreason.HistoricalRejectReason', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalrejectreason directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalrejectreason directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalrejectreason #datatable': {
                    activate: this.onActivateCard
                },
                'historicalrejectreason #detailform': {
                    activate: this.onActivateCard
                },
                'historicalrejectreason #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalrejectreason #table': {
                    click: this.onTableButtonClick
                },
                'historicalrejectreason #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalrejectreason #next': {
                    click: this.onNextButtonClick
                },
                'historicalrejectreason #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalrejectreason #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalrejectreason #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
