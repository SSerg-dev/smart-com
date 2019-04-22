Ext.define('App.controller.tpm.retailtype.HistoricalRetailType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalretailtype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalretailtype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalretailtype #datatable': {
                    activate: this.onActivateCard
                },
                'historicalretailtype #detailform': {
                    activate: this.onActivateCard
                },
                'historicalretailtype #detail': {
                    click: this.switchToDetailForm
                },
                'historicalretailtype #table': {
                    click: this.onTableButtonClick
                },
                'historicalretailtype #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalretailtype #next': {
                    click: this.onNextButtonClick
                },
                'historicalretailtype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalretailtype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalretailtype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
