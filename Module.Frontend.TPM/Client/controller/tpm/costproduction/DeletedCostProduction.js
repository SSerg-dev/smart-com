Ext.define('App.controller.tpm.costproduction.DeletedCostProduction', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcostproduction directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcostproduction directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcostproduction #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcostproduction #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcostproduction #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcostproduction #table': {
                    click: this.onTableButtonClick
                },
                'deletedcostproduction #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcostproduction #next': {
                    click: this.onNextButtonClick
                },
                'deletedcostproduction #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcostproduction #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcostproduction #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcostproduction #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});