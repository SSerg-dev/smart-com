﻿Ext.define('App.controller.tpm.promo.DeletedPromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromo directorygrid': {
                    itemdblclick: this.switchToDetailForm,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridDeltedPromoAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromo #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromo #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromo #detail': {
                    click: this.switchToDetailForm
                },
                'deletedpromo #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromo #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromo #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    },

    onGridDeltedPromoAfterrender: function (grid) {
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        if (mode) {
            if (mode.data.value != 1) {
                var indexh = this.getColumnIndex(grid, 'TPMmode');
                grid.columnManager.getColumns()[indexh].hide();
            }
            else {
                var deletedPromoGridStore = grid.getStore();
                var deletedPromoGridStoreProxy = deletedPromoGridStore.getProxy();
                deletedPromoGridStoreProxy.extraParams.TPMmode = 'RS';
            }
        }
        this.onGridAfterrender(grid);
    },

    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            if (proxy.extraParams) {
                proxy.extraParams.promoIdHistory = this.getRecordId(selModel.getSelection()[0]);
            } else {
                proxy.extraParams = {
                    promoIdHistory: this.getRecordId(selModel.getSelection()[0])
                }
            }
        }
    },

    switchToDetailForm: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (!grid.editorModel || grid.editorModel.name != 'ToChangeEditorDetailWindowModel') {
            grid.editorModel = Ext.create('App.model.tpm.utils.ToChangeEditorDetailWindowModel', {
                grid: grid
            });
        }    
        if (selModel.hasSelection()) {
            grid.editorModel.startDetailRecord(selModel.getSelection()[0]);
        } else {
            console.log('No selection');
        }
    }
});
