Ext.define('App.controller.tpm.sale.HistoricalSale', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalsale directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalsale directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalsale #datatable': {
                    activate: this.onActivateCard
                },
                'historicalsale #detailform': {
                    activate: this.onActivateCard
                },
                'historicalsale #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalsale #table': {
                    click: this.onTableButtonClick
                },
                'historicalsale #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalsale #next': {
                    click: this.onNextButtonClick
                },
                'historicalsale #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalsale #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalsale #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
