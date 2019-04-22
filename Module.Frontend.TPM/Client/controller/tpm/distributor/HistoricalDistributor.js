Ext.define('App.controller.tpm.distributor.HistoricalDistributor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaldistributor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaldistributor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaldistributor #datatable': {
                    activate: this.onActivateCard
                },
                'historicaldistributor #detailform': {
                    activate: this.onActivateCard
                },
                'historicaldistributor #detail': {
                    click: this.onDetailButtonClick
                },
                'historicaldistributor #table': {
                    click: this.onTableButtonClick
                },
                'historicaldistributor #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaldistributor #next': {
                    click: this.onNextButtonClick
                },
                'historicaldistributor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaldistributor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaldistributor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
