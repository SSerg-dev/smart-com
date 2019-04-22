Ext.define('App.controller.tpm.region.Region', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'region[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'region directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'region #datatable': {
                    activate: this.onActivateCard
                },
                'region #detailform': {
                    activate: this.onActivateCard
                },
                'region #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'region #detailform #next': {
                    click: this.onNextButtonClick
                },
                'region #detail': {
                    click: this.onDetailButtonClick
                },
                'region #table': {
                    click: this.onTableButtonClick
                },
                'region #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'region #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'region #createbutton': {
                    click: this.onCreateButtonClick
                },
                'region #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'region #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'region #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'region #refresh': {
                    click: this.onRefreshButtonClick
                },
                'region #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'region #exportbutton': {
                    click: this.onExportButtonClick
                },
                'region #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'region #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'region #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
