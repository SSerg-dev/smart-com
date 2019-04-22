Ext.define('App.controller.tpm.actualproductsview.ActualProductsView', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'actualproduct[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'actualproduct directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'actualproduct #datatable': {
                    activate: this.onActivateCard
                },
                'actualproduct #detailform': {
                    activate: this.onActivateCard
                },
                'actualproduct #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'actualproduct #detailform #next': {
                    click: this.onNextButtonClick
                },
                'actualproduct #detail': {
                    click: this.onDetailButtonClick
                },
                'actualproduct #table': {
                    click: this.onTableButtonClick
                },
                'actualproduct #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'actualproduct #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'actualproduct #createbutton': {
                    click: this.onCreateButtonClick
                },
                'actualproduct #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'actualproduct #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'actualproduct #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'actualproduct #refresh': {
                    click: this.onRefreshButtonClick
                },
                'actualproduct #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'actualproduct #exportbutton': {
                    click: this.onExportButtonClick
                },
                'actualproduct #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'actualproduct #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'actualproduct #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                // TODO: убрать
                // actualproduct filter (при создании узла иерархии продуктов)
                'filteractualproduct #applyactualproductfilterbutton': {
                    click: this.onApplyProductFilterClick
                }
            }
        });
    },

    //// TODO: убрать
    //onApplyProductFilterClick: function (button) {
    //    var textarea = button.up().up().down('textarea'),
    //        text = textarea.getValue(),
            
    //        actualproductGrid = button.up().up().up().down('actualproduct').down('directorygrid'),
    //        actualproductStore = actualproductGrid.getStore(),
    //        extendedFilter = actualproductStore.extendedFilter;

    //    extendedFilter.filter = this.getProductFilter(text);
    //    extendedFilter.reloadStore();
    //},

    //// TODO: убрать
    //getProductFilter: function (text) {
    //    var textfiltermodel = Ext.create('App.extfilter.core.TextFilterModel');
    //    if (!Ext.isEmpty(text)) {
    //        try {
    //            var filter = Ext.JSON.decode(text);
    //            return textfiltermodel.deserializeFilter(filter);
    //        } catch (e) {
    //            console.error('Parse extended text filter error: ', e);
    //        }
    //    }

    //    return null;
    //}
});
