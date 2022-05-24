Ext.define('App.controller.tpm.actualcogstn.HistoricalActualCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalactualcogstn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalactualcogstn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalactualcogstn #datatable': {
                    activate: this.onActivateCard
                },
                'historicalactualcogstn #detailform': {
                    activate: this.onActivateCard
                },
                'historicalactualcogstn #detail': {
                    click: this.switchToDetailForm
                },
                'historicalactualcogstn #table': {
                    click: this.onTableButtonClick
                },
                'historicalactualcogstn #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalactualcogstn #next': {
                    click: this.onNextButtonClick
                },
                'historicalactualcogstn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalactualcogstn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalactualcogstn #close': {
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
