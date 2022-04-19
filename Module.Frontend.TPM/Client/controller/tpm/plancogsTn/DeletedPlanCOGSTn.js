Ext.define('App.controller.tpm.plancogsTn.DeletedPlanCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcogsplanTn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcogsplanTn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcogsplanTn #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcogsplanTn #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcogsplanTn #detail': {
                    click: this.switchToDetailForm
                },
                'deletedcogsplanTn #table': {
                    click: this.onTableButtonClick
                },
                'deletedcogsplanTn #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcogsplanTn #next': {
                    click: this.onNextButtonClick
                },
                'deletedcogsplanTn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcogsplanTn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcogsplanTn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcogsplanTn #close': {
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
