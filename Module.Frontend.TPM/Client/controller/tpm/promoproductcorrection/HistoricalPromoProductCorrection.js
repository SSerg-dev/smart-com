Ext.define('App.controller.tpm.promoproductcorrection.HistoricalPromoProductCorrection', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromoproductcorrection directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromoproductcorrection directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromoproductcorrection #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromoproductcorrection #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromoproductcorrection #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpromoproductcorrection #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromoproductcorrection #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromoproductcorrection #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromoproductcorrection #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromoproductcorrection #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromoproductcorrection #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
