Ext.define('App.controller.tpm.product.Product', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'product[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'product directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'product #datatable': {
                    activate: this.onActivateCard
                },
                'product #detailform': {
                    activate: this.onActivateCard
                },
                'product #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'product #detailform #next': {
                    click: this.onNextButtonClick
                },
                'product #detail': {
                    click: this.onDetailButtonClick
                },
                'product #table': {
                    click: this.onTableButtonClick
                },
                'product #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'product #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'product #createbutton': {
                    click: this.onCreateButtonClick
                },
                'product #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'product #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'product #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'product #refresh': {
                    click: this.onRefreshButtonClick
                },
                'product #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'product #exportbutton': {
                    click: this.onExportButtonClick
                },
                'product #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'product #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'product #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                // TODO: убрать
                // product filter (при создании узла иерархии продуктов)
                'filterproduct #applyproductfilterbutton': {
                    click: this.onApplyProductFilterClick
                }
            }
        });
    },

    onDetailButtonClick: function (button) {
        this.callParent(arguments);
        if (button.up('productlist')) {
            Ext.ComponentQuery.query('producteditor #edit')[0].setVisible(false);
        }
    },

    // TODO: убрать
    onApplyProductFilterClick: function (button) {
        var textarea = button.up().up().down('textarea'),
            text = textarea.getValue(),
            
            productGrid = button.up().up().up().down('product').down('directorygrid'),
            productStore = productGrid.getStore(),
            extendedFilter = productStore.extendedFilter;

        extendedFilter.filter = this.getProductFilter(text);
        extendedFilter.reloadStore();
    },

    // TODO: убрать
    getProductFilter: function (text) {
        var textfiltermodel = Ext.create('App.extfilter.core.TextFilterModel');
        if (!Ext.isEmpty(text)) {
            try {
                var filter = Ext.JSON.decode(text);
                return textfiltermodel.deserializeFilter(filter);
            } catch (e) {
                console.error('Parse extended text filter error: ', e);
            }
        }

        return null;
    }
});
