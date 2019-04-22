Ext.define('App.controller.tpm.demandpricelist.DemandPriceList', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'demandpricelist[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'demandpricelist directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'demandpricelist #datatable': {
                    activate: this.onActivateCard
                },
                'demandpricelist #detailform': {
                    activate: this.onActivateCard
                },
                'demandpricelist #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'demandpricelist #detailform #next': {
                    click: this.onNextButtonClick
                },
                'demandpricelist #detail': {
                    click: this.onDetailButtonClick
                },
                'demandpricelist #table': {
                    click: this.onTableButtonClick
                },
                'demandpricelist #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'demandpricelist #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'demandpricelist #createbutton': {
                    click: this.onCreateButtonClick
                },
                'demandpricelist #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'demandpricelist #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'demandpricelist #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'demandpricelist #refresh': {
                    click: this.onRefreshButtonClick
                },
                'demandpricelist #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'demandpricelist #exportbutton': {
                    click: this.onExportButtonClick
                },
                'demandpricelist #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'demandpricelist #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'demandpricelist #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
