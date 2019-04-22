Ext.define('App.view.tpm.promo.PromoActivityDetailsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promoactivitydetailswindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoActivityDetailsWindow'),
    cls: 'promo-activity-details-window',

    width: "70%",
    minWidth: 800,
    minHeight: 431,
    maxHeight: 431,

    items: [{
        xtype: 'container',
        maxHeight: '100%',
        cls: 'custom-promo-panel-container',
        layout: 'fit',
        items: [{
            xtype: 'custompromopanel',
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
                    title: l10n.ns('tpm', 'Promo').value('InstoreAssumption'),
                    layout: 'hbox',
                    padding: '0 10 10 10',
                    defaults: {
                        cls: 'borderedField-with-lable promo-activity-details-window-field',
                        labelCls: 'borderedField-label',
                        labelWidth: 140,
                        labelSeparator: '',
                        readOnly: true,
                        flex: 1,
                        margin: '0 10 0 0',
                        //editable: false,
                    },
                    items: [{
                        xtype: 'searchcombobox',
                        flex: 1,
                        layout: 'anchor',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('Mechanic'),
                        labelAlign: 'top',
                        name: 'ActualInstoreMechanicId',
                        selectorWidget: 'mechanic',
                        valueField: 'Id',
                        displayField: 'Name',
                        entityType: 'Mechanic',
                        needReadOnly: true,
                        allowBlank: true,
                        store: {
                            type: 'promoformmechanicstore'
                        },
                        onTrigger3Click: function () {
                            var promoController = App.app.getController('tpm.promo.Promo'),
                                promoMechanic = Ext.ComponentQuery.query('#promoActivity_step1')[0],
                                mechanicFields = promoController.getMechanicFields(promoMechanic);

                            promoController.resetFields([
                                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicId,
                                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId,
                                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount
                            ]);

                            promoController.disableFields([
                                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId,
                                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount
                            ]);
                        },


                        mapping: [{
                            from: 'Name',
                            to: 'ActualInstoreMechanicName'
                        }]
                    }, {
                        xtype: 'searchcombobox',
                        flex: 1,
                        layout: 'anchor',
                        fieldLabel: 'Type',
                        labelAlign: 'top',
                        name: 'ActualInstoreMechanicTypeId',
                        selectorWidget: 'mechanictype',
                        valueField: 'Id',
                        displayField: 'Name',
                        entityType: 'MechanicType',
                        needUpdateMappings: true,
                        needReadOnly: true,
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        allowDecimals: false,
                        allowExponential: false,
                        store: {
                            type: 'promoformmechanictypestore'
                        },
                        onTrigger3Click: function () {
                            var promoController = App.app.getController('tpm.promo.Promo'),
                                promoMechanic = Ext.ComponentQuery.query('#promoActivity_step1')[0],
                                mechanicFields = promoController.getMechanicFields(promoMechanic);

                            promoController.resetFields([
                                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId,
                                mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicDiscount
                            ]);
                        },
                        mapping: [{
                            from: 'Name',
                            to: 'ActualInstoreMechanicTypeName'
                        }, {
                            from: 'Discount',
                            to: 'ActualInstoreMechanicDiscount'
                        }]
                    }, {
                        xtype: 'numberfield',
                        margin: 0,
                        layout: 'anchor',
                        name: 'ActualInstoreMechanicDiscount',
                        minValue: 1,
                        maxValue: 100,
                        fieldLabel: 'Discount',
                        labelAlign: 'top',
                        needReadOnly: true,
                        isChecked: true,
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        allowDecimals: false,
                        allowExponential: false,
                    }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoActivityDetailsWindow').value('PlanActivity'),
                    padding: '0 10',
                    layout: 'vbox',
                    items: [{
                        xtype: 'numberfield',
                        name: 'PlanPromoUpliftPercent',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'),
                        readOnly: true,
                        cls: 'promo-activity-details-window-field',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 190,
                        labelSeparator: '',
                        margin: '0 0 5 0',
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'PlanPromoBaselineLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBaselineLSV'),
                        dataIndexes: ['PlanProductBaselineLSV'],
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'PlanPromoIncrementalLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'),
                        dataIndexes: ['PlanProductIncrementalQty'],
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'PlanPromoLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoLSV'),
                        dataIndexes: ['PlanProductLSV'],
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'PlanPostPromoEffect',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPostPromoEffect'),
                        dataIndexes: ['PlanProductPostPromoEffectQty'],
                        margin: '0 0 10 0',
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }]
                }]
            }, {
                xtype: 'container',
                flex: 1,
                items: [{
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoActivityDetailsWindow').value('InvoiceAndPrices'),
                    layout: 'hbox',
                    padding: '0 10 10 10',
                    defaults: {
                        cls: 'borderedField-with-lable promo-activity-details-window-field',
                        labelCls: 'borderedField-label',
                        labelSeparator: '',
                        readOnly: true,
                        flex: 1,
                        margin: '0 10 0 0'
                    },
                    items: [{
                        xtype: 'numberfield',
                        layout: 'anchor',
                        name: 'ActualInStoreShelfPrice',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreShelfPrice'),
                        labelAlign: 'top',
                    }, {
                        xtype: 'textfield',
                        name: 'InvoiceNumber',
                        layout: 'anchor',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceNumber'),
                        labelAlign: 'top',
                        margin: 0
                    }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoActivityDetailsWindow').value('ActualActivity'),
                    padding: '0 10',
                    layout: 'vbox',
                    items: [{
                        xtype: 'numberfield',
                        name: 'ActualPromoUpliftPercent',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoUpliftPercent'),
                        readOnly: true,
                        cls: 'promo-activity-details-window-field',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 190,
                        labelSeparator: '',
                        margin: '0 0 5 0',
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'ActualPromoBaselineLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBaselineLSV'),
                        dataIndexes: ['PlanProductBaselineLSV'],
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'ActualPromoIncrementalLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalLSV'),
                        dataIndexes: ['PlanProductIncrementalQty'],
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'ActualPromoLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoLSV'),
                        dataIndexes: ['ActualProductLSV'],
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'FactPostPromoEffect',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('FactPostPromoEffect'),
                        dataIndexes: ['ActualProductPostPromoEffectQty'],
                        margin: '0 0 10 0',
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
                            afterrender: function (el) {
                                el.triggerCell.addCls('form-info-trigger-cell')
                            },
                            focus: function (field) {
                                this.blockMillion = true;
                            },
                            blur: function (field) {
                                this.blockMillion = false;
                            },
                        }
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