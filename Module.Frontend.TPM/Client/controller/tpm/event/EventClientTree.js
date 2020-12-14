Ext.define('App.controller.tpm.event.EventClientTree', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'eventclienttree directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'eventclienttree #datatable': {
                    activate: this.onActivateCard
                },
                'eventclienttree #detailform': {
                    activate: this.onActivateCard
                },
                'eventclienttree #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'eventclienttree #detailform #next': {
                    click: this.onNextButtonClick
                },
                'eventclienttree #detail': {
                    click: this.onDetailButtonClick
                },
                'eventclienttree #table': {
                    click: this.onTableButtonClick
                },
                'eventclienttree #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'eventclienttree #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'eventclienttree #addbutton': {
                    click: this.onAddButtonClick
                },
                'eventclienttree #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'eventclienttree #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'eventclienttree #refresh': {
                    click: this.onRefreshButtonClick
                },
                'eventclienttree #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'eventclienttree #exportbutton': {
                    click: this.onExportButtonClick
                },
                'eventclienttree #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'eventclienttree #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'eventclienttree #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'basewindow[name=chooseclienttreewindow] #apply': {
                    click: this.onApplyActionButtonClick
                },
                'baseclienttreeview directorygrid': {
                    selectionchange: this.onChooseClientTreeGridCheckChange,
                },    
                'baseclienttreeview gridcolumn[cls=select-all-header]': {
                    headerclick: this.onSelectAllRecordsClick,
                    afterrender: this.clearBaseSelectAllRecordsHandler,
                },
            }
        });
    },

    onAddButtonClick: function (button) {
        var eventClientTree = button.up('eventclienttree'),
            event = eventClientTree.up().down('event'),
            eventGrid = event.down('grid'),        
            selModel = eventGrid.getSelectionModel();

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.showChooseClientTreeWindow(selected.data.Id, eventGrid, eventClientTree.down('grid'));
        }        
    },

    showChooseClientTreeWindow: function (eventId, eventGrid, eventClientTreeGrid) {
        var chooseclienttreewindow = Ext.create('App.view.core.base.BaseModalWindow', {
            title: 'Choose Client',
            name: 'chooseclienttreewindow',
            width: 950,
            height: 650,
            minWidth: 950,
            minHeight: 650,
            eventId: eventId,
            eventGrid: eventGrid,
            eventClientTreeGrid: eventClientTreeGrid,
            layout: 'fit',
            items: [{
                xtype: 'baseclienttreeview',
                listeners: {
                    afterrender: function () {
                        var me = this,
                            grid = me.down('directorygrid');
                        grid.multiSelect = true;// включение множественной выборки
                    }
                }
            }],
            buttons: [{
                text: l10n.ns('core', 'buttons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'buttons').value('ok'),
                ui: 'green-button-footer-toolbar',
                itemId: 'apply',
                disabled: true
            }]
        });

        // проверка на наличие проставленных галок
        var chooseClientTreeWindowGrid = chooseclienttreewindow.down('grid');

        // установака галочек на уже выбранных клиентов при открытии грида в окне создания/редактирования EventClientTree
        if (eventClientTreeGrid) {
            var eventClientTreeStore = eventClientTreeGrid.getStore(),
                count = eventClientTreeStore.getCount(),
                eventClientTreeRecords = count > 0 ? eventClientTreeStore.getRange(0, count) : [],
                chooseClientTreeStore = chooseClientTreeWindowGrid.getStore();

            chooseClientTreeStore.on('load', function () {
                var checkedEventClientTreeRecords = [];

                eventClientTreeRecords.forEach(function (checkedRow) {
                    var item = chooseClientTreeStore.findRecord('Id', checkedRow.data.ClientTreeId);
                    if (item) {
                        checkedEventClientTreeRecords.push(item);
                    }
                });
                chooseClientTreeWindowGrid.getSelectionModel().checkRows(checkedEventClientTreeRecords);
            })
            chooseclienttreewindow.show();
        }
    },


    //// блокировка/разблокировка кнопки Применить при изменении набора галочек в гриде привязки ClientTree к EventClientTree
    onChooseClientTreeGridCheckChange: function (item) {
        var checkedRows = item.checkedRows;
        var grid = item.view.up('grid');
        var button = grid.up('basewindow[name=chooseclienttreewindow]').down('#apply');

        if (checkedRows.length > 0) {
            button.setDisabled(false);
        } else {
            button.setDisabled(true);
        }
    },


    onApplyActionButtonClick: function (button) {
        var clientTree = button.up('basewindow[name=chooseclienttreewindow]'),
            clientTreeGrid = clientTree.down('baseclienttreeview').down('directorygrid'),
            checkedNodes = clientTreeGrid.getSelectionModel().getCheckedRows(),
            checkedIds = new String();

        if (checkedNodes) {
            checkedNodes.forEach(function (item) {
                checkedIds = checkedIds.concat(item.data.Id.toString() + ';');
            });
        }

        //Удаляем последнюю запятую, что бы работал парсер на беке
        checkedIds = checkedIds.slice(0, -1);

        var window = button.up('window'),
            eventGrid = window.eventGrid,
            eventId = eventGrid.getSelectionModel().getSelection()[0].getId();

        window.setLoading(l10n.ns('core').value('savingText'));
        $.ajax({
            type: "POST",
            cache: false,
            url: "/odata/EventClientTrees?eventId=" + eventId,
            data: checkedIds,
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (response) {
                var result = Ext.JSON.decode(response.value);
                if (result.success) {
                    window.destroy();
                    window.eventClientTreeGrid.getStore().load();
                    window.setLoading(false);
                }
                else {
                    App.Notify.pushError(result.message);
                    window.setLoading(false);
                }
            }
        });
    },

    onSelectAllRecordsClick: function (headerCt, header) {
        var grid = header.up('directorygrid'),
            win = headerCt.up('selectorwindow'),
            store = grid.getStore(),
            selModel = grid.getSelectionModel(),
            recordsCount = store.getTotalCount(),
            functionChecker = selModel.checkedRows.length == recordsCount ? selModel.uncheckRows : selModel.checkRows;

        if (recordsCount > 0) {
            grid.setLoading(true);
            store.getRange(0, recordsCount, {
                callback: function () {
                    if (recordsCount > 0) {
                        functionChecker.call(selModel, store.getRange(0, recordsCount));
                        grid.setLoading(false);
                    }
                    if (win) {
                        win.down('#select')[selModel.hasChecked() ? 'enable' : 'disable']();
                    }
                }
            });
        }

        grid.fireEvent('selectionchange', selModel);
    },

    clearBaseSelectAllRecordsHandler: function (header) {
        // избавляемся от некорректного обработчика
        var headerCt = header.up('headercontainer');

        if (headerCt.events.headerclick.listeners.length == 2) {
            headerCt.events.headerclick.listeners.pop();
        }
    },
});