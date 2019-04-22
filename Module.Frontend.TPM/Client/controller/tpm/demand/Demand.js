Ext.define('App.controller.tpm.demand.Demand', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'demand[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'demand directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'demand #datatable': {
                    activate: this.onActivateCard
                },
                'demand #detailform': {
                    activate: this.onActivateCard
                },
                'demand #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'demand #detailform #next': {
                    click: this.onNextButtonClick
                },
                'demand #detail': {
                    click: this.onDetailButtonClick
                },
                'demand #table': {
                    click: this.onTableButtonClick
                },
                'demand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'demand #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'demand #createbutton': {
                    click: this.onCreateButtonClick
                },
                'demand #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'demand #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'demand #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'demand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'demand #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'demand #exportbutton': {
                    click: this.onExportButtonClick
                },
                'demand #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'demand #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'demand #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
