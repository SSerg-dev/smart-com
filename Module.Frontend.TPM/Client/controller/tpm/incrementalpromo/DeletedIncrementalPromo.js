Ext.define('App.controller.tpm.incrementalpromo.DeletedIncrementalPromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedincrementalpromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedincrementalpromo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedincrementalpromo #datatable': {
                    activate: this.onActivateCard
                },
                'deletedincrementalpromo #detailform': {
                    activate: this.onActivateCard
                },
                'deletedincrementalpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedincrementalpromo #table': {
                    click: this.onTableButtonClick
                },
                'deletedincrementalpromo #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedincrementalpromo #next': {
                    click: this.onNextButtonClick
                },
                'deletedincrementalpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedincrementalpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedincrementalpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedincrementalpromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
