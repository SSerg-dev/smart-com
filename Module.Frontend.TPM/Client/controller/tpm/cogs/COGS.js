Ext.define('App.controller.tpm.cogs.COGS', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'cogs[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'cogs directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'cogs #datatable': {
                    activate: this.onActivateCard
                },
                'cogs #detailform': {
                    activate: this.onActivateCard
                },
                'cogs #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'cogs #detailform #next': {
                    click: this.onNextButtonClick
                },
                'cogs #detail': {
                    click: this.switchToDetailForm
                },
                'cogs #table': {
                    click: this.onTableButtonClick
                },
                'cogs #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'cogs #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'cogs #createbutton': {
                    click: this.onCreateButtonClick
                },
                'cogs #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'cogs #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'cogs #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'cogs #refresh': {
                    click: this.onRefreshButtonClick
                },
                'cogs #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'cogs #exportbutton': {
                    click: this.onExportButtonClick
                },
                'cogs #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'cogs #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'cogs #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
