Ext.define('App.controller.tpm.promoproduct.PromoProduct', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoproduct[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoproduct directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoproduct #datatable': {
                    activate: this.onActivateCard
                },
                'promoproduct #detailform': {
                    activate: this.onActivateCard
                },
                'promoproduct #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoproduct #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoproduct #detail': {
                    click: this.onDetailButtonClick
                },
                'promoproduct #table': {
                    click: this.onTableButtonClick
                },
                'promoproduct #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoproduct #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoproduct #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoproduct #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoproduct #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoproduct #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoproduct #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoproduct #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoproduct #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoproduct #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoproduct #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoproduct #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});