Ext.define('App.controller.tpm.budgetsubitem.BudgetSubItem', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'budgetsubitem[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'budgetsubitem directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'budgetsubitem #datatable': {
                    activate: this.onActivateCard
                },
                'budgetsubitem #detailform': {
                    activate: this.onActivateCard
                },
                'budgetsubitem #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'budgetsubitem #detailform #next': {
                    click: this.onNextButtonClick
                },
                'budgetsubitem #detail': {
                    click: this.onDetailButtonClick
                },
                'budgetsubitem #table': {
                    click: this.onTableButtonClick
                },
                'budgetsubitem #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'budgetsubitem #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'budgetsubitem #createbutton': {
                    click: this.onCreateButtonClick
                },
                'budgetsubitem #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'budgetsubitem #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'budgetsubitem #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'budgetsubitem #refresh': {
                    click: this.onRefreshButtonClick
                },
                'budgetsubitem #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'budgetsubitem #exportbutton': {
                    click: this.onExportButtonClick
                },
                'budgetsubitem #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'budgetsubitem #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'budgetsubitem #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
