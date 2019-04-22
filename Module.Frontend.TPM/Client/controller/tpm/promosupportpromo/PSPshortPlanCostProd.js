Ext.define('App.controller.tpm.promosupportpromo.PSPshortPlanCostProd', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'pspshortplancostprod[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'pspshortplancostprod directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'pspshortplancostprod #datatable': {
                    activate: this.onActivateCard
                },
                'pspshortplancostprod #detailform': {
                    activate: this.onActivateCard
                },
                'pspshortplancostprod #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'pspshortplancostprod #detailform #next': {
                    click: this.onNextButtonClick
                },
                'pspshortplancostprod #detail': {
                    click: this.onDetailButtonClick
                },
                'pspshortplancostprod #table': {
                    click: this.onTableButtonClick
                },
                'pspshortplancostprod #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'pspshortplancostprod #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'pspshortplancostprod #createbutton': {
                    click: this.onCreateButtonClick
                },
                'pspshortplancostprod #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'pspshortplancostprod #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'pspshortplancostprod #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'pspshortplancostprod #refresh': {
                    click: this.onRefreshButtonClick
                },
                'pspshortplancostprod #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'pspshortplancostprod #exportbutton': {
                    click: this.onExportButtonClick
                },
                'pspshortplancostprod #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'pspshortplancostprod #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'pspshortplancostprod #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
