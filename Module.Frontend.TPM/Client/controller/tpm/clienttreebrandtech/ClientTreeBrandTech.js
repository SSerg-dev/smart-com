Ext.define('App.controller.tpm.clienttreebrandtech.ClientTreeBrandTech', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'clienttreebrandtech[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'clienttreebrandtech[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'clienttreebrandtech directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'clienttreebrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'clienttreebrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'clienttreebrandtech #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'clienttreebrandtech #detailform #next': {
                    click: this.onNextButtonClick
                },
                'clienttreebrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'clienttreebrandtech #table': {
                    click: this.onTableButtonClick
                },
                'clienttreebrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'clienttreebrandtech #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'clienttreebrandtech #createbutton': {
                    click: this.onCreateButtonClick
                },
                'clienttreebrandtech #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'clienttreebrandtech #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'clienttreebrandtech #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'clienttreebrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'clienttreebrandtech #close': {
                    click: this.onCloseButtonClick
                },
                //// import/export
                'clienttreebrandtech #exportbutton': {
                    click: this.onExportButtonClick
                },
                'clienttreebrandtech #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'clienttreebrandtech #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'clienttreebrandtech #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});