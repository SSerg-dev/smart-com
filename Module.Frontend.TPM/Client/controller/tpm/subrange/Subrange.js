Ext.define('App.controller.tpm.subrange.Subrange', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'subrange[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'subrange directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'subrange #datatable': {
                    activate: this.onActivateCard
                },
                'subrange #detailform': {
                    activate: this.onActivateCard
                },
                'subrange #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'subrange #detailform #next': {
                    click: this.onNextButtonClick
                },
                'subrange #detail': {
                    click: this.onDetailButtonClick
                },
                'subrange #table': {
                    click: this.onTableButtonClick
                },
                'subrange #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'subrange #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'subrange #createbutton': {
                    click: this.onCreateButtonClick
                },
                'subrange #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'subrange #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'subrange #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'subrange #refresh': {
                    click: this.onRefreshButtonClick
                },
                'subrange #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'subrange #exportbutton': {
                    click: this.onExportButtonClick
                },
                'subrange #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'subrange #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'subrange #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
