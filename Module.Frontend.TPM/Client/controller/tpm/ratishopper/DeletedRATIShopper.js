Ext.define('App.controller.tpm.ratishopper.DeletedRATIShopper', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedratishopper directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedratishopper directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedratishopper #datatable': {
                    activate: this.onActivateCard
                },
                'deletedratishopper #detailform': {
                    activate: this.onActivateCard
                },
                'deletedratishopper #detail': {
                    click: this.switchToDetailForm
                },
                'deletedratishopper #table': {
                    click: this.onTableButtonClick
                },
                'deletedratishopper #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedratishopper #next': {
                    click: this.onNextButtonClick
                },
                'deletedratishopper #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedratishopper #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedratishopper #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedratishopper #close': {
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
