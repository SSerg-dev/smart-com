Ext.define('App.controller.tpm.planpostpromoeffect.HistoricalPlanPostPromoEffect', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalplanpostpromoeffect directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalplanpostpromoeffect directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalplanpostpromoeffect #datatable': {
                    activate: this.onActivateCard
                },
                'historicalplanpostpromoeffect #detailform': {
                    activate: this.onActivateCard
                },
                'historicalplanpostpromoeffect #detail': {
                    click: this.switchToDetailForm
                },
                'historicalplanpostpromoeffect #table': {
                    click: this.onTableButtonClick
                },
                'historicalplanpostpromoeffect #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalplanpostpromoeffect #next': {
                    click: this.onNextButtonClick
                },
                'historicalplanpostpromoeffect #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalplanpostpromoeffect #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalplanpostpromoeffect #close': {
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
