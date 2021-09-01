Ext.define('App.controller.core.rpasetting.RPASetting', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'rpasetting[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'rpasetting[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'rpasetting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'rpasetting #datatable': {
                    activate: this.onActivateCard
                },
                'rpasetting #detailform': {
                    activate: this.onActivateCard
                },
                'rpasetting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'rpasetting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'rpasetting #detail': {
                    click: this.onDetailButtonClick
                },
                'rpasetting #table': {
                    click: this.onTableButtonClick
                },
                'rpasetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'rpasetting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'rpasetting #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'rpasetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'rpasetting #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'rpasetting #exportbutton': {
                    click: this.onExportButtonClick
                },
                'rpasetting #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'rpasetting #loadimporttemplatecsvbutton': {
                    click: this.onLoadImportTemplateCSVButtonClick
                },
                'rpasetting #loadimporttemplatexlsxbutton': {
                    click: this.onLoadImportTemplateXLSXButtonClick
                },
                'rpasetting #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});