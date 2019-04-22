Ext.define('App.controller.tpm.commercialnet.CommercialNet', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'commercialnet[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'commercialnet directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'commercialnet #datatable': {
                    activate: this.onActivateCard
                },
                'commercialnet #detailform': {
                    activate: this.onActivateCard
                },
                'commercialnet #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'commercialnet #detailform #next': {
                    click: this.onNextButtonClick
                },
                'commercialnet #detail': {
                    click: this.onDetailButtonClick
                },
                'commercialnet #table': {
                    click: this.onTableButtonClick
                },
                'commercialnet #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'commercialnet #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'commercialnet #createbutton': {
                    click: this.onCreateButtonClick
                },
                'commercialnet #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'commercialnet #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'commercialnet #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'commercialnet #refresh': {
                    click: this.onRefreshButtonClick
                },
                'commercialnet #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'commercialnet #exportbutton': {
                    click: this.onExportButtonClick
                },
                'commercialnet #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'commercialnet #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'commercialnet #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
