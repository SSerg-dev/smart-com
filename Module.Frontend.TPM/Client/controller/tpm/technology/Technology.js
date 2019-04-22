Ext.define('App.controller.tpm.technology.Technology', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'technology[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'technology directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'technology #datatable': {
                    activate: this.onActivateCard
                },
                'technology #detailform': {
                    activate: this.onActivateCard
                },
                'technology #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'technology #detailform #next': {
                    click: this.onNextButtonClick
                },
                'technology #detail': {
                    click: this.onDetailButtonClick
                },
                'technology #table': {
                    click: this.onTableButtonClick
                },
                'technology #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'technology #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'technology #createbutton': {
                    click: this.onCreateButtonClick
                },
                'technology #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'technology #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'technology #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'technology #refresh': {
                    click: this.onRefreshButtonClick
                },
                'technology #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'technology #exportbutton': {
                    click: this.onExportButtonClick
                },
                'technology #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'technology #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'technology #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
