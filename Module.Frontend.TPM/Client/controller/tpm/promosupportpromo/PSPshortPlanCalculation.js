Ext.define('App.controller.tpm.promosupportpromo.PSPshortPlanCalculation', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'pspshortplancalculation[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'pspshortplancalculation directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'pspshortplancalculation #datatable': {
                    activate: this.onActivateCard
                },
                'pspshortplancalculation #detailform': {
                    activate: this.onActivateCard
                },
                'pspshortplancalculation #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'pspshortplancalculation #detailform #next': {
                    click: this.onNextButtonClick
                },
                'pspshortplancalculation #detail': {
                    click: this.onDetailButtonClick
                },
                'pspshortplancalculation #table': {
                    click: this.onTableButtonClick
                },
                'pspshortplancalculation #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'pspshortplancalculation #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'pspshortplancalculation #createbutton': {
                    click: this.onCreateButtonClick
                },
                'pspshortplancalculation #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'pspshortplancalculation #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'pspshortplancalculation #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'pspshortplancalculation #refresh': {
                    click: this.onRefreshButtonClick
                },
                'pspshortplancalculation #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'pspshortplancalculation #exportbutton': {
                    click: this.onExportButtonClick
                },
                'pspshortplancalculation #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'pspshortplancalculation #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'pspshortplancalculation #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
