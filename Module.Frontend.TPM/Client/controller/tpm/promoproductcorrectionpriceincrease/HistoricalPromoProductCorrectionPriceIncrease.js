Ext.define('App.controller.tpm.promoproductcorrectionpriceincrease.HistoricalPromoProductCorrectionPriceIncrease', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromoproductcorrectionpriceincrease directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromoproductcorrectionpriceincrease directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromoproductcorrectionpriceincrease #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromoproductcorrectionpriceincrease #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromoproductcorrectionpriceincrease #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpromoproductcorrectionpriceincrease #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromoproductcorrectionpriceincrease #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromoproductcorrectionpriceincrease #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromoproductcorrectionpriceincrease #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromoproductcorrectionpriceincrease #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromoproductcorrectionpriceincrease #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
