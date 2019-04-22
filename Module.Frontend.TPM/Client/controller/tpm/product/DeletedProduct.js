Ext.define('App.controller.tpm.product.DeletedProduct', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedproduct directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedproduct directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedproduct #datatable': {
                    activate: this.onActivateCard
                },
                'deletedproduct #detailform': {
                    activate: this.onActivateCard
                },
                'deletedproduct #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedproduct #table': {
                    click: this.onTableButtonClick
                },
                'deletedproduct #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedproduct #next': {
                    click: this.onNextButtonClick
                },
                'deletedproduct #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedproduct #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedproduct #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedproduct #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
