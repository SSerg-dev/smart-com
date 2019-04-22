Ext.define('App.controller.tpm.commercialnet.HistoricalCommercialNet', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcommercialnet directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcommercialnet directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcommercialnet #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcommercialnet #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcommercialnet #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcommercialnet #table': {
                    click: this.onTableButtonClick
                },
                'historicalcommercialnet #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcommercialnet #next': {
                    click: this.onNextButtonClick
                },
                'historicalcommercialnet #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcommercialnet #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcommercialnet #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
