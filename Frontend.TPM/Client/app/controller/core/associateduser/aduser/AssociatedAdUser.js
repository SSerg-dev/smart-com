Ext.define('App.controller.core.associateduser.aduser.AssociatedAdUser', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                'associatedaduseruser[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'associatedaduseruser directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'associatedaduseruser #datatable': {
                    activate: this.onActivateCard
                },
                'associatedaduseruser #detailform': {
                    activate: this.onActivateCard
                },
                'associatedaduseruser #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'associatedaduseruser #detailform #next': {
                    click: this.onNextButtonClick
                },
                'associatedaduseruser #detail': {
                    click: this.switchToDetailForm
                },
                'associatedaduseruser #table': {
                    click: this.onTableButtonClick
                },
                'associatedaduseruser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'associatedaduseruser #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'associatedaduseruser #createbutton': {
                    click: this.onCreateButtonClick
                },
                'associatedaduseruser #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'associatedaduseruser #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'associatedaduseruser #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'associatedaduseruser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'associatedaduseruser #close': {
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

            Ext.widget('basereviewwindow', {
                items: {
                    xtype: 'historicalassociatedaduseruser',
                    baseModel: model
                }
            })
            .show().down('grid').getStore()
            .setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(selModel.getSelection()[0])
            });
        }
    },

    onDeletedButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            model = panel.getBaseModel();

        Ext.widget('basereviewwindow', {
            items: {
                xtype: 'deletedassociatedaduseruser',
                baseModel: model
            }
        })
        .show();
    }
});