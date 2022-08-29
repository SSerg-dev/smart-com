Ext.define('App.controller.tpm.rsmode.DeletedRSmode', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedrspromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedrspromo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedrspromo #datatable': {
                    activate: this.onActivateCard
                },
                'deletedrspromo #detailform': {
                    activate: this.onActivateCard
                },
                'deletedrspromo #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedrspromo #table': {
                    click: this.onTableButtonClick
                },
                'deletedrspromo #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedrspromo #next': {
                    click: this.onNextButtonClick
                },
                'deletedrspromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedrspromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedrspromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedrspromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
