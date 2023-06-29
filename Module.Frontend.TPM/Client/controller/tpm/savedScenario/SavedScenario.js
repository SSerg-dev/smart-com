Ext.define('App.controller.tpm.savedScenario.SavedScenario', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'savedScenario[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'savedScenario directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    extfilterchange: this.onExtFilterChange
                },
                'savedScenario #datatable': {
                    activate: this.onActivateCard
                },
                'savedScenario #detailform': {
                    activate: this.onActivateCard
                },
                'savedScenario #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'savedScenario #detailform #next': {
                    click: this.onNextButtonClick
                },
                'savedScenario #detail': {
                    click: this.switchToDetailForm
                },
                'savedScenario #table': {
                    click: this.onTableButtonClick
                },
                'savedScenario #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'savedScenario #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'savedScenario #onapprovalbutton': {
                    click: this.onOnApprovalButtonClick
                },
                'savedScenario #approvebutton': {
                    click: this.onApproveButtonClick
                },
                'savedScenario #massapprovebutton': {
                    click: this.onMassApproveButtonClick
                },
                'savedScenario #declinebutton': {
                    click: this.onDeclineButtonClick
                },
                'savedScenario #calculatebutton': {
                    click: this.onCalculateButtonClick
                },
                'savedScenario #uploadscenariobutton': {
                    click: this.onUploadScenarioButtonClick
                },
                'savedScenario #showlogbutton': {
                    click: this.onShowLogButtonClick
                },
                'savedScenario #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'savedScenario #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'savedScenario #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'savedScenario #refresh': {
                    click: this.onRefreshButtonClick
                },
                'savedScenario #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'savedScenario #exportbutton': {
                    click: this.onExportButtonClick
                },
                'savedScenario #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'savedScenario #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'savedScenario #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },    
});
