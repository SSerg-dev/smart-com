Ext.define('App.controller.tpm.distributor.DeletedDistributor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deleteddistributor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deleteddistributor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deleteddistributor #datatable': {
                    activate: this.onActivateCard
                },
                'deleteddistributor #detailform': {
                    activate: this.onActivateCard
                },
                'deleteddistributor #detail': {
                    click: this.onDetailButtonClick
                },
                'deleteddistributor #table': {
                    click: this.onTableButtonClick
                },
                'deleteddistributor #prev': {
                    click: this.onPrevButtonClick
                },
                'deleteddistributor #next': {
                    click: this.onNextButtonClick
                },
                'deleteddistributor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deleteddistributor #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deleteddistributor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deleteddistributor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
