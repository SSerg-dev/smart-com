Ext.define('App.controller.core.loophandler.AdminLoopHandler', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'adminloophandler[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm
                },
                'adminloophandler directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'adminloophandler #datatable': {
                    activate: this.onActivateCard
                },
                'adminloophandler #detailform': {
                    activate: this.onActivateCard
                },
                'adminloophandler #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'adminloophandler #detailform #next': {
                    click: this.onNextButtonClick
                },
                'adminloophandler #detail': {
                    click: this.switchToDetailForm
                },
                'adminloophandler #table': {
                    click: this.onTableButtonClick
                },
                'adminloophandler #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'adminloophandler #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'adminloophandler #createbutton': {
                    click: this.onCreateButtonClick
                },
                'adminloophandler #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'adminloophandler #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'adminloophandler #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'adminloophandler #refresh': {
                    click: this.onRefreshButtonClick
                },
                'adminloophandler #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'adminloophandler #exportbutton': {
                    click: this.onExportButtonClick
                },
                'adminloophandler #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'adminloophandler #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'adminloophandler #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'adminloophandler #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});