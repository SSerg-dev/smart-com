Ext.define('App.controller.tpm.rsmode.DeletedRSmode', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedrsmode directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedrsmode directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedrsmode #datatable': {
                    activate: this.onActivateCard
                },
                'deletedrsmode #detailform': {
                    activate: this.onActivateCard
                },
                'deletedrsmode #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedrsmode #table': {
                    click: this.onTableButtonClick
                },
                'deletedrsmode #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedrsmode #next': {
                    click: this.onNextButtonClick
                },
                'deletedrsmode #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedrsmode #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedrsmode #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedrsmode #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
