Ext.define('App.view.tpm.promo.PromoBudgetsDetailsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promobudgetsdetailswindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoBudgetsDetailsWindow'),
    cls: 'promo-budgets-details-window',

    width: "70%",
    height: "80%",
    minWidth: 800,
    minHeight: 500,

    items: [{
        xtype: 'container',
        maxHeight: '100%',
        cls: 'custom-promo-panel-container',
        layout: 'fit',
        items: [{
            xtype: 'custompromopanel',
            // Место  для скролла
            padding: '5 0 5 10',
            autoScroll: true,
            overflowY: 'scroll',
            cls: 'custom-promo-panel scrollpanel',
            layout: {
                type: 'hbox',
                align: 'stretchmax',
            },
            items: [{
                xtype: 'container',
                margin: '0 10 0 0',
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoBudgetsDetailsWindow').value('PlanTotalCostBudgets'),
                    layout: 'vbox',
                    padding: '5 10 10 10',
                    defaults: {
                        cls: 'borderedField-with-lable promo-budgets-details-window-field',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                        //editable: false,
                    },
                    items: [{
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCost'),
                        name: 'PlanPromoCost',
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIMarketing'),
                        name: 'PlanPromoTIMarketing',
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProduction'),
                        name: 'PlanPromoCostProduction',
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIShopper'),
                        name: 'PlanPromoTIShopper',
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBranding'),
                        name: 'PlanPromoBranding',
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBTL'),
                        name: 'PlanPromoBTL',
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoBudgetsDetailsWindow').value('PlanMarketingTi'),
                    padding: '5 10 10 10',
                    defaults: {
                        cls: 'promo-budgets-details-window-trigger',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                    },
                    items: [{
                        xtype: 'promobudgetdetails',
                        hidden: true,
                        widget: 'pspshortplancalculation',
                        budgetName: 'Marketing',
                        budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
                        needCustomPromoPanel: false,
                        additionalClsForTrigger: 'promo-budgets-details-window-trigger',
                    }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoBudgetsDetailsWindow').value('PlanCostProduction'),
                    padding: '5 10 10 10',
                    defaults: {
                        cls: 'promo-budgets-details-window-trigger',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                    },
                    items: [{
                        xtype: 'promobudgetdetails',
                        widget: 'pspshortplancostprod',
                        costProd: true,
                        hidden: true,
                        budgetName: 'Marketing',
                        budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
                        needCustomPromoPanel: false,
                        additionalClsForTrigger: 'promo-budgets-details-window-trigger',                        
                    }]
                }]
            }, {
                xtype: 'container',
                title: l10n.ns('tpm', 'PromoBudgetsDetailsWindow').value('ActualTotalCostBudgets'),
                flex: 1,
                items: [{
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoBudgetsDetailsWindow').value('ActualTotalCostBudgets'),
                    padding: '5 10 10 10',
                    margin: '0 18 10 0',
                    layout: 'vbox',
                    defaults: {
                        cls: 'borderedField-with-lable promo-budgets-details-window-field',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                        //editable: false,
                    },
                    items: [{
                        xtype: 'numberfield',
                        name: 'ActualPromoCost',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCost'),
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoTIMarketing',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIMarketing'),
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoCostProduction',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCostProduction'),
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoTIShopper',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIShopper'),
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoBranding',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBranding'),
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'numberfield',
                        name: 'ActualPromoBTL',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBTL'),
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
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoBudgetsDetailsWindow').value('ActualMarketingTi'),
                    padding: '5 10 10 10',
                    margin: '0 18 10 0',
                    defaults: {
                        cls: 'promo-budgets-details-window-trigger',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                    },
                    items: [{
                        xtype: 'promobudgetdetails',
                        widget: 'pspshortfactcalculation',
                        hidden: true,
                        fact: true,
                        budgetName: 'Marketing',
                        budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
                        needCustomPromoPanel: false,
                        additionalClsForTrigger: 'promo-budgets-details-window-trigger',                        
                    }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoBudgetsDetailsWindow').value('ActualCostProduction'),
                    padding: '5 10 10 10',
                    margin: '0 18 10 0',
                    defaults: {
                        cls: 'promo-budgets-details-window-trigger',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                    },
                    items: [{
                        xtype: 'promobudgetdetails',
                        widget: 'pspshortfactcostprod',
                        costProd: true,
                        fact: true,
                        hidden: true,
                        budgetName: 'Marketing',
                        budgetItemsName: ['X-sites', 'Catalog', 'POSM'],
                        needCustomPromoPanel: false,
                        additionalClsForTrigger: 'promo-budgets-details-window-trigger',                        
                    }]
                }]
            }]
        }]
    }],


    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'close'
    }]
});