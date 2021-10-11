Ext.define('App.controller.tpm.calendarcompetitorbrandtechcolor.DeletedCalendarCompetitorBrandTechColor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcalendarcompetitorbrandtechcolor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcalendarcompetitorbrandtechcolor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcalendarcompetitorbrandtechcolor #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcalendarcompetitorbrandtechcolor #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcalendarcompetitorbrandtechcolor #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcalendarcompetitorbrandtechcolor #table': {
                    click: this.onTableButtonClick
                },
                'deletedcalendarcompetitorbrandtechcolor #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcalendarcompetitorbrandtechcolor #next': {
                    click: this.onNextButtonClick
                },
                'deletedcalendarcompetitorbrandtechcolor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcalendarcompetitorbrandtechcolor #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcalendarcompetitorbrandtechcolor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcalendarcompetitorbrandtechcolor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
