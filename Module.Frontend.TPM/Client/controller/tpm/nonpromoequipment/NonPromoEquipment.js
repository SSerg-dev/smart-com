Ext.define('App.controller.tpm.nonpromoequipment.NonPromoEquipment', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'nonpromoequipment[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'nonpromoequipment directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'nonpromoequipment #datatable': {
                    activate: this.onActivateCard
                },
                'nonpromoequipment #detailform': {
                    activate: this.onActivateCard
                },
                'nonpromoequipment #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'nonpromoequipment #detailform #next': {
                    click: this.onNextButtonClick
                },
                'nonpromoequipment #detail': {
                    click: this.onDetailButtonClick
                },
                'nonpromoequipment #table': {
                    click: this.onTableButtonClick
                },
                'nonpromoequipment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'nonpromoequipment #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'nonpromoequipment #createbutton': {
                    click: this.onCreateButtonClick
                },
                'nonpromoequipment #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'nonpromoequipment #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'nonpromoequipment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'nonpromoequipment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'nonpromoequipment #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'nonpromoequipment #exportbutton': {
                    click: this.onExportButtonClick
                },
                'nonpromoequipment #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'nonpromoequipment #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'nonpromoequipment #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    }
});
