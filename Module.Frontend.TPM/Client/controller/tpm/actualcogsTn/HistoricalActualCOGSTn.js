Ext.define('App.controller.tpm.actualcogsTn.HistoricalActualCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalactualcogsTn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalactualcogsTn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalactualcogsTn #datatable': {
                    activate: this.onActivateCard
                },
                'historicalactualcogsTn #detailform': {
                    activate: this.onActivateCard
                },
                'historicalactualcogsTn #detail': {
                    click: this.switchToDetailForm
                },
                'historicalactualcogsTn #table': {
                    click: this.onTableButtonClick
                },
                'historicalactualcogsTn #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalactualcogsTn #next': {
                    click: this.onNextButtonClick
                },
                'historicalactualcogsTn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalactualcogsTn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalactualcogsTn #close': {
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
