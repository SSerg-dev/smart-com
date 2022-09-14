Ext.define('App.controller.tpm.increasebaseline.IncreaseBaseLine', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'increasebaseline[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'increasebaseline directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'increasebaseline #datatable': {
                    activate: this.onActivateCard
                },
                'increasebaseline #detailform': {
                    activate: this.onActivateCard
                },
                'increasebaseline #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'increasebaseline #detailform #next': {
                    click: this.onNextButtonClick
                },
                'increasebaseline #detail': {
                    click: this.onDetailButtonClick
                },
                'increasebaseline #table': {
                    click: this.onTableButtonClick
                },
                'increasebaseline #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'increasebaseline #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'increasebaseline #createbutton': {
                    click: this.onCreateButtonClick
                },
                'increasebaseline #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'increasebaseline #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'increasebaseline #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'increasebaseline #refresh': {
                    click: this.onRefreshButtonClick
                },
                'increasebaseline #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'increasebaseline #exportbutton': {
                    click: this.onExportButtonClick
                },
                'increasebaseline #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'increasebaseline #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'increasebaseline #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});
