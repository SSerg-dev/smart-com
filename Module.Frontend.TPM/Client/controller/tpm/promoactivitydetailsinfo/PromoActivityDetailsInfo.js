Ext.define('App.controller.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoactivitydetailsinfo[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoactivitydetailsinfo directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoactivitydetailsinfo #datatable': {
                    activate: this.onActivateCard
                },
                'promoactivitydetailsinfo #detailform': {
                    activate: this.onActivateCard
                },
                'promoactivitydetailsinfo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoactivitydetailsinfo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoactivitydetailsinfo #detail': {
                    click: this.onDetailButtonClick
                },
                'promoactivitydetailsinfo #table': {
                    click: this.onTableButtonClick
                },
                'promoactivitydetailsinfo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoactivitydetailsinfo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoactivitydetailsinfo #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoactivitydetailsinfo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoactivitydetailsinfo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoactivitydetailsinfo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoactivitydetailsinfo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoactivitydetailsinfo #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoactivitydetailsinfo #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoactivitydetailsinfo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoactivitydetailsinfo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoactivitydetailsinfo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
});