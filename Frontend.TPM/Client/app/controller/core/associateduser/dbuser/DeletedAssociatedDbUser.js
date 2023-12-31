Ext.define('App.controller.core.associateduser.dbuser.DeletedAssociatedDbUser', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedassociateddbuseruser directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedassociateddbuseruser directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'deletedassociateddbuseruser #datatable': {
                    activate: this.onActivateCard
                },
                'deletedassociateddbuseruser #detailform': {
                    activate: this.onActivateCard
                },
                'deletedassociateddbuseruser #detail': {
                    click: this.switchToDetailForm
                },
                'deletedassociateddbuseruser #table': {
                    click: this.onTableButtonClick
                },
                'deletedassociateddbuseruser #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedassociateddbuseruser #next': {
                    click: this.onNextButtonClick
                },
                //

                'deletedassociateddbuseruser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedassociateddbuseruser #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedassociateddbuseruser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedassociateddbuseruser #close': {
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
                    xtype: 'historicalassociateddbuseruser',
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
    }
});