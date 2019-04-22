Ext.define('App.controller.tpm.client.Client', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'client[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick,
                },
                'client directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'client #datatable': {
                    activate: this.onActivateCard
                },
                'client #detailform': {
                    activate: this.onActivateCard
                },
                'client #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'client #detailform #next': {
                    click: this.onNextButtonClick
                },
                'client #detail': {
                    click: this.onDetailButtonClick
                },
                'client #table': {
                    click: this.onTableButtonClick
                },
                'client #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'client #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'client #createbutton': {
                    click: this.onCreateButtonClick
                },
                'client #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'client #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'client #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'client #refresh': {
                    click: this.onRefreshButtonClick
                },
                'client #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'client #exportbutton': {
                    click: this.onExportButtonClick
                },
                'client #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'client #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'client #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
