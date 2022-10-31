Ext.define('App.controller.tpm.promoproductcorrectionpriceincrease.DeletedPromoProductCorrectionPriceIncrease', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromoproductcorrectionpriceincrease directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromoproductcorrectionpriceincrease directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromoproductcorrectionpriceincrease #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromoproductcorrectionpriceincrease #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromoproductcorrectionpriceincrease #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpromoproductcorrectionpriceincrease #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromoproductcorrectionpriceincrease #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromoproductcorrectionpriceincrease #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromoproductcorrectionpriceincrease #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromoproductcorrectionpriceincrease #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromoproductcorrectionpriceincrease #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromoproductcorrectionpriceincrease #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
