Ext.define('App.controller.tpm.metricslivehistory.MetricsLiveHistory', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'metricslivehistory[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'metricslivehistory[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'metricslivehistory directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'metricslivehistory #datatable': {
                    activate: this.onActivateCard
                },
                'metricslivehistory #detailform': {
                    activate: this.onActivateCard
                },
                'metricslivehistory #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'metricslivehistory #detailform #next': {
                    click: this.onNextButtonClick
                },
                'metricslivehistory #detail': {
                    click: this.onDetailButtonClick
                },
                'metricslivehistory #table': {
                    click: this.onTableButtonClick
                },
                'metricslivehistory #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'metricslivehistory #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'metricslivehistory #createbutton': {
                    click: this.onCreateButtonClick
                },
                'metricslivehistory #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'metricslivehistory #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'metricslivehistory #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'metricslivehistory #refresh': {
                    click: this.onRefreshButtonClick
                },
                'metricslivehistory #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'metricslivehistory #exportbutton': {
                    click: this.onExportButtonClick
                },
                'metricslivehistory #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'metricslivehistory #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'metricslivehistory #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});