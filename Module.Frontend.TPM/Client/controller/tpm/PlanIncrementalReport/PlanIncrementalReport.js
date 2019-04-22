Ext.define('App.controller.tpm.planincrementalreport.PlanIncrementalReport', {
    extend: 'App.controller.core.AssociatedDirectory',
    //mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'planincrementalreport[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'planincrementalreport[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'planincrementalreport directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'planincrementalreport #datatable': {
                    activate: this.onActivateCard
                },
                'planincrementalreport #detailform': {
                    activate: this.onActivateCard
                },
                'planincrementalreport #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'planincrementalreport #detailform #next': {
                    click: this.onNextButtonClick
                },
                'planincrementalreport #detail': {
                    click: this.onDetailButtonClick
                },
                'planincrementalreport #table': {
                    click: this.onTableButtonClick
                },
                'planincrementalreport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'planincrementalreport #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'planincrementalreport #createbutton': {
                    click: this.onCreateButtonClick
                },
                'planincrementalreport #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'planincrementalreport #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'planincrementalreport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'planincrementalreport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'planincrementalreport #close': {
                    click: this.onCloseButtonClick
                },
                'planincrementalreport #exportbutton': {
                    click: this.onExportButtonClick
                },
                
            }
        });
    },
});