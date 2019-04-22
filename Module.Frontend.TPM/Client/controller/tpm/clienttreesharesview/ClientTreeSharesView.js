Ext.define('App.controller.tpm.clienttreesharesview.ClientTreeSharesView', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'clienttreesharesview[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'clienttreesharesview[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'clienttreesharesview directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'clienttreesharesview #datatable': {
                    activate: this.onActivateCard
                },
                'clienttreesharesview #detailform': {
                    activate: this.onActivateCard
                },
                'clienttreesharesview #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'clienttreesharesview #detailform #next': {
                    click: this.onNextButtonClick
                },
                'clienttreesharesview #detail': {
                    click: this.onDetailButtonClick
                },
                'clienttreesharesview #table': {
                    click: this.onTableButtonClick
                },
                'clienttreesharesview #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'clienttreesharesview #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'clienttreesharesview #createbutton': {
                    click: this.onCreateButtonClick
                },
                'clienttreesharesview #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'clienttreesharesview #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'clienttreesharesview #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'clienttreesharesview #refresh': {
                    click: this.onRefreshButtonClick
                },
                'clienttreesharesview #close': {
                    click: this.onCloseButtonClick
                },
                //// import/export
                'clienttreesharesview #exportbutton': {
                    click: this.onExportButtonClick
                },
                'clienttreesharesview #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'clienttreesharesview #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'clienttreesharesview #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});