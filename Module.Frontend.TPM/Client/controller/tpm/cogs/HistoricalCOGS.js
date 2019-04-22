Ext.define('App.controller.tpm.cogs.HistoricalCOGS', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcogs directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcogs directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcogs #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcogs #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcogs #detail': {
                    click: this.switchToDetailForm
                },
                'historicalcogs #table': {
                    click: this.onTableButtonClick
                },
                'historicalcogs #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcogs #next': {
                    click: this.onNextButtonClick
                },
                'historicalcogs #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcogs #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcogs #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
