Ext.define('App.controller.tpm.budget.Budget', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'budget[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'budget[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'budget directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'budget #datatable': {
                    activate: this.onActivateCard
                },
                'budget #detailform': {
                    activate: this.onActivateCard
                },
                'budget #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'budget #detailform #next': {
                    click: this.onNextButtonClick
                },
                'budget #detail': {
                    click: this.onDetailButtonClick
                },
                'budget #table': {
                    click: this.onTableButtonClick
                },
                'budget #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'budget #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'budget #createbutton': {
                    click: this.onCreateButtonClick
                },
                'budget #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'budget #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'budget #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'budget #refresh': {
                    click: this.onRefreshButtonClick
                },
                'budget #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'budget #exportbutton': {
                    click: this.onExportButtonClick
                },
                'budget #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'budget #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'budget #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});