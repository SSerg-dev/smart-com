Ext.define('App.view.tpm.promo.PromoMechanic', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promomechanic',

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
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'fieldset',
                title: 'Mars',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
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
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    store: {
                        type: 'promoformmechanicstore'
                    },
                    onTrigger3Click: function () {
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
                    },
                    mapping: [{
                        from: 'Name',
                        to: 'MarsMechanicName'
                    }]
                }, {
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
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    store: {
                        type: 'promoformmechanictypestore'
                    },
                    onTrigger3Click: function () {
                        var promoController = App.app.getController('tpm.promo.Promo'),
                            promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
                            mechanicFields = promoController.getMechanicFields(promoMechanic);

                        promoController.resetFields([
                            mechanicFields.marsMechanicFields.marsMechanicTypeId,
                            mechanicFields.marsMechanicFields.marsMechanicDiscount,
                        ]);
                    },
                    mapping: [{
                        from: 'Name',
                        to: 'MarsMechanicTypeName'
                    }, {
                        from: 'Discount',
                        to: 'MarsMechanicDiscount'
                    }]
                }, {
                    xtype: 'numberfield',
                    flex: 1,
                    layout: 'anchor',
                    name: 'MarsMechanicDiscount',
                    minValue: 1,
                    maxValue: 100,
                    fieldLabel: 'Discount',
                    labelAlign: 'top',
                    padding: '0 5 5 5',
                    needReadOnly: true,
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    allowDecimals: false,
                    allowExponential: false,
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                }]
            }, {
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'Promo').value('InstoreAssumption'),
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
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
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    customTip: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicNameTip'),
                    store: {
                        type: 'promoformmechanicstore'
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
                    },
                    mapping: [{
                        from: 'Name',
                        to: 'PlanInstoreMechanicName'
                    }]
                }, {
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
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    customTip: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicTypeNameTip'),
                    store: {
                        type: 'promoformmechanictypestore'
                    },
                    onTrigger3Click: function () {
                        var promoController = App.app.getController('tpm.promo.Promo'),
                            promoMechanic = Ext.ComponentQuery.query('promomechanic')[0],
                            mechanicFields = promoController.getMechanicFields(promoMechanic);

                        promoController.resetFields([
                            mechanicFields.instoreMechanicFields.instoreMechanicTypeId,
                            mechanicFields.instoreMechanicFields.instoreMechanicDiscount,
                        ]);
                    },
                    mapping: [{
                        from: 'Name',
                        to: 'PlanInstoreMechanicTypeName'
                    }, {
                        from: 'Discount',
                        to: 'PlanInstoreMechanicDiscount'
                    }]
                }, {
                    xtype: 'numberfield',
                    flex: 1,
                    layout: 'anchor',
                    name: 'PlanInstoreMechanicDiscount',
                    minValue: 1,
                    maxValue: 100,
                    fieldLabel: 'Discount',
                    labelAlign: 'top',
                    padding: '0 5 5 5',
                    needReadOnly: true,
                    allowBlank: false,
                    allowOnlyWhitespace: false,
                    allowDecimals: false,
                    allowExponential: false,
                    crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
                    customTip: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicDiscountTip'),
                }]
            }]
        }, {
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
        }, {
            xtype: 'custompromopanel',
            minWidth: 245,
            flex: 1,
            padding: '0 10 10 10',
            items: [{
                xtype: 'textarea',
                name: 'PromoComment',
                fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicComment'),
                labelAlign: 'top',
                cls: 'promo-textarea',
                needReadOnly: true,
                crudAccess: ['Administrator', 'FunctionalExpert', 'CustomerMarketing', 'KeyAccountManager'],
            }]
        }]
    }]
})