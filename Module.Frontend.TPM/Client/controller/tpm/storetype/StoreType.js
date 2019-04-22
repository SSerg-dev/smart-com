Ext.define('App.controller.tpm.storetype.StoreType', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'storetype[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'storetype directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'storetype #datatable': {
                    activate: this.onActivateCard
                },
                'storetype #detailform': {
                    activate: this.onActivateCard
                },
                'storetype #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'storetype #detailform #next': {
                    click: this.onNextButtonClick
                },
                'storetype #detail': {
                    click: this.onDetailButtonClick
                },
                'storetype #table': {
                    click: this.onTableButtonClick
                },
                'storetype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'storetype #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'storetype #createbutton': {
                    click: this.onCreateButtonClick
                },
                'storetype #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'storetype #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'storetype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'storetype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'storetype #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'storetype #exportbutton': {
                    click: this.onExportButtonClick
                },
                'storetype #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'storetype #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'storetype #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
