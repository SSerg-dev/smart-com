Ext.define('App.controller.tpm.program.DeletedProgram', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedprogram directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedprogram directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedprogram #datatable': {
                    activate: this.onActivateCard
                },
                'deletedprogram #detailform': {
                    activate: this.onActivateCard
                },
                'deletedprogram #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedprogram #table': {
                    click: this.onTableButtonClick
                },
                'deletedprogram #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedprogram #next': {
                    click: this.onNextButtonClick
                },
                'deletedprogram #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedprogram #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedprogram #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedprogram #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
