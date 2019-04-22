Ext.define('App.controller.tpm.promoroireport.PromoROIReport', {
    extend: 'App.controller.core.AssociatedDirectory',
    //mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoroireport[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'promoroireport[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'promoroireport directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoroireport #datatable': {
                    activate: this.onActivateCard
                },
                'promoroireport #detailform': {
                    activate: this.onActivateCard
                },
                'promoroireport #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoroireport #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoroireport #detail': {
                    click: this.onDetailButtonClick
                },
                'promoroireport #table': {
                    click: this.onTableButtonClick
                },
                'promoroireport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoroireport #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoroireport #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoroireport #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoroireport #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoroireport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoroireport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoroireport #close': {
                    click: this.onCloseButtonClick
                },
                'promoroireport #exportbutton': {
                    click: this.onExportButtonClick
                },
                
            }
        });
    },
});