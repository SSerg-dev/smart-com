Ext.define('App.controller.tpm.promosales.PromoSales', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promosales directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promosales[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'promosales #datatable': {
                    activate: this.onActivateCard
                },
                'promosales #detailform': {
                    activate: this.onActivateCard
                },
                'promosales #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promosales #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promosales #detail': {
                    click: this.switchToDetailForm
                },
                'promosales #table': {
                    click: this.onTableButtonClick
                },
                'promosales #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promosales #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promosales #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promosales #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promosales #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promosales #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promosales #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promosales #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promosales #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promosales #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promosales #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promosales #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
