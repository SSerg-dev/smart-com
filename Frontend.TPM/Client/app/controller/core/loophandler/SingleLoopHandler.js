Ext.define('App.controller.core.loophandler.SingleLoopHandler', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'singleloophandler[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm
                },
                'singleloophandler directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'singleloophandler #datatable': {
                    activate: this.onActivateCard
                },
                'singleloophandler #detailform': {
                    activate: this.onActivateCard
                },
                'singleloophandler #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'singleloophandler #detailform #next': {
                    click: this.onNextButtonClick
                },
                'singleloophandler #detail': {
                    click: this.switchToDetailForm
                },
                'singleloophandler #table': {
                    click: this.onTableButtonClick
                },
                'singleloophandler #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'singleloophandler #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'singleloophandler #refresh': {
                    click: this.onRefreshButtonClick
                },
                'singleloophandler #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'singleloophandler #exportbutton': {
                    click: this.onExportButtonClick
                },
                'singleloophandler #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'singleloophandler #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'singleloophandler #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'singleloophandler #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});