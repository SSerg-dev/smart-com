Ext.define('App.view.tpm.promo.PromoCalculation', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promocalculation',

    initComponent: function () {
        this.callParent(arguments);

        // PlanPromoBTL может редактировать только Customer Marketing
        // чтобы не городить лишних функций в контроллере пока проверку делаем тут
        var btl = this.down('numberfield[name=PlanPromoBTL]');
        var currentRole = App.UserInfo.getCurrentRole();
        var editableBtl = btl.editableRoles.indexOf(currentRole['SystemName']) >= 0;

        btl.setEditable(editableBtl);
    },

    items: [{
        xtype: 'container',
        cls: 'promo-editor-custom-scroll-items',
        items: [{
            xtype: 'panel',
            name: 'calculations_step1',
            itemId: 'calculations_step1',
            bodyStyle: { "background-color": "#eceff1" },
            cls: 'promoform-item-wrap',
            height: 210,
            header: {
                title: l10n.ns('tpm', 'promoStap').value('calculationStep1'),
                cls: 'promo-header-item',
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'custompromopanel',
                padding: '30 0 24 0',
                layout: {
                    type: 'hbox',
                    align: 'middle',
                    pack: 'center'
                },
                items: [{
                    xtype: 'container',
                    width: '100%',
                    name: 'costAndBudget',
                    margin: '0 10 0 20',
                    layout: 'vbox',
                    buttonId: 'btn_calculations_step1',
                    defaults: {
                        minValue: 0,
                        padding: '0 0 10 0',
                    },
                    items: [{
                        xtype: 'container',
                        layout: 'hbox',
                        width: '100%',
                        defaults: {
                            minValue: 0,
                            labelAlign: 'top',
                            padding: '0 10 0 0',
                            flex: 4
                        },
                        items: [{
                            xtype: 'label',
                            text: l10n.ns('tpm', 'Promo').value('Plan'),
                            width: 40,
                            maxWidth: 40,
                            margin: '29 0 0 0',
                            style: {
                                fontWeight: 'bold',
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'PlanPromoTIShopper',
                            editable: false,
                            hideTrigger: true,
                            readOnly: true,
                            needReadOnly: true,
                            setReadOnly: function () { return false },
                            width: 120,
                            minWidth: 120,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIShopper'),
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'trigger',
                            name: 'PlanPromoTIMarketing',
                            format: '0.00',
                            needReadOnly: false,
                            editable: false,
                            setReadOnly: function () { return false; },
                            trigger1Cls: 'form-info-trigger',
                            width: 120,
                            minWidth: 120,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIMarketing'),
                            // для правильной локализации
                            valueToRaw: function (value) {
                                return Ext.util.Format.number(value, '0.00');
                            },
                            rawToValue: function (value) {
                                var parsedValue = parseFloat(String(value).replace(Ext.util.Format.decimalSeparator, "."))
                                return isNaN(parsedValue) ? null : parsedValue;
                            },
                            onTrigger1Click: function () {
                                var promoWindow = this.up('promoeditorcustom');
                                var marketingTiDetail = Ext.widget('promobudgetdetails');

                                marketingTiDetail.record = promoWindow;
                                marketingTiDetail.widget = 'pspshortplancalculation';
                                marketingTiDetail.parentField = this;
                                marketingTiDetail.budgetName = 'Marketing';
                                marketingTiDetail.budgetItemsName = ['X-sites', 'Catalog', 'POSM'];
                                marketingTiDetail.title = 'Marketing TI details';
                                marketingTiDetail.callBackFunction = function () {
                                    App.model.tpm.promo.Promo.load(promoWindow.promoId, {
                                        callback: function (record, operation) {
                                            var planCostProd = promoWindow.down('promocalculation trigger[name=PlanPromoCostProduction]');
                                            var factMarkTI = promoWindow.down('promocalculation trigger[name=ActualPromoTIMarketing]');
                                            var factCostProd = promoWindow.down('promocalculation trigger[name=ActualPromoCostProduction]');

                                            planCostProd.setValue(record.data.PlanPromoCostProduction)
                                            factMarkTI.setValue(record.data.ActualPromoTIMarketing);
                                            factCostProd.setValue(record.data.ActualPromoCostProduction);
                                        }
                                    });
                                };
                                marketingTiDetail.show();
                            },
                            listeners: {
                                afterrender: function (el) {
                                    el.triggerCell.addCls('form-info-trigger-cell')
                                },
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'PlanPromoBranding',
                            hideTrigger: true,
                            needReadOnly: true,
                            width: 100,
                            minWidth: 100,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBranding'),
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'PlanPromoBTL',
                            hideTrigger: true,
                            needReadOnly: true,
                            width: 80,
                            minWidth: 80,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBTL'),
                            editableRoles: ['Administrator', 'CMManager', 'CustomerMarketing', 'FunctionalExpert'],
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'trigger',
                            name: 'PlanPromoCostProduction',
                            width: 135,
                            minWidth: 135,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProduction'),
                            format: '0.00',
                            needReadOnly: false,
                            editable: false,
                            setReadOnly: function () { return false; },
                            trigger1Cls: 'form-info-trigger',
                            // для правильной локализации
                            valueToRaw: function (value) {
                                return Ext.util.Format.number(value, '0.00');
                            },
                            rawToValue: function (value) {
                                var parsedValue = parseFloat(String(value).replace(Ext.util.Format.decimalSeparator, "."))
                                return isNaN(parsedValue) ? null : parsedValue;
                            },
                            onTrigger1Click: function () {
                                var promoWindow = this.up('promoeditorcustom');
                                var costProdDetail = Ext.widget('promobudgetdetails');

                                costProdDetail.record = promoWindow;
                                costProdDetail.widget = 'pspshortplancostprod';
                                costProdDetail.costProd = true;
                                costProdDetail.parentField = this;
                                costProdDetail.budgetName = 'Marketing';
                                costProdDetail.budgetItemsName = ['X-sites', 'Catalog', 'POSM'];
                                costProdDetail.title = 'Cost production details';
                                costProdDetail.callBackFunction = function () {
                                    App.model.tpm.promo.Promo.load(promoWindow.promoId, {
                                        callback: function (record, operation) {
                                            var planMarkTI = promoWindow.down('promocalculation trigger[name=PlanPromoTIMarketing]');
                                            var factMarkTI = promoWindow.down('promocalculation trigger[name=ActualPromoTIMarketing]');
                                            var factCostProd = promoWindow.down('promocalculation trigger[name=ActualPromoCostProduction]');

                                            planMarkTI.setValue(record.data.PlanPromoTIMarketing);
                                            factMarkTI.setValue(record.data.ActualPromoTIMarketing);
                                            factCostProd.setValue(record.data.ActualPromoCostProduction);
                                        }
                                    })
                                };
                                costProdDetail.show();
                            },
                            listeners: {
                                afterrender: function (el) {
                                    el.triggerCell.addCls('form-info-trigger-cell')
                                },
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'PlanPromoCost',
                            editable: false,
                            hideTrigger: true,
                            readOnly: true,
                            needReadOnly: true,
                            setReadOnly: function () { return false },
                            width: 100,
                            minWidth: 100,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCost'),
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }]
                    }, {
                        xtype: 'container',
                        width: '100%',
                        layout: 'hbox',
                        defaults: {
                            minValue: 0,
                            labelAlign: 'top',
                            padding: '0 10 0 0',
                            flex: 4
                        },
                        items: [{
                            xtype: 'label',
                            text: l10n.ns('tpm', 'Promo').value('Fact'),
                            width: 40,
                            maxWidth: 40,
                            margin: '8 0 0 0',
                            style: {
                                fontWeight: 'bold',
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'ActualPromoTIShopper',
                            hideTrigger: true,
                            needReadOnly: true,
                            width: 120,
                            minWidth: 120,
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'trigger',
                            name: 'ActualPromoTIMarketing',
                            needReadOnly: false,
                            editable: false,
                            width: 120,
                            minWidth: 120,
                            setReadOnly: function () { return false; },
                            trigger1Cls: 'form-info-trigger',
                            // для правильной локализации
                            valueToRaw: function (value) {
                                return Ext.util.Format.number(value, '0.00');
                            },
                            rawToValue: function (value) {
                                var parsedValue = parseFloat(String(value).replace(Ext.util.Format.decimalSeparator, "."))
                                return isNaN(parsedValue) ? null : parsedValue;
                            },
                            onTrigger1Click: function () {
                                var promoWindow = this.up('promoeditorcustom');
                                var marketingTiDetail = Ext.widget('promobudgetdetails');

                                marketingTiDetail.record = promoWindow;
                                marketingTiDetail.widget = 'pspshortfactcalculation';
                                marketingTiDetail.parentField = this;
                                marketingTiDetail.fact = true;
                                marketingTiDetail.budgetName = 'Marketing';
                                marketingTiDetail.budgetItemsName = ['X-sites', 'Catalog', 'POSM'];
                                marketingTiDetail.title = 'Marketing TI details';
                                marketingTiDetail.show();
                            },
                            listeners: {
                                afterrender: function (el) {
                                    el.triggerCell.addCls('form-info-trigger-cell')
                                },
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'ActualPromoBranding',
                            hideTrigger: true,
                            needReadOnly: true,
                            width: 100,
                            minWidth: 100,
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'ActualPromoBTL',
                            hideTrigger: true,
                            needReadOnly: true,
                            width: 80,
                            minWidth: 80,
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'trigger',
                            name: 'ActualPromoCostProduction',
                            needReadOnly: false,
                            editable: false,
                            width: 135,
                            minWidth: 135,
                            setReadOnly: function () { return false; },
                            trigger1Cls: 'form-info-trigger',
                            // для правильной локализации
                            valueToRaw: function (value) {
                                return Ext.util.Format.number(value, '0.00');
                            },
                            rawToValue: function (value) {
                                var parsedValue = parseFloat(String(value).replace(Ext.util.Format.decimalSeparator, "."))
                                return isNaN(parsedValue) ? null : parsedValue;
                            },
                            onTrigger1Click: function () {
                                var promoWindow = this.up('promoeditorcustom');
                                var costProdDetail = Ext.widget('promobudgetdetails');

                                costProdDetail.record = promoWindow;
                                costProdDetail.widget = 'pspshortfactcostprod';
                                costProdDetail.costProd = true;
                                costProdDetail.parentField = this;
                                costProdDetail.fact = true;
                                costProdDetail.budgetName = 'Marketing';
                                costProdDetail.budgetItemsName = ['X-sites', 'Catalog', 'POSM'];
                                costProdDetail.title = 'Cost production details';
                                costProdDetail.show();
                            },
                            listeners: {
                                afterrender: function (el) {
                                    el.triggerCell.addCls('form-info-trigger-cell')
                                },
                                change: this.calculationChangeListener
                            } 
                        }, {
                            xtype: 'numberfield',
                            name: 'ActualPromoCost',
                            editable: false,
                            hideTrigger: true,
                            readOnly: true,
                            needReadOnly: true,
                            setReadOnly: function () { return false },
                            width: 100,
                            minWidth: 100,
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }]
                    }]
                }]
            }]
        }, {
            xtype: 'panel',
            name: 'calculations_step2',
            itemId: 'calculations_step2',
            bodyStyle: { "background-color": "#eceff1" },
            cls: 'promoform-item-wrap',
            height: 210,
            header: {
                title: l10n.ns('tpm', 'promoStap').value('calculationStep2'),
                cls: 'promo-header-item',
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'custompromopanel',
                padding: '10 0 26 0',
                layout: {
                    type: 'hbox',
                    align: 'middle',
                    pack: 'center'
                },
                items: [{
                    xtype: 'container',
                    width: '100%',
                    name: 'activity',
                    margin: '0 10 0 20',
                    layout: 'vbox',
                    buttonId: 'btn_calculations_step2',
                    defaults: {
                        minValue: 0,
                        padding: '0 0 10 0',
                    },
                    items: [{
                        xtype: 'container',
                        layout: 'hbox',
                        width: '100%',
                        defaults: {
                            minValue: 0,
                            labelAlign: 'top',
                            padding: '0 10 0 0',
                            flex: 4
                        },
                        items: [{
                            xtype: 'label',
                            text: l10n.ns('tpm', 'Promo').value('Plan'),
                            margin: '48 0 0 0',
                            width: 40,
                            maxWidth: 40,
                            style: {
                                fontWeight: 'bold',
                            }
                        }, {
                            xtype: 'container',
                            layout: 'vbox',
                            flex: 4,
                            cls: 'promo-calculation-activity-uplift-container',
                            items: [{
                                xtype: 'container',
                                cls: 'promo-calculation-activity-uplift-top',
                                width: '100%',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle',
                                    pack: 'center'
                                },
                                items: [{
                                    xtype: 'container',
                                    html: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'),
                                    flex: 1
                                }, {
                                    xtype: 'checkbox',
                                    cls: 'promo-calculation-activity-uplift-checkbox',
                                    boxLabel: 'Locked update',
                                    name: 'NeedRecountUplift',
                                    disabled: true,
                                    flex: 1,
                                    listeners: {
                                        change: this.calculationUpliftCheckbox
                                    }
                                }]
                            }, {
                                xtype: 'container',
                                width: '100%',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                items: [{
                                    xtype: 'numberfield',
                                    width: '100%',
                                    name: 'PlanPromoUpliftPercent',
                                    needReadOnly: true,
                                    minValue: 0,
                                    listeners: {
                                        change: this.calculationChangeListener
                                    }
                                }]
                            }]
                        }, {
                            xtype: 'numberfield',
                            name: 'PlanPromoIncrementalLSV',
                            hideTrigger: true,
                            needReadOnly: true,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'),
                            padding: '18 10 0 0',
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'PlanPromoIncrementalLSV',
                            hideTrigger: true,
                            needReadOnly: true,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'),
                            padding: '18 10 0 0',
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'trigger',
                            name: 'PlanPromoPostPromoEffectLSV',
                            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoPostPromoEffectLSV'),
                            padding: '18 10 0 0',
                            needReadOnly: false,
                            editable: false,
                            width: 120,
                            minWidth: 120,
                            setReadOnly: function () { return false; },
                            trigger1Cls: 'form-info-trigger',
                            onTrigger1Click: function () {
                                var window = Ext.ComponentQuery.query('promoeditorcustom')[0];
                                var promoController = App.app.getController('tpm.promo.Promo');
                                var record = promoController.getRecord(window);
                                var widget = Ext.widget('postpromoeffectdetails');

                                var planPostPromoEffectW1 = {
                                    xtype: 'numberfield',
                                    name: 'PlanPromoPostPromoEffectLSV',
                                    readOnly: true,
                                    cls: 'readOnlyField',
                                    fieldLabel: l10n.ns('tpm', 'PostPromoEffectDetails').value('PlanPromoPostPromoEffectLSVW1'),
                                    labelStyle: 'margin-top: 6px !important',
                                    labelSeparator: '',
                                    labelWidth: 200,
                                    value: record.data.PlanPromoPostPromoEffectLSVW1
                                };

                                var planPostPromoEffectW2 = {
                                    xtype: 'numberfield',
                                    name: 'PlanPromoPostPromoEffectLSV',
                                    readOnly: true,
                                    cls: 'readOnlyField',
                                    fieldLabel: l10n.ns('tpm', 'PostPromoEffectDetails').value('PlanPromoPostPromoEffectLSVW2'),
                                    labelStyle: 'margin-top: 6px !important',
                                    labelSeparator: '',
                                    labelWidth: 200,
                                    value: record.data.PlanPromoPostPromoEffectLSVW2
                                };

                                widget.down('fieldset').add(planPostPromoEffectW1, planPostPromoEffectW2);

                                widget.show();
                            },
                            listeners: {
                                afterrender: function (el) {
                                    var window = Ext.ComponentQuery.query('promoeditorcustom')[0];
                                    var promoController = App.app.getController('tpm.promo.Promo');
                                    var record = promoController.getRecord(window);

                                    el.triggerCell.addCls('form-info-trigger-cell');
                                    el.setValue(record.data.PlanPromoPostPromoEffectLSV || 0);
                                },
                                change: this.calculationChangeListener
                            }
                        }]
                    }, {
                        xtype: 'container',
                        width: '100%',
                        layout: 'hbox',
                        defaults: {
                            minValue: 0,
                            labelAlign: 'top',
                            padding: '0 10 0 0',
                            flex: 4
                        },
                        items: [{
                            xtype: 'label',
                            text: l10n.ns('tpm', 'Promo').value('Fact'),
                            margin: '8 0 0 0',
                            width: 40,
                            maxWidth: 40,
                            style: {
                                fontWeight: 'bold',
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'ActualPromoUpliftPercent',
                            needReadOnly: true,
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'ActualPromoIncrementalLSV',
                            hideTrigger: true,
                            needReadOnly: true,
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'numberfield',
                            name: 'ActualPromoLSV',
                            hideTrigger: true,
                            needReadOnly: true,
                            listeners: {
                                change: this.calculationChangeListener
                            }
                        }, {
                            xtype: 'trigger',
                            name: 'ActualPromoPostPromoEffectLSV',
                            needReadOnly: false,
                            editable: false,
                            width: 120,
                            minWidth: 120,
                            setReadOnly: function () { return false; },
                            trigger1Cls: 'form-info-trigger',
                            onTrigger1Click: function () {
                                var window = Ext.ComponentQuery.query('promoeditorcustom')[0];
                                var promoController = App.app.getController('tpm.promo.Promo');
                                var record = promoController.getRecord(window);
                                var widget = Ext.widget('postpromoeffectdetails');

                                var factPostPromoEffectW1 = {
                                    xtype: 'numberfield',
                                    name: 'ActualPromoPostPromoEffectLSVW1',
                                    readOnly: true,
                                    cls: 'readOnlyField',
                                    fieldLabel: l10n.ns('tpm', 'PostPromoEffectDetails').value('ActualPromoPostPromoEffectLSVW1'),
                                    labelSeparator: '',
                                    labelStyle: 'margin-top: 6px !important',
                                    labelWidth: 200,
                                    value: record.data.ActualPromoPostPromoEffectLSVW1
                                };

                                var factPostPromoEffectW2 = {
                                    xtype: 'numberfield',
                                    name: 'ActualPromoPostPromoEffectLSVW2',
                                    readOnly: true,
                                    cls: 'readOnlyField',
                                    fieldLabel: l10n.ns('tpm', 'PostPromoEffectDetails').value('ActualPromoPostPromoEffectLSVW2'),
                                    labelSeparator: '',
                                    labelStyle: 'margin-top: 6px !important',
                                    labelWidth: 200,
                                    value: record.data.ActualPromoPostPromoEffectLSVW2
                                };

                                widget.down('fieldset').add(factPostPromoEffectW1, factPostPromoEffectW2);
                                widget.show();
                            },
                            listeners: {
                                afterrender: function (el) {
                                    var window = Ext.ComponentQuery.query('promoeditorcustom')[0];
                                    var promoController = App.app.getController('tpm.promo.Promo');
                                    var record = promoController.getRecord(window);

                                    el.triggerCell.addCls('form-info-trigger-cell');
                                    el.setValue(record.data.ActualPromoPostPromoEffectLSV || 0);
                                },
                                change: this.calculationChangeListener
                            }
                        }]
                    }]
                }]
            }]
        }, {
            xtype: 'panel',
            name: 'calculations_step3',
            itemId: 'calculations_step3',
            bodyStyle: { "background-color": "#99a9b1" },
            cls: 'promoform-item-wrap',
            needToSetHeight: true,
            header: {
                title: l10n.ns('tpm', 'promoStap').value('calculationStep3'),
                cls: 'promo-header-item',
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            items: [{
                xtype: 'container',
                width: '100%',
                cls: 'custom-promo-panel-container',
                layout: 'fit',
                items: [{
                    xtype: 'custompromopanel',
                    padding: '30 0 32 0',
                    layout: {
                        type: 'hbox',
                        align: 'middle',
                        pack: 'center'
                    },
                    items: [{
                        xtype: 'container',
                        width: '100%',
                        name: 'financialIndicator',
                        margin: '0 10 0 20',
                        layout: 'vbox',
                        buttonId: 'btn_calculations_step3',
                        defaults: {
                            minValue: 0,
                            padding: '0 0 10 0',
                        },
                        items: [{
                            xtype: 'container',
                            layout: 'hbox',
                            width: '100%',
                            defaults: {
                                minValue: 0,
                                labelAlign: 'top',
                                padding: '0 10 0 0',
                                flex: 4
                            },
                            items: [{
                                xtype: 'label',
                                text: l10n.ns('tpm', 'Promo').value('Plan'),
                                width: 40,
                                maxWidth: 40,
                                margin: '29 0 0 0',
                                style: {
                                    fontWeight: 'bold',
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'PlanPromoROIPercent',
                                needReadOnly: true,
                                fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoROIPercent'),
                                listeners: {
                                    change: this.calculationChangeListener
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'PlanPromoIncrementalNSV',
                                hideTrigger: true,
                                needReadOnly: true,
                                fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalNSV'),
                                listeners: {
                                    change: this.calculationChangeListener
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'PlanPromoNetIncrementalNSV',
                                hideTrigger: true,
                                needReadOnly: true,
                                fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalNSV'),
                                listeners: {
                                    change: this.calculationChangeListener
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'PlanPromoIncrementalMAC',
                                hideTrigger: true,
                                needReadOnly: true,
                                fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalMAC'),
                                listeners: {
                                    change: this.calculationChangeListener
                                }
                            }]
                        }, {
                            xtype: 'container',
                            width: '100%',
                            layout: 'hbox',
                            defaults: {
                                minValue: 0,
                                labelAlign: 'top',
                                padding: '0 10 0 0',
                                flex: 4
                            },
                            items: [{
                                xtype: 'label',
                                text: l10n.ns('tpm', 'Promo').value('Fact'),
                                width: 40,
                                maxWidth: 40,
                                margin: '8 0 0 0',
                                style: {
                                    fontWeight: 'bold',
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'ActualPromoROIPercent',
                                needReadOnly: true,
                                listeners: {
                                    change: this.calculationChangeListener
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'ActualPromoIncrementalNSV',
                                hideTrigger: true,
                                needReadOnly: true,
                                listeners: {
                                    change: this.calculationChangeListener
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'ActualPromoNetIncrementalNSV',
                                hideTrigger: true,
                                needReadOnly: true,
                                listeners: {
                                    change: this.calculationChangeListener
                                }
                            }, {
                                xtype: 'numberfield',
                                name: 'ActualPromoIncrementalMAC',
                                hideTrigger: true,
                                needReadOnly: true,
                                listeners: {
                                    change: this.calculationChangeListener
                                },
                            }]
                        }]
                    }]
                }]
            }]
        }]
    }]
})