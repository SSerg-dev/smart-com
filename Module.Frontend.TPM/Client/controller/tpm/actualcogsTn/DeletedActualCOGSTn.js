Ext.define('App.controller.tpm.actualcogsTn.DeletedActualCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedactualcogsTn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedactualcogsTn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedactualcogsTn #datatable': {
                    activate: this.onActivateCard
                },
                'deletedactualcogsTn #detailform': {
                    activate: this.onActivateCard
                },
                'deletedactualcogsTn #detail': {
                    click: this.switchToDetailForm
                },
                'deletedactualcogsTn #table': {
                    click: this.onTableButtonClick
                },
                'deletedactualcogsTn #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedactualcogsTn #next': {
                    click: this.onNextButtonClick
                },
                'deletedactualcogsTn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedactualcogsTn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedactualcogsTn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedactualcogsTn #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
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
            proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
        }
    }
});
