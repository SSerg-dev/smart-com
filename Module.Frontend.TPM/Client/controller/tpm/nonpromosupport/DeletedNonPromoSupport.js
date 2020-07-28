Ext.define('App.controller.tpm.nonpromosupport.DeletedNonPromoSupport', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletednonpromosupport directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletednonpromosupport directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletednonpromosupport #datatable': {
                    activate: this.onActivateCard
                },
                'deletednonpromosupport #detailform': {
                    activate: this.onActivateCard
                },
                'deletednonpromosupport #detail': {
                    click: this.onDetailButtonClick
                },
                'deletednonpromosupport #table': {
                    click: this.onTableButtonClick
                },
                'deletednonpromosupport #prev': {
                    click: this.onPrevButtonClick
                },
                'deletednonpromosupport #next': {
                    click: this.onNextButtonClick
                },
                'deletednonpromosupport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletednonpromosupport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletednonpromosupport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletednonpromosupport #close': {
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