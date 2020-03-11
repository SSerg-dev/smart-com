Ext.define('App.controller.core.filebuffer.FileBuffer', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'filebuffer[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick,
                },
                'filebuffer directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'filebuffer #datatable': {
                    activate: this.onActivateCard
                },
                'filebuffer #detailform': {
                    activate: this.onActivateCard
                },
                'filebuffer #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'filebuffer #detailform #next': {
                    click: this.onNextButtonClick
                },
                'filebuffer #detail': {
                    click: this.onDetailButtonClick
                },
                'filebuffer #table': {
                    click: this.onTableButtonClick
                },
                'filebuffer #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'filebuffer #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'filebuffer #createbutton': {
                    click: this.onCreateButtonClick
                },
                'filebuffer #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'filebuffer #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'filebuffer #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'filebuffer #refresh': {
                    click: this.onRefreshButtonClick
                },
                'filebuffer #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'filebuffer #exportbutton': {
                    click: this.onExportButtonClick
                },
                'filebuffer #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'filebuffer #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'filebuffer #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'filebuffer #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});