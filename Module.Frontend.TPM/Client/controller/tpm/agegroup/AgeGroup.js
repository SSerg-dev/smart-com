Ext.define('App.controller.tpm.agegroup.AgeGroup', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'agegroup[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'agegroup directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'agegroup #datatable': {
                    activate: this.onActivateCard
                },
                'agegroup #detailform': {
                    activate: this.onActivateCard
                },
                'agegroup #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'agegroup #detailform #next': {
                    click: this.onNextButtonClick
                },
                'agegroup #detail': {
                    click: this.onDetailButtonClick
                },
                'agegroup #table': {
                    click: this.onTableButtonClick
                },
                'agegroup #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'agegroup #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'agegroup #createbutton': {
                    click: this.onCreateButtonClick
                },
                'agegroup #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'agegroup #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'agegroup #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'agegroup #refresh': {
                    click: this.onRefreshButtonClick
                },
                'agegroup #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'agegroup #exportbutton': {
                    click: this.onExportButtonClick
                },
                'agegroup #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'agegroup #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'agegroup #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
