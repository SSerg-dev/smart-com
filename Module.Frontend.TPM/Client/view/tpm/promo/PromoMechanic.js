Ext.define('App.view.tpm.promo.PromoMechanic', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promomechanic',

    LinkedPromoes: null,

    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'hbox',
            align: 'stretchmax'
        },
        items: [
            {
                xtype: 'custompromopanel',
                minWidth: 245,
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [
                    {
                        xtype: 'fieldset',
                        title: 'Mars',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'searchcombobox',
                                flex: 1,
                                layout: 'anchor',
                                padding: '0 5 5 5',
                                fieldLabel: 'Mechanic',
                                labelAlign: 'top',
                                name: 'MarsMechanicId',
                                selectorWidget: 'mechanic',
                                valueField: 'Id',
                                displayField: 'Name',
                                entityType: 'Mechanic',
                                needReadOnly: true,
                                allowBlank: false,
                                allowOnlyWhitespace: false,
                                allowDecimals: false,
                                allowExponential: false,
                                crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
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

                                            window.on('resize', function () { window.show() }, this);
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
                                    Ext.util.Observable.capture(this, function (evname) { console.log(evname, arguments); })

                                    var promoController = App.app.getController('tpm.promo.Promo'),
                                        promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
                                        mechanicFields = promoController.getMechanicFields(promoMechanic);

                                    promoController.resetFields([
                                        mechanicFields.marsMechanicFields.marsMechanicId,
                                        mechanicFields.marsMechanicFields.marsMechanicTypeId,
                                        mechanicFields.marsMechanicFields.marsMechanicDiscount,
                                    ]);

                                    promoController.disableFields([
                                        mechanicFields.marsMechanicFields.marsMechanicTypeId,
                                        mechanicFields.marsMechanicFields.marsMechanicDiscount,
                                    ]);
                                    this.validate();
                                },
                                mapping: [{
                                    from: 'Name',
                                    to: 'MarsMechanicName'
                                }]
                            },
                            {
                                xtype: 'searchcombobox',
                                flex: 1,
                                layout: 'anchor',
                                padding: '0 5 5 5',
                                fieldLabel: 'Type',
                                labelAlign: 'top',
                                name: 'MarsMechanicTypeId',
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
                                crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                //store: {
                                //    type: 'promoformmechanictypestore'
                                //},
                                store: {
                                    type: 'simplestore',
                                    autoLoad: false,
                                    alias: 'store.promoformmechanictypestore',
                                    model: 'App.model.tpm.mechanictype.MechanicTypeForClient',
                                    extendedFilter: {
                                        xclass: 'App.ExtFilterContext',
                                        supportedModels: [{
                                            xclass: 'App.ExtSelectionFilterModel',
                                            model: 'App.model.tpm.mechanictype.MechanicTypeForClient',
                                            modelId: 'efselectionmodel'
                                        }]
                                    }
                                },
                                onTrigger3Click: function () {
                                    var promoController = App.app.getController('tpm.promo.Promo'),
                                        promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
                                        mechanicFields = promoController.getMechanicFields(promoMechanic);

                                    promoController.resetFields([
                                        mechanicFields.marsMechanicFields.marsMechanicTypeId,
                                        mechanicFields.marsMechanicFields.marsMechanicDiscount,
                                    ]);
                                    this.validate();
                                },
                                mapping: [{
                                    from: 'Name',
                                    to: 'MarsMechanicTypeName'
                                }, {
                                    from: 'Discount',
                                    to: 'MarsMechanicDiscount'
                                }]
                            },
                            {
                                xtype: 'numberfield',
                                flex: 1,
                                layout: 'anchor',
                                name: 'MarsMechanicDiscount',
                                minValue: 0,
                                maxValue: 100,
                                fieldLabel: 'Discount',
                                labelAlign: 'top',
                                padding: '0 5 5 5',
                                needReadOnly: true,
                                allowBlank: false,
                                allowOnlyWhitespace: false,
                                allowDecimals: true,
                                allowExponential: false,
                                crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                            }
                        ]
                    },
                    {
                        xtype: 'fieldset',
                        title: l10n.ns('tpm', 'Promo').value('InstoreAssumption'),
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'searchcombobox',
                                flex: 1,
                                layout: 'anchor',
                                padding: '0 5 5 5',
                                fieldLabel: 'Mechanic',
                                labelAlign: 'top',
                                name: 'PlanInstoreMechanicId',
                                selectorWidget: 'mechanic',
                                valueField: 'Id',
                                displayField: 'Name',
                                entityType: 'Mechanic',
                                needReadOnly: true,
                                allowBlank: true,
                                crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                customTip: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicNameTip'),
                                // store: {
                                //     type: 'promoformmechanicstore'
                                // },
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
                                            window.on('resize', function () { window.show() }, this);
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
                                        promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
                                        mechanicFields = promoController.getMechanicFields(promoMechanic);

                                    promoController.resetFields([
                                        mechanicFields.instoreMechanicFields.instoreMechanicId,
                                        mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                                        mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
                                    ]);

                                    promoController.disableFields([
                                        mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                                        mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
                                    ]);
                                    //this.validate();
                                },
                                listeners: {
                                    change: function (field, newVal, oldVal) {
                                        this.up('promoeditorcustom').down('#PlanInstoreMechanicIdInActivity').setRawValue(field.rawValue);
                                    }
                                },
                                mapping: [{
                                    from: 'Name',
                                    to: 'PlanInstoreMechanicName'
                                }]
                            },
                            {
                                xtype: 'searchcombobox',
                                flex: 1,
                                layout: 'anchor',
                                padding: '0 5 5 5',
                                fieldLabel: 'Type',
                                labelAlign: 'top',
                                name: 'PlanInstoreMechanicTypeId',
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
                                crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                customTip: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicTypeNameTip'),
                                //store: {
                                //    type: 'promoformmechanictypestore'
                                //},
                                store: {
                                    type: 'simplestore',
                                    autoLoad: false,
                                    alias: 'store.promoformmechanictypestore',
                                    model: 'App.model.tpm.mechanictype.MechanicTypeForClient',
                                    extendedFilter: {
                                        xclass: 'App.ExtFilterContext',
                                        supportedModels: [{
                                            xclass: 'App.ExtSelectionFilterModel',
                                            model: 'App.model.tpm.mechanictype.MechanicTypeForClient',
                                            modelId: 'efselectionmodel'
                                        }]
                                    }
                                },
                                onTrigger3Click: function () {
                                    var promoController = App.app.getController('tpm.promo.Promo'),
                                        promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
                                        mechanicFields = promoController.getMechanicFields(promoMechanic);

                                    promoController.resetFields([
                                        mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                                        mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
                                    ]);
                                    //this.validate();
                                },
                                listeners: {
                                    change: function (field, newVal, oldVal) {
                                        this.up('promoeditorcustom').down('#PlanInstoreMechanicTypeIdInActivity').setRawValue(field.rawValue);
                                    }
                                },
                                mapping: [{
                                    from: 'Name',
                                    to: 'PlanInstoreMechanicTypeName'
                                }, {
                                    from: 'Discount',
                                    to: 'PlanInstoreMechanicDiscount'
                                }]
                            },
                            {
                                xtype: 'numberfield',
                                flex: 1,
                                layout: 'anchor',
                                name: 'PlanInstoreMechanicDiscount',
                                minValue: 0,
                                maxValue: 100,
                                fieldLabel: 'Discount',
                                labelAlign: 'top',
                                padding: '0 5 5 5',
                                needReadOnly: true,
                                allowBlank: false,
                                allowOnlyWhitespace: false,
                                allowDecimals: true,
                                allowExponential: false,
                                crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                customTip: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicDiscountTip'),
                                listeners: {
                                    change: function (field, newVal, oldVal) {
                                        var discount = field.rawValue.replace(",", ".");
                                        var parsedDiscount = parseFloat(discount).toFixed(2);
                                        if (parsedDiscount != 'NaN') {
                                            this.up('promoeditorcustom').down('#PlanInstoreMechanicDiscountInActivity').setRawValue(parsedDiscount);
                                        } else {
                                            this.up('promoeditorcustom').down('#PlanInstoreMechanicDiscountInActivity').setRawValue(null);
                                        }
                                    }
                                },
                            }
                        ]
                    },
                    {
                        xtype: 'textarea',
                        flex: 1,
                        maxHeight: 70,
                        padding: '0 0 5 0',
                        name: 'PromoComment',
                        fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicComment'),
                        labelAlign: 'top',
                        cls: 'promo-textarea',
                        needReadOnly: true,
                        crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                    }
                ]
            },
            {
                xtype: 'splitter',
                itemId: 'splitter_3',
                cls: 'custom-promo-panel-splitter',
                collapseOnDblClick: false,
                listeners: {
                    dblclick: {
                        fn: function (event, el) {
                            var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_3]')[0];
                            cmp.tracker.getPrevCmp().flex = 1;
                            cmp.tracker.getNextCmp().flex = 1;
                            cmp.ownerCt.updateLayout();
                        },
                        element: 'el'
                    }
                }
            },
            {
                xtype: 'custompromopanel',
                minWidth: 245,
                flex: 1,
                padding: '10 10 10 10',
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                    pack: 'center '
                },
                items: [
                    {
                        xtype: 'container',
                        flex: 1,
                        maxHeight: 56,
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelAlign: 'left',
                        },
                        items: [
                            {
                                xtype: 'container',
                                name: 'GrowthAcceleration',
                                height: 40,
                                margin: '4 0 10 0',
                                style: 'border: 1px solid #ebebeb',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                flex: 1.3,
                                items: [
                                    {
                                        xtype: 'checkboxfield',
                                        crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                        needReadOnly: true,
                                        //readOnly: true,
                                        margin: '5 0 0 5',
                                        name: 'GrowthAccelerationCheckbox',
                                        boxLabel: '<b>' + l10n.ns('tpm', 'Promo').value('GrowthAcceleration') + '</b>',
                                        flex: 1,
                                        listeners: {
                                            change: function (val) {
                                                var planPanel = Ext.ComponentQuery.query('#promoBudgets_step4_planpanel')[0];
                                                var actualPanel = Ext.ComponentQuery.query('#promoBudgets_step4_actualpanel')[0];
                                                var button = Ext.ComponentQuery.query('#btn_promoBudgets_step4')[0];
                                                planPanel.setDisabled(!val.getValue());
                                                actualPanel.setDisabled(!val.getValue());
                                                button.setDisabled(!val.getValue());
                                                if (val.getValue()) {

                                                    button.removeCls('disabled');
                                                }
                                                else {
                                                    button.addCls('disabled');
                                                }
                                            }
                                        }
                                    },
                                    {
                                        xtype: 'checkboxfield',
                                        name: 'IsInExchange',
                                        //crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                        boxLabel: '<b>' + l10n.ns('tpm', 'Promo').value('GAInExcnange') + '</b>',
                                        margin: '5 0 0 5',
                                        flex: 1,
                                        //needReadOnly: true,
                                    }
                                ]
                            },
                            {
                                xtype: 'container',
                                padding: '0 5 5 0'
                            },
                            {
                                xtype: 'container',
                                name: 'ApolloExport',
                                height: 40,
                                margin: '4 0 10 0',
                                style: 'border: 1px solid #ebebeb',
                                flex: 0.7,
                                items: [{
                                    xtype: 'checkboxfield',
                                    crudAccess: ['Administrator', 'SupportAdministrator', 'CustomerMarketing', 'KeyAccountManager', 'DemandPlanning'],
                                    needReadOnly: true,
                                    //readOnly: true,
                                    margin: '5 0 0 5',
                                    name: 'ApolloExportCheckbox',
                                    boxLabel: '<b>' + l10n.ns('tpm', 'Promo').value('ApolloExport') + '</b>',
                                }]
                            }
                        ]
                    },
                    {
                        xtype: 'container',
                        style: 'border: 1px solid #ebebeb',
                        flex: 1,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'container',
                                flex: 1,
                                itemId: 'choosenPromoPanel',
                                autoScroll: true,
                                cls: 'scrollpanel hScrollPanel',
                                layout: {
                                    type: 'hbox',
                                    align: 'top',
                                },
                                items: []
                            },
                            {
                                xtype: 'container',
                                height: 33,
                                layout: {
                                    type: 'hbox',
                                    align: 'top',
                                    pack: 'center'
                                },
                                items: [
                                    {
                                        xtype: 'tbspacer',
                                        flex: 1
                                    },
                                    {
                                        xtype: 'button',
                                        margin: '0 10 0 0',
                                        width: 150,
                                        cls: 'hierarchyButton hierarchyButtonList',
                                        itemId: 'promoMechanicAddPromoBtn',
                                        text: l10n.ns('tpm', 'Promo').value('SelectPromo'),
                                        tooltip: l10n.ns('tpm', 'Promo').value('SelectPromo'),
                                        glyph: 0xf208,
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        ]
    }],
    fillSelectedPromoes: function (record) {
        if (record.data.IsInExchange) {
            var selectedBtns = [];
            var selectedPanel = this.down('#choosenPromoPanel');
            var promoEditorCustom = this.up('promoeditorcustom');
            selectedPanel.removeAll();
            promoEditorCustom.setLoading(true);

            breeze.EntityQuery
                .from('Promoes')
                .where('MasterPromoId', '==', record.data.Id)
                .withParameters({
                    $actionName: 'GetFilteredData',
                    $method: 'POST',
                })
                .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
                .execute()
                .then(function (data) {
                    data.results.forEach(function (item) {
                        var butt = {
                            xtype: 'container',
                            style: 'border: 1px solid #ebebeb',
                            margin: '10 0 10 10',
                            height: 120,
                            layout: {
                                type: 'vbox',
                                align: 'middle',
                                pack: 'center'
                            },
                            items: [
                                {
                                    xtype: 'label',
                                    text: 'ID: ' + item.Number,
                                    padding: '5',
                                    width: 98,
                                    style: 'display:inline-block;text-align:center'
                                },
                                {
                                    xtype: 'label',
                                    text: item.Name,
                                    padding: '5',
                                    width: 98,
                                    style: 'display:inline-block;text-align:center'
                                },
                            ]
                        };
                        selectedBtns.push(butt);
                    });
                    selectedPanel.add(selectedBtns)

                    promoEditorCustom.setLoading(false);
                })
                .fail(function (data) {
                    promoEditorCustom.setLoading(false);
                    App.Notify.pushError(data.message);
                })
        }
    }
})