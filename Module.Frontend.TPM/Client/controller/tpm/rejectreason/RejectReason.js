Ext.define('App.controller.tpm.rejectreason.RejectReason', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'rejectreason[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'rejectreason directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'rejectreason #datatable': {
                    activate: this.onActivateCard
                },
                'rejectreason #detailform': {
                    activate: this.onActivateCard
                },
                'rejectreason #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'rejectreason #detailform #next': {
                    click: this.onNextButtonClick
                },
                'rejectreason #detail': {
                    click: this.onDetailButtonClick
                },
                'rejectreason #table': {
                    click: this.onTableButtonClick
                },
                'rejectreason #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'rejectreason #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'rejectreason #createbutton': {
                    click: this.onCreateButtonClick
                },
                'rejectreason #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'rejectreason #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'rejectreason #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'rejectreason #refresh': {
                    click: this.onRefreshButtonClick
                },
                'rejectreason #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'rejectreason #exportbutton': {
                    click: this.onExportButtonClick
                },
                'rejectreason #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'rejectreason #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'rejectreason #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
