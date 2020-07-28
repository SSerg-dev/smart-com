Ext.define('App.controller.tpm.actualcogs.HistoricalActualCOGS', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalactualcogs directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalactualcogs directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalactualcogs #datatable': {
                    activate: this.onActivateCard
                },
                'historicalactualcogs #detailform': {
                    activate: this.onActivateCard
                },
                'historicalactualcogs #detail': {
                    click: this.switchToDetailForm
                },
                'historicalactualcogs #table': {
                    click: this.onTableButtonClick
                },
                'historicalactualcogs #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalactualcogs #next': {
                    click: this.onNextButtonClick
                },
                'historicalactualcogs #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalactualcogs #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalactualcogs #close': {
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
