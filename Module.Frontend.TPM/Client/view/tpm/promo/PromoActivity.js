Ext.define('App.view.tpm.promo.PromoActivity', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoactivity',

    initComponent: function () {
        this.callParent(arguments);
    },

    items: [{
        xtype: 'container',
        cls: 'promo-editor-custom-scroll-items',
        // promoActivity_step1
        items: [{
            xtype: 'panel',
            name: 'promoActivity_step1',
            itemId: 'promoActivity_step1',
            buttonId: 'btn_promoActivity_step1',
            bodyStyle: { "background-color": "#eceff1" },
            cls: 'promoform-item-wrap',
            header: {
                title: l10n.ns('tpm', 'promoStap').value('promoActivityStep1'),
                cls: 'promo-header-item'
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
                    title: l10n.ns('tpm', 'PromoActivity').value('InstoreAssumption'),
                    name: 'instoreAssumptionFieldset',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'searchcombobox',
                        flex: 1,
                        layout: 'anchor',
                        padding: '0 5 5 5',
                        fieldLabel: l10n.ns('tpm', 'PromoActivity').value('Mechanic'),
                        labelAlign: 'top',
                        name: 'ActualInstoreMechanicId',
                        selectorWidget: 'mechanic',
                        valueField: 'Id',
                        displayField: 'Name',
                        entityType: 'Mechanic',
                        needReadOnly: true,
                        allowBlank: true,
                        crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
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
                        padding: '0 5 5 5',
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
                        crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
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
                        flex: 1,
                        layout: 'anchor',
                        name: 'ActualInstoreMechanicDiscount',
                        minValue: 1,
                        maxValue: 100,
                        fieldLabel: 'Discount',
                        labelAlign: 'top',
                        padding: '0 5 5 5',
                        needReadOnly: true,
                        isChecked: true,
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        allowDecimals: false,
                        allowExponential: false,
                        crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                        listeners: {
                            change: this.invoiceAndPricesChangeListener
                        }
                    }]
                }]
            }, {
                xtype: 'splitter',
                itemId: 'splitter_activity1',
                cls: 'custom-promo-panel-splitter',
                collapseOnDblClick: false,
                listeners: {
                    dblclick: {
                        fn: function (event, el) {
                            var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_activity1]')[0];
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
                    title: l10n.ns('tpm', 'PromoActivity').value('InvoiceAndPrices'),
                    name: 'invoiceAndPricesFieldset',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'numberfield',
                        flex: 1,
                        layout: 'anchor',
                        name: 'ActualInStoreShelfPrice',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreShelfPrice'),
                        labelAlign: 'top',
                        padding: '0 5 5 5',
                        needReadOnly: true,
                        isChecked: true,
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        allowDecimals: true,
                        allowExponential: false,
                        crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                        listeners: {
                            change: this.invoiceAndPricesChangeListener
                        }
                    }, {
                        xtype: 'textfield',
                        name: 'InvoiceNumber',
                        flex: 1,
                        layout: 'anchor',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceNumber'),
                        labelAlign: 'top',
                        padding: '0 5 5 5',
                        needReadOnly: true,
                        isChecked: true,
                        allowBlank: false,
                        allowOnlyWhitespace: false,
                        crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                        listeners: {
                            change: this.invoiceAndPricesChangeListener
                        }
                    }]
                }]
            }]
        },
        // promoActivity_step2
        {
            xtype: 'panel',
            name: 'promoActivity_step2',
            itemId: 'promoActivity_step2',
            bodyStyle: { "background-color": "#99a9b1" },
            cls: 'promoform-item-wrap',
            needToSetHeight: true,
            header: {
                title: l10n.ns('tpm', 'promoStap').value('promoActivityStep2'),
                cls: 'promo-header-item',
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'container',
                style: { "background-color": "#eceff1" },
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
                        title: l10n.ns('tpm', 'PromoActivity').value('Plan'),
                        layout: {
                            type: 'vbox',
                            align: 'stretch',
                            pack: 'center'
                        },
                        padding: '0 10 10 10',
                        defaults: {
                            margin: '5 0 0 0',
                        },
                        items: [{
                            xtype: 'container',
                            height: '100%',
                            layout: {
                                type: 'hbox',
                                align: 'top',
                                pack: 'center'
                            },
                            items: [{
                                xtype: 'numberfield',
                                name: 'PlanPromoUpliftPercent',
                                hideTrigger: true,
                                readOnly: true,
                                needReadOnly: true,
                                labelWidth: 190,
                                fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'),
                                cls: 'borderedField-with-lable',
                                labelCls: 'borderedField-label',
                                flex: 1,
                                crudAccess: ['Administrator', 'FunctionalExpert', 'DemandPlanning'],
                            }, {
                                xtype: 'checkbox',
                                labelSeparator: '',
                                readOnly: true,
                                needReadOnly: true,
                                itemId: 'PromoUpliftLockedUpdateCheckbox',
                                name: 'NeedRecountUplift',
                                boxLabel: 'Locked Update',
                                labelAlign: 'right',
                                style: 'margin-left: 10px',
                                crudAccess: ['Administrator', 'FunctionalExpert', 'DemandPlanning'],
                            }]
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
                    }, {
                        xtype: 'tbspacer',
                        flex: 1
                    }]
                }, {
                    xtype: 'splitter',
                    itemId: 'splitter_activity2',
                    cls: 'custom-promo-panel-splitter',
                    collapseOnDblClick: false,
                    listeners: {
                        dblclick: {
                            fn: function (event, el) {
                                var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_activity2]')[0];
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
                        title: l10n.ns('tpm', 'PromoActivity').value('Actuals'),
                        name: 'activity',
                        layout: {
                            type: 'vbox',
                            align: 'stretch',
                            pack: 'center'
                        },
                        padding: '0 10 10 10',
                        defaults: {
                            margin: '5 0 0 0'
                        },
                        items: [{
                            xtype: 'numberfield',
                            name: 'ActualPromoUpliftPercent',
                            editable: false,
                            hideTrigger: true,
                            readOnly: true,
                            needReadOnly: true,
                            isChecked: true,
                            setReadOnly: function () { return false },
                            labelWidth: 190,
                            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoUpliftPercent'),
                            cls: 'borderedField-with-lable',
                            labelCls: 'borderedField-label',
                            listeners: {
                                change: this.activityChangeListener,
                            }
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
                                change: this.activityChangeListener,
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
                                change: this.activityChangeListener,
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
                                change: this.activityChangeListener,
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
                                change: this.activityChangeListener,
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
                    }, {
                        xtype: 'container',
                        height: 33,
                        flex: 1,
                        layout: {
                            type: 'hbox',
                            align: 'top',
                            pack: 'center'
                        },
                        items: [{
                            xtype: 'tbspacer',
                            flex: 1
                        }, {
                            xtype: 'button',
                            cls: 'promoStep-dockedBtn',
                            itemId: 'activityUploadPromoProducts',
                            text: l10n.ns('tpm', 'PromoActivity').value('UpdateActuals'),
                            tooltip: l10n.ns('tpm', 'PromoActivity').value('UpdateActuals'),
                            glyph: 0xf552
                        }]
                    }]
                }] 
            }]
        }]
    }]
})