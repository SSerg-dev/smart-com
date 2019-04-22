Ext.define('App.controller.tpm.promosales.DeletedPromoSales', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromosales directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromosales directorygrid': {
                    itemdblclick: this.switchToDetailForm,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromosales #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromosales #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromosales #detail': {
                    click: this.switchToDetailForm
                },
                'deletedpromosales #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromosales #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromosales #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromosales #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromosales #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromosales #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromosales #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
