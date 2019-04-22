Ext.define('App.controller.tpm.demandpricelist.HistoricalDemandPriceList', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaldemandpricelist directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaldemandpricelist directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaldemandpricelist #datatable': {
                    activate: this.onActivateCard
                },
                'historicaldemandpricelist #detailform': {
                    activate: this.onActivateCard
                },
                'historicaldemandpricelist #detail': {
                    click: this.onDetailButtonClick
                },
                'historicaldemandpricelist #table': {
                    click: this.onTableButtonClick
                },
                'historicaldemandpricelist #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaldemandpricelist #next': {
                    click: this.onNextButtonClick
                },
                'historicaldemandpricelist #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaldemandpricelist #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaldemandpricelist #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
