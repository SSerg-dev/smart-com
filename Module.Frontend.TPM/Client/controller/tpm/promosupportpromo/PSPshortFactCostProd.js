Ext.define('App.controller.tpm.promosupportpromo.PSPshortFactCostProd', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'pspshortfactcostprod[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'pspshortfactcostprod directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'pspshortfactcostprod #datatable': {
                    activate: this.onActivateCard
                },
                'pspshortfactcostprod #detailform': {
                    activate: this.onActivateCard
                },
                'pspshortfactcostprod #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'pspshortfactcostprod #detailform #next': {
                    click: this.onNextButtonClick
                },
                'pspshortfactcostprod #detail': {
                    click: this.onDetailButtonClick
                },
                'pspshortfactcostprod #table': {
                    click: this.onTableButtonClick
                },
                'pspshortfactcostprod #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'pspshortfactcostprod #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'pspshortfactcostprod #createbutton': {
                    click: this.onCreateButtonClick
                },
                'pspshortfactcostprod #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'pspshortfactcostprod #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'pspshortfactcostprod #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'pspshortfactcostprod #refresh': {
                    click: this.onRefreshButtonClick
                },
                'pspshortfactcostprod #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'pspshortfactcostprod #exportbutton': {
                    click: this.onExportButtonClick
                },
                'pspshortfactcostprod #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'pspshortfactcostprod #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'pspshortfactcostprod #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
