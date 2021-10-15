Ext.define('App.controller.tpm.competitor.HistoricalCompetitor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcompetitor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcompetitor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcompetitor #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcompetitor #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcompetitor #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcompetitor #table': {
                    click: this.onTableButtonClick
                },
                'historicalcompetitor #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcompetitor #next': {
                    click: this.onNextButtonClick
                },
                'historicalcompetitor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcompetitor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcompetitor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
