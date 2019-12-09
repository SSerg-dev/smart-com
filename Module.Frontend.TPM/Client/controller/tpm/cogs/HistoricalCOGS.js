Ext.define('App.controller.tpm.cogs.HistoricalCOGS', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcogs directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcogs directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcogs #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcogs #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcogs #detail': {
                    click: this.switchToDetailForm
                },
                'historicalcogs #table': {
                    click: this.onTableButtonClick
                },
                'historicalcogs #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcogs #next': {
                    click: this.onNextButtonClick
                },
                'historicalcogs #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcogs #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcogs #close': {
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
            store.setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(selModel.getSelection()[0])
            });
        }
    }
});
