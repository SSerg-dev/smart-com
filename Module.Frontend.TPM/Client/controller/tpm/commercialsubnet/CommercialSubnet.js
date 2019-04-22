Ext.define('App.controller.tpm.commercialsubnet.CommercialSubnet', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'commercialsubnet[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'commercialsubnet directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'commercialsubnet #datatable': {
                    activate: this.onActivateCard
                },
                'commercialsubnet #detailform': {
                    activate: this.onActivateCard
                },
                'commercialsubnet #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'commercialsubnet #detailform #next': {
                    click: this.onNextButtonClick
                },
                'commercialsubnet #detail': {
                    click: this.onDetailButtonClick
                },
                'commercialsubnet #table': {
                    click: this.onTableButtonClick
                },
                'commercialsubnet #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'commercialsubnet #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'commercialsubnet #createbutton': {
                    click: this.onCreateButtonClick
                },
                'commercialsubnet #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'commercialsubnet #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'commercialsubnet #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'commercialsubnet #refresh': {
                    click: this.onRefreshButtonClick
                },
                'commercialsubnet #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'commercialsubnet #exportbutton': {
                    click: this.onExportButtonClick
                },
                'commercialsubnet #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'commercialsubnet #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'commercialsubnet #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
