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
                    title: l10n.ns('tpm', 'PromoActivity').value('PlanInstoreAssumption'),
                    name: 'planInstoreAssumptionFieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [{
                            xtype: 'textfield',
                            name: 'PlanInstoreMechanicIdInActivity',
                            flex: 1,
                            layout: 'anchor',
                            padding: '0 5 5 5',
                            fieldLabel: l10n.ns('tpm', 'PromoActivity').value('Mechanic'),
                            labelAlign: 'top',
                            id: 'PlanInstoreMechanicIdInActivity',
                            readOnly: true,
                            readOnlyCls: 'readOnlyField',
                            needReadOnly: true,
                            setReadOnly: function () { return false },
                            allowBlank: true,
                            customTip: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicNameTip'),
                            crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
                        }, {
                            xtype: 'textfield',
                            name: 'PlanInstoreMechanicTypeIdInActivity',
                            flex: 1,
                            layout: 'anchor',
                            padding: '0 5 5 5',
                            fieldLabel: 'Type',
                            labelAlign: 'top',
                            id: 'PlanInstoreMechanicTypeIdInActivity',
                            entityType: 'MechanicType',
                            readOnly: true,
                            readOnlyCls: 'readOnlyField',
                            needReadOnly: true,
                            setReadOnly: function () { return false },
                            crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
                        }, {
                            xtype: 'numberfield',
                            name: 'PlanInstoreMechanicDiscountInActivity',
                            flex: 1,
                            layout: 'anchor',
                            id: 'PlanInstoreMechanicDiscountInActivity',
                            minValue: 1,
                            maxValue: 100,
                            fieldLabel: 'Discount',
                            labelAlign: 'top',
                            padding: '0 5 5 5',
                            readOnly: true,
                            readOnlyCls: 'readOnlyField',
                            needReadOnly: true,
                            setReadOnly: function () { return false },
                            crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
                        }]
                    },{
                        xtype: 'numberfield',
                        flex: 1,
                        layout: 'anchor',
                        name: 'PlanInStoreShelfPrice',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInStoreShelfPrice'),
                        labelAlign: 'left',
                        labelWidth: 120,
                        padding: '0 5 5 5',
                        readOnly: true,
                        readOnlyCls: 'readOnlyField',
                        needReadOnly: true,
                        isChecked: true,
                        allowBlank: true,
                        allowOnlyWhitespace: true,
                        allowDecimals: true,
                        allowExponential: false,
                        crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
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
                        title: l10n.ns('tpm', 'PromoActivity').value('ActualInStore'),
                        name: 'actualInStoreFieldset',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        items: [{
                            xtype: 'container',
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
                                readOnlyCls: 'readOnlyField',
                                needReadOnly: true,
                                allowBlank: true,
                                crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                customTip: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicNameTip'),
                                //store: {
                                //    type: 'promoformmechanicstore'
                                //},
                                store: {
                                    type: 'simplestore',
                                    autoLoad: false,
                                    model: 'App.model.tpm.mechanic.Mechanic',
                                    extendedFilter: {
                                        xclass: 'App.ExtFilterContext',
                                        supportedModels: [{
                                            xclass: 'App.ExtSelectionFilterModel',
                                            model: 'App.model.tpm.mechanic.Mechanic',
                                            modelId: 'efselectionmodel'
                                        }]
                                    }
                                },
                                onFocus: function (field) {
                                    var promoController = App.app.getController('tpm.promo.Promo')
                                    var status = promoController.getPromoType();
                                    if (status != false) {
                                        var filter = this.getStore().fixedFilters || {};
                                        filter['PromoStatusNameFilter'] = {
                                            property: 'PromoTypes.Name',
                                            operation: 'Equals',
                                            value: status
                                        };
                                        this.getStore().fixedFilters = filter;
                                    } else {
                                        App.Notify.pushError(l10n.ns('tpm', 'Promo').value('MechanicGetError'));
                                    }
                                },
                                onTrigger1Click: function () {
                                    var promoController = App.app.getController('tpm.promo.Promo')
                                    var status = promoController.getPromoType();

                                    if (status != false) {
                                        var filter = this.getStore().fixedFilters || {};
                                        filter['PromoStatusNameFilter'] = {
                                            property: 'PromoTypes.Name',
                                            operation: 'Equals',
                                            value: status
                                        };
                                        this.getStore().fixedFilters = filter;
                                        this.self.superclass.onTriggerClick.call(this);
                                    } else {
                                        App.Notify.pushError(l10n.ns('tpm', 'Promo').value('MechanicGetError'));
                                    }
                                },
                                onTrigger2Click: function () {

                                    var promoController = App.app.getController('tpm.promo.Promo')
                                    var status = promoController.getPromoType();
                                    if (status != false) {
                                        var window = this.createWindow();

                                        if (window) {
                                            var filter = this.getStore().fixedFilters || {};
                                            filter['PromoStatusNameFilter'] = {
                                                property: 'PromoTypes.Name',
                                                operation: 'Equals',
                                                value: status
                                            };
                                            this.getStore().fixedFilters = filter;
                                            //this.getStore().setFixedFilter('PromoStatusNameFilter', {
                                            //    property: 'PromoType.Name',
                                            //    operation: 'Equals',
                                            //    value: status
                                            //}); 
                                            window.show();

                                            this.getStore().load();
                                        }
                                    } else {
                                        App.Notify.pushError(l10n.ns('tpm', 'Promo').value('MechanicGetError'));
                                    }
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
                                padding: '0 5 5 5',
                                fieldLabel: 'Type',
                                labelAlign: 'top',
                                name: 'ActualInstoreMechanicTypeId',
                                selectorWidget: 'mechanictype',
                                valueField: 'Id',
                                displayField: 'Name',
                                entityType: 'MechanicType',
                                needUpdateMappings: true,
                                readOnlyCls: 'readOnlyField',
                                needReadOnly: true,
                                allowBlank: false,
                                allowOnlyWhitespace: false,
                                allowDecimals: false,
                                allowExponential: false,
                                disabled: true,
                                crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                customTip: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicTypeNameTip'),
                                //store: {
                                //    type: 'promoformmechanictypestore'
                                //},
                                store: {
                                    type: 'simplestore',
                                    autoLoad: false,
                                    alias: 'store.promoformmechanictypestore',
                                    model: 'App.model.tpm.mechanictype.MechanicType',
                                    extendedFilter: {
                                        xclass: 'App.ExtFilterContext',
                                        supportedModels: [{
                                            xclass: 'App.ExtSelectionFilterModel',
                                            model: 'App.model.tpm.mechanictype.MechanicType',
                                            modelId: 'efselectionmodel'
                                        }]
                                    }
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
                                flex: 1,
                                layout: 'anchor',
                                name: 'ActualInStoreDiscount',
                                minValue: 1,
                                maxValue: 100,
                                fieldLabel: 'Discount',
                                labelAlign: 'top',
                                padding: '0 5 5 5',
                                readOnlyCls: 'readOnlyField',
                                needReadOnly: true,
                                isChecked: true,
                                allowBlank: false,
                                allowOnlyWhitespace: false,
                                allowDecimals: false,
                                allowExponential: false,
                                disabled: true,
                                crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                customTip: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicDiscountTip'),
                            }]
                        }, {
                            xtype: 'numberfield',
                            flex: 1,
                            layout: 'anchor',
                            name: 'ActualInStoreShelfPrice',
                            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualInStoreShelfPrice'),
                            labelAlign: 'left',
                            labelWidth: 120,
                            padding: '0 5 5 5',
                            readOnlyCls: 'readOnlyField',
                            needReadOnly: true,
                            isChecked: true,
                            allowBlank: true,
                            allowOnlyWhitespace: true,
                            allowDecimals: true,
                            allowExponential: false,
                            crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
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
                            itemId: 'ContainerPlanPromoUplift',
                            height: '100%',
                            cls: 'editable-trigger-field',
                            layout: {
                                type: 'hbox',
                                align: 'top',
                                pack: 'center'
                            },
                            setReadable: function (read) {
                                if (!read && !this.down('checkbox[itemId=PromoUpliftLockedUpdateCheckbox]').isDisabled()) {
                                    this.down('triggerfielddetails[name=PlanPromoUpliftPercent]').isReadable = false;
                                } else {
                                    this.down('triggerfielddetails[name=PlanPromoUpliftPercent]').isReadable = true;
                                }
                            },
                            items: [{
                                xtype: 'triggerfielddetails',
								name: 'PlanPromoUpliftPercent',
                                windowType: 'promoproductsview',
                                labelWidth: 190,
                                fieldLabel: l10n.ns('tpm', 'Promo').value('PromoUpliftPercent'),
                                tooltip: l10n.ns('tpm', 'PromoActivity').value('UpdateActuals'),
                                flex: 1,
                                isReadable: true,
                                readOnly: false,
                                crudAccess: ['Administrator', 'FunctionalExpert', 'DemandPlanning'],
                                listeners: {
                                    afterrender: function (el) {
                                        el.triggerCell.addCls('form-info-trigger-cell');

                                        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                                        if (this.crudAccess.indexOf(currentRole) === -1) {
                                            this.up('container[itemId=ContainerPlanPromoUplift]').setReadable(true);
                                        }
                                    }
                                },
                                validator: function (value) {
                                    if (this.editable) {
                                        //Заменяем запятую на точку для парсера
                                        value = value.split(",").join(".");
                                        var floatValue = parseFloat(value);
                                        if (floatValue != value || floatValue <= 0 || value === "") {
                                            return l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercentError');
                                        } else {
                                            return true;
                                        }
                                    } else {
                                        return true;
                                    }
                                },
                                changeEditable: function (setToEditable) {
                                    if (setToEditable) {
                                        this.setEditable(true);
                                        this.up('container').addCls('editable-trigger-field');
                                    } else {
                                        this.setEditable(false);
                                        this.up('container').removeCls('editable-trigger-field');
                                    }
                                    this.validate();
                                }
                            }, {
                                xtype: 'checkbox',
                                labelSeparator: '',
                                readOnly: true,
                                needReadOnly: true,
                                itemId: 'PromoUpliftLockedUpdateCheckbox',
                                name: 'NeedRecountUplift',
                                labelAlign: 'right',
                                style: 'margin-left: 10px',
                                crudAccess: ['Administrator', 'FunctionalExpert', 'DemandPlanning'],
                                listeners: {
                                    afterRender: function () {
                                        var planUpliftContainer = this.up('container');
                                        var planPromoUpliftNumberField = planUpliftContainer.down('triggerfielddetails[name=PlanPromoUpliftPercent]');
                                        var planPromoUpliftGlyphLock = planUpliftContainer.down('button[itemId=GlyphLock]');
                                        if (this.value) {
                                            planUpliftContainer.setReadable(true);
                                            planPromoUpliftGlyphLock.setGlyph(0xf33e);
                                            planPromoUpliftNumberField.changeEditable(true);
                                        } else {
                                            planUpliftContainer.setReadable(false);
                                            planPromoUpliftGlyphLock.setGlyph(0xf33f);
                                            planPromoUpliftNumberField.changeEditable(false);
                                        }
                                    },
                                    change: function (checkbox, newValue, oldValue) {
                                        var planUpliftContainer = this.up('container');
                                        var planPromoUpliftNumberField = planUpliftContainer.down('triggerfielddetails[name=PlanPromoUpliftPercent]');
                                        var planPromoUpliftGlyphLock = planUpliftContainer.down('button[itemId=GlyphLock]');
                                        if (newValue) {
                                            planUpliftContainer.setReadable(true);
                                            planPromoUpliftGlyphLock.setGlyph(0xf33e);
                                            planPromoUpliftNumberField.changeEditable(true);
                                        } else {
                                            planPromoUpliftGlyphLock.setGlyph(0xf33f);
                                            planUpliftContainer.setReadable(false);
                                            planPromoUpliftNumberField.changeEditable(false);
                                        }
                                    }
                                }
                            }, {
                                xtype: 'button',
                                itemId: 'GlyphLock',
                                cls: 'promo-calculation-activity-uplift-btnglyph',
                                glyph: 0xf33e,
                                width: 30,
                                height: 30,
                            }]
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
							xtype: 'triggerfielddetails',
							name: 'ActualPromoUpliftPercent',
							fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoUpliftPercent'),
							dataIndexes: ['ActualProductUpliftPercent'],
							rawToValue: function () {
								var parsedValue = parseFloat(String(this.originValue).replace(Ext.util.Format.decimalSeparator, "."))
								return isNaN(parsedValue) ? null : parsedValue;
							},
							listeners: {
								change: this.activityChangeListener,
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
                                fieldLabel: l10n.ns('tpm', 'Promo').value('PromoLSV'),
                                dataIndexes: ['ActualProductLSV'],
                                value: '0',
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
                                name: 'ActualPromoLSVByCompensation',
                                fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSVByCompensation'),
                                dataIndexes: ['ActualPromoLSVByCompensation', 'ActualProductLSVByCompensation'],
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
                            xtype: 'textfield',
                            name: 'InvoiceNumber',
                            flex: 1,
                            layout: 'anchor',
                            regex: /^([0-9a-zA-ZА-Яа-я]{4,}[,])*[0-9a-zA-ZА-Яа-я]{4,}$/,
                            regexText: l10n.ns('tpm', 'Promo').value('InvoiceNumberRegex'),
                            fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceNumber'),
                            labelAlign: 'left',
                            //Для одного уровня с остальными полями
                            labelWidth: 190,
                            padding: '0 5 5 5',
                            margin: '25 0 0 0',
                            needReadOnly: true,
                            isChecked: true,
                            allowBlank: true,
                            allowOnlyWhitespace: true,
                            crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
                        }, {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            flex: 1,
                            layout: 'anchor',
                            regex: /^([0-9a-zA-ZА-Яа-я]{4,}[,])*[0-9a-zA-ZА-Яа-я]{4,}$/,
                            regexText: l10n.ns('tpm', 'Promo').value('DocumentNumberRegex'),
                            fieldLabel: l10n.ns('tpm', 'Promo').value('DocumentNumber'),
                            labelAlign: 'left',
                            //Для одного уровня с остальными полями
                            labelWidth: 190,
                            padding: '0 5 5 5',
                            needReadOnly: true,
                            isChecked: true,
                            allowBlank: true,
                            allowOnlyWhitespace: true,
                            crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
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
                            glyph: 0xf552,
                            disabled: true
                        }]
                    }]
                }] 
            }]
        }]
    }]
})