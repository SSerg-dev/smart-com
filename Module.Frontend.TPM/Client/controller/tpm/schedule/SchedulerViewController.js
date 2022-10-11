Ext.define('App.controller.tpm.schedule.SchedulerViewController', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.tpm.promo.Promo'],
    alias: 'controller.schedulerviewcontroller',

    rowCount: 3,
    startEndModel: null,
    canEditInRSmode: boolean = false,

    init: function () {
        this.listen({
            component: {
                'schedulecontainer': {
                    afterrender: function () {
                        var createButton = Ext.ComponentQuery.query('#createbutton')[0];
                        if (createButton) {
                            if (!this.getAllowedActionsForCurrentRoleAndResource('Promoes').some(function (action) { return action === createButton.action; })) {
                                createButton.hide();
                            }
                        }
                    }
                },
                'schedulecontainer #shiftprevbutton': {
                    click: this.onShiftPrevButtonClick
                },
                'schedulecontainer #shiftnextbutton': {
                    click: this.onShiftNextButtonClick
                },
                'schedulecontainer component[itemgroup=shiftpresetbutton]': {
                    click: this.onShiftPresetButtonClick
                },
                'schedulecontainer component[itemgroup=shiftmodebutton]': {
                    click: this.onShiftModeButtonClick
                },
                'schedulecontainer #nascheduler': {
                    afterrender: this.onScheduleAfterRender,
                    eventclick: this.onEventClick,
                    eventdblclick: this.onEventdbClick,
                    extfilterchange: this.onExtFilterChange,
                    beforeeventresize: this.onpromoBeforeEventResize,
                    beforeeventresizefinalize: this.onEventResize,
                    beforedragcreatefinalize: this.onPromoDragCreation,
                    eventcontextmenu: this.promoRightButtonClick,

                    beforeeventdrag: this.onpromoBeforeEventDrag,
                    beforeeventdropfinalize: this.onpromoBeforeEventDrop,

                    rowselectionchange: this.highlightRow,
                },
                'schedulecontainer #nascheduler [xtype=gridpanel]': {
                    load: this.onResourceStoreLoad,
                    resize: this.onResizeGrid
                },
                'schedulecontainer #clientsPromoTypeFilterLabel': {
                    afterrender: this.onClientsPromoTypeFilterAfterrender
                },
                'readonlydirectorytoolbar #promoDetail': {
                    click: this.onPromoDetailButtonClick
                },
                'schedulecontainer #refresh': {
                    click: this.onRefreshButtonClick
                },
                'schedulecontainer #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'schedulecontainer #createbuttonall': {
                    click: this.onAllCreateButtonClick
                },
                'scheduletypewindow #ok': {
                    click: this.onPromoTypeOkButtonClick
                },
                'scheduletypewindow button[itemId!=ok]': {
                    click: this.onSelectionButtonClick
                },
                'scheduletypewindow': {
                    afterrender: this.onPromoTypeAfterRender
                },
                'schedulecontainer #createinoutbutton': {
                    click: this.onCreateInOutButtonClick
                },
                'schedulecontainer #schedulefilterdraftpublbutton': {
                    click: this.onFilterDraftPublButtonClick
                },
                'schedulecontainer #schedulefilterdraftbutton': {
                    click: this.onFilterDraftButtonClick
                },

                'promodetailtabpanel #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'schedulecontainer #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'schedulecontainer #ExportYearSchedule': {
                    click: this.onExportSchedulerButtonClick
                }
            }
        })
    },

    onpromoBeforeEventResize: function (scheduler, record, e, eOpts) {
        return this.isResizable(record);
    },

    isResizable: function (rec) {
        var res = true;
        if (rec.get('TypeName') == 'Competitor') {
            res = false;
        }
        if (rec.get('MasterPromoId') != null) {
            res = false;
        }
        if (rec.get('IsOnHold') == true) {
            res = false;
        }
        return res;
    },

    onpromoBeforeEventDrag: function (scheduler, record, e, eOpts) {
        return this.isDraggable(record);
    },

    isDraggable: function (rec) {
        var res = false;
        if (App.UserInfo.getCurrentRole()['SystemName'] == 'SupportAdministrator' && rec.get('TypeName') != 'Competitor') {
            res = true;
        } else {
            res = rec.get('PromoStatusSystemName') && (['Draft', 'DraftPublished', 'OnApproval', 'Approved', 'Planned'].includes(rec.get('PromoStatusSystemName')) && rec.get('TypeName') != 'Competitor');
        }
        if (rec.get('TypeName') != 'Competitor' && rec.get('MasterPromoId') != null) {
            res = false;
        }
        if (rec.get('IsOnHold') == true) {
            res = false;
        }
        return res;
    },

    onpromoBeforeEventDrop: function (view, dragContext, e, eOpts) {
        var me = this;
        var record = dragContext.eventRecords[0];
        me.__dragContext = dragContext;
        var calendarGrid = Ext.ComponentQuery.query('scheduler');
        var dispatchesStart = record.get('DispatchesStart');
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        var calcDispatchesStart = dragContext.startDate.setDate(dragContext.startDate.getDate() - 15);
        if (calendarGrid.length > 0) {
            me.calendarSheduler = calendarGrid[0];
        }
        if (mode) {
            if (mode.data.value == 1) {
                var RSmodeController = App.app.getController('tpm.rsmode.RSmode');
                RSmodeController.getRSPeriod(function (returnValue) {
                    StartDateRS = new Date(returnValue.StartDate);
                    EndDateRS = new Date(returnValue.EndDate);
                });

            if (calcDispatchesStart < StartDateRS || EndDateRS < calcDispatchesStart) {
                App.Notify.pushInfo(l10n.ns('tpm', 'text').value('wrongRSPeriodDates'));
                dragContext.finalize(false);
                return false;
                }
            } 
        }
        if (dragContext.timeDiff == 0) {
            dragContext.finalize(false);
            return false;
        } else if (dragContext.startDate < new Date(new Date().toDateString()) && App.UserInfo.getCurrentRole()['SystemName'] != 'SupportAdministrator') {
            App.Notify.pushInfo(l10n.ns('tpm', 'text').value('wrongStartDate'));
            dragContext.finalize(false);
            return false;
        } else {
            //Ext.WindowMgr.zseed = 10000000;
            var confWindow = Ext.Msg.show({
                title: 'Please confirm',
                msg: 'Do you want to update the Promo: ' + record.get('Name'),
                buttons: Ext.Msg.YESNO,
                icon: Ext.Msg.QUESTION,
                fn: function (btn) {
                    me.onDragAndDropConfirm(btn)
                },
            });
            confWindow.addCls('always-on-top');
            return false;
        }
    },
    //
    onDragAndDropConfirm: function (btn) {
        var me = this;
        if (btn === 'yes') {
            var newMonth = me.__dragContext.startDate.getMonth() + 1;
            if (newMonth == 1 || newMonth == 12) {
                var selectbudgetyearwindow = Ext.widget('selectbudgetyearwindow');
                selectbudgetyearwindow.context = me.__dragContext;
                selectbudgetyearwindow.down('#okBudgetYearButton').on('click', this.onOkBudgetYearWindowDragClick, this);
                selectbudgetyearwindow.on('afterrender', this.afterBudgetYearWindowDragRender, this);
                selectbudgetyearwindow.on('close', this.onBudgetYearWindowClose, this);
                selectbudgetyearwindow.addCls('always-on-top');
                selectbudgetyearwindow.show();
            }
            else {
                me.onBudgetYearDropChoose();
            }
        }
        else {
            me.__dragContext.finalize(false);
        }
    },

    onOkBudgetYearWindowDragClick: function (button) {
        var combo = Ext.ComponentQuery.query('#budgetYearCombo')[0];
        var budgetYear = combo.getValue();
        if (budgetYear !== null) {
            var win = Ext.WindowManager.getActive();
            if (win) {
                win.isConfirmed = true;
                win.close();
            }
            this.onBudgetYearDropChoose(button, budgetYear);
        }
    },

    afterBudgetYearWindowDragRender: function (window) {
        var me = this;
        var comboStore = window.down('combobox').getStore();
        comboStore.removeAll();
        var month = me.__dragContext.startDate.getMonth() + 1;
        var year = me.__dragContext.startDate.getFullYear();
        var userRole = App.UserInfo.getCurrentRole()['SystemName'];
        if (['SupportAdministrator', 'DemandFinance'].includes(userRole)) {
            comboStore.add({ year: year - 2 });
            comboStore.add({ year: year - 1 });
            comboStore.add({ year: year });
            comboStore.add({ year: year + 1 });
        }
        else {
            if (month == 1) {
                year--;
            }
            comboStore.add({ year: year });
            comboStore.add({ year: year + 1 });
        }
    },

    onBudgetYearWindowClose: function (window) {
        if (!window.isConfirmed) {
            window.context.finalize(false);
        }
    },

    // Подтверждение изменения времени promo и установка DispatchesDate аналогично переносу 2 концов
    onBudgetYearDropChoose: function (btn, budgetYear) {
        var me = this;
        var dragContext = me.__dragContext;
        me.calendarSheduler.setLoading(true);
        var eventRecord = dragContext.eventRecords[0],
            resourceRecord = dragContext.resourceRecord,
            promoStore = me.getPromoStore();

        promoStore.load({
            id: eventRecord.getId(),
            scope: this,
            callback: function (records, operation, success) {
                var record = records[0];

                var daysForDispatchDateFromClientSettings = this.getDaysForDispatchDateFromClientSettings(
                    resourceRecord.data.IsBeforeStart, resourceRecord.data.DaysStart, resourceRecord.data.IsDaysStart);

                var dispatchDateForCurrentClientAfterResize = null;
                var dispatchDateForCurrentPromoAfterResize = null;

                var deltaDaysBeforeAndAfterResize = dragContext.timeDiff / 3600 / 24 / 1000;

                // Если настройки dispatch клиента корректны
                if (daysForDispatchDateFromClientSettings !== null) {

                    //Начало
                    dispatchDateForCurrentClientAfterResize = Ext.Date.add(
                        dragContext.startDate, Ext.Date.DAY, daysForDispatchDateFromClientSettings);

                    record.set('DispatchesStart', dispatchDateForCurrentClientAfterResize);
                }
                else {
                    dispatchDateForCurrentPromoAfterResize = Ext.Date.add(record.get('DispatchesStart'), Ext.Date.DAY, deltaDaysBeforeAndAfterResize);
                    record.set('DispatchesStart', dispatchDateForCurrentPromoAfterResize);
                }

                daysForDispatchDateFromClientSettings = null;
                //Конец
                daysForDispatchDateFromClientSettings = this.getDaysForDispatchDateFromClientSettings(
                    resourceRecord.data.IsBeforeEnd, resourceRecord.data.DaysEnd, resourceRecord.data.IsDaysEnd);

                if (daysForDispatchDateFromClientSettings !== null) {
                    dispatchDateForCurrentClientAfterResize = Ext.Date.add(
                        dragContext.endDate, Ext.Date.DAY, daysForDispatchDateFromClientSettings);
                    //т.к в  dragContext.endDate в дате присутствует 23ч 59м 59с убираем их
                    dispatchDateForCurrentClientAfterResize = Ext.Date.add(dispatchDateForCurrentClientAfterResize, Ext.Date.SECOND, 1);
                    dispatchDateForCurrentClientAfterResize = Ext.Date.add(dispatchDateForCurrentClientAfterResize, Ext.Date.DAY, -1);

                    record.set('DispatchesEnd', dispatchDateForCurrentClientAfterResize);
                }
                else {
                    dispatchDateForCurrentPromoAfterResize = Ext.Date.add(record.get('DispatchesEnd'), Ext.Date.DAY, deltaDaysBeforeAndAfterResize);
                    record.set('DispatchesEnd', dispatchDateForCurrentPromoAfterResize);
                }
                var dispStart = record.get('DispatchesStart'),
                    dispEnd = record.get('DispatchesEnd'),
                    startDispatchDateBiggerThanEnd = dispStart > dispEnd,
                    endDispatchDateLessThanStart = dispEnd < dispStart;

                // Если dispatch start и dispatch end наехали друг на друга, то показываем ошибку и возвращаем исходные параметры dispatch
                if (startDispatchDateBiggerThanEnd === true || endDispatchDateLessThanStart === true) {
                    record.reject();
                    dragContext.finalize(false);

                    App.Notify.pushInfo('Dispatch start date must be less than dispatch end date.');
                } else {
                    //Возврат выделения
                    me.calendarSheduler.down('gridview').on('refresh', (function () { me.highlightRow(null, new Array(resourceRecord)); }));
                    // выравниваем время с учётом часового пояса
                    var offset = dragContext.startDate.getTimezoneOffset() / 60.0;
                    record.set('StartDate', Sch.util.Date.add(dragContext.startDate, Sch.util.Date.HOUR, -(offset + 3)));
                    var fixedEndDate = new Date(dragContext.endDate.getFullYear(), dragContext.endDate.getMonth(), dragContext.endDate.getDate(), 0, 0, 0);
                    fixedEndDate = Sch.util.Date.add(fixedEndDate, Sch.util.Date.HOUR, -(offset + 3));
                    record.set('EndDate', fixedEndDate);
                    if (budgetYear != null)
                        record.set('BudgetYear', budgetYear);
                    else {
                        budgetYear = dragContext.startDate.getFullYear();
                        record.set('BudgetYear', budgetYear);
                    }
                    record.save({
                        callback: function (record, operation, success) {
                            if (success) {
                                dragContext.finalize(true);
                                me.eventStoreLoading(me.calendarSheduler.getEventStore());
                                me.calendarSheduler.setLoading(false);
                            } else {
                                me.calendarSheduler.setLoading(false);
                                dragContext.finalize(false);
                            }
                        }
                    });
                }

            }
        })
    },

    onExportSchedulerButtonClick: function (button) {
        var selectyearwindow = Ext.widget('selectyearwindow');
        selectyearwindow.down('#exportforyear').on('click', this.onExportForYearButtonClick, this, button);
        selectyearwindow.show();
    },

    onExportForYearButtonClick: function (button, scope, panelButton) {
        var me = this;
        var selectyearwindow = button.up('selectyearwindow'),
            year = selectyearwindow.down("numberfield").getValue();

        setTimeout(App.System.openUserTasksPanel, 0);

        me.sendExportRequest(year);
    },

    sendExportRequest: function (year) {
        var me = this,
            scheduler = Ext.ComponentQuery.query('schedulecontainer')[0].down('scheduler'),
            store = scheduler.getEventStore(),
            clientStore = scheduler.getResourceStore(),
            competitorNames = [],
            typeNames = [],
            competitorCheckBoxes = scheduler.competitorsCheckboxesConfig,
            typeCheckBoxes = scheduler.typesCheckboxesConfig,
            ids = clientStore.getRange(0, clientStore.getCount() - 1).map(
                function (client) {
                    return client.get('ObjectId')
                }),
            actionName = 'ExportSchedule',
            resource = 'PromoViews';

        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        if (mode) {
            if (mode.data.value == 1) {
                resource = 'PromoRSViews';
            }
        }

        competitorCheckBoxes.map(
            function (checkbox) {
                if (checkbox.checked == true)
                    competitorNames.push(checkbox.inputValue);
            });
        typeCheckBoxes.map(
            function (checkbox) {
                if (checkbox.checked == true)
                    typeNames.push(checkbox.inputValue);
            });

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                $data: {
                    clients: ids,
                    competitors: competitorNames,
                    types: typeNames,
                    year: year
                }
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                var userLoopHandler = Ext.ComponentQuery.query('userloophandler')[0];
                var userLoopHandlerGrid = userLoopHandler.down('grid');
                var selectionModel = userLoopHandlerGrid.getSelectionModel();
                var record = selectionModel.getSelection()[0];
                var userLoopHandlerStore = userLoopHandlerGrid.getStore();
                var loopHandlerViewLogWindow = me.loopHandlerViewLogWindowCreate(record);
                var textField = loopHandlerViewLogWindow.down('field[name=logtext]');
                var messageForNotify = l10n.ns('tpm', 'Schedule').value('ExportTaskCreated') + ' ' + l10n.ns('tpm', 'Schedule').value('ExportTaskDetailsPath');

                textField.setValue(l10n.ns('tpm', 'Schedule').value('ExportBeforeMessageLog') + '\n');

                // После создания задачи экспорта нужно выделить первую запись (только что созданная задача).
                userLoopHandlerStore.on({
                    scope: this,
                    single: true,
                    load: function (store, records) {
                        // Если вызывается из календаря.
                        if (Ext.ComponentQuery.query('schedulecontainer')[0]) {
                            record = records[0];
                            selectionModel.select(record);
                        }

                        // Показываем окно с логом.
                        var calculatingInfoWindow = Ext.create('App.view.tpm.promocalculating.CalculatingInfoWindow', { handlerId: record.get('Id') });
                        var downloadSchedulerFileBtn = calculatingInfoWindow.down('#downloadSchedulerFile');

                        downloadSchedulerFileBtn.setVisible(true);
                        calculatingInfoWindow.on({
                            beforeclose: function (window) {
                                if ($.connection.tasksLogHub)
                                    $.connection.tasksLogHub.server.unsubscribeLog(window.handlerId);

                                App.Notify.pushInfo(messageForNotify);
                            }
                        });

                        calculatingInfoWindow.show();
                        Ext.ComponentQuery.query('selectyearwindow')[0].close();

                        calculatingInfoWindow.down('triggerfield[name=Status]').on({
                            change: function (field) {
                                if (field.hasCls('completeField')) {
                                    // Получение ссылки для скачивания файла.
                                    breeze.EntityQuery
                                        .from('LoopHandlers')
                                        .withParameters({
                                            $actionName: 'Parameters',
                                            $method: 'POST',
                                            $entity: record.getProxy().getBreezeEntityByRecord(record)
                                        })
                                        .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                                        .execute()
                                        .then(function (data) {
                                            var resultData = data.httpResponse.data.value;
                                            var result = JSON.parse(resultData);
                                            var fileName = result.OutcomingParameters.File.Value.Name;

                                            if (fileName) {
                                                downloadSchedulerFileBtn.addListener('click', function () {
                                                    location.assign(document.location.href + '/api/File/ExportDownload?filename=' + fileName);
                                                });

                                                downloadSchedulerFileBtn.setDisabled(false);
                                            } else {
                                                App.Notify.pushError(l10n.ns('tpm', 'Schedule').value('ExportFileIsNotReady'));
                                            }
                                        })
                                        .fail(function (data) {
                                            App.Notify.pushError(l10n.ns('tpm', 'Schedule').value('ExportFileIsNotReady'));
                                        });
                                }
                            }
                        })

                        $.connection.tasksLogHub.server.subscribeLog(record.get('Id'));

                    }
                });

                userLoopHandlerStore.load();
            })
            .fail(function (data) {
                scheduler.setLoading(false);
                if (data.status == 500) {
                    App.Notify.pushError(data.body.value);
                } else {
                    App.Notify.pushInfo(data.body.value);
                }
            });
    },

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
        query = proxy.applyExtendedFilter(operation, query);
        query = proxy.applySorting(operation, query);
        query = proxy.applyPaging(operation, query);
        query = query.inlineCount();
        return query;
    },

    onHistoryButtonClick: function (button) {
        var promoDetailPanel = button.up('promodetailtabpanel');
        var record = promoDetailPanel.event;

        if (record) {
            if (record.get('CompetitorName') != null) {
                var model = Ext.ModelManager.getModel('App.model.tpm.competitorpromo.CompetitorPromo'),
                    viewClassName = "App.view.tpm.competitorpromo.HistoricalCompetitorPromo";

                var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
                baseReviewWindow.show();

                var grid = baseReviewWindow.down('grid');
                var store = baseReviewWindow.down('grid').getStore();
                var proxy = store.getProxy();
                if (proxy.extraParams) {
                    proxy.extraParams.Id = this.getRecordId(record);
                } else {
                    proxy.extraParams = {
                        Id: this.getRecordId(record)
                    }
                }

                proxy.extraParams.promoIdHistory = this.getRecordId(record);

                store.on({
                    load: function (records, operation, success) {
                        var selModel = grid.getSelectionModel();

                        if (!selModel.hasSelection() && records.data.length > 0) {
                            selModel.select(0);
                            grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                        } else if (selModel.hasSelection() && records.data.length > 0) {
                            var selected = selModel.getSelection()[0];
                            if (store.indexOfId(selected.getId()) === -1) {
                                selModel.select(0);
                                grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                            }
                        } else if (records.data.length === 0) {
                            selModel.deselectAll();
                        }
                    }
                });

                store.load();
            }
            else {
                var model = Ext.ModelManager.getModel('App.model.tpm.promo.Promo'),
                    viewClassName = "App.view.tpm.promo.CustomHistoricalPromo";

                var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
                baseReviewWindow.show();

                var grid = baseReviewWindow.down('grid');
                var store = baseReviewWindow.down('grid').getStore();
                var proxy = store.getProxy();
                if (proxy.extraParams) {
                    proxy.extraParams.Id = this.getRecordId(record);
                } else {
                    proxy.extraParams = {
                        Id: this.getRecordId(record)
                    }
                }

                proxy.extraParams.promoIdHistory = this.getRecordId(record);

                store.on({
                    load: function (records, operation, success) {
                        var selModel = grid.getSelectionModel();

                        if (!selModel.hasSelection() && records.data.length > 0) {
                            selModel.select(0);
                            grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                        } else if (selModel.hasSelection() && records.data.length > 0) {
                            var selected = selModel.getSelection()[0];
                            if (store.indexOfId(selected.getId()) === -1) {
                                selModel.select(0);
                                grid.fireEvent('itemclick', grid, grid.getSelectionModel().getLastSelected());
                            }
                        } else if (records.data.length === 0) {
                            selModel.deselectAll();
                        }
                    }
                });

                store.load();
            }
        }
    },

    onDeletedButtonClick: function (button) {
        this.createDeletedWindow(button).show();
    },

    createDeletedWindow: function (button) {
        var model = Ext.ModelManager.getModel('App.model.tpm.promo.Promo');
        var viewClassName = "App.view.tpm.promo.DeletedPromo";

        var window = Ext.widget('basereviewwindow', {
            items: Ext.create(viewClassName, {
                baseModel: model
            })
        });
        return window;
    },

    getRecordId: function (record) {
        var idProperty = record.idProperty;
        return record.get(idProperty);
    },

    onFilterDraftPublButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        var store = scheduler.getEventStore();

        var ids = [];
        var nodes = [];
        //var baseFilter = store.fixedFilters['statusfilter']; // Сохраняем фильтр по типу
        //var ids = ['statusfilter'];
        //var nodes = [{
        //    property: baseFilter.property,
        //    operation: baseFilter.operation,
        //    value: baseFilter.value,
        //}];
        //
        if (!button.hasCls('sheduler_promostatusfilter_button_selected')) {
            ids.push('PromoStatusfilter');
            nodes.push({
                property: 'PromoStatusSystemName',
                operation: 'Equals',
                value: 'DraftPublished'
            });
            //store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, false);

            this.eventStoreLoading(store);
            button.up('custombigtoolbar').down('#schedulefilterdraftbutton').removeCls('sheduler_promostatusfilter_button_selected');
            button.addClass('sheduler_promostatusfilter_button_selected');
        } else {
            delete store.fixedFilters['PromoStatusfilter'];
            button.up('custombigtoolbar').down('#schedulefilterdraftbutton').removeCls('sheduler_promostatusfilter_button_selected');
            //store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, false);
            this.eventStoreLoading(store);
            button.removeCls('sheduler_promostatusfilter_button_selected');
        }
    },

    onFilterDraftButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        var store = scheduler.getEventStore();
        var ids = [];
        var nodes = [];

        //var baseFilter = store.fixedFilters['statusfilter']; // Сохраняем фильтр по типу
        //var ids = ['statusfilter'];
        //var nodes = [{
        //    property: baseFilter.property,
        //    operation: baseFilter.operation,
        //    value: baseFilter.value,
        //}];

        if (!button.hasCls('sheduler_promostatusfilter_button_selected')) {
            ids.push('PromoStatusfilter');
            nodes.push({
                property: 'PromoStatusSystemName',
                operation: 'Equals',
                value: 'Draft'
            });
            //store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, false);
            this.eventStoreLoading(store);
            button.up('custombigtoolbar').down('#schedulefilterdraftpublbutton').removeCls('sheduler_promostatusfilter_button_selected');
            button.addClass('sheduler_promostatusfilter_button_selected');
        } else {
            delete store.fixedFilters['PromoStatusfilter'];
            button.up('custombigtoolbar').down('#schedulefilterdraftpublbutton').removeCls('sheduler_promostatusfilter_button_selected');
            //store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, false);
            this.eventStoreLoading(store);
            button.removeCls('sheduler_promostatusfilter_button_selected');
        }
    },
    promoRightButtonClick: function (panel, rec, e) {
        var me = this;
        if (rec.get('TypeName') == 'Competitor') return;
        if (rec.get('MasterPromoId') != null) return;
        if (rec.get('IsOnHold') == true) return;
        e.stopEvent();
        var status = rec.get('PromoStatusSystemName').toLowerCase();
        var promoStore = me.getPromoStore();
        var mode = this.getTPMmode();
        var isDeletable = status == 'draft' || status == 'draftpublished';
        var isEditable = false;
        var isPlannable = false;
        if (App.UserInfo.getCurrentRole()['SystemName'] == 'SupportAdministrator') {
            isDeletable = true;
        }
        if (['Administrator', 'CMManager', 'CustomerMarketing', 'FunctionalExpert', 'KeyAccountManager', 'DemandPlanning'].includes(App.UserInfo.getCurrentRole()['SystemName'])) {
            isEditable = true;
        }
        if ((status == 'onapproval' || status == 'approved') && mode == 'RS') {
            isDeletable = true;
        }
        if ((status == 'planned' || status == 'started' || status == 'finished') && mode == 'RS') {
            isEditable = false;
        }
        if (['Administrator', 'KeyAccountManager', 'FunctionalExpert'].includes(App.UserInfo.getCurrentRole()['SystemName']) && status == 'approved' && mode != 'RS') {
            isPlannable = true;
        }
        var postAccess = me.getAllowedActionsForCurrentRoleAndResource('Promoes').some(function (action) { return action === 'Post' });
        if (!panel.ctx) {
            panel.ctx = new Ext.menu.Menu({
                width: 90,
                items: [{
                    text: l10n.ns('tpm', 'Schedule').value('Copy'),
                    glyph: 0xf18f,
                    hidden: !postAccess,
                    handler: function () {
                        panel.setLoading(true);
                        promoStore.load({
                            id: panel.ctx.recId,
                            scope: this,
                            callback: function (records, operation, success) {
                                if (success && records[0]) {
                                    panel.eventCopy = records[0];
                                    panel.setLoading(false);
                                } else {
                                    panel.setLoading(false);
                                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedCopy'))
                                }
                            }
                        });
                    },

                }, {
                    itemId: 'promodeletebutton',
                    text: l10n.ns('tpm', 'Schedule').value('Delete'),
                    glyph: 0xf5e8,
                    hidden: !postAccess,
                    handler: function () {
                        Ext.Msg.show({
                            title: l10n.ns('core').value('deleteWindowTitle'),
                            msg: l10n.ns('core').value('deleteConfirmMessage'),
                            fn: onMsgBoxClose,
                            scope: this,
                            icon: Ext.Msg.QUESTION,
                            buttons: Ext.Msg.YESNO,
                            buttonText: {
                                yes: l10n.ns('core', 'buttons').value('delete'),
                                no: l10n.ns('core', 'buttons').value('cancel')
                            }
                        });
                        function onMsgBoxClose(buttonId) {
                            // Удаление Промо
                            if (buttonId === 'yes') {
                                panel.setLoading(true);
                                promoStore.load({
                                    id: panel.ctx.recId,
                                    scope: this,
                                    callback: function (records, operation, success) {
                                        if (success && records[0]) {
                                            records[0].destroy({
                                                scope: this,
                                                success: function () {
                                                    me.eventStoreLoading(panel.getEventStore());
                                                    panel.setLoading(false);
                                                },
                                                failure: function () {
                                                    panel.setLoading(false);
                                                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'))
                                                }
                                            });;
                                        } else {
                                            panel.setLoading(false);
                                            App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                                        }
                                    }
                                });
                            }
                        }
                    }
                }, {
                    itemId: 'promoeditbutton',
                    text: l10n.ns('tpm', 'Schedule').value('Edit'),
                    glyph: 0xf64f,
                    hidden: !postAccess,
                    handler: function (button) {
                        // RSmode
                        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
                        var mode = settingStore.findRecord('name', 'mode');
                        if (mode) {
                            if (mode.data.value == 0) {
                                promoStore.getProxy().extraParams.TPMmode = 'Current';
                            }
                            else if (mode.data.value == 1) {
                                promoStore.getProxy().extraParams.TPMmode = 'RS';
                            }
                        }
                        panel.up('schedulecontainer').setLoading(true);
                        promoStore.load({
                            id: panel.ctx.recId,
                            scope: this,
                            callback: function (records, operation, success) {
                                if (success && records[0]) {
                                    var promoeditorcustom = Ext.widget('promoeditorcustom');
                                    promoeditorcustom.isCreating = false;
                                    promoeditorcustom.assignedRecord = records[0];

                                    me.mixins["App.controller.tpm.promo.Promo"].bindAllLoadEvents.call(me, promoeditorcustom, records[0], false);
                                    me.mixins["App.controller.tpm.promo.Promo"].fillPromoForm.call(me, promoeditorcustom, records[0], false, false);

                                    promoStatusName = records[0].get('PromoStatusName');

                                    //Для блокирования кнопки продуктов
                                    promoeditorcustom.productsSetted = true;


                                    //Установка readOnly полям, для которых текущая роль не входит в crudAccess
                                    me.setFieldsReadOnlyForSomeRole(promoeditorcustom);

                                    var isPromoWasStarted = (['Started', 'Finished', 'Closed'].indexOf(promoStatusName) >= 0);
                                    var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                                    if (isPromoWasStarted && currentRole !== 'SupportAdministrator') {
                                        me.blockStartedPromoDateChange(promoeditorcustom, me);
                                    }

                                    me.setFieldsReadOnlyForSomeRole(promoeditorcustom);
                                } else {
                                    panel.up('schedulecontainer').setLoading(false);
                                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedView'));
                                }
                            }
                        });

                    },
                },
                {
                    itemId: 'promoplanbutton',
                    text: l10n.ns('tpm', 'Schedule').value('Plan'),
                    glyph: 0xf12c,
                    hidden: !postAccess,
                    handler: function (button) {
                        panel.up('schedulecontainer').setLoading(true);
                        var statusId;
                        $.ajax({
                            dataType: 'json',
                            url: '/odata/PromoStatuss',
                            success: function (promoStatusData) {
                                for (var i = 0; i < promoStatusData.value.length; i++) {
                                    if (promoStatusData.value[i].SystemName == "Planned") {
                                        statusId = promoStatusData.value[i].Id;
                                        break;
                                    }
                                }
                                var params = 'id=' + panel.ctx.recId + '&promoNewStatusId=' + statusId;
                                $.ajax({
                                    dataType: 'json',
                                    url: '/odata/Promoes/ChangeStatus?' + params,
                                    type: 'POST',
                                    success: function (data) {
                                        panel.up('schedulecontainer').setLoading(false);
                                        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                                        if (scheduler) {
                                            scheduler.resourceStore.reload();
                                            scheduler.eventStore.reload();
                                        }
                                    },
                                    error: function (data) {
                                        panel.up('schedulecontainer').setLoading(false);
                                        App.Notify.pushError(data.statusText);
                                    }
                                });
                            },
                            error: function () {
                                panel.up('schedulecontainer').setLoading(false);
                                App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                            }
                        });
                    },
                }
                ]
            });
        }
        if (panel.ctx && postAccess) {
            panel.ctx.recId = rec.getId();
            panel.ctx.down('#promodeletebutton').setVisible(isDeletable);
            panel.ctx.down('#promoeditbutton').setVisible(isEditable);
            panel.ctx.down('#promoplanbutton').setVisible(isPlannable);
            panel.ctx.showAt(e.getXY());
        }
    },

    onPromoDragCreation: function (view, createContext, e, el, eOpts) {
        var me = this;
        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
        var typeToCreate = null;
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        var calcDispatchesStart = createContext.start.setDate(createContext.start.getDate() - 15);
        if (createContext.resourceRecord.get('TypeName') == 'Competitor') {
            createContext.finalize(false);
            return false;
        }
        if (mode) {
            if (mode.data.value == 1) {
                var RSmodeController = App.app.getController('tpm.rsmode.RSmode');
                RSmodeController.getRSPeriod(function (returnValue) {
                    StartDateRS = new Date(returnValue.StartDate);
                    EndDateRS = new Date(returnValue.EndDate);
                });

            if (calcDispatchesStart < StartDateRS || EndDateRS < calcDispatchesStart) {
                return me.finalizeContextWithError(createContext, l10n.ns('tpm', 'text').value('wrongRSPeriodDates'));
                }
            }
        }
        if (createContext.start > new Date(new Date().toDateString()) || App.UserInfo.getCurrentRole()['SystemName'] == 'SupportAdministrator') {
            var schedulerData,
                ClientTypeName = createContext.resourceRecord.get('TypeName') + ' Promo',
                isInOutClient = false,
                needSelectWindow = false;

            if (ClientTypeName === 'Regular Promo') {
                typeToCreate = scheduler.regPromoType;
            } else if (ClientTypeName === 'InOut Promo') {
                typeToCreate = scheduler.inOutPromoType;
                isInOutClient = true;
            } else if (ClientTypeName === 'Other Promo') {
                typeToCreate = scheduler.otherPromoTypes;
                needSelectWindow = true;
            };
            createContext.end = me.getDayEndDateTime(createContext.end);
            if (!view.schedulerView.eventCopy) {
                schedulerData = { schedulerContext: createContext };
                schedulerData.isCopy = false;
                me.selectCreatePromoMethod(me, schedulerData, isInOutClient, typeToCreate, needSelectWindow, scheduler.otherPromoTypes);
            } else {
                var ctx = new Ext.menu.Menu({
                    cls: "scheduler-context-menu",
                    items: [{
                        text: 'Paste',
                        glyph: 0xf191,
                        handler: function () {
                            if ((ClientTypeName != 'Other Promo' && ClientTypeName != view.schedulerView.eventCopy.get('PromoTypesName'))
                                || (ClientTypeName === 'Other Promo' && ['InOut Promo', 'Regular Promo'].includes(view.schedulerView.eventCopy.get('PromoTypesName')))) {
                                return me.finalizeContextWithError(createContext, l10n.ns('tpm', 'Schedule').value('CopyInOutError'));
                            } else {
                                schedulerData = view.schedulerView.eventCopy;
                                schedulerData.schedulerContext = createContext;
                                schedulerData.isCopy = true;
                                if (ClientTypeName === 'Other Promo') {
                                    for (var i = 0; i < scheduler.otherPromoTypes.length; i++) {
                                        if (scheduler.otherPromoTypes[i].Name == view.schedulerView.eventCopy.get('PromoTypesName')) {
                                            typeToCreate = scheduler.otherPromoTypes[i];
                                            break;
                                        }
                                    }
                                }
                                me.selectCreatePromoMethod(me, schedulerData, isInOutClient, typeToCreate, false, scheduler.otherPromoTypes);
                            }
                        }
                    }, {
                        text: 'Create',
                        glyph: 0xf0f3,
                        handler: function () {
                            schedulerData = { schedulerContext: createContext };
                            schedulerData.isCopy = false;
                            me.selectCreatePromoMethod(me, schedulerData, isInOutClient, typeToCreate, needSelectWindow, scheduler.otherPromoTypes);
                        }
                    }]
                });
                ctx.showAt(e.getXY());
            }
            createContext.finalize(false); // убрать выделенную область
            return false; // чтобы предотвратить автоматическое создание промо
        } else {
            return me.finalizeContextWithError(createContext, l10n.ns('tpm', 'text').value('wrongStartDate'));
        }
    },

    selectCreatePromoMethod: function (me, schedulerData, isInOutClient, typeToCreate, needSelectWindow, otherTypes) {
        if (!needSelectWindow) {
            me.createPromo(schedulerData, isInOutClient, typeToCreate);
        } else {
            me.promoDragCreationWindow(schedulerData, otherTypes);
        }
    },

    finalizeContextWithError: function (context, message) {
        App.Notify.pushError(message);
        context.finalize(false); // убрать выделенную область
        return false; // чтобы предотвратить автоматическое создание промо
    },

    onCreateButtonClick: function () {
        this.detailButton = null;
        this.createPromo();
    },

    createPromo: function (schedulerData, inOut, promotype) {
        this.mixins["App.controller.tpm.promo.Promo"].onCreateButtonClick.call(this, null, null, schedulerData, inOut, promotype);
    },
    onCreateRegularButtonClick: function (promotype, schedulerData) {
        this.detailButton = null;
        this.createPromo(schedulerData, false, promotype);
    },
    onCreateInOutButtonClick: function (promotype, schedulerData) {
        this.detailButton = null;
        this.createPromo(schedulerData, true, promotype);
    },
    onCreateLoyaltyButtonClick: function (promotype, schedulerData) {
        this.detailButton = null;
        this.createPromo(schedulerData, false, promotype);
    },
    onCreateDynamicButtonClick: function (promotype, schedulerData) {
        this.detailButton = null;
        this.createPromo(schedulerData, false, promotype);
    },
    onSelectionButtonClick: function (button) {
        var window = button.up('window');
        var fieldsetWithButtons = window.down('fieldset');

        fieldsetWithButtons.items.items.forEach(function (item) {
            item.down('button').up('container').removeCls('promo-type-select-list-container-button-clicked');
            item.down('button').addCls('promo-type-select-list-container-button-shplack');
        });

        button.up('container').addCls('promo-type-select-list-container-button-clicked');

        button.removeCls('promo-type-select-list-container-button-shplack');
        window.selectedButton = button;
    },

    onPromoTypeOkButtonClick: function (button, e) {
        var me = this;
        var window = button.up('window');
        if (window.selectedButton != null) {
            var selectedButtonText = window.selectedButton.budgetRecord;
            var method = "onCreate" + selectedButtonText.SystemName + "ButtonClick";
            if (me[method] != undefined) {
                me[method](selectedButtonText, window.schedulerData);
                window.close();
            } else {
                App.Notify.pushError('Не найдено типа промо ' + selectedButtonText.Name);
            }
        } else {
            App.Notify.pushError(l10n.ns('tpm', 'PromoTypes').value('NoPromoType'));
        }
    },

    onPromoTypeAfterRender: function (window) {
        var closeButton = window.down('#close');
        var okButton = window.down('#ok');

        closeButton.setText(l10n.ns('tpm', 'PromoType').value('ModalWindowCloseButton'));
        okButton.setText(l10n.ns('tpm', 'PromoType').value('ModalWindowOkButton'));
        window.selectedButton = null;
    },

    onAllCreateButtonClick: function (button) {
        var supportType = Ext.widget('scheduletypewindow');
        var mask = new Ext.LoadMask(supportType, { msg: "Please wait..." });

        supportType.show();
        mask.show();

        supportType.createPromoSupportButton = button;

        var query = breeze.EntityQuery
            .from('PromoTypes')
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                if (data.httpResponse.data.results.length > 0) {
                    supportType.down('#scheduletypewindowInnerContainer').show();
                    data.httpResponse.data.results.forEach(function (item) {
                        // Контейнер с кнопкой (обводится бордером при клике)
                        var promoTypeItem = Ext.widget({
                            extend: 'Ext.container.Container',
                            width: 'auto',
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [{
                                xtype: 'button',
                                enableToggle: true,
                                cls: 'promo-type-select-list-button',
                            }]
                        });

                        promoTypeItem.addCls('promo-type-select-list-container-button');
                        //promoTypeItem.down('button').style = { borderLeft: '6px solid ' + 'rgb(179, 193, 210)' };
                        promoTypeItem.down('button').addCls('promo-type-select-list-container-button-shplack');
                        promoTypeItem.down('button').setText(item.Name);
                        promoTypeItem.down('button').renderData.glyphCls = 'promo-type-select-list-button';
                        promoTypeItem.down('button').setGlyph(parseInt('0x' + item.Glyph, 16));

                        promoTypeItem.down('button').budgetRecord = item;
                        supportType.down('fieldset').add(promoTypeItem);
                    });
                } else {
                    Ext.ComponentQuery.query('promotypewindow')[0].close();
                    App.Notify.pushError('Не найдено записей типа промо ');
                }

                mask.hide();
            })
            .fail(function () {
                App.Notify.pushError('Ошибка при выполнении операции');
                mask.hide();
            })
    },

    promoDragCreationWindow: function (schedulerData, promoTypes) {
        var supportType = Ext.widget('scheduletypewindow');
        supportType.down('#scheduletypewindowInnerContainer').show();
        var mask = new Ext.LoadMask(supportType, { msg: "Please wait..." });
        supportType.show();
        supportType.minHeight = 240;
        supportType.setHeight(240);
        supportType.schedulerData = schedulerData;
        mask.show();

        promoTypes.forEach(function (item) {
            // Контейнер с кнопкой (обводится бордером при клике)
            var promoTypeItem = Ext.widget({
                extend: 'Ext.container.Container',
                width: 'auto',
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'button',
                    enableToggle: true,
                    cls: 'promo-type-select-list-button',
                }]
            });

            promoTypeItem.addCls('promo-type-select-list-container-button');
            promoTypeItem.down('button').style = { borderLeft: '6px solid ' + 'rgb(179, 193, 210)' };
            promoTypeItem.down('button').setText(item.Name);
            promoTypeItem.down('button').renderData.glyphCls = 'promo-type-select-list-button';
            promoTypeItem.down('button').setGlyph(parseInt('0x' + item.Glyph, 16));

            promoTypeItem.down('button').budgetRecord = item;
            supportType.down('fieldset').add(promoTypeItem);
        });
        mask.hide();
    },

    setButtonState: function (window, visible) {
        window.down('#changePromo').setVisible(!visible);
        window.down('#savePromo').setVisible(visible);
        window.down('#saveAndClosePromo').setVisible(visible);
    },

    setTabsState: function (promoWindow, disabled) {
        promoWindow.down('#history').setDisabled(disabled);
        promoWindow.down('#decline').setDisabled(disabled);

        if (disabled) {
            promoWindow.down('#history').addClass('disabled');
            promoWindow.down('#decline').addClass('disabled');
        } else {
            promoWindow.down('#history').removeCls('disabled');
            promoWindow.down('#decline').removeCls('disabled');
        }

        promoWindow.down('#budget').setDisabled(disabled);
        promoWindow.down('#demand').setDisabled(disabled);
        promoWindow.down('#finance').setDisabled(disabled);
    },

    onEventResize: function (s, resizeContext) {
        var me = this;
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        var calcDispatchesStart = resizeContext.start.setDate(resizeContext.start.getDate() - 15);
        if (resizeContext.eventRecord.get('TypeName') == 'Competitor') {
            me.__resizeContext.finalize(false);
            return false;
        }
        var calendarGrid = Ext.ComponentQuery.query('scheduler');
        //Проверка по дате начала
        if ((resizeContext.eventRecord.start < new Date(new Date().toDateString()) || resizeContext.start < new Date(new Date().toDateString())) && App.UserInfo.getCurrentRole()['SystemName'] != 'SupportAdministrator') {
            App.Notify.pushError(l10n.ns('tpm', 'text').value('wrongStartDate'));
            resizeContext.finalize(false);
            //Открытый календарь - обновить его
            if (calendarGrid.length > 0) {
                calendarGrid[0].resourceStore.load();
            }
            return false;
        }
        if (mode) {
            if (mode.data.value == 1) {
                var RSmodeController = App.app.getController('tpm.rsmode.RSmode');
                RSmodeController.getRSPeriod(function (returnValue) {
                    StartDateRS = new Date(returnValue.StartDate);
                    EndDateRS = new Date(returnValue.EndDate);
                });

            if (calcDispatchesStart < StartDateRS || EndDateRS < calcDispatchesStart) {
                App.Notify.pushInfo(l10n.ns('tpm', 'text').value('wrongRSPeriodDates'));
                resizeContext.finalize(false);
                if(calendarGrid.length > 0) {
                    calendarGrid[0].resourceStore.load();
                }
                return false;
                }
            }
        }

        var system = Ext.ComponentQuery.query('system')[0];
        if (system.tabBar.activeTab) {
            system.tabBar.activeTab.deactivate();
            system.tabBar.activeTab = null;
            system.activeTab = null;
            system.collapse();
        }

        // Save reference to context to be able to finalize drop operation after user clicks yes/no button.
        me.__resizeContext = resizeContext;
        if (calendarGrid.length > 0) {
            me.calendarSheduler = calendarGrid[0];
        }

        Ext.Msg.confirm('Please confirm',
            'Do you want to update the Promo: ' + resizeContext.eventRecord.get('Name'),
            me.onResizeConfirm,  // The button callback
            me);                 // scope
        return false;
    },

    onResizeConfirm: function (btn) {
        var me = this;
        if (btn === 'yes') {
            var newMonth = me.__resizeContext.start.getMonth() + 1;
            if (newMonth == 1 || newMonth == 12) {
                var selectbudgetyearwindow = Ext.widget('selectbudgetyearwindow');
                selectbudgetyearwindow.context = me.__resizeContext;
                selectbudgetyearwindow.down('#okBudgetYearButton').on('click', this.onOkBudgetYearWindowDropClick, this);
                selectbudgetyearwindow.on('afterrender', this.afterBudgetYearWindowDropRender, this);
                selectbudgetyearwindow.on('close', this.onBudgetYearWindowClose, this);
                selectbudgetyearwindow.show();
            }
            else {
                me.onBudgetYearResizeChoose();
            }
        }
        else {
            me.__resizeContext.finalize(false);
        }
    },

    afterBudgetYearWindowDropRender: function (window) {
        var me = this;
        var comboStore = window.down('combobox').getStore();
        comboStore.removeAll();
        var month = me.__resizeContext.start.getMonth() + 1;
        var year = me.__resizeContext.start.getFullYear();
        var userRole = App.UserInfo.getCurrentRole()['SystemName'];
        if (['SupportAdministrator', 'DemandFinance'].includes(userRole)) {
            comboStore.add({ year: year - 2 });
            comboStore.add({ year: year - 1 });
            comboStore.add({ year: year });
            comboStore.add({ year: year + 1 });
        }
        else {
            if (month == 1) {
                year--;
            }
            comboStore.add({ year: year });
            comboStore.add({ year: year + 1 });
        }
    },

    onOkBudgetYearWindowDropClick: function (button) {
        var combo = Ext.ComponentQuery.query('#budgetYearCombo')[0];
        var budgetYear = combo.getValue();
        if (budgetYear !== null) {
            var win = Ext.WindowManager.getActive();
            if (win) {
                win.isConfirmed = true;
                win.close();
            }
            this.onBudgetYearResizeChoose(button, budgetYear);
        }
    },

    // Подтверждение изменения продолжительности промо
    onBudgetYearResizeChoose: function (btn, budgetYear) {
        var me = this;
        me.calendarSheduler.setLoading(true);
        var resizeContext = me.__resizeContext,
            eventRecord = resizeContext.eventRecord,
            resourceRecord = resizeContext.resourceRecord,
            promoStore = this.getPromoStore();

        promoStore.load({
            id: eventRecord.getId(),
            scope: this,
            callback: function (records, operation, success) {
                if (success) {
                    var record = records[0];

                    // Локальные копии dispatch date до всех изменений
                    var localDispatchesStart = record.get('DispatchesStart');
                    var localDispatchesEnd = record.get('DispatchesEnd');

                    // Сдвиг левой границы
                    var curStartDate = record.get('StartDate');
                    var curEndDate = record.get('EndDate');
                    if (Ext.Date.isEqual(resizeContext.start, curStartDate) === false) {
                        var daysForDispatchDateFromClientSettings = me.getDaysForDispatchDateFromClientSettings(
                            resourceRecord.data.IsBeforeStart, resourceRecord.data.DaysStart, resourceRecord.data.IsDaysStart);

                        var dispatchDateForCurrentClientAfterResize = null;
                        var deltaDaysBeforeAndAfterResize = null;
                        var dispatchDateForCurrentPromoAfterResize = null;

                        // Если настройки dispatch клиента корректны
                        if (daysForDispatchDateFromClientSettings !== null) {
                            dispatchDateForCurrentClientAfterResize = Ext.Date.add(
                                resizeContext.start, Ext.Date.DAY, daysForDispatchDateFromClientSettings);

                            deltaDaysBeforeAndAfterResize = (resizeContext.start - curStartDate) / 3600 / 24 / 1000;
                            dispatchDateForCurrentPromoAfterResize = Ext.Date.add(localDispatchesStart, Ext.Date.DAY, deltaDaysBeforeAndAfterResize);

                            // Если dispatch дата, сформированная из настроек клиента, совпадает с текущей dispatch датой
                            if (Ext.Date.isEqual(dispatchDateForCurrentPromoAfterResize, dispatchDateForCurrentClientAfterResize) === true) {
                                record.set('DispatchesStart', dispatchDateForCurrentPromoAfterResize);
                            }
                        }
                    } else // Сдвиг правой границы
                        if (Ext.Date.isEqual(resizeContext.end, curEndDate) === false) {
                            var daysForDispatchDateFromClientSettings = me.getDaysForDispatchDateFromClientSettings(
                                resourceRecord.data.IsBeforeEnd, resourceRecord.data.DaysEnd, resourceRecord.data.IsDaysEnd);

                            var dispatchDateForCurrentClientAfterResize = null;
                            var deltaDaysBeforeAndAfterResize = null;
                            var dispatchDateForCurrentPromoAfterResize = null;

                            // Если настройки dispatch клиента корректны
                            if (daysForDispatchDateFromClientSettings !== null) {
                                dispatchDateForCurrentClientAfterResize = Ext.Date.add(
                                    resizeContext.end, Ext.Date.DAY, daysForDispatchDateFromClientSettings);

                                deltaDaysBeforeAndAfterResize = (resizeContext.end - curEndDate) / 3600 / 24 / 1000;
                                dispatchDateForCurrentPromoAfterResize = Ext.Date.add(localDispatchesEnd, Ext.Date.DAY, deltaDaysBeforeAndAfterResize);

                                // Если dispatch дата, сформированная из настроек клиента, совпадает с текущей dispatch датой
                                if (Ext.Date.isEqual(dispatchDateForCurrentPromoAfterResize, dispatchDateForCurrentClientAfterResize) === true) {
                                    record.set('DispatchesEnd', dispatchDateForCurrentPromoAfterResize);
                                }
                            }
                        }
                    var dispStart = record.get('DispatchesStart');
                    var dispEnd = record.get('DispatchesEnd');
                    var startDispatchDateBiggerThanEnd = dispStart > dispEnd;
                    var endDispatchDateLessThanStart = dispEnd < dispStart;
                    // Если dispatch start и dispatch end наехали друг на друга, то показываем ошибку и возвращаем исходные параметры dispatch
                    if (startDispatchDateBiggerThanEnd === true || endDispatchDateLessThanStart === true) {
                        record.reject();
                        this.__resizeContext.finalize(false);
                        me.calendarSheduler.setLoading(false);
                        App.Notify.pushInfo('Dispatch start date must be less than dispatch end date.');
                    } else {
                        //Возврат выделения
                        me.calendarSheduler.down('gridview').on('refresh', (function () { me.highlightRow(null, new Array(resourceRecord)); }));
                        //record.set('StartDate', resizeContext.start);
                        // выравниваем время с учётом часового пояса
                        var offset = resizeContext.start.getTimezoneOffset() / 60.0;
                        record.set('StartDate', Sch.util.Date.add(resizeContext.start, Sch.util.Date.HOUR, -(offset + 3)));
                        var fixedEndDate = new Date(resizeContext.end.getFullYear(), resizeContext.end.getMonth(), resizeContext.end.getDate(), 0, 0, 0);
                        fixedEndDate = Sch.util.Date.add(fixedEndDate, Sch.util.Date.HOUR, -(offset + 3));
                        record.set('EndDate', fixedEndDate);
                        //record.data.EndDate = resizeContext.end;
                        //record.set('EndDate', resizeContext.end);
                        if (budgetYear != null)
                            record.set('BudgetYear', budgetYear);
                        else {
                            var isOnInvoice = record.get('IsOnInvoice');
                            budgetYear = resizeContext.start.getFullYear();
                            record.set('BudgetYear', budgetYear);
                        }
                        record.save({
                            callback: function (record, operation, success) {
                                if (success) {
                                    me.__resizeContext.finalize(true);
                                    me.eventStoreLoading(me.calendarSheduler.getEventStore());
                                    me.calendarSheduler.setLoading(false);
                                } else {
                                    me.calendarSheduler.setLoading(false);
                                    me.__resizeContext.finalize(false);
                                }
                            }
                        });
                    }
                }
            }
        });
    },

    getDaysForDispatchDateFromClientSettings: function (isBefore, days, isDays) {
        if (isBefore !== null, days !== null, isDays !== null) {
            days = isDays ? days : days * 7;
            return isBefore === true ? -days : days;
        }

        return null;
    },

    onExtFilterChange: function (ctx) {
        var container = Ext.ComponentQuery.query('schedulecontainer')[0];
        var clearButton = container.down('#extfilterclearbutton'),
            isFilterEmpty = ctx && ctx.isEmpty();

        if (clearButton) {
            clearButton.setDisabled(isFilterEmpty);

            var text = isFilterEmpty
                ? l10n.ns('core', 'filter').value('filterEmptyStatus')
                : l10n.ns('core', 'filter').value('filterNotEmptyStatus');

            clearButton.setText(text);
            clearButton.setTooltip(text);
        }
        var scheduler = container.down('scheduler');
        var store = scheduler.getEventStore();
        if (store.uniqueObjectIds) {
            this.eventStoreLoading(store);
        };
    },

    onFilterButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        var store = scheduler.getEventStore();
        Ext.widget('extfilter', store.getExtendedFilter()).show();
    },

    onClientsPromoTypeFilterAfterrender: function (label) {
        label.getEl().on('click', this.onClientsPromoTypeFilterClick, label);
    },

    onClientsPromoTypeFilterClick: function (mouseEvent, label) {
        Ext.widget('clientPromoTypeFilter').show();
    },

    onRefreshButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        var store = scheduler.getEventStore();
        this.eventStoreLoading(store);
    },

    onPromoDetailButtonClick: function (button) {
        var promoDetailPanel = button.up('promodetailtabpanel');
        var record = promoDetailPanel.event;
        if (record) {
            button.assignedRecord = record;

            Ext.ComponentQuery.query('readonlydirectorytoolbar')[0].setDisabled(true);
            this.mixins["App.controller.tpm.promo.Promo"].onDetailButtonClick.call(this, button);
        }
    },

    setButtonState: function (window, visible) {
        window.down('#changePromo').setVisible(!visible);
        window.down('#savePromo').setVisible(visible);
        window.down('#saveAndClosePromo').setVisible(visible);
    },

    // Переключени режимов календаря Марс/Григорианский
    onShiftModeButtonClick: function (button) {
        var me = this,
            scheduler = button.up('panel').down('scheduler');
        // флаги режима МАРС-календаря
        if (button.marsMode != scheduler.isMarsMode) {
            if (button.marsMode) {
                button.setText(this.getCalendarModeSwitchText(l10n.ns('tpm', 'Schedule').value('NAMARS')));
                scheduler.up('panel').down('[presetId=monthQuarter]').presetId = 'marsmonthQuarter';
                scheduler.up('panel').down('[presetId=weekMonth]').setText(l10n.ns('tpm', 'Schedule').value('Period'));
                scheduler.up('panel').down('[presetId=weekMonth]').presetId = 'marsweekMonth';
                scheduler.up('panel').down('[presetId=dayWeek]').presetId = 'marsdayWeek';
            } else {
                button.setText(this.getCalendarModeSwitchText(l10n.ns('tpm', 'Schedule').value('NAStandard')));
                scheduler.up('panel').down('[presetId=marsmonthQuarter]').presetId = 'monthQuarter';
                scheduler.up('panel').down('[presetId=marsweekMonth]').setText(l10n.ns('tpm', 'Schedule').value('Quarter'));
                scheduler.up('panel').down('[presetId=marsweekMonth]').presetId = 'weekMonth';
                scheduler.up('panel').down('[presetId=marsdayWeek]').presetId = 'dayWeek';
            }
        }
        //Ext.get(Ext.dom.Query.select('.sch-mode-btn-selected')[0]).removeCls('sch-mode-btn-selected');
        //button.addCls('sch-mode-btn-selected');

        scheduler.isMarsMode = button.marsMode;
        scheduler.timeAxis.setMarsMode(button.marsMode);
        button.setMarsMode(!button.marsMode);

        me.onShiftPresetButtonClick(scheduler.up('panel').down('[active=true]'));
        me.setScroll();
        me.highlightRow(scheduler.lockedGrid.getSelectionModel(), scheduler.lockedGrid.getSelectionModel().getSelection());
    },
    // Добавляем иконки и соответствующий текст для кнопки переключения режимов
    getCalendarModeSwitchText: function (mode) {
        return Ext.String.format('Calendar type  {0}  {1}  {2}', '<span class="mdi mdi-arrow-left-drop-circle scheduler-modebutton-text"></span><span style="font-size: 15px;">', mode, '</span><span class="mdi mdi-arrow-right-drop-circle scheduler-modebutton-text"></span>');
    },

    onShiftPresetButtonClick: function (button) {
        var startDate, endDate;
        var preset = button.presetId;
        switch (preset) {
            // календарь по неделям
            case 'marsdayWeek':
            case 'dayWeek':
                var curMonth = new Date().getMonth()
                var startYear = curMonth == 0 ? new Date().getFullYear() - 1 : new Date().getFullYear();
                var endYear = curMonth == 11 ? new Date().getFullYear() + 1 : new Date().getFullYear();
                var startMonth = curMonth == 0 ? 11 : curMonth - 1;
                var endMonth = curMonth == 11 ? 0 : curMonth + 1;
                var lastDate = Ext.Date.getLastDateOfMonth(new Date(startYear, startMonth)).getDate();
                startDate = new Date(startYear, startMonth, lastDate);
                endDate = new Date(endYear, endMonth, 6);
                break;
            // календарь по месяцам
            case 'marsweekMonth':
            case 'weekMonth':
                var curMonth = new Date().getMonth();
                var curYear = new Date().getFullYear();
                //var curQuarter = Math.floor(curMonth / 3) + 1;
                //var startYear = curQuarter == 1 ? new Date().getFullYear() - 1 : new Date().getFullYear();
                //var startMonth = curQuarter == 1 ? 11 : (curQuarter * 3) - 4;
                //var endMonth = curQuarter == 4 ? 0 : (curQuarter * 3) + 1;
                //var endYear = curQuarter == 4 ? new Date().getFullYear() + 1 : new Date().getFullYear();
                startDate = new Date(curYear, curMonth - 1, 1);
                endDate = new Date(curYear, curMonth + 2, 0);
                break;
            // календарь по кварталам
            case 'marsmonthQuarter':
            case 'monthQuarter':
                startDate = new Date(new Date().getFullYear() - 1, 10, 01);
                endDate = new Date(new Date().getFullYear() + 1, 3, 01);
                break;

        }
        Ext.get(Ext.dom.Query.select('.sch-preset-btn-selected')[0]).removeCls('sch-preset-btn-selected');
        button.addCls('sch-preset-btn-selected');
        button.up('panel').down('[active=true]').active = false;
        button.active = true;
        var scheduler = button.up('panel').down('scheduler');
        scheduler.switchViewPreset(button.presetId, startDate, endDate);
        this.highlightRow(scheduler.lockedGrid.getSelectionModel(), scheduler.lockedGrid.getSelectionModel().getSelection());
    },

    onShiftPrevButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        scheduler.shiftPrevious();
        this.highlightRow(scheduler.lockedGrid.getSelectionModel(), scheduler.lockedGrid.getSelectionModel().getSelection());
    },
    onShiftNextButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        scheduler.shiftNext();
        this.highlightRow(scheduler.lockedGrid.getSelectionModel(), scheduler.lockedGrid.getSelectionModel().getSelection());
    },

    // Simple anymatch, case insensitive search
    onTaskHighlightFieldKeyUp: function (field, e) {
        var value = field.getValue().toLowerCase();

        this.doHighlight(field, e.getKey() === e.ESC ? '' : value);
    },

    doHighlight: function (field, value) {
        var store = this.getView().eventStore;

        if (!value) {
            store.clearFilter();
            field.setValue('');
        } else {
            store.each(function (task) {
                if (task.getName().toLowerCase().indexOf(value) >= 0) {
                    task.set('Cls', 'match');
                } else {
                    task.set('Cls', '');
                }
            });
        }

        this.getView()[value.length > 0 ? 'addCls' : 'removeCls']('highlighting');
    },

    onClearHighlightClick: function (field) {
        this.doHighlight(field, '');
    },

    onScheduleAfterRender: function (scheduler) {
        var me = this;

        scheduler.setLoading('Loading promoes'); // beforeCrudOperationStart - ставит лоадер, который снимается до окончания закрузки стора, TODO: оставить только этот лоадер

        scheduler.baseClientsStore = Ext.create('Ext.data.Store', {
            model: 'App.model.tpm.baseclient.BaseClient',
            type: 'simplestore',
            idProperty: 'Id',
            autoLoad: true
        });

        breeze.EntityQuery
            .from('PromoTypes')
            .withParameters({
                $method: 'GET'
            })
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                scheduler.typesCheckboxesConfig = [];
                scheduler.otherPromoTypes = [];
                var competitorType = {
                    Name: 'Competitor Promo',
                    SystemName: 'Competitor',
                    Glyph: 'FD01'
                };
                data.results.push(competitorType);
                data.results.forEach(function (el) {
                    if (el.Name === 'Regular Promo') {
                        scheduler.regPromoType = el;
                    } else if (el.Name === 'InOut Promo') {
                        scheduler.inOutPromoType = el;
                    } else if (el.Name === 'Competitor Promo') {
                        scheduler.competitorPromoType = el;
                    } else {
                        scheduler.otherPromoTypes.push(el);
                    }

                    var beforeBoxLabelTextTpl = new Ext.Template('<span class="mdi filter-mark-icon">&#x{glyph}</span>');
                    scheduler.typesCheckboxesConfig.push({
                        name: el.Name,
                        inputValue: el.Name,
                        checked: true,
                        boxLabel: '<span style="vertical-align: text-top;">' + el.Name + '</span>',
                        xtype: 'checkbox',
                        beforeBoxLabelTextTpl: beforeBoxLabelTextTpl.apply({ glyph: el.Glyph }),
                    })
                });
            })
            .fail(function (data) {
                App.Notify.pushError('Ошибка при выполнении операции');
            });

        breeze.EntityQuery
            .from('Competitors')
            .withParameters({
                $method: 'GET'
            })
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                scheduler.competitorsCheckboxesConfig = [];
                data.results.forEach(function (el) {
                    scheduler.competitorsCheckboxesConfig.push({
                        name: el.Name,
                        inputValue: el.Name,
                        checked: true,
                        boxLabel: '<span style="vertical-align: text-top;">' + el.Name + '</span>',
                        xtype: 'checkbox',
                    })
                });
            })
            .fail(function (data) {
                App.Notify.pushError('Ошибка при выполнении операции');
            });

        scheduler.baseClientsStore.on('load', function (store, records) {
            scheduler.clientsFilterConfig = [];
            records.forEach(function (el) {
                scheduler.clientsFilterConfig.push({
                    name: el.data.Name,
                    inputValue: el.data.Name,
                    checked: true,
                    boxLabel: el.data.Name,
                    xtype: 'checkbox'
                })
            });
        });

        scheduler.up('panel').down('[presetId=marsweekMonth]').addCls('sch-preset-btn-selected');
        //scheduler.up('panel').down('[marsMode=true]').addCls('sch-mode-btn-selected');
        // проброс load стора в грид для доступа из контроллера
        scheduler.relayEvents(scheduler.getEventStore(), ['extfilterchange']);
        scheduler.getEventStore().on('load', function () {
            // выделение первой строки после загрузки стора событий
            me.highlightRow(scheduler.lockedGrid.getSelectionModel(), scheduler.lockedGrid.getSelectionModel().getSelection());
            scheduler.setLoading(false);
        })
        // при сортировке клиентов пропадает выделение строки в календаре
        scheduler.lockedGrid.columns[0].getOwnerHeaderCt().on('sortchange', function () {
            me.highlightRow(scheduler.lockedGrid.getSelectionModel(), scheduler.lockedGrid.getSelectionModel().getSelection());

            //Изменяем порядок загрузки при сортировке
            var resourceStore = scheduler.getResourceStore();
            var eventStore = scheduler.getEventStore();
            var objectIds = me.getObjectIds(resourceStore);
            var oldArray = eventStore.uniqueObjectIds;
            var loaded = false;
            var inoutPromoId, regPromoId;
            var incldedIds = [];
            eventStore.uniqueObjectIds = [];

            for (var i = 0; i < objectIds.length; i++) {
                for (var j = 0; j < oldArray.length; j++) {
                    if (objectIds[i] == oldArray[j].objectId) {
                        incldedIds.push(j);
                        loaded = oldArray[j].loaded;
                        regPromoId = oldArray[j].regPromoId;
                        inoutPromoId = oldArray[j].inoutPromoId;
                        otherPromoId = oldArray[j].otherPromoId;
                        competitorPromoId = oldArray[j].competitorPromoId;
                        break;
                    }
                };

                eventStore.uniqueObjectIds.push({
                    objectId: objectIds[i],
                    loaded: loaded,
                    regPromoId: regPromoId,
                    inoutPromoId: inoutPromoId,
                    otherPromoId: otherPromoId,
                    competitorPromoId: competitorPromoId,
                });
            };

            //Если часть записей скрыта и не отфильтровалась, всё равно добавляем их в загрузку
            if (eventStore.uniqueObjectIds.length != oldArray.length) {
                for (var i = 0; i < oldArray.length; i++) {
                    if (!incldedIds.includes(i)) {
                        eventStore.uniqueObjectIds.push(oldArray[i])
                    }
                }
            }
        });
        scheduler.lockedGrid.relayEvents(scheduler.lockedGrid.getStore(), ['load']);
        scheduler.relayEvents(scheduler.lockedGrid.getSelectionModel(), ['selectionchange'], 'row');
        // добавление кнопки "Детали" в нижнюю панель
        var system = Ext.ComponentQuery.query('system')[0];
        var promoTab = {
            title: l10n.ns('tpm', 'compositePanelTitles').value('PromoDetail'),
            itemId: 'promoDetailTab',
            items: [{
                xtype: 'promodetailtabpanel',
                selectedUI: 'blue-selectable-panel'
            }
            ],

            tabConfig: {
                ui: 'system-panel-tab-button',
                border: '1 1 0 0'
            },
        }
        system.add(promoTab);

        /*Ext.create('Ext.ux.window.Notification', {
            position: 'br',
            cls: 'ux-notification-light',
            closable: true,
            autoClose: false,
            title: l10n.ns('tpm', 'Schedule').value('Attention'),
            iconCls: 'x-message-box-info',
            html: l10n.ns('tpm', 'Schedule').value('Notify'),
        }).show();*/

        // Сокрытие функционала в зависиости от текущей роли.
        this.accessIsDeniedForCurrentRoles('Promoes');
    },

    delayOnEventClick: function (scheduler, eventRecord) {
        this.fillTabPanel(eventRecord, scheduler, true);
    },
    // При даблклике срабатывает onEventdbClick и 2 раза onEventClick
    singleClickTask: new Ext.util.DelayedTask(this.delayOnEventClick),

    onEventdbClick: function (panel, eventRecord, button) {
        if (eventRecord.get('TypeName') == 'Competitor') return;
        this.singleClickTask.cancel();
        var promoStore = this.getPromoStore();
        // RSmode
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        if (mode) {
            if (mode.data.value == 0) {
                promoStore.getProxy().extraParams.TPMmode = 'Current';
            }
            else if (mode.data.value == 1) {
                promoStore.getProxy().extraParams.TPMmode = 'RS';
            }
        }
        panel.up('schedulecontainer').setLoading(true);
        promoStore.load({
            id: eventRecord.data.Id,
            scope: this,
            callback: function (records, operation, success) {
                if (success && records[0]) {
                    button.assignedRecord = records[0];
                    this.mixins["App.controller.tpm.promo.Promo"].onDetailButtonClick.call(this, button);
                } else {
                    panel.up('schedulecontainer').setLoading(false);
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedView'));
                }
            }
        });
    },

    onEventClick: function (scheduler, eventRecord, e, eOpts) {
        this.singleClickTask.delay(200, this.delayOnEventClick, this, [scheduler, eventRecord]);
    },

    onResourceStoreLoad: function () {
        var grid = Ext.ComponentQuery.query('#nascheduler')[0].lockedGrid;
        grid.getSelectionModel().select(0);
        grid.fireEvent('rowclick', grid, 0);
        var record = grid.selModel.getSelection()[0];
        var scheduler = grid.up('#nascheduler');
        var resourceStore = scheduler.getResourceStore();
        var eventStore = scheduler.getEventStore();
        var ng = scheduler.normalGrid;

        scheduler.down('schedulergridview').preserveScrollOnRefresh = true;

        var objectIds = this.getObjectIds(resourceStore);
        eventStore.uniqueObjectIds = [];
        for (var i = 0; i < objectIds.length; i++) {
            eventStore.uniqueObjectIds.push({
                objectId: objectIds[i],
                loaded: false,
                regPromoId: resourceStore.data.items[i * this.rowCount],
                inoutPromoId: resourceStore.data.items[i * this.rowCount + 1],
                otherPromoId: resourceStore.data.items[i * this.rowCount + 2],
                competitorPromoId: resourceStore.data.items[i * this.rowCount + 3],
                competitorPromoIds: resourceStore.data.items.slice(i * this.rowCount + 3, resourceStore.data.items.length),
            })
        };

        eventStore.on('load', function () {
            // выделение первой строки после загрузки стора событий
            me.highlightRow(scheduler.lockedGrid.getSelectionModel(), scheduler.lockedGrid.getSelectionModel().getSelection());
            scheduler.setLoading(false);
        });
        resourceStore.on('refresh', function () {
            me.setLoadingText(eventStore.uniqueObjectIds, ng);
        });
        //Удаляем фильтр при перезагрузке
        if (!scheduler.up('panel').down('#schedulefilterdraftbutton').hasCls('sheduler_promostatusfilter_button_selected')
            && !scheduler.up('panel').down('#schedulefilterdraftpublbutton').hasCls('sheduler_promostatusfilter_button_selected')) {
            delete eventStore.fixedFilters['PromoStatusfilter'];
        }

        this.eventStoreLoading(eventStore);
        // при изменении содержимого календаря меняем скролл
        var me = this;
        $('#' + scheduler.down('schedulergridview').id).on("DOMSubtreeModified", function () {
            var table = $(this).find('table');
            var heightTable = table.height();
            var currentHeightScroll = $('#scrollSchedulerH').height();

            if (heightTable != undefined && heightTable != 0 && heightTable != currentHeightScroll)
                me.setScroll();
        });
        scheduler.down('schedulergridview').on("beforerefresh", function () {
            return true;
        });

        // Notifications
        if (this.getAllowedActionsForCurrentRoleAndResource('Promoes').some(function (action) { return action === 'Post'; })) {
            this.showOrHideNotificationsPanel();
        }
    },

    //Получаем уникальные Id
    getObjectIds: function (resourceStore) {
        var objectIds = [];
        resourceStore.data.items.forEach(function (item) {
            objectIds.push(item.data.ObjectId);
        });
        return Array.from(new Set(objectIds));
    },

    getPromoStore: function () {
        return Ext.ComponentQuery.query('#nascheduler')[0].promoStore;
    },

    getCompetitorPromoStore: function () {
        return Ext.ComponentQuery.query('#nascheduler')[0].competitorPromoStore;
    },

    // заполнение дашборда
    // events - запись(массив записей) промо. resourceRecord - клиент(для суммы по клиенту), showTab - необходимость открыть нижнюю панель
    fillTabPanel: function (events, scheduler, showTab) {
        var system = Ext.ComponentQuery.query('system')[0],
            promoPanel = system.down('promodetailtabpanel'),
            competitorPromoPanel = system.down('promodetailtabpanel'),
            promoStore = this.getPromoStore(),
            competitorPromoStore = this.getCompetitorPromoStore();
        if (events.get('TypeName') == 'Competitor') {
            competitorPromoStore.load({
                id: events.getId(), //set the id here
                scope: this,
                callback: function (records, operation, success) {
                    if (success) {
                        var rec = records[0];
                        competitorPromoPanel.event = rec;
                        // Заголовок 1-й панели дашборда - название промо
                        var promoDetailPanel = competitorPromoPanel.down('#promodetailpanel');
                        promoDetailPanel.setBodyStyle("margin-top:4px");
                        var promoDetailButton = competitorPromoPanel.down('#promoDetail');
                        promoDetailButton.hide();
                        // Полный вид механики с параметрами
                        if (rec && rec.data) {
                            promoDetailPanel.update(rec.data);
                            if (showTab) {
                                system.setActiveTab('promoDetailTab');
                            }
                        }
                    }
                }
            });
        } else {
            promoStore.load({
                id: events.getId(), //set the id here
                scope: this,
                callback: function (records, operation, success) {
                    if (success) {
                        var rec = records[0];
                        promoPanel.event = rec;
                        // Заголовок 1-й панели дашборда - название промо
                        var promoDetailPanel = promoPanel.down('#promodetailpanel');

                        promoDetailPanel.setBodyStyle("margin-top:17px");
                        var promoDetailButton = competitorPromoPanel.down('#promoDetail');
                        promoDetailButton.show(true);
                        // Полный вид механики с параметрами
                        if (rec && rec.data) {
                            promoDetailPanel.update(rec.data);
                            if (showTab) {
                                system.setActiveTab('promoDetailTab');
                            }
                        }
                    }
                }
            });
        }
    },

    onResizeGrid: function (grid, width, height, oldWidth, oldHeight, eOpts) {
        if (!grid.isLocked)
            this.setScroll();
    },

    setScroll: function () {
        // замена скролла в календаре
        var normalGrid = Ext.ComponentQuery.query('#nascheduler')[0].normalGrid;
        var normalGridHtml = $('#' + normalGrid.id);
        var schedulergridview = normalGrid.down('schedulergridview');
        var schedulerHtml = $('#' + schedulergridview.id);
        var table = schedulerHtml.find('table');
        var heightScroll = table.length > 0 ? table.height() : 0;
        var scrollSheduler = $('#scrollScheduler');
        // если скролла есть, то обновить, иначе создать
        if (scrollSheduler.length > 0) {
            scrollSheduler.height(schedulerHtml.height());
            $('#scrollSchedulerH').height(heightScroll);
            scrollSheduler.data('jsp').reinitialise();
            $('#scrollScheduler').data('jsp').scrollToY(schedulerHtml.scrollTop());
        } else {
            schedulerHtml.css('overflow', 'hidden');
            schedulerHtml.css('overflow-y', 'hidden');

            normalGridHtml.append('<div id="scrollScheduler" class="scrollpanel" style="height: ' + schedulerHtml.height() + 'px;">'
                + '<div id="scrollSchedulerH" style="height: ' + heightScroll + 'px;"></div></div>');

            $('#scrollScheduler').jScrollPane();
            $('#scrollScheduler').data('jsp').scrollToY(schedulerHtml.scrollTop());
            $('#scrollScheduler').on('jsp-scroll-y', function (event, scrollPositionY, isAtTop, isAtBottom) {
                schedulerHtml.scrollTop(scrollPositionY);
                return false;
            });
        }
    },

    highlightRow: function (self, records) {
        if (records.length > 0) {
            var ng = Ext.ComponentQuery.query('#nascheduler')[0].normalGrid;
            //this can be improved to remove only classes that are unselected
            for (var j = 0; j < ng.getStore().getCount(); j++) {
                ng.view.removeRowCls(j, 'x-grid-row-selected x-grid-row-focused');
            }
            for (var i = 0; i < records.length; i++) {
                var ind = ng.getStore().indexOf(records[i]);
                ng.view.addRowCls(ind, 'x-grid-row-selected');
                if (i == records.length - 1)
                    ng.view.addRowCls(ind, 'x-grid-row-focused');
            }
        }
    },

    // Notifications
    showOrHideNotificationsPanel: function () {
        var containerForRecentMenuItems = Ext.ComponentQuery.query('recentmenuitems')[0].up();
        var recentMenuItems = containerForRecentMenuItems.down('recentmenuitems');

        if (recentMenuItems.hidden === false) {
            recentMenuItems.hide(null, function () {
                if (!containerForRecentMenuItems.down('notification')) {
                    containerForRecentMenuItems.add(Ext.widget('notification'));
                    containerForRecentMenuItems.doLayout();
                } else {
                    containerForRecentMenuItems.down('#panelForNotifications').show();
                    containerForRecentMenuItems.down('#notificationButton').show();
                }
            });
        } else {
            containerForRecentMenuItems.down('#panelForNotifications').show();
        }

        var scheduleContainer = Ext.ComponentQuery.query('schedulecontainer')[0];
        scheduleContainer.addListener('beforedestroy', function () {
            containerForRecentMenuItems.down('#panelForNotifications').hide();
            containerForRecentMenuItems.down('#notificationButton').hide();
            recentMenuItems.show();
        });
    },

    // Сокрытие функционала в зависиости от текущей роли
    accessIsDeniedForCurrentRoles: function (resource) {
        var schedulerGrid = Ext.ComponentQuery.query('#nascheduler')[0];
        var allowedActions = this.getAllowedActionsForCurrentRoleAndResource(resource);

        // Запрещаем создание промо методом выделения + запрещаем resize промо + скрываем уведомление о возможности создания промо.
        if (!allowedActions.some(function (action) { return action === 'Post'; })) {
            // Запрет на создание промо методом выделения + запрет на resize.
            schedulerGrid.setReadOnly(true);
            schedulerGrid.createConfig.hoverTip.disabled = true;
        } else {
            // Без этой строки, после смены роли на ту, где создание промо возможно, hoverTip при наведении не появляется.
            schedulerGrid.createConfig.hoverTip.disabled = false;
        }
    },

    // Возвращает все actions для текущей роли и ресурса.
    getAllowedActionsForCurrentRoleAndResource: function (resource) {
        var allowedActions = [];

        // Получаем список точек достуа текущей роли и ресурса.
        var accessPointsForCurrentRoleAndResouce = App.UserInfo.getCurrentRole().AccessPoints.filter(function (point) {
            return point.Resource === resource;
        });

        // Сбор всех actions для текущей роли и ресурса.
        accessPointsForCurrentRoleAndResouce.forEach(function (point) {
            if (!allowedActions.some(function (action) { return action === point.Action })) {
                allowedActions.push(point.Action);
            }
        });

        return allowedActions;
    },

    accessDeniedForRSmode: function (rec) {
        if (mode.data.value == 1) {
            if (
                (
                    new Date(rec.get("PromoDispatchStartDate")) > new Date(StartDateRS) &&
                    new Date(rec.get("PromoDispatchStartDate")) <= new Date(EndDateRS)
                ) &&
                (
                    rec.get("PromoStatusName") != "Draft" &&
                    rec.get("PromoStatusName") != "Planned" &&
                    rec.get("PromoStatusName") != "Started" &&
                    rec.get("PromoStatusName") != "Finished" &&
                    rec.get("PromoStatusName") != "Closed" &&
                    rec.get("PromoStatusName") != "Cancelled"
                ) &&
                (
                    !rec.get("IsGrowthAcceleration") ||
                    !rec.get("IsInExchange") 
                )
            ) {                    
                this.canEditInRSmode = true;
            } else {                   
                this.canEditInRSmode = false;
            }
        }       
    },

    loopHandlerViewLogWindowCreate: function (record) {
        var loopHandlerViewLogWindow = Ext.widget('loophandlerviewlogwindow', {
            buttons: [{
                text: l10n.ns('core', 'buttons').value('download'),
                itemId: 'download',
                disabled: true,
            }, {
                text: l10n.ns('core', 'buttons').value('close'),
                itemId: 'close'
            }]
        });

        return loopHandlerViewLogWindow;
    },
    // Возвращает переданную дату с временем 23:59:59. TODO: перенести в Util
    getDayEndDateTime: function (date) {
        return new Date(date.setHours(23, 59, 59))
    },

    eventStoreLoading: function (store, clientId) {
        $('#scrollScheduler').data('jsp').scrollToY(0);
        store.removeAll();
        store.uniqueObjectIds.forEach(function (item) {
            item.loaded = false;
        });
        var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
        var ng = scheduler.normalGrid;
        this.setLoadingText(store.uniqueObjectIds, ng);
        if (store.isLoading()) {
            store.resetLoading = true;
        } else {
            this.loadingRecursion(store, clientId);
        }
    },

    //Для загрузки используйте функцию выше
    loadingRecursion: function (store, clientId) {
        if (store.resetLoading) {
            clientId = 0;
            store.data.clear();
            store.resetLoading = false;
        }
        if (!clientId) clientId = 0;
        var newFilter = {
            property: 'ClientTreeId',
            operation: 'Equals',
            value: store.uniqueObjectIds[clientId].objectId
        };
        var filter = store.fixedFilters || {};
        filter['clientfilter'] = newFilter;
        store.fixedFilters = filter;

        // RSmode
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        var StartDateRS = null;
        var EndDateRS = null;
        if (mode) {
            if (mode.data.value == 0) {
                store.getProxy().extraParams.TPMmode = 'Current';
            }
            else if (true) {
                store.getProxy().extraParams.TPMmode = 'RS';
                var RSmodeController = App.app.getController('tpm.rsmode.RSmode');
                RSmodeController.getRSPeriod(function (returnValue) {
                    StartDateRS = new Date(returnValue.StartDate);
                    EndDateRS = new Date(returnValue.EndDate);
                });
            }
        }

        store.suspendEvent("refresh");
        store.load({
            scope: this,
            addRecords: true,
            callback: function (records, operation, success) {
                if (StartDateRS) {
                    records = records.map(function (record) {
                        if (record.data.DispatchesStart < StartDateRS || EndDateRS < record.data.DispatchesStart) {
                            record.set('IsOnHold', true);
                        }
                        if (record.data.MasterPromoId) {
                            record.set('IsOnHold', true);
                        }
                        if (record.data.IsGrowthAcceleration || record.data.IsInExchange) {
                            record.set('IsOnHold', true);
                        }
                        return record
                    });
                }

                //Если закрыли календарь - перестаем грузить
                var nascheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                if (nascheduler) {
                    if (!store.resetLoading) {
                        this.renderEvents(store.uniqueObjectIds[clientId].regPromoId, store.uniqueObjectIds[clientId].inoutPromoId, store.uniqueObjectIds[clientId].otherPromoId, store.uniqueObjectIds[clientId].competitorPromoIds);
                        store.uniqueObjectIds[clientId].loaded = true;
                        clientId = 0;
                        while (store.uniqueObjectIds[clientId] && store.uniqueObjectIds[clientId].loaded) {
                            clientId = clientId + 1;
                        };
                    };
                    if (store.uniqueObjectIds.length > clientId) {
                        this.loadingRecursion(store, clientId);
                    }
                }
            }
        });
    },

    renderEvents: function (regId, inoutId, otherPromoId, competitorPromoIds) {
        var ng = Ext.ComponentQuery.query('#nascheduler')[0].normalGrid;
        var lg = Ext.ComponentQuery.query('#nascheduler')[0].lockedGrid;
        var renderId = regId;
        var records = []//ng.view.getViewRange();
        var rowsCount = 3 + competitorPromoIds.length;
        for (var i = 0; i <= rowsCount; i++) {
            records.push(renderId);
            var eventNode = ng.view.getNode(renderId, false);
            var resourceNode = lg.view.getNode(renderId, false);
            if (eventNode) {
                while (eventNode.hasChildNodes()) {
                    eventNode.removeChild(eventNode.lastChild);
                }
                ng.view.tpl.append(eventNode, ng.view.collectData(records, ng.view.all.startIndex));
                resourceNode.style.height = eventNode.scrollHeight.toString() + "px";
            };
            if (i == 0) {
                renderId = inoutId;
            } else if (i == 1) {
                renderId = otherPromoId;
            } else if (i > 1) {
                renderId = competitorPromoIds[i - 2];
            }

            records = [];
        };
    },

    setLoadingText: function (uniqueObjectIds, ng) {
        for (var j = 0; j < uniqueObjectIds.length; j++) {
            if (!uniqueObjectIds[j].loaded) {

                var node = ng.view.getNode(uniqueObjectIds[j].regPromoId, false);
                if (node && !(node.childNodes[1] && node.childNodes[1].textContent === 'Loading...')) {
                    Ext.DomHelper.append(node, '<td class="overlay">Loading...</td>');
                }

                node = ng.view.getNode(uniqueObjectIds[j].inoutPromoId, false);
                if (node && !(node.childNodes[1] && node.childNodes[1].textContent === 'Loading...')) {
                    Ext.DomHelper.append(node, '<td class="overlay">Loading...</td>');
                }

                node = ng.view.getNode(uniqueObjectIds[j].otherPromoId, false);
                if (node && !(node.childNodes[1] && node.childNodes[1].textContent === 'Loading...')) {
                    Ext.DomHelper.append(node, '<td class="overlay">Loading...</td>');
                }
                for (i = 0; i < uniqueObjectIds[j].competitorPromoIds.length; i++) {
                    node = ng.view.getNode(uniqueObjectIds[j].competitorPromoIds[i], false);
                    if (node && !(node.childNodes[1] && node.childNodes[1].textContent === 'Loading...')) {
                        Ext.DomHelper.append(node, '<td class="overlay">Loading...</td>');
                    }
                }
            }
        };
    },

    getTPMmode: function () {
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        if (mode) {
            if (mode.data.value == 0) {
                return 'Current';
            }
            else if (mode.data.value == 1) {
                return 'RS';
            } else
                return 'Current'
        }
    }
});
