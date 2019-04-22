Ext.define('App.controller.tpm.promoproduct.DeletedPromoProduct', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromoproduct directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromoproduct directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromoproduct #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromoproduct #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromoproduct #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpromoproduct #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromoproduct #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromoproduct #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromoproduct #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromoproduct #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromoproduct #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromoproduct #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
