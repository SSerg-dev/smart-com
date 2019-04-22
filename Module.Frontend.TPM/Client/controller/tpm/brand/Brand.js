Ext.define('App.controller.tpm.brand.Brand', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'brand[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'brand directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'brand #datatable': {
                    activate: this.onActivateCard
                },
                'brand #detailform': {
                    activate: this.onActivateCard
                },
                'brand #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'brand #detailform #next': {
                    click: this.onNextButtonClick
                },
                'brand #detail': {
                    click: this.onDetailButtonClick
                },
                'brand #table': {
                    click: this.onTableButtonClick
                },
                'brand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'brand #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'brand #createbutton': {
                    click: this.onCreateButtonClick
                },
                'brand #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'brand #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'brand #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'brand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'brand #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'brand #exportbutton': {
                    click: this.onExportButtonClick
                },
                'brand #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'brand #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'brand #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
