Ext.define('App.controller.tpm.mechanictype.HistoricalMechanicType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalmechanictype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalmechanictype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalmechanictype #datatable': {
                    activate: this.onActivateCard
                },
                'historicalmechanictype #detailform': {
                    activate: this.onActivateCard
                },
                'historicalmechanictype #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalmechanictype #table': {
                    click: this.onTableButtonClick
                },
                'historicalmechanictype #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalmechanictype #next': {
                    click: this.onNextButtonClick
                },
                'historicalmechanictype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalmechanictype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalmechanictype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
