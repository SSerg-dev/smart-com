Ext.define('App.view.tpm.promo.PromoPeriod', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoperiod',

    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'hbox',
            align: 'stretchmax'
        },
        items: [{
            xtype: 'custompromopanel',
            minWidth: 245,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: 'Duration',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'datefield',
                    editable: false,
                    name: 'DurationStartDate',
                    flex: 1,
                    padding: '0 5 5 5',
                    fieldLabel: 'Start date',
                    labelAlign: 'top',
                    needReadOnly: true,
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    onExpand: function () {
                        var field = Ext.ComponentQuery.query('datefield[name=DurationStartDate]')[0];
                        var endDateField = field.up().down('datefield[name=DurationEndDate]');

                        setMonthPicker(field, endDateField);
                    },
                    listeners: {
                        afterrender: function (field) {
                            if (App.UserInfo.getCurrentRole()['SystemName'] != "SupportAdministrator") {
                                var minValue = new Date();
                                var currentTimeZoneOffsetInHours = minValue.getTimezoneOffset();
                                var minValueInt = minValue.getTime();
                                field.setMinValue(new Date(minValueInt + currentTimeZoneOffsetInHours * 60000 + 10800000));
                                field.getPicker().setValue(field.minValue);
                            }
                            var promoeditorcustom = field.up('promoeditorcustom');
                            if (promoeditorcustom.rsStartEnd) {
                                var startdate = new Date(promoeditorcustom.rsStartEnd.StartDate);
                                startdate.setDate(startdate.getDate() + 14);
                                var enddate = new Date(promoeditorcustom.rsStartEnd.EndDate);
                                field.setMinValue(startdate);
                                field.setMaxValue(enddate);
                            }
                        },
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');
                            if (newValue /*&& field.isValid()*/) {
                                var promoController = App.app.getController('tpm.promo.Promo');
                                var budgetYearCombo = field.up('promoeditorcustom').down('promobudgetyear').down('combobox');
                                var budgetYearComboStore = budgetYearCombo.getStore();
                                var userRole = App.UserInfo.getCurrentRole()['SystemName'];
                                var isOffInvoice = Ext.getCmp('OffInvoice').getValue();

                                var newStore = new Ext.data.Store({
                                    fields: ['year']
                                });

                                if (isOffInvoice) {
                                    var month = newValue.getMonth() + 1;
                                    var year = newValue.getFullYear();

                                    if (['SupportAdministrator', 'DemandFinance'].includes(userRole)) {
                                        newStore.add({ year: year - 2 });
                                        newStore.add({ year: year - 1 });
                                        newStore.add({ year: year });
                                        newStore.add({ year: year + 1 });
                                    }
                                    else {
                                        if (month == 1) {
                                            year--;
                                        }
                                        if (month == 12 || month == 1) {
                                            newStore.add({ year: year });
                                            newStore.add({ year: year + 1 });
                                        }
                                        else {
                                            newStore.add({ year: year });
                                        }
                                    }

                                    budgetYearComboStore.loadData(newStore.getRange(), false);

                                    if (month == 12 || month == 1) {
                                        budgetYearCombo.setValue();
                                    }
                                    else {
                                        budgetYearCombo.setValue(year);
                                    }
                                }

                                var endDateField = field.up().down('datefield[name=DurationEndDate]');
                                var endDateValue = endDateField.getValue();
                                var promoClientForm = field.up('promoeditorcustom').down('promoclient');

                                if (promoClientForm && promoClientForm.clientTreeRecord) {
                                    var record = promoClientForm.clientTreeRecord;
                                    var isBeforeStart = record.IsBeforeStart;
                                    var daysStart = record.DaysStart;
                                    var isDaysStart = record.IsDaysStart;
                                    var dispatchStartDate = field.up('promoperiod').down('[name=DispatchStartDate]');
                                    var daysForDispatchStart = record.DaysStart;

                                    if (isBeforeStart !== null && daysStart !== null && isDaysStart !== null) {
                                        if (!isDaysStart) {
                                            daysForDispatchStart *= 7;
                                        }

                                        var resultDateForDispatchStart = null;

                                        if (isBeforeStart) {
                                            resultDateForDispatchStart = Ext.Date.add(field.getValue(), Ext.Date.DAY, -daysForDispatchStart);
                                        } else {
                                            resultDateForDispatchStart = Ext.Date.add(field.getValue(), Ext.Date.DAY, daysForDispatchStart);
                                        }

                                        if (resultDateForDispatchStart) {
                                            dispatchStartDate.setValue(resultDateForDispatchStart);
                                        }
                                    }
                                }

                                endDateField.setMinValue(newValue);
                                endDateField.getPicker().setMinDate(newValue);
                                endDateField.validate();

                                //Установка завершенности при promo в статусе Closed, Finished, Started
                                var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                var status = field.up('promoeditorcustom').promoStatusName;
                                if (promoPeriodButton) {
                                    if (status == 'Closed' || status == 'Finished' || status == 'Started') {
                                        promoPeriodButton.removeCls('notcompleted');
                                        promoPeriodButton.setGlyph(0xf133);
                                        promoPeriodButton.isComplete = true;
                                    }
                                }

                                if (endDateValue /*&& endDateField.isValid()*/ && newValue) {
                                    validDates = true;

                                    panel.durationPeriod = 'c ' + Ext.Date.format(newValue, "d.m.Y") + ' по ' + Ext.Date.format(endDateValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        panel.durationPeriod + '<br>Dispatch: ' + (panel.dispatchPeriod ? panel.dispatchPeriod : '') + '</p>';

                                    var days = getDaysBetweenDates(endDateValue, newValue); //(endDateValue - newValue) / 86400000;
                                    var titleText = 'Promo duration (' + ++days + (days === 1 ? ' day)' : ' days)');
                                    field.up().setTitle(titleText);
                                    promoPeriodButton.setText(text);

                                    if (panel.durationPeriod && panel.dispatchPeriod) {
                                        promoPeriodButton.removeCls('notcompleted');
                                        promoPeriodButton.setGlyph(0xf133);
                                        promoPeriodButton.isComplete = true;
                                    }

                                    var panel = field.up('promoeditorcustom');
                                    var mainTab = panel.down('button[itemId=btn_promo]');
                                    var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                    checkMainTab(stepButtons, mainTab);
                                }

                                // сбрасываем эвент на стандартный
                                var promoeditorcustom = field.up('promoeditorcustom');
                                if (oldValue && !promoeditorcustom.isCreating) {                                    
                                    if (!['SupportAdministrator'].includes(userRole) && newValue != oldValue) {                                        
                                        promoController.refreshPromoEvent(field.up('promoeditorcustom'), true);
                                    }
                                }
                                if (promoeditorcustom.isCreating) {
                                    promoController.setEventBTL();
                                }                                
                            }

                            if (!validDates) {
                                panel.durationPeriod = null;
                                var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: <br>Dispatch: '
                                    + (panel.dispatchPeriod ? panel.dispatchPeriod : '') + '</p>';

                                var titleText = 'Promo duration';
                                field.up().setTitle(titleText);
                                promoPeriodButton.setText(text);

                                promoPeriodButton.addCls('notcompleted');
                                promoPeriodButton.setGlyph(0xf130);
                                promoPeriodButton.isComplete = false;

                                var panel = field.up('promoeditorcustom');
                                var mainTab = panel.down('button[itemId=btn_promo]');
                                var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                checkMainTab(stepButtons, mainTab);
                            }
                        }
                    }
                }, {
                    xtype: 'datefield',
                    editable: false,
                    name: 'DurationEndDate',
                    flex: 1,
                    padding: '0 5 5 5',
                    fieldLabel: 'End date',
                    labelAlign: 'top',
                    minValue: new Date(),
                    needReadOnly: true,
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    // переопределение нужно для того чтобы при преобразовании значения в дату подклеивать время 23:59:59 для корректного отображения в календаре
                    //parseDate: function (value) {
                    //    if (!value || Ext.isDate(value)) {
                    //        return value;
                    //    }

                    //    var me = this,
                    //        val = me.safeParse(value, me.format),
                    //        altFormats = me.altFormats,
                    //        altFormatsArray = me.altFormatsArray,
                    //        i = 0,
                    //        len;

                    //    if (!val && altFormats) {
                    //        altFormatsArray = altFormatsArray || altFormats.split('|');
                    //        len = altFormatsArray.length;
                    //        for (; i < len && !val; ++i) {
                    //            val = me.safeParse(value, altFormatsArray[i]);
                    //        }
                    //    }
                    //    return new Date(val.setHours(23, 59, 59));
                    //},
                    onExpand: function () {
                        var field = Ext.ComponentQuery.query('datefield[name=DurationEndDate]')[0];
                        var startDateField = field.up().down('datefield[name=DurationStartDate]');

                        setMonthPicker(field, startDateField);
                    },
                    listeners: {
                        afterrender: function (field) {
                            var promoeditorcustom = field.up('promoeditorcustom');
                            if (promoeditorcustom.rsStartEnd) {
                                var startdate = new Date(promoeditorcustom.rsStartEnd.StartDate);
                                startdate.setDate(startdate.getDate() + 14);
                                var enddate = new Date(promoeditorcustom.rsStartEnd.EndDate);
                                field.setMinValue(startdate);
                                field.setMaxValue(enddate);
                            }
                        },
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');

                            if (newValue /*&& field.isValid()*/) {
                                var promoController = App.app.getController('tpm.promo.Promo');
                                var startDateField = field.up().down('datefield[name=DurationStartDate]');
                                var startDateValue = startDateField.getValue();
                                var promoClientForm = field.up('promoeditorcustom').down('promoclient');

                                if (promoClientForm && promoClientForm.clientTreeRecord) {
                                    var record = promoClientForm.clientTreeRecord;
                                    var isBeforeEnd = record.IsBeforeEnd;
                                    var daysEnd = record.DaysEnd;
                                    var isDaysEnd = record.IsDaysEnd;
                                    var dispatchEndDate = field.up('promoperiod').down('[name=DispatchEndDate]');
                                    var daysForDispatchEnd = record.DaysEnd;

                                    if (isBeforeEnd !== null && daysEnd !== null && isDaysEnd !== null) {

                                        if (!isDaysEnd) {
                                            daysForDispatchEnd *= 7;
                                        }

                                        var resultDateForDispatchEnd = null;

                                        if (isBeforeEnd) {
                                            resultDateForDispatchEnd = Ext.Date.add(field.getValue(), Ext.Date.DAY, -daysForDispatchEnd);
                                        } else {
                                            resultDateForDispatchEnd = Ext.Date.add(field.getValue(), Ext.Date.DAY, daysForDispatchEnd);
                                        }

                                        if (resultDateForDispatchEnd) {
                                            dispatchEndDate.setValue(resultDateForDispatchEnd);
                                        }
                                    }
                                }


                                //startDateField.getPicker().setMaxDate(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                                startDateField.setMaxValue(newValue);
                                startDateField.getPicker().setMaxDate();
                                startDateField.validate();

                                //Установка завершенности при promo в статусе Closed, Finished, Started
                                var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                var status = field.up('promoeditorcustom').promoStatusName;
                                if (promoPeriodButton) {
                                    if (status == 'Closed' || status == 'Finished' || status == 'Started') {
                                        promoPeriodButton.removeCls('notcompleted');
                                        promoPeriodButton.setGlyph(0xf133);
                                        promoPeriodButton.isComplete = true;
                                    }
                                }

                                if (startDateValue /*&& startDateField.isValid()*/ && newValue) {
                                    validDates = true;

                                    panel.durationPeriod = 'c ' + Ext.Date.format(startDateValue, "d.m.Y") + ' по ' + Ext.Date.format(newValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        panel.durationPeriod + '<br>Dispatch: ' + (panel.dispatchPeriod ? panel.dispatchPeriod : '') + '</p>';

                                    var days = getDaysBetweenDates(newValue, startDateValue); //(newValue - startDateValue) / 86400000;
                                    var titleText = 'Promo duration (' + ++days + (days === 1 ? ' day)' : ' days)');
                                    field.up().setTitle(titleText);
                                    promoPeriodButton.setText(text);

                                    if (panel.durationPeriod && panel.dispatchPeriod) {
                                        promoPeriodButton.removeCls('notcompleted');
                                        promoPeriodButton.setGlyph(0xf133);
                                        promoPeriodButton.isComplete = true;
                                    }

                                    var panel = field.up('promoeditorcustom');
                                    var mainTab = panel.down('button[itemId=btn_promo]');
                                    var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                    checkMainTab(stepButtons, mainTab);
                                }

                                var promoeditorcustom = field.up('promoeditorcustom');
                                if (promoeditorcustom.isCreating) {
                                    promoController.setEventBTL();
                                }
                            }

                            if (!validDates) {
                                panel.durationPeriod = null;
                                var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: <br>Dispatch: '
                                    + (panel.dispatchPeriod ? panel.dispatchPeriod : '') + '</p>';

                                var titleText = 'Promo duration';
                                field.up().setTitle(titleText);
                                promoPeriodButton.setText(text);

                                promoPeriodButton.addCls('notcompleted');
                                promoPeriodButton.setGlyph(0xf130);
                                promoPeriodButton.isComplete = false;

                                var panel = field.up('promoeditorcustom');
                                var mainTab = panel.down('button[itemId=btn_promo]');
                                var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                checkMainTab(stepButtons, mainTab);
                            }
                        }
                    },
                }]
            }]
        }, {
            xtype: 'splitter',
            itemId: 'splitter_4',
            cls: 'custom-promo-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_4]')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            xtype: 'custompromopanel',
            minWidth: 245,
            flex: 1,
            items: [{
                xtype: 'fieldset',
                title: 'Dispatch',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'datefield',
                    editable: false,
                    name: 'DispatchStartDate',
                    flex: 1,
                    layout: 'anchor',
                    padding: '0 5 5 5',
                    fieldLabel: 'Start date',
                    labelAlign: 'top',
                    needReadOnly: true,
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager', 'DemandPlanning'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    onExpand: function () {
                        var field = Ext.ComponentQuery.query('datefield[name=DispatchStartDate]')[0];
                        var endDateField = field.up().down('datefield[name=DispatchEndDate]');

                        setMonthPicker(field, endDateField);
                    },
                    listeners: {
                        afterrender: function (field) {
                            var promoeditorcustom = field.up('promoeditorcustom');
                            if (promoeditorcustom.rsStartEnd) {
                                field.setMinValue(new Date(promoeditorcustom.rsStartEnd.StartDate));
                                field.setMaxValue(new Date(promoeditorcustom.rsStartEnd.EndDate));
                            }
                        },
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');

                            if (newValue /*&& field.isValid()*/) {
                                var budgetYearCombo = field.up('promoeditorcustom').down('promobudgetyear').down('combobox');
                                var budgetYearComboStore = budgetYearCombo.getStore();
                                var userRole = App.UserInfo.getCurrentRole()['SystemName'];
                                var isOnInvoice = Ext.getCmp('OnInvoice').getValue();
                                var newStore = new Ext.data.Store({
                                    fields: ['year']
                                });
                                if (isOnInvoice) {
                                    var month = newValue.getMonth() + 1;
                                    var year = newValue.getFullYear();
                                    if (['SupportAdministrator', 'DemandFinance'].includes(userRole)) {
                                        newStore.add({ year: year - 2 });
                                        newStore.add({ year: year - 1 });
                                        newStore.add({ year: year });
                                        newStore.add({ year: year + 1 });
                                    }
                                    else {
                                        if (month == 1) {
                                            year--;
                                        }
                                        if (month == 12 || month == 1) {
                                            newStore.add({ year: year });
                                            newStore.add({ year: year + 1 });
                                        }
                                        else {
                                            newStore.add({ year: year });
                                        }
                                    }

                                    budgetYearComboStore.loadData(newStore.getRange(), false);

                                    if (month == 12 || month == 1) {
                                        //budgetYearCombo.setValue();
                                    }
                                    else {
                                        budgetYearCombo.setValue(year);
                                    }
                                }

                                var endDateField = field.up().down('datefield[name=DispatchEndDate]');
                                var endDateValue = endDateField.getValue();

                                endDateField.setMinValue(newValue);
                                endDateField.getPicker().setMinDate(newValue);
                                endDateField.validate();

                                if (endDateValue && endDateField.isValid() && newValue) {
                                    validDates = true;

                                    panel.dispatchPeriod = 'c ' + Ext.Date.format(newValue, "d.m.Y") + ' по ' + Ext.Date.format(endDateValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        (panel.durationPeriod ? panel.durationPeriod : '') + '<br>Dispatch: ' + panel.dispatchPeriod + '</p>';

                                    var days = getDaysBetweenDates(endDateValue, newValue); //(endDateValue - newValue) / 86400000;
                                    var titleText = 'Dispatch (' + ++days + (days === 1 ? ' day)' : ' days)');
                                    field.up().setTitle(titleText);
                                    promoPeriodButton.setText(text);

                                    if (panel.durationPeriod && panel.dispatchPeriod) {
                                        promoPeriodButton.removeCls('notcompleted');
                                        promoPeriodButton.setGlyph(0xf133);
                                        promoPeriodButton.isComplete = true;
                                    }

                                    var panel = field.up('promoeditorcustom');
                                    var mainTab = panel.down('button[itemId=btn_promo]');
                                    var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                    checkMainTab(stepButtons, mainTab);
                                }
                            }

                            if (!validDates) {
                                panel.dispatchPeriod = null;
                                var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                    (panel.durationPeriod ? panel.durationPeriod : '') + '<br>Dispatch: </p>';

                                var titleText = 'Dispatch';
                                field.up().setTitle(titleText);
                                promoPeriodButton.setText(text);

                                promoPeriodButton.addCls('notcompleted');
                                promoPeriodButton.setGlyph(0xf130);
                                promoPeriodButton.isComplete = false;

                                var panel = field.up('promoeditorcustom');
                                var mainTab = panel.down('button[itemId=btn_promo]');
                                var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                checkMainTab(stepButtons, mainTab);
                            }
                        }
                    }
                }, {
                    xtype: 'datefield',
                    editable: false,
                    name: 'DispatchEndDate',
                    flex: 1,
                    padding: '0 5 5 5',
                    fieldLabel: 'End date',
                    labelAlign: 'top',
                    needReadOnly: true,
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager', 'DemandPlanning'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    onExpand: function () {
                        var field = Ext.ComponentQuery.query('datefield[name=DispatchEndDate]')[0];
                        var startDateField = field.up().down('datefield[name=DispatchStartDate]');

                        setMonthPicker(field, startDateField);
                    },
                    listeners: {
                        afterrender: function (field) {
                            var promoeditorcustom = field.up('promoeditorcustom');
                            if (promoeditorcustom.rsStartEnd) {
                                field.setMinValue(new Date(promoeditorcustom.rsStartEnd.StartDate));
                                field.setMaxValue(new Date(promoeditorcustom.rsStartEnd.EndDate));
                            }
                        },
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');

                            if (newValue /*&& field.isValid()*/) {
                                var startDateField = field.up().down('datefield[name=DispatchStartDate]');
                                var startDateValue = startDateField.getValue();

                                //startDateField.getPicker().setMaxDate(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                                startDateField.setMaxValue(newValue);
                                startDateField.getPicker().setMaxDate();
                                startDateField.validate();

                                if (startDateValue /*&& startDateField.isValid()*/ && newValue) {
                                    validDates = true;

                                    panel.dispatchPeriod = 'c ' + Ext.Date.format(startDateValue, "d.m.Y") + ' по ' + Ext.Date.format(newValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        (panel.durationPeriod ? panel.durationPeriod : '') + '<br>Dispatch: ' + panel.dispatchPeriod + '</p>';

                                    var days = getDaysBetweenDates(newValue, startDateValue); //(newValue - startDateValue) / 86400000;
                                    var titleText = 'Dispatch (' + ++days + (days === 1 ? ' day)' : ' days)');
                                    field.up().setTitle(titleText);
                                    promoPeriodButton.setText(text);

                                    if (panel.durationPeriod && panel.dispatchPeriod) {
                                        promoPeriodButton.removeCls('notcompleted');
                                        promoPeriodButton.setGlyph(0xf133);
                                        promoPeriodButton.isComplete = true;
                                    }

                                    var panel = field.up('promoeditorcustom');
                                    var mainTab = panel.down('button[itemId=btn_promo]');
                                    var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                    checkMainTab(stepButtons, mainTab);
                                }
                            }

                            if (!validDates) {
                                panel.dispatchPeriod = null;
                                var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                    (panel.durationPeriod ? panel.durationPeriod : '') + '<br>Dispatch: </p>';

                                var titleText = 'Dispatch';
                                field.up().setTitle(titleText);
                                promoPeriodButton.setText(text);

                                promoPeriodButton.addCls('notcompleted');
                                promoPeriodButton.setGlyph(0xf130);
                                promoPeriodButton.isComplete = false;

                                var panel = field.up('promoeditorcustom');
                                var mainTab = panel.down('button[itemId=btn_promo]');
                                var stepButtons = panel.down('panel[itemId=promo]').down('custompromotoolbar');

                                checkMainTab(stepButtons, mainTab);
                            }
                        }
                    }
                }]
            }]
        }]
    }]
})