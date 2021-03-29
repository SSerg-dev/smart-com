Ext.define('App.controller.tpm.budgetitem.BudgetItemShort', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'budgetitemshort[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'budgetitemshort directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'budgetitemshort #datatable': {
                    activate: this.onActivateCard
                },
                'budgetitemshort #detailform': {
                    activate: this.onActivateCard
                },
                'budgetitemshort #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'budgetitemshort #detailform #next': {
                    click: this.onNextButtonClick
                },
                'budgetitemshort #detail': {
                    click: this.onDetailButtonClick
                },
                'budgetitemshort #table': {
                    click: this.onTableButtonClick
                },
                'budgetitemshort #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'budgetitemshort #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'budgetitemshort #createbutton': {
                    click: this.onCreateButtonClick
                },
                'budgetitemshort #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'budgetitemshort #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'budgetitemshort #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'budgetitemshort #refresh': {
                    click: this.onRefreshButtonClick
                },
                'budgetitemshort #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'budgetitemshort #exportbutton': {
                    click: this.onExportButtonClick
                },
                'budgetitemshort #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'budgetitemshort #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'budgetitemshort #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
