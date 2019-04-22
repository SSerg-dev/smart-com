Ext.define('App.controller.tpm.retailtype.RetailType', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'retailtype[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'retailtype directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'retailtype #datatable': {
                    activate: this.onActivateCard
                },
                'retailtype #detailform': {
                    activate: this.onActivateCard
                },
                'retailtype #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'retailtype #detailform #next': {
                    click: this.onNextButtonClick
                },
                'retailtype #detail': {
                    click: this.switchToDetailForm
                },
                'retailtype #table': {
                    click: this.onTableButtonClick
                },
                'retailtype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'retailtype #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'retailtype #createbutton': {
                    click: this.onCreateButtonClick
                },
                'retailtype #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'retailtype #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'retailtype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'retailtype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'retailtype #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'retailtype #exportbutton': {
                    click: this.onExportButtonClick
                },
                'retailtype #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'retailtype #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'retailtype #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
