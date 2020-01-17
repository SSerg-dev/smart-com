Ext.define('App.controller.core.associateduser.dbuser.AssociatedDbUser', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                'associateddbuseruser[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad
                },
                'associateddbuseruser directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'associateddbuseruser #datatable': {
                    activate: this.onActivateCard
                },
                'associateddbuseruser #detailform': {
                    activate: this.onActivateCard
                },
                'associateddbuseruser #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'associateddbuseruser #detailform #next': {
                    click: this.onNextButtonClick
                },
                //

                'associateddbuseruser #detail': {
                    click: this.switchToDetailForm
                },
                'associateddbuseruser #table': {
                    click: this.onTableButtonClick
                },
                'associateddbuseruser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'associateddbuseruser #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'associateddbuseruser #createbutton': {
                    click: this.onCreateButtonClick
                },
                'associateddbuseruser #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'associateddbuseruser #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'associateddbuseruser #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'associateddbuseruser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'associateddbuseruser #close': {
                    click: this.onCloseButtonClick
                },

                'associateddbuseruser #changepassbutton': {
                    click: this.onChangePassButtonClick
                },
                '#changeuserpasswindow #ok': {
                    click: this.onOkChangeUserPassClick
                }
            }
        });
    },


    onChangePassButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var window = Ext.widget('passwordchangingwindow', {
                id: 'changeuserpasswindow'
            });
            window.down('editorform').loadRecord(selModel.getSelection()[0]);
            window.show();
        }
    },

    onOkChangeUserPassClick: function (button) {
        var grid = this.getSelectedGrid(),
            panel = grid.up('combineddirectorypanel'),
            window = button.up('window'),
            form = window.down('editorform'),
            record;

        form.getForm().updateRecord();
        record = form.getRecord();

        if (!form.getForm().isValid()) {
            return;
        }

        panel.setLoading(true);
        window.setLoading(true);
        breeze.EntityQuery
            .from('UserDTOs')
            .withParameters({
                $actionName: 'ChangePassword',
                $method: 'POST',
                $entity: record.getProxy().getBreezeEntityByRecord(record),
                $data: {
                    password: record.get('Password')
                }
            })
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function () {
                grid.setLoadMaskDisabled(true);
                grid.getStore().on({
                    single: true,
                    load: function (records, operation, success) {
                        panel.setLoading(false);
                        grid.setLoadMaskDisabled(false);
                        window.setLoading(false);
                        window.close();
                    }
                });
                grid.getStore().load();
            })
            .fail(function () {
                panel.setLoading(false);
                window.setLoading(false);
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

            store.setFixedFilter('HistoricalObjectId', {
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
                xtype: 'deletedassociateddbuseruser',
                baseModel: model
            }
        })
        .show();
    }
});