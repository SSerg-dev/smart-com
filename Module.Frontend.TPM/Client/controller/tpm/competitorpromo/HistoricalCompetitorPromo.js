Ext.define('App.controller.tpm.competitorpromo.HistoricalCompetitorPromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcompetitorpromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcompetitorpromo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcompetitorpromo #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcompetitorpromo #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcompetitorpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcompetitorpromo #table': {
                    click: this.onTableButtonClick
                },
                'historicalcompetitorpromo #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcompetitorpromo #next': {
                    click: this.onNextButtonClick
                },
                'historicalcompetitorpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcompetitorpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcompetitorpromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
