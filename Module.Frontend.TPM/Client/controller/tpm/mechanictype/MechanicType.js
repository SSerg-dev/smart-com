Ext.define('App.controller.tpm.mechanictype.MechanicType', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'mechanictype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'mechanictype #datatable': {
                    activate: this.onActivateCard
                },
                'mechanictype #detailform': {
                    activate: this.onActivateCard
                },
                'mechanictype #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'mechanictype #detailform #next': {
                    click: this.onNextButtonClick
                },
                'mechanictype #detail': {
                    click: this.onDetailButtonClick
                },
                'mechanictype #table': {
                    click: this.onTableButtonClick
                },
                'mechanictype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'mechanictype #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'mechanictype #createbutton': {
                    click: this.onCreateButtonClick
                },
                'mechanictype #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'mechanictype #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'mechanictype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'mechanictype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'mechanictype #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'mechanictype #exportbutton': {
                    click: this.onExportButtonClick
                },
                'mechanictype #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'mechanictype #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'mechanictype #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
