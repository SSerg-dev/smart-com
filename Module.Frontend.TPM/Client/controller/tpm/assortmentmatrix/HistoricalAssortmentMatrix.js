Ext.define('App.controller.tpm.assortmentmatrix.HistoricalAssortmentMatrix', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalassortmentmatrix directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalassortmentmatrix directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalassortmentmatrix #datatable': {
                    activate: this.onActivateCard
                },
                'historicalassortmentmatrix #detailform': {
                    activate: this.onActivateCard
                },
                'historicalassortmentmatrix #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalassortmentmatrix #table': {
                    click: this.onTableButtonClick
                },
                'historicalassortmentmatrix #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalassortmentmatrix #next': {
                    click: this.onNextButtonClick
                },
                'historicalassortmentmatrix #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalassortmentmatrix #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalassortmentmatrix #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
