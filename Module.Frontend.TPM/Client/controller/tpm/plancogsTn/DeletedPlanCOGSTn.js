Ext.define('App.controller.tpm.plancogstn.DeletedPlanCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedplancogstn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedplancogstn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedplancogstn #datatable': {
                    activate: this.onActivateCard
                },
                'deletedplancogstn #detailform': {
                    activate: this.onActivateCard
                },
                'deletedplancogstn #detail': {
                    click: this.switchToDetailForm
                },
                'deletedplancogstn #table': {
                    click: this.onTableButtonClick
                },
                'deletedplancogstn #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedplancogstn #next': {
                    click: this.onNextButtonClick
                },
                'deletedplancogstn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedplancogstn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedplancogstn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedplancogstn #close': {
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
