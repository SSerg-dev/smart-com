Ext.define('App.view.tpm.promo.PromoActivityDetailsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promoactivitydetailswindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoActivityDetailsWindow'),
    cls: 'promo-activity-details-window',

    width: "70%",
    minWidth: 800,
    minHeight: 595,

    items: [{
        xtype: 'container',
        height: '100%',
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
                    title: l10n.ns('tpm', 'PromoActivityDetailsWindow').value('PlanInstoreAssumption'),
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
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'fieldset',
                        cls: 'specialOne',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
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
                            name: 'PlanInstoreMechanicIdInActivity',
                            selectorWidget: 'mechanic',
                            valueField: 'Id',
                            displayField: 'Name',
                            entityType: 'Mechanic',
                            needReadOnly: true,
                            allowBlank: true,
                            store: {
                                type: 'promoformmechanicstore'
                            },
                        }, {
                            xtype: 'searchcombobox',
                            flex: 1,
                            layout: 'anchor',
                            fieldLabel: 'Type',
                            labelAlign: 'top',
                            name: 'PlanInstoreMechanicTypeIdInActivity',
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
                        }, {
                            xtype: 'numberfield',
                            margin: 0,
                            layout: 'anchor',
                            name: 'PlanInstoreMechanicDiscountInActivity',
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
                            xtype: 'numberfield',
                            layout: 'anchor',
                            name: 'PlanInStoreShelfPrice',
                            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreShelfPrice'),
                            labelAlign: 'left',
                            cls: 'promo-activity-details-window-field',
                            labelCls: 'borderedField-label',
                            width: '100%',
                            labelWidth: 190,
                            labelSeparator: '',
                            margin: '5 0 0 0',
                        }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoActivityDetailsWindow').value('PlanActivity'),
                    padding: '0 10',
                    layout: 'vbox',
                    items: [{
						xtype: 'triggerfielddetails',
						name: 'PlanPromoUpliftPercent',
						fieldLabel: l10n.ns('tpm', 'Promo').value('PromoUpliftPercent'),
						windowType: 'promoproductsview',
						dataIndexes: ['PlanPromoUpliftPercent'],
						rawToValue: function () {
							var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
							return isNaN(parsedValue) ? null : parsedValue;
						},
						listeners: {
							afterrender: function (el) {
								el.triggerCell.addCls('form-info-trigger-cell')
							}
						}
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'PlanPromoBaselineLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PromoBaselineLSV'),
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
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PromoIncrementalLSV'),
                        dataIndexes: ['PlanProductIncrementalLSV'],
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
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PromoLSV'),
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
                        name: 'PlanPromoPostPromoEffectLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PromoPostPromoEffectLSV'),
                        dataIndexes: ['PlanProductPostPromoEffectLSV'],
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
                    title: l10n.ns('tpm', 'PromoActivityDetailsWindow').value('ActualInStore'),
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
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
                        xtype: 'fieldset',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        cls: 'specialOne',
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
                                        mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount
                                    ]);

                                    promoController.disableFields([
                                        mechanicFields.actualInstoreMechanicFields.actualInstoreMechanicTypeId,
                                        mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount
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
                                        mechanicFields.actualInstoreMechanicFields.actualInStoreDiscount
                                    ]);
                                },
                                mapping: [{
                                    from: 'Name',
                                    to: 'ActualInstoreMechanicTypeName'
                                }, {
                                    from: 'Discount',
                                    to: 'ActualInStoreDiscount'
                                }]
                            }, {
                                xtype: 'numberfield',
                                margin: 0,
                                layout: 'anchor',
                                name: 'ActualInStoreDiscount',
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
                            xtype: 'numberfield',
                            layout: 'anchor',
                            name: 'ActualInStoreShelfPrice',
                            dataIndexes: ['ActualInStoreShelfPrice'],
                            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreShelfPrice'),
                            cls: 'promo-activity-details-window-field',
                            labelCls: 'borderedField-label',
                            labelAlign: 'left',
                            width: '100%',
                            labelWidth: 190,
                            labelSeparator: '',
                            margin: '5 0 0 0',
                    }]
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoActivityDetailsWindow').value('ActualActivity'),
                    padding: '0 10',
                    layout: 'vbox',
                    items: [{
						xtype: 'triggerfielddetails',
						name: 'ActualPromoUpliftPercent',
						fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoUpliftPercent'),
						dataIndexes: ['ActualProductUpliftPercent'],
						rawToValue: function () {
							var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
							return isNaN(parsedValue) ? null : parsedValue;
						},
						listeners: {
							afterrender: function (el) {
								el.triggerCell.addCls('form-info-trigger-cell')
							}
						}
                    }, {
                        xtype: 'triggerfielddetails',
                        name: 'ActualPromoBaselineLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBaselineLSV'),
                        dataIndexes: ['ActualProductBaselineLSV'],
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
                        dataIndexes: ['ActualProductIncrementalLSV'],
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
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSV'),
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
                        name: 'ActualPromoLSVByCompensation',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSVByCompensation'),
                        dataIndexes: ['ActualProductLSVByCompensation'],
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
                        name: 'ActualPromoPostPromoEffectLSV',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoPostPromoEffectLSV'),
                        dataIndexes: ['ActualProductPostPromoEffectLSV'],
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
                            name: 'InvoiceTotal',
                            fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceTotal'),
                            dataIndexes: ['InvoiceTotal', 'InvoiceTotalProduct', 'ActualProductPCQty'],
                            labelAlign: 'left',
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
                            xtype: 'textfield',
                            name: 'InvoiceNumber',
                            flex: 1,
                            layout: 'anchor',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceNumber'),
                            labelAlign: 'left',
                            width: '100%',
                            //Для одного уровня с остальными полями
                            labelWidth: 190,
                            margin: '25 0 5 0',
                            needReadOnly: true,
                            isChecked: true,
                            allowBlank: true,
                            allowOnlyWhitespace: true,
                            cls: 'disableBG',
                    }, {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            flex: 1,
                            layout: 'anchor',
                            width: '100%',
                            fieldLabel: l10n.ns('tpm', 'Promo').value('DocumentNumber'),
                            labelAlign: 'left',
                            width: '100%',
                            //Для одного уровня с остальными полями
                            labelWidth: 190,
                            margin: '0 0 5 0',
                            needReadOnly: true,
                            isChecked: true,
                            allowBlank: true,
                            allowOnlyWhitespace: true,
                            cls: 'disableBG',
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