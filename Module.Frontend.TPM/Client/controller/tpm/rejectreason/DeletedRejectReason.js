Ext.define('App.controller.tpm.rejectreason.DeletedRejectReason', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedrejectreason directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedrejectreason directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedrejectreason #datatable': {
                    activate: this.onActivateCard
                },
                'deletedrejectreason #detailform': {
                    activate: this.onActivateCard
                },
                'deletedrejectreason #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedrejectreason #table': {
                    click: this.onTableButtonClick
                },
                'deletedrejectreason #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedrejectreason #next': {
                    click: this.onNextButtonClick
                },
                'deletedrejectreason #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedrejectreason #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedrejectreason #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedrejectreason #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
