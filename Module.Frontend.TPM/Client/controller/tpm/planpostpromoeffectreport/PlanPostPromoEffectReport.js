Ext.define('App.controller.tpm.planpostpromoeffectreport.PlanPostPromoEffectReport', {
    extend: 'App.controller.core.AssociatedDirectory',
    //mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'planpostpromoeffectreport[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'planpostpromoeffectreport[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'planpostpromoeffectreport directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'planpostpromoeffectreport #datatable': {
                    activate: this.onActivateCard
                },
                'planpostpromoeffectreport #detailform': {
                    activate: this.onActivateCard
                },
                'planpostpromoeffectreport #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'planpostpromoeffectreport #detailform #next': {
                    click: this.onNextButtonClick
                },
                'planpostpromoeffectreport #detail': {
                    click: this.onDetailButtonClick
                },
                'planpostpromoeffectreport #table': {
                    click: this.onTableButtonClick
                },
                'planpostpromoeffectreport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'planpostpromoeffectreport #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'planpostpromoeffectreport #createbutton': {
                    click: this.onCreateButtonClick
                },
                'planpostpromoeffectreport #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'planpostpromoeffectreport #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'planpostpromoeffectreport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'planpostpromoeffectreport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'planpostpromoeffectreport #close': {
                    click: this.onCloseButtonClick
                },
                'planpostpromoeffectreport #exportbutton': {
                    click: this.onExportButtonClick
                },
                
            }
        });
    },
});