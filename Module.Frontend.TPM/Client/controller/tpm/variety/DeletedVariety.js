Ext.define('App.controller.tpm.variety.DeletedVariety', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedvariety directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedvariety directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedvariety #datatable': {
                    activate: this.onActivateCard
                },
                'deletedvariety #detailform': {
                    activate: this.onActivateCard
                },
                'deletedvariety #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedvariety #table': {
                    click: this.onTableButtonClick
                },
                'deletedvariety #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedvariety #next': {
                    click: this.onNextButtonClick
                },
                'deletedvariety #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedvariety #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedvariety #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedvariety #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
