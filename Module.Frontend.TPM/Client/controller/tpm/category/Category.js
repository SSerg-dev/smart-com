Ext.define('App.controller.tpm.category.Category', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'category[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'category directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'category #datatable': {
                    activate: this.onActivateCard
                },
                'category #detailform': {
                    activate: this.onActivateCard
                },
                'category #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'category #detailform #next': {
                    click: this.onNextButtonClick
                },
                'category #detail': {
                    click: this.onDetailButtonClick
                },
                'category #table': {
                    click: this.onTableButtonClick
                },
                'category #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'category #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'category #createbutton': {
                    click: this.onCreateButtonClick
                },
                'category #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'category #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'category #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'category #refresh': {
                    click: this.onRefreshButtonClick
                },
                'category #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'category #exportbutton': {
                    click: this.onExportButtonClick
                },
                'category #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'category #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'category #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
