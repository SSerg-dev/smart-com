Ext.define('App.controller.tpm.tradeinvestment.HistoricalTradeInvestment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaltradeinvestment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaltradeinvestment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaltradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'historicaltradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'historicaltradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'historicaltradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'historicaltradeinvestment #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaltradeinvestment #next': {
                    click: this.onNextButtonClick
                },
                'historicaltradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaltradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaltradeinvestment #close': {
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
