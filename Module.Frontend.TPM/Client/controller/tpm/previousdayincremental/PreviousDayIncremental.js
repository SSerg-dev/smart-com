Ext.define('App.controller.tpm.previousdayincremental.PreviousDayIncremental', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'previousdayincremental[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'previousdayincremental directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'previousdayincremental #datatable': {
                    activate: this.onActivateCard
                },
                'previousdayincremental #detailform': {
                    activate: this.onActivateCard
                },
                'previousdayincremental #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'previousdayincremental #detailform #next': {
                    click: this.onNextButtonClick
                },
                'previousdayincremental #detail': {
                    click: this.onDetailButtonClick
                },
                'previousdayincremental #table': {
                    click: this.onTableButtonClick
                },
                'previousdayincremental #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'previousdayincremental #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'previousdayincremental #createbutton': {
                    click: this.onCreateButtonClick
                },
                'previousdayincremental #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'previousdayincremental #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'previousdayincremental #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'previousdayincremental #refresh': {
                    click: this.onRefreshButtonClick
                },
                'previousdayincremental #close': {
                    click: this.onCloseButtonClick
                }, 
            }
        });
    }, 
});
