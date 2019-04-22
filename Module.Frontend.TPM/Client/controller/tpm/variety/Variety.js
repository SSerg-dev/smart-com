Ext.define('App.controller.tpm.variety.Variety', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'variety[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'variety directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'variety #datatable': {
                    activate: this.onActivateCard
                },
                'variety #detailform': {
                    activate: this.onActivateCard
                },
                'variety #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'variety #detailform #next': {
                    click: this.onNextButtonClick
                },
                'variety #detail': {
                    click: this.onDetailButtonClick
                },
                'variety #table': {
                    click: this.onTableButtonClick
                },
                'variety #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'variety #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'variety #createbutton': {
                    click: this.onCreateButtonClick
                },
                'variety #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'variety #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'variety #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'variety #refresh': {
                    click: this.onRefreshButtonClick
                },
                'variety #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'variety #exportbutton': {
                    click: this.onExportButtonClick
                },
                'variety #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'variety #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'variety #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
