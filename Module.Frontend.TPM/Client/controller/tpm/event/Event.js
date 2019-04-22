Ext.define('App.controller.tpm.event.Event', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'event[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick,
                },
                'event directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'event #datatable': {
                    activate: this.onActivateCard
                },
                'event #detailform': {
                    activate: this.onActivateCard
                },
                'event #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'event #detailform #next': {
                    click: this.onNextButtonClick
                },
                'event #detail': {
                    click: this.onDetailButtonClick
                },
                'event #table': {
                    click: this.onTableButtonClick
                },
                'event #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'event #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'event #createbutton': {
                    click: this.onCreateButtonClick
                },
                'event #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'event #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'event #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'event #refresh': {
                    click: this.onRefreshButtonClick
                },
                'event #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'event #exportbutton': {
                    click: this.onExportButtonClick
                },
                'event #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'event #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'event #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
