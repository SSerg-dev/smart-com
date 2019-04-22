Ext.define('App.controller.tpm.promosales.HistoricalPromoSales', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromosales directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromosales directorygrid': {
                    itemdblclick: this.switchToDetailForm,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromosales #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromosales #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromosales #detail': {
                    click: this.switchToDetailForm
                },
                'historicalpromosales #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromosales #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromosales #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromosales #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromosales #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromosales #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});