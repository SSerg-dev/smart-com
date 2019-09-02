Ext.define('App.controller.tpm.schedule.SchedulerViewController', {
    extend: 'App.controller.core.CombinedDirectory',
    mixins: ['App.controller.tpm.promo.Promo'],
    alias: 'controller.schedulerviewcontroller',

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
                'readonlydirectorytoolbar #promoDetail': {
                    click: this.onPromoDetailButtonClick
                },
                'schedulecontainer #refresh': {
                    click: this.onRefreshButtonClick
                },
                'schedulecontainer #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'schedulecontainer #createbutton': {
                    click: this.onCreateButtonClick
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

    onpromoBeforeEventDrag: function (scheduler, record, e, eOpts) {
        return this.isDraggable(record);
    },

    isDraggable: function (rec) {
        return rec.get('PromoStatusSystemName') && (rec.get('PromoStatusSystemName') == 'Draft' || rec.get('PromoStatusSystemName') == 'DraftPublished') && (rec.get('StartDate') > Date.now());
    },

    onpromoBeforeEventDrop: function (view, dragContext, e, eOpts) {
        var me = this;
        var record = dragContext.eventRecords[0];
        me.__dragContext = dragContext;
        var calendarGrid = Ext.ComponentQuery.query('scheduler');
        if (calendarGrid.length > 0) {
            me.calendarSheduler = calendarGrid[0];
        }
        if (dragContext.timeDiff == 0) {
            dragContext.finalize(false);
            return false;
        } else if (dragContext.startDate < Date.now()) {
            App.Notify.pushInfo('New start date must be after current date.');
            dragContext.finalize(false);
            return false;
        } else {
            //// Save reference to context to be able to finalize drop operation after user clicks yes/no button.
            Ext.Msg.confirm('Please confirm',
                'Do you want to update the Promo: ' + record.get('Name'),
                me.onDragAndDropConfirm,  // The button callback
                me);                 // scope
            return false;
        }
    },

    // Подтверждение изменения времени promo и установка DispatchesDate аналогично переносу 2 концов
    onDragAndDropConfirm: function (btn) {
        var me = this;
        var dragContext = me.__dragContext;
        if (btn === 'yes') {
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
                        dispatchDateForCurrentPromoAfterResize = Ext.Date.add(record.get('DispatchesStart'), Ext.Date.DAY, deltaDaysBeforeAndAfterResize);
                        // Если dispatch дата, сформированная из настроек клиента, совпадает с текущей dispatch датой
                        if (Ext.Date.isEqual(dispatchDateForCurrentPromoAfterResize, dispatchDateForCurrentClientAfterResize) === true) {
                            record.set('DispatchesStart', dispatchDateForCurrentPromoAfterResize);
                        }
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
                        dispatchDateForCurrentPromoAfterResize = Ext.Date.add(record.get('DispatchesEnd'), Ext.Date.DAY, deltaDaysBeforeAndAfterResize);

                        // Если dispatch дата, сформированная из настроек клиента, совпадает с текущей dispatch датой
                        if (Ext.Date.isEqual(dispatchDateForCurrentPromoAfterResize, dispatchDateForCurrentClientAfterResize) === true) {
                            record.set('DispatchesEnd', dispatchDateForCurrentPromoAfterResize);
                        }
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
                        //record.set('StartDate', dragContext.startDate);
                        //record.data.EndDate = dragContext.endDate;
                        // в dragContext endDate с временем 23.59...
                        var fixedEndDate = new Date(dragContext.endDate.getFullYear(), dragContext.endDate.getMonth(), dragContext.endDate.getDate(), 0, 0, 0);
                        fixedEndDate = Sch.util.Date.add(fixedEndDate, Sch.util.Date.HOUR, -(offset + 3));
                        record.set('EndDate', fixedEndDate);
                        record.save({
                            callback: function (record, operation, success) {
                                if (success) {
                                    dragContext.finalize(true);
                                    me.calendarSheduler.getEventStore().load(function (records, operation, success) {
                                        me.calendarSheduler.setLoading(false);
                                    });
                                } else {
                                    me.calendarSheduler.setLoading(false);
                                    dragContext.finalize(false);
                                }
                            }
                        });
                    }

                }
            })
        } else {
            dragContext.finalize(false);
        }
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
        button.up('window').setLoading(true);

        me.sendExportRequest(year);
    },

    sendExportRequest: function (year) {
        var me = this,
            scheduler = Ext.ComponentQuery.query('schedulecontainer')[0].down('scheduler'),
            store = scheduler.getEventStore(),
            clientStore = scheduler.getResourceStore(),
            ids = clientStore.getRange(0, clientStore.getCount() - 1).map(
                function (client) {
                    return client.get('ObjectId')
                }),
            actionName = 'ExportSchedule',
            resource = 'Promoes';

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                $data: {
                    clients: ids,
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
            var model = Ext.ModelManager.getModel('App.model.tpm.promo.Promo');
            var viewClassName = "App.view.tpm.promo.HistoricalPromo";
            Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) })
                .show().down('grid').getStore()
                .setFixedFilter('HistoricalObjectId', {
                    property: '_ObjectId',
                    operation: 'Equals',
                    value: this.getRecordId(record)//selModel.getSelection()[0].getId()
                });
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
        var baseFilter = store.fixedFilters['statusfilter']; // Сохраняем фильтр по типу
        var ids = ['statusfilter'];
        var nodes = [{
            property: baseFilter.property,
            operation: baseFilter.operation,
            value: baseFilter.value,
        }];

        if (!button.hasCls('sheduler_promostatusfilter_button_selected')) {
            ids.push('PromoStatusfilter');
            nodes.push({
                property: 'PromoStatusSystemName',
                operation: 'Equals',
                value: 'DraftPublished'
            });
            store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, true);


            store.load();
            button.up('custombigtoolbar').down('#schedulefilterdraftbutton').removeCls('sheduler_promostatusfilter_button_selected');
            button.addClass('sheduler_promostatusfilter_button_selected');
        } else {
            button.up('custombigtoolbar').down('#schedulefilterdraftbutton').removeCls('sheduler_promostatusfilter_button_selected');
            store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, true);
            store.load();
            button.removeCls('sheduler_promostatusfilter_button_selected');
        }
    },

    onFilterDraftButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        var store = scheduler.getEventStore();
        var baseFilter = store.fixedFilters['statusfilter']; // Сохраняем фильтр по типу
        var ids = ['statusfilter'];
        var nodes = [{
            property: baseFilter.property,
            operation: baseFilter.operation,
            value: baseFilter.value,
        }];

        if (!button.hasCls('sheduler_promostatusfilter_button_selected')) {
            ids.push('PromoStatusfilter');
            nodes.push({
                property: 'PromoStatusSystemName',
                operation: 'Equals',
                value: 'Draft'
            });
            store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, true);
            store.load();
            button.up('custombigtoolbar').down('#schedulefilterdraftpublbutton').removeCls('sheduler_promostatusfilter_button_selected');
            button.addClass('sheduler_promostatusfilter_button_selected');
        } else {
            button.up('custombigtoolbar').down('#schedulefilterdraftpublbutton').removeCls('sheduler_promostatusfilter_button_selected');
            store.clearFixedFilters(true);
            store.setSeveralFixedFilters(ids, nodes, true);
            store.load();
            button.removeCls('sheduler_promostatusfilter_button_selected');
        }
    },
    promoRightButtonClick: function (panel, rec, e) {
        var me = this;
        e.stopEvent();
        var status = rec.get('PromoStatusSystemName').toLowerCase();
        var isDeletable = status == 'draft' || status == 'draftpublished';
        var postAccess = me.getAllowedActionsForCurrentRoleAndResource('Promoes').some(function (action) { return action === 'Post' });
        if (!panel.ctx) {
            panel.ctx = new Ext.menu.Menu({
                width: 90,
                items: [{
                    text: l10n.ns('tpm', 'Schedule').value('Copy'),
                    glyph: 0xf18f,
                    hidden: !postAccess,
                    handler: function () {
                        panel.eventCopy = panel.ctx.rec;
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
                                var record = panel.ctx.rec;
                                record.destroy({
                                    scope: this,
                                    success: function () {
                                        panel.getEventStore().load(function (records, operation, success) {
                                            panel.setLoading(false);
                                        });
                                    },
                                    failure: function () {
                                        panel.setLoading(false);
                                        App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'))
                                    }
                                });
                            }
                        }
                    }
                }, {
                    text: l10n.ns('tpm', 'Schedule').value('View'),
                    glyph: 0xfba8,
                    handler: function (button) {
                        button.assignedRecord = panel.ctx.rec;
                        me.mixins["App.controller.tpm.promo.Promo"].onDetailButtonClick.call(me, button);
                    }
                }]
            });
        }
        if (panel.ctx) {
            promoStore = me.getPromoStore();
            promoStore.load({
                id: rec.getId(),
                scope: this,
                callback: function (records, operation, success) {
                    if (success) {
                        panel.ctx.rec = records[0];
                        panel.ctx.down('#promodeletebutton').setVisible(postAccess && isDeletable);
                        panel.ctx.showAt(e.getXY());
                    }
                }
            });
        }
    },

    onPromoDragCreation: function (view, createContext, e, el, eOpts) {
        var me = this;
        if (createContext.start > Date.now()) {
                var schedulerData,
                isInOutClient = createContext.resourceRecord.get('InOut');
            createContext.end = me.getDayEndDateTime(createContext.end);          
            if (!view.schedulerView.eventCopy) {
                schedulerData = { schedulerContext: createContext };
                schedulerData.isCopy = false;
                me.createPromo(schedulerData, isInOutClient);
            } else {
                var ctx = new Ext.menu.Menu({
                    cls: "scheduler-context-menu",
                    items: [{
                        text: 'Paste',
                        glyph: 0xf191,
                        handler: function () {
                            if (!!isInOutClient != !!view.schedulerView.eventCopy.get('InOut')) {
                                return me.finalizeContextWithError(createContext, l10n.ns('tpm', 'Schedule').value('CopyInOutError'));
                            } else {
                                schedulerData = view.schedulerView.eventCopy;
                                schedulerData.schedulerContext = createContext
                                schedulerData.isCopy = true;
								me.createPromo(schedulerData, isInOutClient);
                            }
                        }
                    }, {
                        text: 'Create',
                        glyph: 0xf0f3,
                        handler: function () {
                            schedulerData = { schedulerContext: createContext };
                            schedulerData.isCopy = false;
                            me.createPromo(schedulerData, isInOutClient);
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

    finalizeContextWithError: function (context, message) {
        App.Notify.pushError(message);
        context.finalize(false); // убрать выделенную область
        return false; // чтобы предотвратить автоматическое создание промо
    },

    onCreateButtonClick: function () {
        this.detailButton = null;
        this.createPromo();
    },

    createPromo: function (schedulerData, inOut) {
        this.mixins["App.controller.tpm.promo.Promo"].onCreateButtonClick.call(this, null, null, schedulerData, inOut);
    },

    onCreateInOutButtonClick: function () {
        this.detailButton = null;
        this.createPromo(null, true);
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
        var calendarGrid = Ext.ComponentQuery.query('scheduler');
        //Проверка по дате начала
        if (resizeContext.eventRecord.start < Date.now() || resizeContext.start < Date.now()) {
            App.Notify.pushError(l10n.ns('tpm', 'text').value('wrongStartDate'));
            resizeContext.finalize(false);
            //Открытый календарь - обновить его
            if (calendarGrid.length > 0) {
                calendarGrid[0].resourceStore.load();
            }
            return false;
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

    // Подтверждение изменения продолжительности промо
    onResizeConfirm: function (btn) {
        var me = this;
        if (btn === 'yes') {
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
                            record.save({
                                callback: function (record, operation, success) {
                                    if (success) {
                                        me.__resizeContext.finalize(true);
                                        me.calendarSheduler.getEventStore().load(function (records, operation, success) {
                                            me.calendarSheduler.setLoading(false);
                                        });
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
        } else {
            me.__resizeContext.finalize(false);
        }
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
    },

    onFilterButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        var store = scheduler.getEventStore();
        Ext.widget('extfilter', store.getExtendedFilter()).show();
    },

    onRefreshButtonClick: function (button) {
        var scheduler = button.up('panel').down('scheduler');
        scheduler.reloadEventsStore();
    },

    onPromoDetailButtonClick: function (button) {
        var promoDetailPanel = button.up('promodetailtabpanel');
        var record = promoDetailPanel.event;
        button.assignedRecord = record;

        Ext.ComponentQuery.query('readonlydirectorytoolbar')[0].setDisabled(true);
        this.mixins["App.controller.tpm.promo.Promo"].onDetailButtonClick.call(this, button);
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
        });
        scheduler.lockedGrid.relayEvents(scheduler.lockedGrid.getStore(), ['load']);
        scheduler.relayEvents(scheduler.lockedGrid.getSelectionModel(), ['selectionchange'], 'row');
        // добавление кнопки "Детали" в нижнюю панель
        var system = Ext.ComponentQuery.query('system')[0];
        var promoTab = {
            title: l10n.ns('tpm', 'compositePanelTitles').value('PromoDetail'),
            itemId: 'promoDetailTab',
            items: {
                xtype: 'promodetailtabpanel',
                selectedUI: 'blue-selectable-panel'
            },

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
        this.fillTabPanel(eventRecord, scheduler, false);
    },
    // При даблклике срабатывает onEventdbClick и 2 раза onEventClick
    singleClickTask: new Ext.util.DelayedTask(this.delayOnEventClick),

    onEventdbClick: function (scheduler, eventRecord, e, eOpts) {
        this.singleClickTask.cancel();
        this.fillTabPanel(eventRecord, scheduler, true)
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

        // при изменении содержимого календаря меняем скролл
        var me = this;
        $('#' + scheduler.down('schedulergridview').id).on("DOMSubtreeModified", function () {
            var table = $(this).find('table');
            var heightTable = table.height();
            var currentHeightScroll = $('#scrollSchedulerH').height();

            if (heightTable != undefined && heightTable != 0 && heightTable != currentHeightScroll)
                me.setScroll();
        });

        // Notifications
        if (this.getAllowedActionsForCurrentRoleAndResource('Promoes').some(function (action) { return action === 'Post'; })) {
            this.showOrHideNotificationsPanel();
        }
    },

    getPromoStore: function () {
        return Ext.ComponentQuery.query('#nascheduler')[0].promoStore;
    },

    // заполнение дашборда
    // events - запись(массив записей) промо. resourceRecord - клиент(для суммы по клиенту), showTab - необходимость открыть нижнюю панель
    fillTabPanel: function (events, scheduler, showTab) {
        var system = Ext.ComponentQuery.query('system')[0],
            promoPanel = system.down('promodetailtabpanel'),
            promoStore = this.getPromoStore();
        promoStore.load({
            id: events.getId(), //set the id here
            scope: this,
            callback: function (records, operation, success) {
                if (success) {
                    var rec = records[0];
                    promoPanel.event = rec;
                    // Заголовок 1-й панели дашборда - название промо
                    var promoDetailPanel = promoPanel.down('#promodetailpanel');

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
    }
});
