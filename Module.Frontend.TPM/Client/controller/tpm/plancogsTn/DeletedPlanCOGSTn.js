Ext.define('App.controller.tpm.plancogsTn.DeletedPlanCOGSTn', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedplancogsTn directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedplancogsTn directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedplancogsTn #datatable': {
                    activate: this.onActivateCard
                },
                'deletedplancogsTn #detailform': {
                    activate: this.onActivateCard
                },
                'deletedplancogsTn #detail': {
                    click: this.switchToDetailForm
                },
                'deletedplancogsTn #table': {
                    click: this.onTableButtonClick
                },
                'deletedplancogsTn #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedplancogsTn #next': {
                    click: this.onNextButtonClick
                },
                'deletedplancogsTn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedplancogsTn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedplancogsTn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedplancogsTn #close': {
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
