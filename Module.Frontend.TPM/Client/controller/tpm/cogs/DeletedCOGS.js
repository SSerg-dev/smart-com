Ext.define('App.controller.tpm.cogs.DeletedCOGS', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcogs directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcogs directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcogs #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcogs #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcogs #detail': {
                    click: this.switchToDetailForm
                },
                'deletedcogs #table': {
                    click: this.onTableButtonClick
                },
                'deletedcogs #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcogs #next': {
                    click: this.onNextButtonClick
                },
                'deletedcogs #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcogs #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcogs #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcogs #close': {
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
