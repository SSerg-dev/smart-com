Ext.define('App.controller.tpm.budgetitem.BudgetItem', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'budgetitem[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'budgetitem directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'budgetitem #datatable': {
                    activate: this.onActivateCard
                },
                'budgetitem #detailform': {
                    activate: this.onActivateCard
                },
                'budgetitem #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'budgetitem #detailform #next': {
                    click: this.onNextButtonClick
                },
                'budgetitem #detail': {
                    click: this.onDetailButtonClick
                },
                'budgetitem #table': {
                    click: this.onTableButtonClick
                },
                'budgetitem #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'budgetitem #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'budgetitem #createbutton': {
                    click: this.onCreateButtonClick
                },
                'budgetitem #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'budgetitem #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'budgetitem #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'budgetitem #refresh': {
                    click: this.onRefreshButtonClick
                },
                'budgetitem #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'budgetitem #exportbutton': {
                    click: this.onExportButtonClick
                },
                'budgetitem #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'budgetitem #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'budgetitem #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
