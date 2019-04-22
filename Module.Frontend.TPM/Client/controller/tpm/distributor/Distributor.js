Ext.define('App.controller.tpm.distributor.Distributor', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'distributor[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'distributor directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'distributor #datatable': {
                    activate: this.onActivateCard
                },
                'distributor #detailform': {
                    activate: this.onActivateCard
                },
                'distributor #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'distributor #detailform #next': {
                    click: this.onNextButtonClick
                },
                'distributor #detail': {
                    click: this.onDetailButtonClick
                },
                'distributor #table': {
                    click: this.onTableButtonClick
                },
                'distributor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'distributor #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'distributor #createbutton': {
                    click: this.onCreateButtonClick
                },
                'distributor #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'distributor #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'distributor #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'distributor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'distributor #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'distributor #exportbutton': {
                    click: this.onExportButtonClick
                },
                'distributor #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'distributor #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'distributor #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
