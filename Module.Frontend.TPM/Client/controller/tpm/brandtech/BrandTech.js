Ext.define('App.controller.tpm.brandtech.BrandTech', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'brandtech[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'brandtech directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'brandtech #datatable': {
                    activate: this.onActivateCard
                },
                'brandtech #detailform': {
                    activate: this.onActivateCard
                },
                'brandtech #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'brandtech #detailform #next': {
                    click: this.onNextButtonClick
                },
                'brandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'brandtech #table': {
                    click: this.onTableButtonClick
                },
                'brandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'brandtech #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'brandtech #createbutton': {
                    click: this.onCreateButtonClick
                },
                'brandtech #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'brandtech #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'brandtech #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'brandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'brandtech #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'brandtech #exportbutton': {
                    click: this.onExportButtonClick
                },
                'brandtech #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'brandtech #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'brandtech #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
