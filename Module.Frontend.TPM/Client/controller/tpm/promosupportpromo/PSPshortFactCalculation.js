Ext.define('App.controller.tpm.promosupportpromo.PSPshortFactCalculation', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'pspshortfactcalculation[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'pspshortfactcalculation directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'pspshortfactcalculation #datatable': {
                    activate: this.onActivateCard
                },
                'pspshortfactcalculation #detailform': {
                    activate: this.onActivateCard
                },
                'pspshortfactcalculation #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'pspshortfactcalculation #detailform #next': {
                    click: this.onNextButtonClick
                },
                'pspshortfactcalculation #detail': {
                    click: this.onDetailButtonClick
                },
                'pspshortfactcalculation #table': {
                    click: this.onTableButtonClick
                },
                'pspshortfactcalculation #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'pspshortfactcalculation #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'pspshortfactcalculation #createbutton': {
                    click: this.onCreateButtonClick
                },
                'pspshortfactcalculation #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'pspshortfactcalculation #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'pspshortfactcalculation #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'pspshortfactcalculation #refresh': {
                    click: this.onRefreshButtonClick
                },
                'pspshortfactcalculation #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'pspshortfactcalculation #exportbutton': {
                    click: this.onExportButtonClick
                },
                'pspshortfactcalculation #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'pspshortfactcalculation #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'pspshortfactcalculation #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
