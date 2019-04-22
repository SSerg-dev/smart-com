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
                    minValue: new Date(),
                    needReadOnly: true,
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');

                            if (newValue && field.isValid()) {
                                var endDateField = field.up().down('datefield[name=DurationEndDate]');
                                var endDateValue = endDateField.getValue();
                                
                                selectionModel = field.up('promoeditorcustom').down('clienttreegrid').getSelectionModel();
                                var checked = field.up('promoeditorcustom').down('clienttreegrid').getView().getChecked();

                                if (checked.length != 0 && selectionModel.hasSelection()) {
                                    var record = selectionModel.getSelection()[0];
                                    var isBeforeStart = record.get('IsBeforeStart');
                                    var daysStart = record.get('DaysStart');
                                    var isDaysStart = record.get('IsDaysStart');
                                    var dispatchStartDate = field.up('promoperiod').down('[name=DispatchStartDate]');
                                    var daysForDispatchStart = record.get('DaysStart');

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

                                //endDateField.getPicker().setMinDate(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                                endDateField.setMinValue(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                                endDateField.getPicker().setMinDate();

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

                                if (endDateValue && endDateField.isValid() && newValue) {
                                    validDates = true;

                                    panel.durationPeriod = 'c ' + Ext.Date.format(newValue, "d.m.Y") + ' по ' + Ext.Date.format(endDateValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        panel.durationPeriod + '<br>Dispatch: ' + (panel.dispatchPeriod ? panel.dispatchPeriod : '') + '</p>';

                                    var days = (endDateValue - newValue) / 86400000;
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
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');

                            if (newValue && field.isValid()) {
                                var startDateField = field.up().down('datefield[name=DurationStartDate]');
                                var startDateValue = startDateField.getValue();
                                
                                selectionModel = field.up('promoeditorcustom').down('clienttreegrid').getSelectionModel();
                                var checked = field.up('promoeditorcustom').down('clienttreegrid').getView().getChecked();

                                if (checked.length != 0 && selectionModel.hasSelection()) {
                                    var record = selectionModel.getSelection()[0];
                                    var isBeforeEnd = record.get('IsBeforeEnd');
                                    var daysEnd = record.get('DaysEnd');
                                    var isDaysEnd = record.get('IsDaysEnd');
                                    var dispatchEndDate = field.up('promoperiod').down('[name=DispatchEndDate]');
                                    var daysForDispatchEnd = record.get('DaysEnd');

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
                                startDateField.setMaxValue(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                                startDateField.getPicker().setMaxDate();
                                

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

                                if (startDateValue && startDateField.isValid() && newValue) {
                                    validDates = true;

                                    panel.durationPeriod = 'c ' + Ext.Date.format(startDateValue, "d.m.Y") + ' по ' + Ext.Date.format(newValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        panel.durationPeriod + '<br>Dispatch: ' + (panel.dispatchPeriod ? panel.dispatchPeriod : '') + '</p>';

                                    var days = (newValue - startDateValue) / 86400000;
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
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');

                            if (newValue && field.isValid()) {
                                var endDateField = field.up().down('datefield[name=DispatchEndDate]');
                                var endDateValue = endDateField.getValue();                                

                                //endDateField.getPicker().setMinDate(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                                endDateField.setMinValue(Ext.Date.add(newValue, Ext.Date.DAY, 1));
                                endDateField.getPicker().setMinDate();
                                endDateField.validate();

                                if (endDateValue && endDateField.isValid() && newValue) {
                                    validDates = true;

                                    panel.dispatchPeriod = 'c ' + Ext.Date.format(newValue, "d.m.Y") + ' по ' + Ext.Date.format(endDateValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        (panel.durationPeriod ? panel.durationPeriod : '') + '<br>Dispatch: ' + panel.dispatchPeriod + '</p>';

                                    var days = (endDateValue - newValue) / 86400000;
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
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    maxText: l10n.ns('tpm', 'Promo').value('failMaxDate'),
                    minText: l10n.ns('tpm', 'Promo').value('failMinDate'),
                    listeners: {
                        change: function (field, newValue, oldValue) {
                            var validDates = false;
                            var panel = field.up('promoperiod');

                            if (newValue && field.isValid()) {
                                var startDateField = field.up().down('datefield[name=DispatchStartDate]');
                                var startDateValue = startDateField.getValue();                                

                                //startDateField.getPicker().setMaxDate(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                                startDateField.setMaxValue(Ext.Date.add(newValue, Ext.Date.DAY, -1));
                                startDateField.getPicker().setMaxDate();
                                startDateField.validate();

                                if (startDateValue && startDateField.isValid() && newValue) {
                                    validDates = true;

                                    panel.dispatchPeriod = 'c ' + Ext.Date.format(startDateValue, "d.m.Y") + ' по ' + Ext.Date.format(newValue, "d.m.Y");
                                    var promoPeriodButton = Ext.ComponentQuery.query('button[itemId=btn_promo_step4]')[0];
                                    var text = '<b>' + l10n.ns('tpm', 'promoStap').value('basicStep4') + '</b><br><p>Promo: ' +
                                        (panel.durationPeriod ? panel.durationPeriod : '') + '<br>Dispatch: ' + panel.dispatchPeriod + '</p>';

                                    var days = (newValue - startDateValue) / 86400000;
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