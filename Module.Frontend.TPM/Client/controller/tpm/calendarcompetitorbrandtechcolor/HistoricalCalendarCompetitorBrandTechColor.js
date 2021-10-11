Ext.define('App.controller.tpm.calendarcompetitorbrandtechcolor.HistoricalCalendarCompetitorBrandTechColor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcalendarcompetitorbrandtechcolor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcalendarcompetitorbrandtechcolor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcalendarcompetitorbrandtechcolor #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcalendarcompetitorbrandtechcolor #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcalendarcompetitorbrandtechcolor #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcalendarcompetitorbrandtechcolor #table': {
                    click: this.onTableButtonClick
                },
                'historicalcalendarcompetitorbrandtechcolor #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcalendarcompetitorbrandtechcolor #next': {
                    click: this.onNextButtonClick
                },
                'historicalcalendarcompetitorbrandtechcolor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcalendarcompetitorbrandtechcolor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcalendarcompetitorbrandtechcolor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
