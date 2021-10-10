Ext.define('App.controller.tpm.calendarcompetitorcompany.HistoricalCalendarCompetitorCompany', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcalendarcompetitorcompany directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcalendarcompetitorcompany directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcalendarcompetitorcompany #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcalendarcompetitorcompany #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcalendarcompetitorcompany #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcalendarcompetitorcompany #table': {
                    click: this.onTableButtonClick
                },
                'historicalcalendarcompetitorcompany #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcalendarcompetitorcompany #next': {
                    click: this.onNextButtonClick
                },
                'historicalcalendarcompetitorcompany #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcalendarcompetitorcompany #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcalendarcompetitorcompany #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
