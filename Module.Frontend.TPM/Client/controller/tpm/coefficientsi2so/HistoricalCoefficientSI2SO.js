Ext.define('App.controller.tpm.coefficientsi2so.HistoricalCoefficientSI2SO', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcoefficientsi2so directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcoefficientsi2so directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcoefficientsi2so #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcoefficientsi2so #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcoefficientsi2so #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcoefficientsi2so #table': {
                    click: this.onTableButtonClick
                },
                'historicalcoefficientsi2so #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcoefficientsi2so #next': {
                    click: this.onNextButtonClick
                },
                'historicalcoefficientsi2so #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcoefficientsi2so #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcoefficientsi2so #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
