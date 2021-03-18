Ext.define('App.controller.tpm.ratishopper.HistoricalRATIShopper', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalratishopper directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalratishopper directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalratishopper #datatable': {
                    activate: this.onActivateCard
                },
                'historicalratishopper #detailform': {
                    activate: this.onActivateCard
                },
                'historicalratishopper #detail': {
                    click: this.switchToDetailForm
                },
                'historicalratishopper #table': {
                    click: this.onTableButtonClick
                },
                'historicalratishopper #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalratishopper #next': {
                    click: this.onNextButtonClick
                },
                'historicalratishopper #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalratishopper #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalratishopper #close': {
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
