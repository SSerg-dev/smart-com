Ext.define('App.controller.tpm.planpostpromoeffect.DeletedPlanPostPromoEffect', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedplanpostpromoeffect directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedplanpostpromoeffect directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedplanpostpromoeffect #datatable': {
                    activate: this.onActivateCard
                },
                'deletedplanpostpromoeffect #detailform': {
                    activate: this.onActivateCard
                },
                'deletedplanpostpromoeffect #detail': {
                    click: this.switchToDetailForm
                },
                'deletedplanpostpromoeffect #table': {
                    click: this.onTableButtonClick
                },
                'deletedplanpostpromoeffect #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedplanpostpromoeffect #next': {
                    click: this.onNextButtonClick
                },
                'deletedplanpostpromoeffect #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedplanpostpromoeffect #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedplanpostpromoeffect #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedplanpostpromoeffect #close': {
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
