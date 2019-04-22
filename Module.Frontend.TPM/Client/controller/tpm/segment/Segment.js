Ext.define('App.controller.tpm.segment.Segment', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'segment[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'segment directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'segment #datatable': {
                    activate: this.onActivateCard
                },
                'segment #detailform': {
                    activate: this.onActivateCard
                },
                'segment #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'segment #detailform #next': {
                    click: this.onNextButtonClick
                },
                'segment #detail': {
                    click: this.onDetailButtonClick
                },
                'segment #table': {
                    click: this.onTableButtonClick
                },
                'segment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'segment #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'segment #createbutton': {
                    click: this.onCreateButtonClick
                },
                'segment #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'segment #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'segment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'segment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'segment #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'segment #exportbutton': {
                    click: this.onExportButtonClick
                },
                'segment #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'segment #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'segment #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
