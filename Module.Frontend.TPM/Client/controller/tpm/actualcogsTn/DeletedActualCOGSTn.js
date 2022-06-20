Ext.define('App.controller.tpm.actualcogstn.DeletedActualCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedactualcogstn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedactualcogstn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedactualcogstn #datatable': {
                    activate: this.onActivateCard
                },
                'deletedactualcogstn #detailform': {
                    activate: this.onActivateCard
                },
                'deletedactualcogstn #detail': {
                    click: this.switchToDetailForm
                },
                'deletedactualcogstn #table': {
                    click: this.onTableButtonClick
                },
                'deletedactualcogstn #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedactualcogstn #next': {
                    click: this.onNextButtonClick
                },
                'deletedactualcogstn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedactualcogstn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedactualcogstn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedactualcogstn #close': {
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
