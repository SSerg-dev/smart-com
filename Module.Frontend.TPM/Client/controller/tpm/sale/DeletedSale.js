Ext.define('App.controller.tpm.sale.DeletedSale', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedsale directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedsale directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedsale #datatable': {
                    activate: this.onActivateCard
                },
                'deletedsale #detailform': {
                    activate: this.onActivateCard
                },
                'deletedsale #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedsale #table': {
                    click: this.onTableButtonClick
                },
                'deletedsale #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedsale #next': {
                    click: this.onNextButtonClick
                },
                'deletedsale #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedsale #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedsale #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedsale #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
