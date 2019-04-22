Ext.define('App.controller.tpm.techhighlevel.TechHighLevel', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'techhighlevel[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'techhighlevel directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'techhighlevel #datatable': {
                    activate: this.onActivateCard
                },
                'techhighlevel #detailform': {
                    activate: this.onActivateCard
                },
                'techhighlevel #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'techhighlevel #detailform #next': {
                    click: this.onNextButtonClick
                },
                'techhighlevel #detail': {
                    click: this.onDetailButtonClick
                },
                'techhighlevel #table': {
                    click: this.onTableButtonClick
                },
                'techhighlevel #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'techhighlevel #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'techhighlevel #createbutton': {
                    click: this.onCreateButtonClick
                },
                'techhighlevel #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'techhighlevel #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'techhighlevel #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'techhighlevel #refresh': {
                    click: this.onRefreshButtonClick
                },
                'techhighlevel #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'techhighlevel #exportbutton': {
                    click: this.onExportButtonClick
                },
                'techhighlevel #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'techhighlevel #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'techhighlevel #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
