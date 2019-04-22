Ext.define('App.controller.tpm.program.HistoricalProgram', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalprogram directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalprogram directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalprogram #datatable': {
                    activate: this.onActivateCard
                },
                'historicalprogram #detailform': {
                    activate: this.onActivateCard
                },
                'historicalprogram #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalprogram #table': {
                    click: this.onTableButtonClick
                },
                'historicalprogram #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalprogram #next': {
                    click: this.onNextButtonClick
                },
                'historicalprogram #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalprogram #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalprogram #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
