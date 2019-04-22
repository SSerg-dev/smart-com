Ext.define('App.controller.tpm.commercialsubnet.HistoricalCommercialSubnet', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcommercialsubnet directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcommercialsubnet directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcommercialsubnet #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcommercialsubnet #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcommercialsubnet #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcommercialsubnet #table': {
                    click: this.onTableButtonClick
                },
                'historicalcommercialsubnet #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcommercialsubnet #next': {
                    click: this.onNextButtonClick
                },
                'historicalcommercialsubnet #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcommercialsubnet #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcommercialsubnet #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
