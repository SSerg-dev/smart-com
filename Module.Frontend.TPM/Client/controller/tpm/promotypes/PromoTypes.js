Ext.define('App.controller.tpm.promotypes.PromoTypes', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promotypes[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promotypes directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promotypes #datatable': {
                    activate: this.onActivateCard
                },
                'promotypes #detailform': {
                    activate: this.onActivateCard
                },
                'promotypes #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promotypes #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promotypes #detail': {
                    click: this.onDetailButtonClick
                },
                'promotypes #table': {
                    click: this.onTableButtonClick
                },
                'promotypes #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promotypes #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promotypes #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promotypes #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promotypes #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promotypes #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promotypes #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promotypes #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promotypes #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promotypes #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promotypes #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promotypes #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
