Ext.define('App.controller.tpm.format.Format', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'format[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'format directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'format #datatable': {
                    activate: this.onActivateCard
                },
                'format #detailform': {
                    activate: this.onActivateCard
                },
                'format #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'format #detailform #next': {
                    click: this.onNextButtonClick
                },
                'format #detail': {
                    click: this.onDetailButtonClick
                },
                'format #table': {
                    click: this.onTableButtonClick
                },
                'format #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'format #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'format #createbutton': {
                    click: this.onCreateButtonClick
                },
                'format #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'format #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'format #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'format #refresh': {
                    click: this.onRefreshButtonClick
                },
                'format #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'format #exportbutton': {
                    click: this.onExportButtonClick
                },
                'format #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'format #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'format #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
