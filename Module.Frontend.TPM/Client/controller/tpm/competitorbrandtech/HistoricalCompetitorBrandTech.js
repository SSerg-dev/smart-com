Ext.define('App.controller.tpm.competitorbrandtech.HistoricalCompetitorBrandTech', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcompetitorbrandtech directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcompetitorbrandtech directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcompetitorbrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcompetitorbrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcompetitorbrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcompetitorbrandtech #table': {
                    click: this.onTableButtonClick
                },
                'historicalcompetitorbrandtech #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcompetitorbrandtech #next': {
                    click: this.onNextButtonClick
                },
                'historicalcompetitorbrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcompetitorbrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcompetitorbrandtech #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
