Ext.define('App.controller.core.ReportController', {
    extend: 'Ext.app.Controller',

    init: function () {
        this.listen({
            component: {
                'associatedtesttest #foodIntake': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #testCheating': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #testCheatingAgency': {
                    click: this.onReportButtonClick
                },                
                'associatedtesttest #sendToMiniAgency': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #productsSchedule': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #productsScheduleNoOwner': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #productsScheduleAgency': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #productsScheduleNoOwnerAgency': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #exportToGPDW': {
                    click: this.onReportButtonClick
                },


                // Задание на упаковку
                'associatedtesttest #packTask': {
                    click: this.onReportButtonClick
                },
                // Задание на упаковку - агенство
                'associatedtesttest #packTaskAgency': {
                    click: this.onReportButtonClick
                },
                // Этикетки
                'associatedtesttest #tickets': {
                    click: this.onReportButtonClick
                },
                // Этикетки - агенство
                'associatedtesttest #ticketsAgency': {
                    click: this.onReportButtonClick
                },
                // Анкета (без владельца)
                'associatedtesttest #formForHouseholdScalesNoOwner': {
                    click: this.onReportButtonClick
                },
                // Анкета (без владельца) - агенство
                'associatedtesttest #formForHouseholdScalesNoOwnerAgency': {
                    click: this.onReportButtonClick
                },
                // Анкеты для внесения данных с быт. вес.
                'associatedtesttest #formForHouseholdScales': {
                    click: this.onReportButtonClick
                },
                // Анкеты для внесения данных с быт. вес. - агенство
                'associatedtesttest #formForHouseholdScalesAgency': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #sendToMiniAgency': {
                    click: this.onSendToMiniAgency
                },

                // Нарушения условий выполнения задания
                'owner #cheatingsForOwner': {
                    click: this.onReportButtonClick
                },
                // Нарушения условий выполнения задания - агенство
                'owner #cheatingsForOwnerAgency': {
                    click: this.onReportButtonClick
                },

                // Список владельцев
                'owner #ownersList': {
                    click: this.onReportButtonClick
                },
                // Список владельцев - агенство
                'owner #ownersListAgency': {
                    click: this.onReportButtonClick
                },

                // История кормлений животного
                'owner #ownerFeedingHistory': {
                    click: this.onReportButtonClick
                },


                // История кормлений животного
                'associatedtesttest #resultsInDB': {
                    click: this.onReportButtonClick
                },
                'associatedtesttest #spssExport': {
                    click: this.onReportButtonClick
                },            
                // История использования кормов
                'userloophandler #productsHistory': {
                    click: this.onCreateFoodHistoryReport
                },

                // История использования кормов
                'adminloophandler #productsHistory': {
                    click: this.onCreateFoodHistoryReport
                },
            }
        });
    },

    onReportButtonClick: function (button) {
        var me = this;
        var grid = button.up('combineddirectorypanel').down('directorygrid');
        var panel = grid.up('combineddirectorypanel');

        var selection = grid.getSelectionModel().getSelection();
        if (selection.lenght == 0) {
            App.Notify.pushInfo(l10n.ns('core').value('errorNoSelection'));
            return;
        }
        var rec = selection[0];
        var controller = button.resource != "" ? button.resource : "Report";
        var action = button.action != "" ? button.action : "LinkedReport";
        Ext.Ajax.request({
            url: 'api/' + controller + '/' + action,
            method: 'POST',
            params: {
                Id: rec.get('Id'),
                Name: button.itemId,
                Data: button.ReportData,
                SystemName: ResourceMgr.getModuleSettings('core').SystemName
            },

            success: function () {
                App.System.openUserTasksPanel();
            },
            failure: function () {
               
            }
        });
    },

    getErrorMessage: function (data) {
        var result = 'Unknown error';
        if (data && data.msg) {
            result = data.msg;
        } else if (data && data.message) {
            result = data.msg;
        } else if (data && data.body) {
            if (data.body["odata.error"]) {
                result = data.body["odata.error"].innererror.message;
            } else if (data.body.value) {
                result = data.body.value;
            }
        }
        return result;
    },

    onSendToMiniAgency: function (button) {
        var me = this;
        var grid = button.up('combineddirectorypanel').down('directorygrid');
        var panel = grid.up('combineddirectorypanel');

        var selection = grid.getSelectionModel().getSelection();
        if (selection.lenght == 0) {
            App.Notify.pushInfo(l10n.ns('core').value('errorNoSelection'));
            return;
        }
        var rec = selection[0];

        var wg = Ext.widget('basewindow', {
            height: 400,
            width: 600,
            cls: 'mini-agency-style',
            resizable:false,
            items: [{
                labelAlign: 'top',
                xtype: 'combo',
                minHeight: 30,
                maxHeight: 36,
                allowBlank: false,
                selectOnFocus:true,
                fieldLabel: 'Выберите настройку для отправки в агенство',
                displayField: 'Description',
                valueField:'Id',
                store: {
                    model: 'App.model.core.mailnotificationsetting.MailNotificationSetting',
                    filters: [
                        function (item) {
                            return !item.get('IsDisabled');
                        }
                    ]
                },
            }, {
                fieldLabel: 'Текст сообщения:',
                labelAlign:'top',
                xtype: 'textarea',
                maxHeight: 250,
            }],
            title: 'Отправка почты в мини-агенство. Тема сообщения: Тест ' + rec.get('TestCode')
        });
        var okButton = wg.down('component[itemId=ok]');
        okButton.setText('Отправить');
        okButton.on('click', function (sendBtton) {

            var sendWindow = sendBtton.up('window');
            var settingId = sendWindow.down('combo').getValue();
            if (settingId == null) {
                sendWindow.down('combo').focus();
                return;
            }
            var msg = sendWindow.down('textarea').getValue();

            button.ReportData = Ext.JSON.encode({ message: msg, settingId: settingId });
            me.onReportButtonClick(button);
            wg.close();
        });
        wg.show();

    },

    onCreateFoodHistoryReport: function (button) {
     
        var store = Ext.create('App.store.core.DirectoryStore', {
            model: 'App.model.core.test.Test',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.core.report.ProductsHistoryReportModel',
                            modelId: 'efselectionmodel'
                        }, {
                            xclass: 'App.ExtTextFilterModel',
                            modelId: 'eftextmodel'
                        }]
                    },
    }
            );

        var filterWindow = Ext.widget('selectionextfilter', {
           title: 'История использования кормов'
        });
        filterWindow.initComp(store.getExtendedFilter());
        filterWindow.down('toolbar').hide();
        var applayButton = filterWindow.down('component[itemId=createReport]');

        var me = this;
        applayButton.on('click', function (a, b, c) {
            var modelView = filterWindow.modelContainer.child();
            if (modelView) {
                var formPanel = filterWindow.down('form');
                if (formPanel && !formPanel.getForm().isValid()) {
                    return;
                }
                var dateFields = formPanel.query('datefield');
                if (dateFields[0].getValue() == null || dateFields[1].getValue() == null) {
                    App.Notify.pushInfo('Необходимо заполнить диапазон дат');
                    return;
                }
                modelView.commitChanges();
            }
            filterWindow.getFilterContext().commit();
          


            var query = breeze.EntityQuery
            .from('Tests')
            .withParameters({
                $actionName: 'HistoryReport',
                $method: 'POST'
            });

            query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                filterWindow.close();
                App.System.openUserTasksPanel();
            })
            .fail(function (data) {
                filterWindow.close();
                App.Notify.pushError(me.getErrorMessage(data));
            });

        });
        filterWindow.show();
        filterWindow.setHeight(180);
        filterWindow.setWidth(800);
    },


    //CopyPaste from App.controller.core.ImportExportLogic
    buildQuery: function (query, store) {
        var proxy = store.getProxy();
        var extendedFilters = store.getExtendedFilter().getFilter();
        var operation = new Ext.data.Operation({
            action: 'read',
            filters: store.filters.items,
            fixedFilters: store.fixedFilters,
            extendedFilters: extendedFilters,
            sorters: store.sorters.items,
            groupers: store.groupers.items,
            pageMapGeneration: store.data.pageMapGeneration
        });
        query = proxy.applyExpand(query);
        query = proxy.applyFilter(operation, query);
        query = proxy.applyFixedFilter(operation, query);
        query = proxy.applyExtendedFilter(operation, query);
        query = proxy.applySorting(operation, query);
        return query;
    }
});