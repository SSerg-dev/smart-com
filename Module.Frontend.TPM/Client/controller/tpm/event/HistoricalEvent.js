Ext.define('App.controller.tpm.event.HistoricalEvent', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalevent directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalevent directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalevent #datatable': {
                    activate: this.onActivateCard
                },
                'historicalevent #detailform': {
                    activate: this.onActivateCard
                },
                'historicalevent #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalevent #table': {
                    click: this.onTableButtonClick
                },
                'historicalevent #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalevent #next': {
                    click: this.onNextButtonClick
                },
                'historicalevent #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalevent #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalevent #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
