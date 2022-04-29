Ext.define('App.controller.tpm.plancogstn.HistoricalPlanCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalplancogstn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalplancogstn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalplancogstn #datatable': {
                    activate: this.onActivateCard
                },
                'historicalplancogstn #detailform': {
                    activate: this.onActivateCard
                },
                'historicalplancogstn #detail': {
                    click: this.switchToDetailForm
                },
                'historicalplancogstn #table': {
                    click: this.onTableButtonClick
                },
                'historicalplancogstn #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalplancogstn #next': {
                    click: this.onNextButtonClick
                },
                'historicalplancogstn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalplancogstn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalplancogstn #close': {
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
