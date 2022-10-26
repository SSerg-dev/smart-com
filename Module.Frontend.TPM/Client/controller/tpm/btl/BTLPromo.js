Ext.define('App.controller.tpm.btl.BTLPromo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'btlpromo directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    selectionchange: this.onGridSelectionChangeCustom,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange,
                    itemdblclick: this.onDetailButtonClick
                },
                'btlpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'btlpromo #datatable': {
                    activate: this.onActivateCard
                },
                'btlpromo #detailform': {
                    activate: this.onActivateCard
                },
                'btlpromo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'btlpromo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'btlpromo #table': {
                    click: this.onTableButtonClick
                },
                'btlpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'btlpromo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'btlpromo #addbutton': {
                    click: this.onAddButtonClick
                },
                'btlpromo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'btlpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'btlpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'btlpromo #close': {
                    click: this.onCloseButtonClick
                },
                '#btlpromochoose #apply': {
                    click: this.onApplyActionButtonClick
                },
                '#btlpromochoose directorygrid': {
                    selectionchange: this.onChoosePromoGridCheckChange,
                },
            }
        });
    },

    onAddButtonClick: function (button) {
        var btlPromo = button.up('btlpromo'),
            btl = btlPromo.up().down('btl'),
            btlGrid = btl.down('grid'),
            selModel = btlGrid.getSelectionModel();

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.showChoosePromoWindow(selected.data.Id, btlGrid, btlPromo.down('grid'));
        }
    },

    showChoosePromoWindow: function (btlId, btlGrid, promoLinkedGrid) {
        var choosepromowindow = Ext.create('App.view.core.base.BaseModalWindow', {
            title: 'Choose Promo',
            name: 'choosepromowindow',
            itemId: 'btlpromochoose',
            width: 950,
            height: 650,
            minWidth: 950,
            minHeight: 650,
            btlId: btlId,
            btlGrid: btlGrid,
            btlPromoGrid: promoLinkedGrid,
            layout: 'fit',
            items: [{
                xtype: 'choosepromo'
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

        var choosePromoWindowGrid = choosepromowindow.down('grid');

        choosepromowindow.show();
        var btlRecord = btlGrid.getStore().getById(btlId);
        var prefilter = {
            operator: "and",
            rules: [{
                operation: "NotEqual",
                property: "PromoStatus.Name",
                value: "Deleted"
            }, {
                operation: "NotEqual",
                property: "PromoStatus.Name",
                value: "Cancelled"
            }, {
                operation: "NotEqual",
                property: "PromoStatus.Name",
                value: "Closed"
            }, {
                operation: "NotEqual",
                property: "PromoStatus.Name",
                value: "Draft"
            }, {
                operation: "Equals",
                property: "EventId",
                value: btlRecord.get('EventId'),
            }]
        };

        var startDate;
        var endDate;
        startDate = btlGrid.getSelectionModel().getSelection()[0].get('StartDate');
        endDate = btlGrid.getSelectionModel().getSelection()[0].get('EndDate');
        startDate = changeTimeZone(startDate, 3, -1);
        endDate = changeTimeZone(endDate, 3, -1);

        if (startDate) {
            prefilter.rules.push({
                operation: "GreaterOrEqual",
                property: "EndDate",
                value: startDate
            });
        }
        if (endDate) {
            prefilter.rules.push({
                operation: "LessOrEqual",
                property: "StartDate",
                value: endDate
            });
        }

        choosePromoWindowStore = choosePromoWindowGrid.getStore();
        // Кастыль, чтобы стор раньше веремени не начал загружаться
        choosePromoWindowStore.isReady = false;
        choosePromoWindowStore.on({
            scope: this,
            beforeload: function () {
                return choosePromoWindowStore.isReady;
            },
            single: true
        });
        // Находим номера промо с прикреплёнными BTL, чтобы отфильровать их
        var params = {
            eventId: btlRecord.get('EventId')
        };
        App.Util.makeRequestWithCallback('BTLPromoes', 'GetPromoesWithBTL', params, function (data) {
            var result = Ext.JSON.decode(data.httpResponse.data.value);
            if (result.success) {
                var promoWithBTLNums = result.data;

                promoWithBTLNums.forEach(function (num) {
                    prefilter.rules.push({
                        operation: "NotEqual",
                        property: "Number",
                        value: num
                    });
                });
                choosePromoWindowStore.isReady = true;
                // Скрываем выбранные промо и промо с прикреплённой BTL статьёй
                choosePromoWindowGrid.getStore().setFixedFilter('PreFilter', prefilter);
            }
            else {
                App.Notify.pushError(result.message);
            }
        });
    },

    // блокировка/разблокировка кнопки Применить при изменении набора галочек в гриде привязки ClientTree к EventClientTree
    onChoosePromoGridCheckChange: function (item) {
        var checkedRows = item.checkedRows;
        var grid = item.view.up('grid');
        var applyButton = grid.up('basewindow[name=choosepromowindow]').down('#apply');

        applyButton.setDisabled(!checkedRows.length);
    },

    onApplyActionButtonClick: function (button) {
        var window = button.up('basewindow[name=choosepromowindow]'),
            grid = window.down('grid'),
            associated = window.btlGrid.up('associatedbtlpromo'),
            promoLinkedGrid = associated.down('btlpromo').down('grid'),
            checkedRows = grid.getSelectionModel().getCheckedRows(),
            promoIds = [],
            promoLinkedStore = promoLinkedGrid.getStore(),
            closedPromo = [];

        if (checkedRows) {
            checkedRows.forEach(function (row) {
                if (row.data.PromoStatusName == 'Closed') {
                    closedPromo.push(number);
                }
            });
        }

        if (closedPromo.length > 0) {
            App.Notify.pushError('Closed promoes cannot be deleted. Promo number: ' + closedPromo.join(", "));
        } else {
            if (window.btlId && window.btlGrid) {
                // Привязка нескольких промо к существующему BTL (делается в мастер-детейл)
                if (checkedRows) {
                    checkedRows.forEach(function (item) {
                        promoIds.push(item.data.Id);
                    });
                }
                window.setLoading(l10n.ns('core').value('savingText'));

                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/odata/BTLPromoes/BTLPromoPost?btlId=" + window.btlId,
                    data: JSON.stringify(promoIds),
                    dataType: "json",
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        var result = Ext.JSON.decode(response.value);
                        if (result.success) {
                            promoLinkedGrid.getStore().on('load', function () {
                                window.setLoading(false);
                            });

                            promoLinkedGrid.getStore().load();
                            window.close();
                        } else {
                            App.Notify.pushError(result.message);
                            window.setLoading(false);
                        }
                    }
                });

            } else if (window.promoLinkedGrid) {
                //окно создания PromoSupport
                var promoLinkedStore = window.promoLinkedGrid.getStore();
                var promoLinkedProxy = promoLinkedStore.getProxy();

                if (checkedRows) {
                    checkedRows.forEach(function (item) {
                        var model = {
                            PromoId: item.data.Id,
                            Promo: item.raw,
                        };

                        var btlPromo = promoLinkedProxy.getReader().readRecords(model).records[0];

                        promoLinkedProxy.data.push(btlPromo);
                    });

                    promoLinkedStore.load();
                }

                window.close();
            }
        }
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

    onGridSelectionChangeCustom: function (selModel, selected) {
        if (selected[0] && selected[0].data.PromoStatusName != "Closed") {
            Ext.ComponentQuery.query('btlpromo')[0].down('#deletebutton').enable();
        } else {
            Ext.ComponentQuery.query('btlpromo')[0].down('#deletebutton').disable();
        }
    },
});