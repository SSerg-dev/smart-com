Ext.define('App.controller.tpm.calendarcompetitorcompany.CalendarCompetitorCompany', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'calendarcompetitorcompany[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'calendarcompetitorcompany directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'calendarcompetitorcompany #datatable': {
                    activate: this.onActivateCard
                },
                'calendarcompetitorcompany #detailform': {
                    activate: this.onActivateCard
                },
                'calendarcompetitorcompany #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'calendarcompetitorcompany #detailform #next': {
                    click: this.onNextButtonClick
                },
                'calendarcompetitorcompany #detail': {
                    click: this.onDetailButtonClick
                },
                'calendarcompetitorcompany #table': {
                    click: this.onTableButtonClick
                },
                'calendarcompetitorcompany #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'calendarcompetitorcompany #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'calendarcompetitorcompany #createbutton': {
                    click: this.onCreateButtonClick
                },
                'calendarcompetitorcompany #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'calendarcompetitorcompany #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'calendarcompetitorcompany #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'calendarcompetitorcompany #refresh': {
                    click: this.onRefreshButtonClick
                },
                'calendarcompetitorcompany #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'calendarcompetitorcompany #exportbutton': {
                    click: this.onExportButtonClick
                },
                'calendarcompetitorcompany #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'calendarcompetitorcompany #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'calendarcompetitorcompany #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
