Ext.define('App.controller.tpm.actualcogs.DeletedActualCOGS', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedactualcogs directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedactualcogs directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedactualcogs #datatable': {
                    activate: this.onActivateCard
                },
                'deletedactualcogs #detailform': {
                    activate: this.onActivateCard
                },
                'deletedactualcogs #detail': {
                    click: this.switchToDetailForm
                },
                'deletedactualcogs #table': {
                    click: this.onTableButtonClick
                },
                'deletedactualcogs #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedactualcogs #next': {
                    click: this.onNextButtonClick
                },
                'deletedactualcogs #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedactualcogs #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedactualcogs #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedactualcogs #close': {
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

            store.setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(selModel.getSelection()[0])
            });
        }
    }
});
