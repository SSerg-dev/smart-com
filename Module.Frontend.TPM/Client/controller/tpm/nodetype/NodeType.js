Ext.define('App.controller.tpm.nodetype.NodeType', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'nodetype[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick,
                },
                'nodetype directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'nodetype #datatable': {
                    activate: this.onActivateCard
                },
                'nodetype #detailform': {
                    activate: this.onActivateCard
                },
                'nodetype #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'nodetype #detailform #next': {
                    click: this.onNextButtonClick
                },
                'nodetype #detail': {
                    click: this.onDetailButtonClick
                },
                'nodetype #table': {
                    click: this.onTableButtonClick
                },
                'nodetype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'nodetype #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'nodetype #createbutton': {
                    click: this.onCreateButtonClick
                },
                'nodetype #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'nodetype #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'nodetype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'nodetype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'nodetype #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'nodetype #exportbutton': {
                    click: this.onExportButtonClick
                },
                'nodetype #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'nodetype #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'nodetype #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
