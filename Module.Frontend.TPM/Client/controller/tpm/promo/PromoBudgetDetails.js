Ext.define('App.controller.tpm.promo.PromoBudgetDetails', {
    extend: 'Ext.app.Controller',

    init: function () {
        this.listen({
            component: {
                'promobudgetdetails': {
                    afterrender: this.onAfterRender,
                    added: this.onAdded,
                },
                'promobudgetdetails #addSubItem': {
                    click: this.onAddSubItemClick
                },
            }
        });
    },

    onAfterRender: function (widget) {
        //var addSubItemBtn = widget.down('#addSubItem');
        //var access = App.UserInfo.getCurrentRole().AccessPoints.find(function (el) {
        //    return el.Resource.toLowerCase() == 'promosupportpromoes' && el.Action.toLowerCase() == 'managesubitems'
        //});

        //addSubItemBtn.setDisabled(!access);
        widget.record = Ext.ComponentQuery.query('promoeditorcustom')[0];
    },

    onAdded: function (widget) {
        var prefix = widget.fact ? 'Actual' : 'Plan';
        widget.down('fieldset').setTitle(prefix + ' — ' + l10n.ns('tpm', 'PromoBudgetDetails').value('titleFields'));

        widget.budgetItemsName.forEach(function (itemName) {
            widget.addBudgetItemField(itemName);
        });
    },

    // просмотр подстатей для статьи
    showSubItemDetail: function (promoId, itemName, editable, fact, detailWidget) {
        var me = this;
        var promoSupportPromolWidget = Ext.widget(detailWidget.widget);

        // оставляем только нужные кнопки
        promoSupportPromolWidget.addListener('afterrender', function () {
            var toolbar = promoSupportPromolWidget.down('custombigtoolbar');

            toolbar.down('#createbutton').hide();
            toolbar.down('#deletebutton').hide();

            if ((!editable || Ext.ComponentQuery.query('promoeditorcustom')[0].down('#changePromo').isVisible())) {
                toolbar.down('#updatebutton').hide();
            }
        });

        // загружать нужно только для текущего промо
        var store = promoSupportPromolWidget.down('grid').getStore();
        store.setFixedFilter('PromoIdFilter', {
            operator: 'and',
            rules: [{
                property: 'PromoId',
                operation: 'Equals',
                value: promoId
            }, {
                property: 'PromoSupport.BudgetSubItem.BudgetItem.Name',
                operation: 'Contains',
                value: itemName
            }, {
                property: 'PromoSupport.BudgetSubItem.BudgetItem.Budget.Name',
                operation: 'Contains',
                value: detailWidget.budgetName
            }]
        });

        var selectWindow = Ext.widget('selectorwindow', {
            title: itemName + ' details',
            itemId: Ext.String.format('{0}_{1}_{2}', 'closureUploadPromoProducts', promoSupportPromolWidget.getXType(), 'selectorwindow'),
            items: [promoSupportPromolWidget],
            buttons: [{
                text: l10n.ns('core', 'buttons').value('close'),
                style: { "background-color": "#3F6895" },
                listeners: {
                    click: function (button) {
                        button.up('selectorwindow').close();
                    }
                }
            }],
        });

        // если можем редактировать - обновляем значения после закрытия
        if (editable) {
            selectWindow.addListener('beforeclose', function () {
                me.updateValuesInPromoForm(detailWidget);
            });
        }

        selectWindow.show();
    },

    // открыть грид с подстатьями для выбора
    onAddSubItemClick: function (btn) {
        var me = this;
        var detailWidget = btn.up('promobudgetdetails');
        var picker = this.picker = Ext.widget('selectorwindow', {
            title: l10n.ns('tpm', 'PromoBudgetDetails').value('addSubItem'),
            items: [{
                xtype: 'promosupportchoose',
                isSearch: true
            }]
        });

        var promoSupport = picker.down('promosupportchoose');
        var grid = promoSupport.down('grid');
        var store = grid.getStore();
        var selectedIds = [];

        promoSupport.addListener('afterrender', function () {
            // редактировать ничего нельзя
            var toolbar = picker.down('custompromosupportbigtoolbar');

            toolbar.items.items.forEach(function (el) {
                if (el.xtype == 'button' && el.itemId != 'extfilterbutton')
                    el.hide();
            });
        });

        picker.show();
        picker.down('#select').setDisabled(false);
        picker.down('#select').addListener('click', function () {
            var datesAreValide = true;
            var checkedRows = picker.down('grid').getSelectionModel().checkedRows.items;
            var promoController = App.app.getController('tpm.promo.Promo');
            var promoRecord = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom')[0]);

            checkedRows.forEach(function (row) {
                if (!row.data.StartDate || !row.data.EndDate || row.data.StartDate >= promoRecord.data.EndDate || row.data.EndDate <= promoRecord.data.StartDate) {
                    datesAreValide = false;
                }
            });

            if (!datesAreValide) {
                Ext.Msg.show({
                    title: l10n.ns('core').value('confirmTitle'),
                    msg: l10n.ns('tpm', 'PromoSupportPromo').value('ConfirmNewDatesPromoCard'),
                    fn: onMsgBoxClose,
                    scope: this,
                    icon: Ext.Msg.QUESTION,
                    buttons: Ext.Msg.YESNO,
                    buttonText: {
                        yes: l10n.ns('core', 'buttons').value('save'),
                        no: l10n.ns('core', 'buttons').value('cancel')
                    }
                });
            } else {
                onMsgBoxClose('yes');
            }

            function onMsgBoxClose(buttonId) {
                if (buttonId === 'yes') {
                    // отправляем выбранные статьи           
                    promoSupport.setLoading(true);

                    var selModel = grid.getSelectionModel();

                    // привет Odata и её контроллерам
                    var checked = '';
                    selectedIds.forEach(function (subItemId) {
                        checked += subItemId + ';';
                    });
                    checked = checked.slice(0, -1);

                    var params = 'promoId=' + detailWidget.record.promoId + '&subItemsIds=' + checked + '&budgetName=' + detailWidget.budgetName;

                    $.ajax({
                        dataType: 'json',
                        url: '/odata/PromoSupportPromoes/ManageSubItems?' + params,
                        type: 'POST',
                        success: function () {
                            me.clearFields(detailWidget);
                            me.updateValuesInPromoForm(detailWidget);
                            picker.close();
                            promoSupport.setLoading(false);
                        },
                        error: function (data) {
                            promoSupport.setLoading(false);
                            App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
                        }
                    });
                }
            }
        });

        // чекаем уже прикрепленные подстатьи
        var firstLoad = true;
        store.on('load', function () {
            if (firstLoad) {
                promoSupport.setLoading(true);
                setTimeout(function () {
                    $.ajax({
                        dataType: 'json',
                        url: '/odata/PromoSupportPromoes/GetLinkedSubItems?promoId=' + detailWidget.record.promoId,
                        type: 'POST',
                        success: function (data) {
                            selectedIds = data.data;
                            var selModel = grid.getSelectionModel();

                            selectedIds.forEach(function (promoSupportId) {
                                var record = store.getById(promoSupportId);
                                if (record) {
                                    selModel.checkRows(record);
                                }
                            });

                            selModel.on('checked', function (record) {
                                if (selectedIds.indexOf(record.internalId) == -1) {
                                    selectedIds.push(record.internalId);
                                }
                            });

                            selModel.on('unchecked', function (record) {
                                if (selectedIds.indexOf(record.internalId) != -1) {
                                    delete selectedIds[selectedIds.indexOf(record.internalId)];
                                }
                            });

                            firstLoad = false;
                            promoSupport.setLoading(false);
                        },
                        error: function (data) {
                            promoSupport.setLoading(false);
                            App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
                        }
                    });
                }, 200);
            }
        }, this);

        store.on('prefetch', function () {
            if (selectedIds) {
                var selModel = grid.getSelectionModel();
                selectedIds.forEach(function (promoSupportId) {
                    var record = store.getById(promoSupportId);
                    if (record) {
                        selModel.checkRows(record);
                    }
                });
            }
        }, this);

        // загружать подстатьи нужно только для текущего клиента и текущего бюджета
        store.setFixedFilter('ClientTreeId', this.getFilterForGetPromoSupport(detailWidget));

        columns = grid.headerCt.getGridColumns();
        if (columns.length > 0 && columns[0].hasOwnProperty('isCheckerHd')) {
            columns[0].show();
        }
    },

    // Обновление значений полей в форме PROMO
    updateValuesInPromoForm: function (widget) {
        App.model.tpm.promo.Promo.load(widget.record.promoId, {
            callback: function (record, operation) {
                var promoForm = widget.up('promoeditorcustom');
                var promoController = App.app.getController('tpm.promo.Promo');

                var grid = Ext.ComponentQuery.query('#promoGrid')[0];
                var directorygrid = grid ? grid.down('directorygrid') : null;

                promoForm.model = record;
                promoController.reFillPromoForm(promoForm, record, directorygrid);

                // Если создание из календаря - обновляем календарь
                var scheduler = Ext.ComponentQuery.query('#nascheduler')[0];
                if (scheduler) {
                    scheduler.resourceStore.reload();
                    scheduler.eventStore.reload();
                }
            },
            failure: function () {
                App.Notify.pushError('Error!');
            }
        });
    },

    /*getValuesForItems: function (widget) {
        widget.setLoading(true);

        // список статей
        var itemsName = '';
        if (widget.budgetItemsName) {
            widget.budgetItemsName.forEach(function (itemName) {
                itemsName += itemName + ';'
            });
            itemsName = itemsName.slice(0, -1);
        }

        // получаем суммы по статьям
        var params = 'promoId=' + widget.record.promoId + '&fact=' + widget.fact + '&budgetName=' + widget.budgetName
            + '&itemsName=' + itemsName + '&costProd=' + widget.costProd;

        $.ajax({
            dataType: 'json',
            url: '/odata/PromoSupportPromoes/GetValuesForItems?' + params,
            type: 'POST',
            success: function (data) {
                var prefix1 = widget.fact ? 'Actual' : 'Plan';
                var prefix2 = widget.costProd ? 'CostProd' : '';

                data.data.forEach(function (el) {
                    widget.down('[name=budgetDet-' + prefix1 + prefix2 + el.key + ']').setValue(el.value);
                });
                widget.setLoading(false);
            },
            error: function (data) {
                widget.setLoading(false);
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },*/

    clearFields: function (widget) {
        var budgetItemsPanel = widget.down('fieldset');

        budgetItemsPanel.items.items.forEach(function (trigger) {
            trigger.reset();
        });
    },

    // формирует фильтр для отбора подстатей только для текущего клиента и текущего бюджета
    getFilterForGetPromoSupport: function (widget) {
        // идем по иерархии и родительских тоже подбираем
        var clientHierarchy = '';
        var clientRules = [];
        var clients = widget.record.clientHierarchy.split('>');
        clients.forEach(function (client) {
            if (clientHierarchy.length > 0)
                clientHierarchy += ' > ';

            clientHierarchy += client.trim();

            clientRules.push({
                property: 'ClientTree.FullPathName',
                operation: 'Equals',
                value: clientHierarchy
            });
        });

        var result = {
            operator: 'and',
            rules: [{
                operator: 'or',
                rules: clientRules
            }, {
                property: 'BudgetSubItem.BudgetItem.Budget.Name',
                operation: 'Contains',
                value: widget.budgetName
            }]
        };

        return result;
    }
});
