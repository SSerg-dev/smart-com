Ext.define('App.controller.tpm.eventtype.EventType', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'eventtype[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad
                },
                'eventtype directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'eventtype #datatable': {
                    activate: this.onActivateCard
                },
                'eventtype #detailform': {
                    activate: this.onActivateCard
                },
                'eventtype #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'eventtype #detailform #next': {
                    click: this.onNextButtonClick
                },
                'eventtype #detail': {
                    click: this.onDetailButtonClick
                },
                'eventtype #table': {
                    click: this.onTableButtonClick
                },
                'eventtype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'eventtype #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'eventtype #createbutton': {
                    click: this.onCreateButtonClick
                },
                'eventtype #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'eventtype #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'eventtype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'eventtype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'eventtype #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'eventtype #exportbutton': {
                    click: this.onExportButtonClick
                },
                'eventtype #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'eventtype #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'eventtype #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
