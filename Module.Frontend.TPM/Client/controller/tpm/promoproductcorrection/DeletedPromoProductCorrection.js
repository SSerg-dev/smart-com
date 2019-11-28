Ext.define('App.controller.tpm.promoproductcorrection.DeletedPromoProductCorrection', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromoproductcorrection directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromoproductcorrection directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromoproductcorrection #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromoproductcorrection #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromoproductcorrection #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpromoproductcorrection #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromoproductcorrection #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromoproductcorrection #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromoproductcorrection #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromoproductcorrection #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromoproductcorrection #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromoproductcorrection #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
