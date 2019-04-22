Ext.define('App.controller.tpm.baseline.BaseLine', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'baseline[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'baseline directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'baseline #datatable': {
                    activate: this.onActivateCard
                },
                'baseline #detailform': {
                    activate: this.onActivateCard
                },
                'baseline #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'baseline #detailform #next': {
                    click: this.onNextButtonClick
                },
                'baseline #detail': {
                    click: this.onDetailButtonClick
                },
                'baseline #table': {
                    click: this.onTableButtonClick
                },
                'baseline #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'baseline #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'baseline #createbutton': {
                    click: this.onCreateButtonClick
                },
                'baseline #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'baseline #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'baseline #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'baseline #refresh': {
                    click: this.onRefreshButtonClick
                },
                'baseline #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'baseline #exportbutton': {
                    click: this.onExportButtonClick
                },
                'baseline #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'baseline #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'baseline #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
