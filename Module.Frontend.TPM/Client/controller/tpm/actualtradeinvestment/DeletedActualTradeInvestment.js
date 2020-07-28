Ext.define('App.controller.tpm.actualtradeinvestment.DeletedActualTradeInvestment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedactualtradeinvestment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedactualtradeinvestment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedactualtradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'deletedactualtradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'deletedactualtradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'deletedactualtradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'deletedactualtradeinvestment #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedactualtradeinvestment #next': {
                    click: this.onNextButtonClick
                },
                'deletedactualtradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedactualtradeinvestment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedactualtradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedactualtradeinvestment #close': {
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
