Ext.define('App.controller.tpm.tradeinvestment.DeletedTradeInvestment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedtradeinvestment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedtradeinvestment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedtradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'deletedtradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'deletedtradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'deletedtradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'deletedtradeinvestment #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedtradeinvestment #next': {
                    click: this.onNextButtonClick
                },
                'deletedtradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedtradeinvestment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedtradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedtradeinvestment #close': {
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
