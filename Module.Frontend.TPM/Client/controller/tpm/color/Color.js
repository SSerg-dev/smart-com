Ext.define('App.controller.tpm.color.Color', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'color[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'color directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'color #datatable': {
                    activate: this.onActivateCard
                },
                'color #detailform': {
                    activate: this.onActivateCard
                },
                'color #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'color #detailform #next': {
                    click: this.onNextButtonClick
                },
                'color #detail': {
                    click: this.onDetailButtonClick
                },
                'color #table': {
                    click: this.onTableButtonClick
                },
                'color #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'color #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'color #createbutton': {
                    click: this.onCreateButtonClick
                },
                'color #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'color #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'color #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'color #refresh': {
                    click: this.onRefreshButtonClick
                },
                'color #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'color #exportbutton': {
                    click: this.onExportButtonClick
                },
                'color #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'color #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'color #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
