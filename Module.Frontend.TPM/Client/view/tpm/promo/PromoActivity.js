
Ext.define('App.view.tpm.promo.PromoActivity', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promoactivity',

    initComponent: function () {
        this.callParent(arguments);
    },

    items: [{
        xtype: 'container',
        cls: 'promo-editor-custom-scroll-items',        
        items: [
            {
                // promoActivity_step1
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
                items: [
                    {
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
                                    noneCanEdit: true,
                                    setReadOnly: function () { return false },
                                    allowBlank: true,
                                    customTip: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicNameTip'),
                                    //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
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
                                    noneCanEdit: true,
                                    setReadOnly: function () { return false },
                                    //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
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
                                    allowDecimals: true,
                                    readOnly: true,
                                    readOnlyCls: 'readOnlyField',
                                    noneCanEdit: true,
                                    setReadOnly: function () { return false },
                                    //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                }]
                            }, {
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
                                //needReadOnly: true,
                                isChecked: true,
                                allowBlank: true,
                                allowOnlyWhitespace: true,
                                allowDecimals: true,
                                allowExponential: false,
                                //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                availableRoleStatusActions: {
                                    SupportAdministrator: App.global.Statuses.AllStatuses,
                                    Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                    FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                    CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                    CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                    KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                },
                            }]
                        }]
                    },
                    {
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
                    },
                    {
                        xtype: 'custompromopanel',
                        minWidth: 245,
                        flex: 1,
                        items: [
                            {
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
                                        //needReadOnly: true,
                                        allowBlank: true,
                                        //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                        availableRoleStatusActions: {
                                            SupportAdministrator: App.global.Statuses.AllStatuses,
                                            Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        },
                                        customTip: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicNameTip'),
                                        store: {
                                            type: 'promoformmechanicstore'
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
                                        //needReadOnly: true,
                                        allowBlank: false,
                                        allowOnlyWhitespace: false,
                                        allowDecimals: false,
                                        allowExponential: false,
                                        disabled: true,
                                        //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                        availableRoleStatusActions: {
                                            SupportAdministrator: App.global.Statuses.AllStatuses,
                                            Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        },
                                        customTip: l10n.ns('tpm', 'Promo').value('ActualInStoreMechanicTypeNameTip'),
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
                                        flex: 1,
                                        layout: 'anchor',
                                        name: 'ActualInStoreDiscount',
                                        minValue: 1,
                                        maxValue: 100,
                                        fieldLabel: 'Discount',
                                        labelAlign: 'top',
                                        padding: '0 5 5 5',
                                        readOnlyCls: 'readOnlyField',
                                        //needReadOnly: true,
                                        isChecked: true,
                                        allowBlank: false,
                                        allowOnlyWhitespace: false,
                                        allowDecimals: true,
                                        allowExponential: false,
                                        disabled: true,
                                        //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                        availableRoleStatusActions: {
                                            SupportAdministrator: App.global.Statuses.AllStatuses,
                                            Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                            KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        },
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
                                    //needReadOnly: true,
                                    isChecked: true,
                                    allowBlank: true,
                                    allowOnlyWhitespace: true,
                                    allowDecimals: true,
                                    allowExponential: false,
                                    //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
                                    availableRoleStatusActions: {
                                        SupportAdministrator: App.global.Statuses.AllStatuses,
                                        Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                    },
                                }]
                            }
                        ]
                    }
                ]
            },
            // promoActivity_step2
            {
                xtype: 'panel',
                name: 'promoActivity_step2',
                itemId: 'promoActivity_step2',
                bodyStyle: { "background-color": "#99a9b1" },
                cls: 'promoform-item-wrap',
                minHeight: 500,
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
                    items: [
                        {
                            xtype: 'custompromopanel',
                            minWidth: 245,
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                pack: 'center'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
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
                                    items: [
                                        {
                                            xtype: 'container',
                                            itemId: 'ContainerPlanPromoUplift',
                                            height: '100%',
                                            layout: {
                                                type: 'hbox',
                                                align: 'top',
                                                pack: 'center'
                                            },
                                            items: [
                                                {
                                                    xtype: 'triggerfielddetails',
                                                    name: 'PlanPromoUpliftPercent',
                                                    windowType: 'promoproductsview',
                                                    labelWidth: 190,
                                                    fieldLabel: l10n.ns('tpm', 'Promo').value('PromoUpliftPercent'),
                                                    tooltip: l10n.ns('tpm', 'PromoActivity').value('UpdateActuals'),
                                                    flex: 1,
                                                    readOnly: false,
                                                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'DemandPlanning'],
                                                    //listenersToAdd: {
                                                    //    afterrender: function () {
                                                    //        var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                                                    //        if (this.crudAccess.indexOf(currentRole) === -1) {
                                                    //            this.up('container[itemId=ContainerPlanPromoUplift]').setReadable(true);
                                                    //        }
                                                    //    }
                                                    //},
                                                    validator: function (value) {
                                                        if (this.editable) {
                                                            //Заменяем запятую на точку для парсера
                                                            value = value.split(",").join(".");
                                                            var floatValue = parseFloat(value);
                                                            if (floatValue != value) {
                                                                return l10n.ns('tpm', 'PromoActivity').value('triggerfieldOnlyNumbers');
                                                            } else if (floatValue <= 0 || value === "") {
                                                                return l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercentError');
                                                            } else {
                                                                return true;
                                                            }
                                                        } else {
                                                            return true;
                                                        }
                                                    },
                                                    //changeEditable: function (setToEditable) {
                                                    //    if (setToEditable) {
                                                    //        this.setEditable(true);
                                                    //        this.up('container').addCls('editable-trigger-field');
                                                    //    } else {
                                                    //        this.setEditable(false);
                                                    //        this.up('container').removeCls('editable-trigger-field');
                                                    //    }
                                                    //    this.validate();
                                                    //}
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    labelSeparator: '',
                                                    itemId: 'PromoUpliftLockedUpdateCheckbox',
                                                    name: 'NeedRecountUplift',
                                                    labelAlign: 'right',
                                                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'DemandPlanning'],
                                                    style: 'margin-left: 10px',
                                                    //crudAccess: ['Administrator', 'FunctionalExpert', 'DemandPlanning'],
                                                    availableRoleStatusActions: {
                                                        SupportAdministrator: App.global.Statuses.AllStatuses,
                                                        Administrator: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                        FunctionalExpert: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                        DemandPlanning: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                    },
                                                    availableRoleStatusActionsInOut: {

                                                    },
                                                    listeners: {
                                                        afterRender: function (me) {
                                                            var readonly = me.up('promoeditorcustom').readOnly;
                                                            var planPromoUpliftNumberField = this.up('container').down('triggerfielddetails[name=PlanPromoUpliftPercent]');
                                                            var GlyphLock = this.up('container').down('#GlyphLock');
                                                            var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                                                            var lockAccessCrud = false;

                                                            if (planPromoUpliftNumberField.crudAccess.indexOf(currentRole) === -1) {
                                                                lockAccessCrud = true;
                                                            }
                                                            if ((!this.value || readonly) || lockAccessCrud) {
                                                                planPromoUpliftNumberField.setEditable(false);
                                                                GlyphLock.setGlyph(0xf33e);
                                                                planPromoUpliftNumberField.isReadable = false;
                                                                //planPromoUpliftNumberField.removeCls('readOnlyField');
                                                            } else {
                                                                planPromoUpliftNumberField.setEditable(true);
                                                                GlyphLock.setGlyph(0xf33f);
                                                                planPromoUpliftNumberField.isReadable = true;
                                                                //planPromoUpliftNumberField.addCls('readOnlyField');
                                                            }
                                                        },
                                                        change: function (checkbox, newValue, oldValue) {
                                                            var planPromoUpliftNumberField = this.up('container').down('triggerfielddetails[name=PlanPromoUpliftPercent]');
                                                            var GlyphLock = this.up('container').down('#GlyphLock');
                                                            if (newValue) {
                                                                planPromoUpliftNumberField.setEditable(true);
                                                                GlyphLock.setGlyph(0xf33f);
                                                                planPromoUpliftNumberField.isReadable = true;
                                                                //planPromoUpliftNumberField.removeCls('readOnlyField');
                                                            } else {
                                                                planPromoUpliftNumberField.setEditable(false);
                                                                GlyphLock.setGlyph(0xf33e);
                                                                planPromoUpliftNumberField.isReadable = false;
                                                                //planPromoUpliftNumberField.addCls('readOnlyField');
                                                            }
                                                        },
                                                        enable: function (me) {
                                                            var readonly = me.up('promoeditorcustom').readOnly;
                                                            var planPromoUpliftNumberField = this.up('container').down('triggerfielddetails[name=PlanPromoUpliftPercent]');
                                                            var GlyphLock = this.up('container').down('#GlyphLock');
                                                            if (!this.value || readonly) {
                                                                planPromoUpliftNumberField.setEditable(false);
                                                                planPromoUpliftNumberField.isReadable = false;
                                                                //planPromoUpliftNumberField.removeCls('readOnlyField');
                                                            } else {
                                                                planPromoUpliftNumberField.setEditable(true);
                                                                planPromoUpliftNumberField.isReadable = true;
                                                                //planPromoUpliftNumberField.addCls('readOnlyField');
                                                            }
                                                            GlyphLock.setDisabled(false);
                                                        },
                                                        disable: function (me) {
                                                            var GlyphLock = this.up('container').down('#GlyphLock');
                                                            GlyphLock.setDisabled(true);
                                                        },
                                                    }
                                                },
                                                {
                                                    xtype: 'button',
                                                    itemId: 'GlyphLock',
                                                    cls: 'promo-calculation-activity-uplift-btnglyph',
                                                    glyph: 0xf33e,
                                                    width: 30,
                                                    height: 30,
                                                    availableRoleStatusActions: {
                                                        SupportAdministrator: App.global.Statuses.AllStatuses,
                                                        Administrator: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                        FunctionalExpert: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                        DemandPlanning: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                    },
                                                    availableRoleStatusActionsInOut: {

                                                    },
                                                }
                                            ]
                                        },
                                        {
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                }
                                            }
                                        },
                                        {
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                },
                                                scope: this
                                            }
                                        },
                                        {
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                },
                                            }
                                        },
                                        {
                                            xtype: 'triggerfielddetails',
                                            name: 'PlanPromoPostPromoEffectLSV',
                                            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoPostPromoEffectLSV'),
                                            dataIndexes: ['PlanProductPostPromoEffectLSV', 'PlanProductPostPromoEffectW1', 'PlanProductPostPromoEffectW2'],
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                },
                                            }
                                        }
                                    ]
                                },
                                {
                                    // PlanPriceIncrease
                                    xtype: 'fieldset',
                                    flex: 1,
                                    title: l10n.ns('tpm', 'PromoActivity').value('PlanPriceIncrease'),
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch',
                                        pack: 'center'
                                    },
                                    padding: '0 10 10 10',
                                    defaults: {
                                        margin: '5 0 0 0',
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            itemId: 'ContainerPlanPromoUpliftPI',
                                            height: '100%',
                                            layout: {
                                                type: 'hbox',
                                                align: 'top',
                                                pack: 'center'
                                            },
                                            items: [
                                                {
                                                    xtype: 'triggerfielddetails',
                                                    name: 'PlanPromoUpliftPercentPI',
                                                    windowType: 'promoproductpriceincreasesview',
                                                    labelWidth: 190,
                                                    fieldLabel: l10n.ns('tpm', 'Promo').value('PromoUpliftPercent'),
                                                    tooltip: l10n.ns('tpm', 'PromoActivity').value('UpdateActuals'),
                                                    flex: 1,
                                                    readOnly: false,
                                                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'DemandPlanning'],
                                                    validator: function (value) {
                                                        if (this.editable) {
                                                            //Заменяем запятую на точку для парсера
                                                            value = value.split(",").join(".");
                                                            var floatValue = parseFloat(value);
                                                            if (floatValue != value) {
                                                                return l10n.ns('tpm', 'PromoActivity').value('triggerfieldOnlyNumbers');
                                                            } else if (floatValue <= 0 || value === "") {
                                                                return l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercentError');
                                                            } else {
                                                                return true;
                                                            }
                                                        } else {
                                                            return true;
                                                        }
                                                    },
                                                },
                                                //{
                                                //    xtype: 'checkbox',
                                                //    labelSeparator: '',
                                                //    itemId: 'PromoUpliftLockedUpdateCheckboxPI',
                                                //    name: 'NeedRecountUpliftPI',
                                                //    labelAlign: 'right',
                                                //    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'DemandPlanning'],
                                                //    style: 'margin-left: 10px',
                                                //    //crudAccess: ['Administrator', 'FunctionalExpert', 'DemandPlanning'],
                                                //    availableRoleStatusActions: {
                                                //        SupportAdministrator: App.global.Statuses.AllStatuses,
                                                //        Administrator: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                //        FunctionalExpert: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                //        DemandPlanning: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                //    },
                                                //    availableRoleStatusActionsInOut: {

                                                //    },
                                                //    listeners: {
                                                //        afterRender: function (me) {
                                                //            var readonly = me.up('promoeditorcustom').readOnly;
                                                //            var planPromoUpliftNumberFieldPI = this.up('container').down('triggerfielddetails[name=PlanPromoUpliftPercentPI]');
                                                //            var GlyphLock = this.up('container').down('#GlyphLock');
                                                //            var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                                                //            var lockAccessCrud = false;

                                                //            if (planPromoUpliftNumberFieldPI.crudAccess.indexOf(currentRole) === -1) {
                                                //                lockAccessCrud = true;
                                                //            }
                                                //            if ((!this.value || readonly) || lockAccessCrud) {
                                                //                planPromoUpliftNumberFieldPI.setEditable(false);
                                                //                GlyphLock.setGlyph(0xf33e);
                                                //                planPromoUpliftNumberFieldPI.isReadable = false;
                                                //                //planPromoUpliftNumberField.removeCls('readOnlyField');
                                                //            } else {
                                                //                planPromoUpliftNumberFieldPI.setEditable(true);
                                                //                GlyphLock.setGlyph(0xf33f);
                                                //                planPromoUpliftNumberFieldPI.isReadable = true;
                                                //                //planPromoUpliftNumberField.addCls('readOnlyField');
                                                //            }
                                                //        },
                                                //        change: function (checkbox, newValue, oldValue) {
                                                //            var planPromoUpliftNumberFieldPI = this.up('container').down('triggerfielddetails[name=PlanPromoUpliftPercentPI]');
                                                //            var GlyphLock = this.up('container').down('#GlyphLock');
                                                //            if (newValue) {
                                                //                planPromoUpliftNumberFieldPI.setEditable(true);
                                                //                GlyphLock.setGlyph(0xf33f);
                                                //                planPromoUpliftNumberFieldPI.isReadable = true;
                                                //                //planPromoUpliftNumberField.removeCls('readOnlyField');
                                                //            } else {
                                                //                planPromoUpliftNumberFieldPI.setEditable(false);
                                                //                GlyphLock.setGlyph(0xf33e);
                                                //                planPromoUpliftNumberFieldPI.isReadable = false;
                                                //                //planPromoUpliftNumberField.addCls('readOnlyField');
                                                //            }
                                                //        },
                                                //        enable: function (me) {
                                                //            var readonly = me.up('promoeditorcustom').readOnly;
                                                //            var planPromoUpliftNumberFieldPI = this.up('container').down('triggerfielddetails[name=PlanPromoUpliftPercentPI]');
                                                //            var GlyphLock = this.up('container').down('#GlyphLock');
                                                //            if (!this.value || readonly) {
                                                //                planPromoUpliftNumberFieldPI.setEditable(false);
                                                //                planPromoUpliftNumberFieldPI.isReadable = false;
                                                //                //planPromoUpliftNumberField.removeCls('readOnlyField');
                                                //            } else {
                                                //                planPromoUpliftNumberFieldPI.setEditable(true);
                                                //                planPromoUpliftNumberFieldPI.isReadable = true;
                                                //                //planPromoUpliftNumberField.addCls('readOnlyField');
                                                //            }
                                                //            GlyphLock.setDisabled(false);
                                                //        },
                                                //        disable: function (me) {
                                                //            var GlyphLock = this.up('container').down('#GlyphLock');
                                                //            GlyphLock.setDisabled(true);
                                                //        },
                                                //    }
                                                //},
                                                //{
                                                //    xtype: 'button',
                                                //    itemId: 'GlyphLock',
                                                //    cls: 'promo-calculation-activity-uplift-btnglyph',
                                                //    glyph: 0xf33e,
                                                //    width: 30,
                                                //    height: 30,
                                                //    availableRoleStatusActions: {
                                                //        SupportAdministrator: App.global.Statuses.AllStatuses,
                                                //        Administrator: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                //        FunctionalExpert: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                //        DemandPlanning: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                                //    },
                                                //    availableRoleStatusActionsInOut: {

                                                //    },
                                                //}
                                            ]
                                        },
                                        {
                                            xtype: 'triggerfielddetails',
                                            name: 'PlanPromoBaselineLSVPI',
                                            windowType: 'promoactivitydetailsinfopi',
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                }
                                            }
                                        },
                                        {
                                            xtype: 'triggerfielddetails',
                                            name: 'PlanPromoIncrementalLSVPI',
                                            windowType: 'promoactivitydetailsinfopi',
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                },
                                                scope: this
                                            }
                                        },
                                        {
                                            xtype: 'triggerfielddetails',
                                            name: 'PlanPromoLSVPI',
                                            windowType: 'promoactivitydetailsinfopi',
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                },
                                            }
                                        },
                                        {
                                            xtype: 'triggerfielddetails',
                                            name: 'PlanPromoPostPromoEffectLSVPI',
                                            windowType: 'promoactivitydetailsinfopi',
                                            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoPostPromoEffectLSV'),
                                            dataIndexes: ['PlanProductPostPromoEffectLSV', 'PlanProductPostPromoEffectW1', 'PlanProductPostPromoEffectW2'],
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
                                                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                                return isNaN(parsedValue) ? null : parsedValue;
                                            },
                                            listenersToAdd: {
                                                focus: function (field) {
                                                    this.blockMillion = true;
                                                },
                                                blur: function (field) {
                                                    this.blockMillion = false;
                                                },
                                            }
                                        }
                                    ]
                                },
                            ]
                        },
                        {
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
                        },
                        {
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
                                name: 'activityActuals',
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
                                        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                        return isNaN(parsedValue) ? null : parsedValue;
                                    },
                                    listenersToAdd: {
                                        change: this.activityChangeListener,
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
                                        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                        return isNaN(parsedValue) ? null : parsedValue;
                                    },
                                    listenersToAdd: {
                                        change: this.activityChangeListener,
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
                                        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                        return isNaN(parsedValue) ? null : parsedValue;
                                    },
                                    listenersToAdd: {
                                        change: this.activityChangeListener,
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
                                        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                        return isNaN(parsedValue) ? null : parsedValue;
                                    },
                                    listenersToAdd: {
                                        change: this.activityChangeListener,
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
                                        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                        return isNaN(parsedValue) ? null : parsedValue;
                                    },
                                    listenersToAdd: {
                                        change: this.activityChangeListener,
                                        focus: function (field) {
                                            this.blockMillion = true;
                                        },
                                        blur: function (field) {
                                            this.blockMillion = false;
                                        },
                                    }
                                }, {
                                    xtype: 'field',
                                    name: 'ActualPromoLSVSO',
                                    fieldLabel: l10n.ns('tpm', 'Promo').value('PromoLSVSO'),
                                    dataIndexes: ['ActualProductLSVSO'],
                                    flex: 1,
                                    layout: 'anchor',
                                    readOnlyCls: 'readOnlyField',
                                    labelAlign: 'left',
                                    //Для одного уровня с остальными полями
                                    labelWidth: 190,
                                    padding: '0 5 5 5',
                                    margin: '5 0 0 0',
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
                                        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                        return isNaN(parsedValue) ? null : parsedValue;
                                    },
                                    listenersToAdd: {
                                        change: this.activityChangeListener,
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
                                        var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                                        return isNaN(parsedValue) ? null : parsedValue;
                                    },
                                    listenersToAdd: {
                                        change: this.activityChangeListener,
                                        focus: function (field) {
                                            this.blockMillion = true;
                                        },
                                        blur: function (field) {
                                            this.blockMillion = false;
                                        },
                                    }
                                }, {
                                    xtype: 'container',
                                    itemId: 'ContainerSumInvoice',
                                    layout: {
                                        type: 'hbox',
                                        align: 'top',
                                        pack: 'center'
                                    },
                                    items: [{
                                        xtype: 'triggerfielddetails',
                                        name: 'SumInvoice',
                                        fieldLabel: l10n.ns('tpm', 'Promo').value('SumInvoice'),
                                        dataIndexes: ['ActualProductPCQty', 'SumInvoiceProduct'],
                                        regex: /^-?(\d|\s)*\,?(\d|\s)*$/,
                                        regexText: l10n.ns('tpm', 'Promo').value('SumInvoiceRegex'),
                                        readOnlyCls: 'readOnlyField',
                                        flex: 1,
                                        //Для одного уровня с остальными полями
                                        labelWidth: 190,
                                        allowBlank: true,
                                        clicked: false,
                                        allowOnlyWhitespace: true,
                                        availableRoleStatusActions: {
                                            SupportAdministrator: App.global.Statuses.AllStatuses,
                                            Administrator: App.global.Statuses.Finished,
                                            FunctionalExpert: App.global.Statuses.Finished,
                                            CMManager: App.global.Statuses.Finished,
                                            CustomerMarketing: App.global.Statuses.Finished,
                                            KeyAccountManager: App.global.Statuses.Finished
                                        },
                                        valueToRaw: function (value) {
                                            var result;
                                            if (value == undefined || value == null) {
                                                result = '';
                                            }
                                            else {
                                                if (this.clicked) {
                                                    result = String(value).replace('.', ',');
                                                } else {
                                                    String(value).replace(/\s/g, '');
                                                    result = value.toLocaleString('ru-RU');
                                                }
                                            }
                                            return result;
                                        },
                                        rawToValue: function () {
                                            var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, ".").replace(/\s/g, ''))
                                            return isNaN(parsedValue) ? null : parsedValue;
                                        },
                                        listenersToAdd: {
                                            focus: function (field) {
                                                this.clicked = true;
                                                field.setValue(this.value);
                                            },
                                            blur: function (field) {
                                                this.clicked = false;
                                                field.setValue(this.value);
                                            },
                                        }
                                    }, {
                                        xtype: 'checkbox',
                                        labelSeparator: '',
                                        itemId: 'ManualInputSumInvoiceCheckbox',
                                        name: 'ManualInputSumInvoice',
                                        labelAlign: 'right',
                                        crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'DemandPlanning'],
                                        style: 'margin-left: 10px',
                                        availableRoleStatusActions: {
                                            SupportAdministrator: App.global.Statuses.AllStatuses,
                                            Administrator: App.global.Statuses.Finished,
                                            FunctionalExpert: App.global.Statuses.Finished,
                                            CMManager: App.global.Statuses.Finished,
                                            CustomerMarketing: App.global.Statuses.Finished,
                                            KeyAccountManager: App.global.Statuses.Finished
                                        },
                                        listeners: {
                                            afterRender: function (me) {
                                                var readonly = me.up('promoeditorcustom').readOnly;
                                                var sumInvoiceField = this.up('container').down('triggerfielddetails[name=SumInvoice]');
                                                var GlyphLock = this.up('container').down('#GlyphLock');
                                                var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                                                var lockAccessCrud = false;

                                                if (sumInvoiceField.crudAccess.indexOf(currentRole) === -1) {
                                                    lockAccessCrud = true;
                                                }
                                                if ((!this.value || readonly) || lockAccessCrud) {
                                                    sumInvoiceField.setEditable(false);
                                                    GlyphLock.setGlyph(0xf33e);
                                                    sumInvoiceField.isReadable = false;
                                                    //sumInvoiceField.removeCls('readOnlyField');
                                                } else {
                                                    sumInvoiceField.setEditable(true);
                                                    GlyphLock.setGlyph(0xf33f);
                                                    sumInvoiceField.isReadable = true;
                                                    //sumInvoiceField.addCls('readOnlyField');
                                                }
                                            },
                                            change: function (checkbox, newValue, oldValue) {
                                                var sumInvoiceField = this.up('container').down('triggerfielddetails[name=SumInvoice]');
                                                var GlyphLock = this.up('container').down('#GlyphLock');
                                                if (newValue) {
                                                    sumInvoiceField.setEditable(true);
                                                    GlyphLock.setGlyph(0xf33f);
                                                    sumInvoiceField.isReadable = true;
                                                    //sumInvoiceField.removeCls('readOnlyField');
                                                } else {
                                                    sumInvoiceField.setEditable(false);
                                                    GlyphLock.setGlyph(0xf33e);
                                                    sumInvoiceField.isReadable = false;
                                                    //sumInvoiceField.addCls('readOnlyField');
                                                }
                                            },
                                            enable: function (me) {
                                                var readonly = me.up('promoeditorcustom').readOnly;
                                                var sumInvoiceField = this.up('container').down('triggerfielddetails[name=SumInvoice]');
                                                var GlyphLock = this.up('container').down('#GlyphLock');
                                                if (!this.value || readonly) {
                                                    sumInvoiceField.setEditable(false);
                                                    sumInvoiceField.isReadable = false;
                                                    //sumInvoiceField.removeCls('readOnlyField');
                                                } else {
                                                    sumInvoiceField.setEditable(true);
                                                    sumInvoiceField.isReadable = true;
                                                    //sumInvoiceField.addCls('readOnlyField');
                                                }
                                                GlyphLock.setDisabled(false);
                                            },
                                            disable: function (me) {
                                                var GlyphLock = this.up('container').down('#GlyphLock');
                                                GlyphLock.setDisabled(true);
                                            },
                                        }
                                    }, {
                                        xtype: 'button',
                                        itemId: 'GlyphLock',
                                        cls: 'promo-calculation-activity-uplift-btnglyph',
                                        glyph: 0xf33e,
                                        width: 30,
                                        height: 30,
                                        availableRoleStatusActions: {
                                            SupportAdministrator: App.global.Statuses.AllStatuses,
                                            Administrator: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                            FunctionalExpert: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                            DemandPlanning: App.global.Statuses.AllStatusesBeforeStartedWithoutDraft,
                                        },
                                        availableRoleStatusActionsInOut: {

                                        },
                                    }]
                                }, {
                                    xtype: 'textfield',
                                    name: 'InvoiceNumber',
                                    fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceNumber'),
                                    flex: 1,
                                    layout: 'anchor',
                                    readOnlyCls: 'readOnlyField',
                                    regex: /^([0-9a-zA-ZА-Яа-я]{4,}([,][ ]?))*[0-9a-zA-ZА-Яа-я]{4,}$/,
                                    regexText: l10n.ns('tpm', 'Promo').value('InvoiceNumberRegex'),
                                    labelAlign: 'left',
                                    //Для одного уровня с остальными полями
                                    labelWidth: 190,
                                    padding: '0 5 5 5',
                                    margin: '5 0 0 0',
                                    isChecked: true,
                                    allowBlank: true,
                                    allowOnlyWhitespace: true,
                                    //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
                                    availableRoleStatusActions: {
                                        SupportAdministrator: App.global.Statuses.AllStatuses,
                                        Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                    },
                                }, {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    flex: 1,
                                    layout: 'anchor',
                                    readOnlyCls: 'readOnlyField',
                                    regex: /^([0-9a-zA-ZА-Яа-я]{4,}[,])*[0-9a-zA-ZА-Яа-я]{4,}$/,
                                    regexText: l10n.ns('tpm', 'Promo').value('DocumentNumberRegex'),
                                    fieldLabel: l10n.ns('tpm', 'Promo').value('DocumentNumber'),
                                    labelAlign: 'left',
                                    //Для одного уровня с остальными полями
                                    labelWidth: 190,
                                    padding: '0 5 5 5',
                                    isChecked: true,
                                    allowBlank: true,
                                    allowOnlyWhitespace: true,
                                    //crudAccess: ['Administrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager']
                                    availableRoleStatusActions: {
                                        SupportAdministrator: App.global.Statuses.AllStatuses,
                                        Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                        KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                                    },
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
                                    xtype: 'button',
                                    cls: 'promoStep-dockedBtn',
                                    itemId: 'exportAllPromoProducts',
                                    text: l10n.ns('tpm', 'PromoActivity').value('ExportAllPromoProducts'),
                                    tooltip: l10n.ns('tpm', 'PromoActivity').value('ExportAllPromoProducts'),
                                    glyph: 0xF1DA,
                                    hidden: true,
                                    availableInReadOnlyPromo: true,
                                    listeners: {
                                        afterrender: function (button) {
                                            var currentRole = App.UserInfo.getCurrentRole()['SystemName'];
                                            if (currentRole == 'SupportAdministrator') {
                                                button.show();
                                                button.setDisabled(false);
                                            }
                                        }
                                    },
                                    availableRoleStatusActions: {
                                        SupportAdministrator: App.global.Statuses.AllStatuses,
                                    },
                                }, {
                                    xtype: 'tbspacer',
                                    flex: 1
                                }, {
                                    xtype: 'button',
                                    cls: 'promoStep-dockedBtn',
                                    itemId: 'activityUploadPromoProducts',
                                    text: l10n.ns('tpm', 'PromoActivity').value('UpdateActuals'),
                                    tooltip: l10n.ns('tpm', 'PromoActivity').value('UpdateActuals'),
                                    glyph: 0xf552,
                                    layout: {
                                        type: 'hbox',
                                        align: 'top',
                                        pack: 'center'
                                    },
                                    padding: '2 0 0 2',
                                    margin: '0 0 0 0',
                                    availableRoleStatusActions: {
                                        SupportAdministrator: App.global.Statuses.AllStatuses,
                                        Administrator: App.global.Statuses.Finished,
                                        FunctionalExpert: App.global.Statuses.Finished,
                                        CMManager: App.global.Statuses.Finished,
                                        CustomerMarketing: App.global.Statuses.Finished,
                                        KeyAccountManager: App.global.Statuses.Finished
                                    },
                                    disabled: true,
                                }]
                            }]
                        }
                    ]
                }]
            }
        ]
    }]
})