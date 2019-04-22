Ext.define('App.controller.tpm.demandpricelist.DeletedDemandPriceList', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deleteddemandpricelist directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deleteddemandpricelist directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deleteddemandpricelist #datatable': {
                    activate: this.onActivateCard
                },
                'deleteddemandpricelist #detailform': {
                    activate: this.onActivateCard
                },
                'deleteddemandpricelist #detail': {
                    click: this.onDetailButtonClick
                },
                'deleteddemandpricelist #table': {
                    click: this.onTableButtonClick
                },
                'deleteddemandpricelist #prev': {
                    click: this.onPrevButtonClick
                },
                'deleteddemandpricelist #next': {
                    click: this.onNextButtonClick
                },
                'deleteddemandpricelist #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deleteddemandpricelist #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deleteddemandpricelist #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deleteddemandpricelist #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
