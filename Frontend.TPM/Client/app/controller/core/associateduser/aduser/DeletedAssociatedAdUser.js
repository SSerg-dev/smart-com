Ext.define('App.controller.core.associateduser.aduser.DeletedAssociatedAdUser', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedassociatedaduseruser directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedassociatedaduseruser directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'deletedassociatedaduseruser #datatable': {
                    activate: this.onActivateCard
                },
                'deletedassociatedaduseruser #detailform': {
                    activate: this.onActivateCard
                },
                'deletedassociatedaduseruser #detail': {
                    click: this.switchToDetailForm
                },
                'deletedassociatedaduseruser #table': {
                    click: this.onTableButtonClick
                },
                'deletedassociatedaduseruser #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedassociatedaduseruser #next': {
                    click: this.onNextButtonClick
                },
                //

                'deletedassociatedaduseruser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedassociatedaduseruser #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedassociatedaduseruser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedassociatedaduseruser #close': {
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
                model = panel.getBaseModel();

            var baseReviewWindow = Ext.widget('basereviewwindow', {
                items: {
                    xtype: 'historicalassociatedaduseruser',
                    baseModel: model
                }
            });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            if (proxy.extraParams) {
                proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
            } else {
                proxy.extraParams = {
                    Id: this.getRecordId(selModel.getSelection()[0])
                }
            }
        }
    },
});