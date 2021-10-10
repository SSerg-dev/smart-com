Ext.define('App.controller.tpm.calendarcompetitorcompany.DeletedCalendarCompetitorCompany', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcalendarcompetitorcompany directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcalendarcompetitorcompany directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcalendarcompetitorcompany #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcalendarcompetitorcompany #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcalendarcompetitorcompany #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcalendarcompetitorcompany #table': {
                    click: this.onTableButtonClick
                },
                'deletedcalendarcompetitorcompany #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcalendarcompetitorcompany #next': {
                    click: this.onNextButtonClick
                },
                'deletedcalendarcompetitorcompany #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcalendarcompetitorcompany #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcalendarcompetitorcompany #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcalendarcompetitorcompany #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
