Ext.define('App.controller.tpm.program.Program', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'program[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'program directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'program #datatable': {
                    activate: this.onActivateCard
                },
                'program #detailform': {
                    activate: this.onActivateCard
                },
                'program #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'program #detailform #next': {
                    click: this.onNextButtonClick
                },
                'program #detail': {
                    click: this.onDetailButtonClick
                },
                'program #table': {
                    click: this.onTableButtonClick
                },
                'program #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'program #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'program #createbutton': {
                    click: this.onCreateButtonClick
                },
                'program #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'program #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'program #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'program #refresh': {
                    click: this.onRefreshButtonClick
                },
                'program #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'program #exportbutton': {
                    click: this.onExportButtonClick
                },
                'program #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'program #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'program #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
