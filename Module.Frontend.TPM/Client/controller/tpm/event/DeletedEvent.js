Ext.define('App.controller.tpm.event.DeletedEvent', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedevent directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedevent directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedevent #datatable': {
                    activate: this.onActivateCard
                },
                'deletedevent #detailform': {
                    activate: this.onActivateCard
                },
                'deletedevent #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedevent #table': {
                    click: this.onTableButtonClick
                },
                'deletedevent #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedevent #next': {
                    click: this.onNextButtonClick
                },
                'deletedevent #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedevent #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedevent #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedevent #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
