Ext.define('App.controller.tpm.sale.Sale', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'sale[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'sale[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick
                },
                'sale directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'sale #datatable': {
                    activate: this.onActivateCard
                },
                'sale #detailform': {
                    activate: this.onActivateCard
                },
                'sale #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'sale #detailform #next': {
                    click: this.onNextButtonClick
                },
                'sale #detail': {
                    click: this.onDetailButtonClick
                },
                'sale #table': {
                    click: this.onTableButtonClick
                },
                'sale #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'sale #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'sale #createbutton': {
                    click: this.onCreateCustomButtonClick
                },
                'sale #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'sale #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'sale #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'sale #refresh': {
                    click: this.onRefreshButtonClick
                },
                'sale #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'sale #exportbutton': {
                    click: this.onExportButtonClick
                },
                'sale #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'sale #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'sale #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onCreateCustomButtonClick: function (button) {
        var me = this;
        var grid = this.getGridByButton(button);

        if (grid.inPromoForm) {
            var store = grid.getStore();
            var model = Ext.create(Ext.ModelManager.getModel(store.model));

            if (grid.promoId) {
                model.data.PromoId = grid.promoId;
            }

            grid.editorModel.startCreateRecord(model);
        }
        else {
            me.onCreateButtonClick(button);
        }
    }
});
