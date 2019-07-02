Ext.define('App.controller.tpm.incrementalpromo.IncrementalPromo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'incrementalpromo[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'incrementalpromo directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'incrementalpromo #datatable': {
                    activate: this.onActivateCard
                },
                'incrementalpromo #detailform': {
                    activate: this.onActivateCard
                },
                'incrementalpromo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'incrementalpromo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'incrementalpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'incrementalpromo #table': {
                    click: this.onTableButtonClick
                },
                'incrementalpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'incrementalpromo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'incrementalpromo #createbutton': {
                    click: this.onCreateButtonClick
                },
                'incrementalpromo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'incrementalpromo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'incrementalpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'incrementalpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'incrementalpromo #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'incrementalpromo #exportbutton': {
                    click: this.onExportButtonClick
                },
                'incrementalpromo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'incrementalpromo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'incrementalpromo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
