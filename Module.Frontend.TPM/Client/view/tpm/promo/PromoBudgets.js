Ext.define('App.view.tpm.promo.PromoBudgets', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promobudgets',

    items: [{
        xtype: 'container',
        buttonId: 'btn_promoBudgets_step1',
        cls: 'promo-editor-custom-scroll-items',
        items: [{
            xtype: 'panel',
            name: 'promoBudgets_step1',
            itemId: 'promoBudgets_step1',
            bodyStyle: { "background-color": "#eceff1" },
            cls: 'promoform-item-wrap',
            header: {
                title: l10n.ns('tpm', 'promoStap').value('promoBudgetsStep1'),
                cls: 'promo-header-item',
            },
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
                    name: 'planFieldSetBudgets',
                    title: 'Plan',
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                        pack: 'center',
                    },
                    padding: '0 10 10 10',
                    defaults: {
                        margin: '5 0 0 0',
                    },
                    items: [{
                        xtype: 'numberfield',
                        name: 'PlanPromoCost',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCost'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null;

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'PlanPromoTIMarketing',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIMarketing'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'PlanPromoCostProduction',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProduction'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'PlanPromoTIShopper',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIShopper'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'PlanPromoBranding',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        readOnlyCls: 'readOnlyField',
                        isChecked: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBranding'),
                        value: 0,
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        availableRoleStatusActions: {
                            SupportAdministrator: App.global.Statuses.AllStatuses,
                            Administrator: App.global.Statuses.AllStatusesWithoutDraft,
                            FunctionalExpert: App.global.Statuses.AllStatusesWithoutDraft,
                            CMManager: App.global.Statuses.AllStatusesWithoutDraft,
                            CustomerMarketing: App.global.Statuses.AllStatusesWithoutDraft,
                            KeyAccountManager: App.global.Statuses.AllStatusesWithoutDraft
                        },
                        mouseWheelEnabled: false,
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'PlanPromoBTL',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        readOnlyCls: 'readOnlyField',
                        isChecked: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBTL'),
                        value: 0,
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        availableRoleStatusActions: {
                            SupportAdministrator: App.global.Statuses.AllStatuses,
                            Administrator: App.global.Statuses.AllStatusesWithoutDraft,
                            FunctionalExpert: App.global.Statuses.AllStatusesWithoutDraft,
                            CMManager: App.global.Statuses.AllStatusesWithoutDraft,
                            CustomerMarketing: App.global.Statuses.AllStatusesWithoutDraft
                        },
                        mouseWheelEnabled: false,
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }]
                }]
            }, {
                xtype: 'splitter',
                itemId: 'splitter_budgets1',
                cls: 'custom-promo-panel-splitter',
                collapseOnDblClick: false,
                listeners: {
                    dblclick: {
                        fn: function (event, el) {
                            var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_budgets1]')[0];
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
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                    pack: 'center'
                },
                items: [{
                    xtype: 'fieldset',
                    title: 'Actuals',
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                        pack: 'center',
                    },
                    padding: '0 10 10 10',
                    defaults: {
                        margin: '5 0 0 0',
                    },
                    items: [{
                        xtype: 'numberfield',
                        name: 'ActualPromoCost',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCost'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            //change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },       
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoTIMarketing',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIMarketing'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            //change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },          
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoCostProduction',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCostProduction'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            //change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoTIShopper',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        labelWidth: 190,
                        value: 0,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIShopper'),
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            //change: this.budgetsChangeListener,
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },    
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoBranding',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        isChecked: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBranding'),
                        value: 0,
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        availableRoleStatusActions: {
                            SupportAdministrator: App.global.Statuses.AllStatuses,
                            Administrator: App.global.Statuses.AllStatusesWithoutDraft,
                            FunctionalExpert: App.global.Statuses.AllStatusesWithoutDraft,
                            CMManager: App.global.Statuses.AllStatusesWithoutDraft,
                            CustomerMarketing: App.global.Statuses.AllStatusesWithoutDraft,
                            KeyAccountManager: App.global.Statuses.AllStatusesWithoutDraft,
                        },
                        mouseWheelEnabled: false,
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                        valueToRaw: function (value) {
                            var valueToDisplay = null; 

                            if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    this.originValue = value;
                                    valueToDisplay = value / 1000000.0;
                                }
                            }

                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
                            return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            //change: this.budgetsChangeListener
                            focus: function (field) {
                                this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoBTL',
                        readOnlyCls: 'readOnlyField',
                        hideTrigger: true,
                        readOnly: true,
                        readOnlyCls: 'readOnlyField',
                        isChecked: true,
                        labelWidth: 190,
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBTL'),
                        value: 0,
                        cls: 'borderedField-with-lable',
                        labelCls: 'borderedField-label',
                        availableRoleStatusActions: {
                            SupportAdministrator: App.global.Statuses.AllStatuses,
                            Administrator: App.global.Statuses.AllStatusesWithoutDraft,
                            FunctionalExpert: App.global.Statuses.AllStatusesWithoutDraft,
                            CMManager: App.global.Statuses.AllStatusesWithoutDraft,
                            CustomerMarketing: App.global.Statuses.AllStatusesWithoutDraft
                        },
                        mouseWheelEnabled: false,
                        blockMillion: false, // если true - то преобразовывать в миллионы
                        originValue: null, // настоящее значение
                            valueToRaw: function (value) {
                                
                                //если после двух знаков после запятой есть значения они не отобразятся, но они есть
                            var valueToDisplay = null; 
                             if (value !== null && value !== undefined) {
                                if (this.blockMillion) {
                                    valueToDisplay = value;
                                }
                                else {
                                    if (this.readOnly) {
                                        if (this.originValue == null || this.originValue == 0)
                                            this.originValue = value; 
                                            value = this.originValue;
                                            valueToDisplay = value / 1000000.0;
                                         
                                    } else {
                                        this.originValue = value;
                                        valueToDisplay = value / 1000000.0;
                                    }
                                   
                                }
                            }

                             
                            return Ext.util.Format.number(valueToDisplay, '0.00');
                        },
                        rawToValue: function () {
                            var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."));

                             return isNaN(parsedValue) ? null : parsedValue;
                        },
                        listeners: {
                            //change: this.budgetsChangeListener
                            focus: function (field) {
                                
                                if(!this.readOnly)
                                     this.blockMillion = true;
                                field.setValue(this.originValue);
                                this.blockMillion = false;
                            },
                        }
                    }]
                }]
            }]
        }, {
            xtype: 'panel',
            name: 'promoBudgets_step2',
            itemId: 'promoBudgets_step2',
            bodyStyle: { "background-color": "#eceff1" },
            cls: 'promoform-item-wrap',
            header: {
                title: l10n.ns('tpm', 'promoStap').value('promoBudgetsStep2'),
                cls: 'promo-header-item',
            },
            layout: {
                type: 'hbox',
                align: 'stretchmax'
            },
            items: [{
                xtype: 'promobudgetdetails',
                minWidth: 245,
                flex: 1,
                widget: 'pspshortplancalculation',
                budgetName: 'Marketing',
                budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
            }, {
                xtype: 'splitter',
                itemId: 'splitter_budgets2',
                cls: 'custom-promo-panel-splitter',
                collapseOnDblClick: false,
                listeners: {
                    dblclick: {
                        fn: function (event, el) {
                            var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_budgets2]')[0];
                            cmp.tracker.getPrevCmp().flex = 1;
                            cmp.tracker.getNextCmp().flex = 1;
                            cmp.ownerCt.updateLayout();
                        },
                        element: 'el'
                    }
                }
            }, {
                xtype: 'promobudgetdetails',
                minWidth: 245,
                flex: 1,
                widget: 'pspshortfactcalculation',
                fact: true,
                budgetName: 'Marketing',
                budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
            }]
        }, {
            xtype: 'panel',
            name: 'promoBudgets_step3',
            itemId: 'promoBudgets_step3',
            bodyStyle: { "background-color": "#99a9b1" },
            cls: 'promoform-item-wrap',
            needToSetHeight: true,
            header: {
                title: l10n.ns('tpm', 'promoStap').value('promoBudgetsStep3'),
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
                layout: {
                    type: 'hbox',
                    align: 'stretchmax'
                },
                items: [{
                    xtype: 'promobudgetdetails',
                    minWidth: 245,
                    flex: 1,
                    widget: 'pspshortplancostprod',
                    costProd: true,
                    budgetName: 'Marketing',
                    budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
                }, {
                    xtype: 'splitter',
                    itemId: 'splitter_budgets3',
                    cls: 'custom-promo-panel-splitter',
                    collapseOnDblClick: false,
                    listeners: {
                        dblclick: {
                            fn: function (event, el) {
                                var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_budgets3]')[0];
                                cmp.tracker.getPrevCmp().flex = 1;
                                cmp.tracker.getNextCmp().flex = 1;
                                cmp.ownerCt.updateLayout();
                            },
                            element: 'el'
                        }
                    }
                }, {
                    xtype: 'promobudgetdetails',
                    minWidth: 245,
                    flex: 1,
                    widget: 'pspshortfactcostprod',
                    costProd: true,
                    fact: true,
                    budgetName: 'Marketing',
                    budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
                }]
            }]
        }]
    }]
})