Ext.define('App.controller.tpm.calendarcompetitorbrandtechcolor.CalendarCompetitorBrandTechColor', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'calendarcompetitorbrandtechcolor[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'calendarcompetitorbrandtechcolor directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'calendarcompetitorbrandtechcolor #datatable': {
                    activate: this.onActivateCard
                },
                'calendarcompetitorbrandtechcolor #detailform': {
                    activate: this.onActivateCard
                },
                'calendarcompetitorbrandtechcolor #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'calendarcompetitorbrandtechcolor #detailform #next': {
                    click: this.onNextButtonClick
                },
                'calendarcompetitorbrandtechcolor #detail': {
                    click: this.onDetailButtonClick
                },
                'calendarcompetitorbrandtechcolor #table': {
                    click: this.onTableButtonClick
                },
                'calendarcompetitorbrandtechcolor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'calendarcompetitorbrandtechcolor #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'calendarcompetitorbrandtechcolor #createbutton': {
                    click: this.onCreateButtonClick
                },
                'calendarcompetitorbrandtechcolor #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'calendarcompetitorbrandtechcolor #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'calendarcompetitorbrandtechcolor #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'calendarcompetitorbrandtechcolor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'calendarcompetitorbrandtechcolor #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'calendarcompetitorbrandtechcolor #exportbutton': {
                    click: this.onExportButtonClick
                },
                'calendarcompetitorbrandtechcolor #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'calendarcompetitorbrandtechcolor #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'calendarcompetitorbrandtechcolor #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
