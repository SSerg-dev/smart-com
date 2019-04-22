Ext.define('App.controller.tpm.promoproduct.HistoricalPromoProduct', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromoproduct directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromoproduct directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromoproduct #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromoproduct #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromoproduct #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpromoproduct #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromoproduct #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromoproduct #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromoproduct #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromoproduct #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromoproduct #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
