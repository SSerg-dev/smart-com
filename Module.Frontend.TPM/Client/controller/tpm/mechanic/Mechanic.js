Ext.define('App.controller.tpm.mechanic.Mechanic', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'mechanic[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'mechanic directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'mechanic #datatable': {
                    activate: this.onActivateCard
                },
                'mechanic #detailform': {
                    activate: this.onActivateCard
                },
                'mechanic #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'mechanic #detailform #next': {
                    click: this.onNextButtonClick
                },
                'mechanic #detail': {
                    click: this.onDetailButtonClick
                },
                'mechanic #table': {
                    click: this.onTableButtonClick
                },
                'mechanic #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'mechanic #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'mechanic #createbutton': {
                    click: this.onCreateButtonClick
                },
                'mechanic #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'mechanic #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'mechanic #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'mechanic #refresh': {
                    click: this.onRefreshButtonClick
                },
                'mechanic #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'mechanic #exportbutton': {
                    click: this.onExportButtonClick
                },
                'mechanic #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'mechanic #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'mechanic #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
