Ext.define('App.controller.tpm.plancogsTn.HistoricalPlanCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalplancogsTn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalplancogsTn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalplancogsTn #datatable': {
                    activate: this.onActivateCard
                },
                'historicalplancogsTn #detailform': {
                    activate: this.onActivateCard
                },
                'historicalplancogsTn #detail': {
                    click: this.switchToDetailForm
                },
                'historicalplancogsTn #table': {
                    click: this.onTableButtonClick
                },
                'historicalplancogsTn #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalplancogsTn #next': {
                    click: this.onNextButtonClick
                },
                'historicalplancogsTn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalplancogsTn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalplancogsTn #close': {
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
