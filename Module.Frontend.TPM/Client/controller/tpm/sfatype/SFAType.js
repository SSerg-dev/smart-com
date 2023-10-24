Ext.define('App.controller.tpm.sfatype.SFAType', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'sfatype[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'sfatype directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'sfatype #datatable': {
                    activate: this.onActivateCard
                },
                'sfatype #detailform': {
                    activate: this.onActivateCard
                },
                'sfatype #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'sfatype #detailform #next': {
                    click: this.onNextButtonClick
                },
                'sfatype #detail': {
                    click: this.switchToDetailForm
                },
                'sfatype #table': {
                    click: this.onTableButtonClick
                },
                'sfatype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'sfatype #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'sfatype #createbutton': {
                    click: this.onCreateButtonClick
                },
                'sfatype #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'sfatype #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'sfatype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'sfatype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'sfatype #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'sfatype #exportbutton': {
                    click: this.onExportButtonClick
                },
                'sfatype #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'sfatype #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'sfatype #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
