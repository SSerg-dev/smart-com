Ext.define('App.controller.tpm.client.HistoricalClient', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalclient directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalclient directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalclient #datatable': {
                    activate: this.onActivateCard
                },
                'historicalclient #detailform': {
                    activate: this.onActivateCard
                },
                'historicalclient #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalclient #table': {
                    click: this.onTableButtonClick
                },
                'historicalclient #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalclient #next': {
                    click: this.onNextButtonClick
                },
                'historicalclient #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalclient #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalclient #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
