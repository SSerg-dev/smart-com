Ext.define('App.controller.tpm.mechanic.HistoricalMechanic', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalmechanic directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalmechanic directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalmechanic #datatable': {
                    activate: this.onActivateCard
                },
                'historicalmechanic #detailform': {
                    activate: this.onActivateCard
                },
                'historicalmechanic #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalmechanic #table': {
                    click: this.onTableButtonClick
                },
                'historicalmechanic #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalmechanic #next': {
                    click: this.onNextButtonClick
                },
                'historicalmechanic #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalmechanic #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalmechanic #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
