Ext.define('App.controller.tpm.technology.DeletedTechnology', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedtechnology directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedtechnology directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedtechnology #datatable': {
                    activate: this.onActivateCard
                },
                'deletedtechnology #detailform': {
                    activate: this.onActivateCard
                },
                'deletedtechnology #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedtechnology #table': {
                    click: this.onTableButtonClick
                },
                'deletedtechnology #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedtechnology #next': {
                    click: this.onNextButtonClick
                },
                'deletedtechnology #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedtechnology #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedtechnology #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedtechnology #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
