Ext.define('App.controller.tpm.actualLSV.ActualLSV', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'actuallsv[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'actuallsv directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'actuallsv #datatable': {
                    activate: this.onActivateCard
                },
                'actuallsv #detailform': {
                    activate: this.onActivateCard
                },
                'actuallsv #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'actuallsv #detailform #next': {
                    click: this.onNextButtonClick
                },
                'actuallsv #detail': {
                    click: this.onDetailButtonClick
                },
                'actuallsv #table': {
                    click: this.onTableButtonClick
                },
                'actuallsv #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'actuallsv #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'actuallsv #createbutton': {
                    click: this.onCreateButtonClick
                },
                'actuallsv #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'actuallsv #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'actuallsv #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'actuallsv #refresh': {
                    click: this.onRefreshButtonClick
                },
                'actuallsv #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'actuallsv #exportbutton': {
                    click: this.onExportButtonClick
                },
                'actuallsv #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'actuallsv #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'actuallsv #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
