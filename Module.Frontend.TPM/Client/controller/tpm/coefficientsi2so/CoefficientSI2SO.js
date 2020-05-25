Ext.define('App.controller.tpm.coefficientsi2so.CoefficientSI2SO', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'coefficientsi2so[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'coefficientsi2so directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'coefficientsi2so #datatable': {
                    activate: this.onActivateCard
                },
                'coefficientsi2so #detailform': {
                    activate: this.onActivateCard
                },
                'coefficientsi2so #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'coefficientsi2so #detailform #next': {
                    click: this.onNextButtonClick
                },
                'coefficientsi2so #detail': {
                    click: this.onDetailButtonClick
                },
                'coefficientsi2so #table': {
                    click: this.onTableButtonClick
                },
                'coefficientsi2so #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'coefficientsi2so #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'coefficientsi2so #createbutton': {
                    click: this.onCreateButtonClick
                },
                'coefficientsi2so #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'coefficientsi2so #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'coefficientsi2so #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'coefficientsi2so #refresh': {
                    click: this.onRefreshButtonClick
                },
                'coefficientsi2so #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'coefficientsi2so #exportbutton': {
                    click: this.onExportButtonClick
                },
                'coefficientsi2so #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'coefficientsi2so #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'coefficientsi2so #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
