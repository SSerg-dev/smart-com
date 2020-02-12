Ext.define('App.controller.tpm.actualtradeinvestment.HistoricalActualTradeInvestment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalactualtradeinvestment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalactualtradeinvestment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalactualtradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'historicalactualtradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'historicalactualtradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'historicalactualtradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'historicalactualtradeinvestment #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalactualtradeinvestment #next': {
                    click: this.onNextButtonClick
                },
                'historicalactualtradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalactualtradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalactualtradeinvestment #close': {
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
