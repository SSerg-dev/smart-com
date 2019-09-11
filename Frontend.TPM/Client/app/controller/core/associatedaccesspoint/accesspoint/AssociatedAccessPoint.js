Ext.define('App.controller.core.associatedaccesspoint.accesspoint.AssociatedAccessPoint', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'associatedaccesspointaccesspoint[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'associatedaccesspointaccesspoint directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'associatedaccesspointaccesspoint #datatable': {
                    activate: this.onActivateCard
                },
                'associatedaccesspointaccesspoint #detailform': {
                    activate: this.onActivateCard
                },
                'associatedaccesspointaccesspoint #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'associatedaccesspointaccesspoint #detailform #next': {
                    click: this.onNextButtonClick
                },
                'associatedaccesspointaccesspoint #detail': {
                    click: this.switchToDetailForm
                },
                'associatedaccesspointaccesspoint #table': {
                    click: this.onTableButtonClick
                },
                'associatedaccesspointaccesspoint #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'associatedaccesspointaccesspoint #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'associatedaccesspointaccesspoint #createbutton': {
                    click: this.onCreateButtonClick
                },
                'associatedaccesspointaccesspoint #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'associatedaccesspointaccesspoint #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'associatedaccesspointaccesspoint #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'associatedaccesspointaccesspoint #refresh': {
                    click: this.onRefreshButtonClick
                },
                'associatedaccesspointaccesspoint #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'associatedaccesspointaccesspoint #exportbutton': {
                    click: this.onExportButtonClick
                },
                'associatedaccesspointaccesspoint #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'associatedaccesspointaccesspoint #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'associatedaccesspointaccesspoint #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'associatedaccesspointaccesspoint #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});