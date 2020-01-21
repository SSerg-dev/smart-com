Ext.define('App.view.tpm.promo.PromoBudgetDetails', {
    extend: 'App.view.tpm.common.customPromoPanel',
    alias: 'widget.promobudgetdetails',

    // запись промо
    record: null,
    // название виджета для отображения деталей 
    widget: null,
    // Если смотрим фактические то true
    fact: false,
    // true, если Cost Production, false если CostTE
    costProd: false,
    // имя бюджета
    budgetName: null,
    // страховка до выяснений обстоятельств
    budgetItemsName: null,
    // если нужна custompromopanel
    needCustomPromoPanel: null,
    // дополнительный стиль триггера
    additionalClsForTrigger: null,

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'center'
    },

    getEditableField: function (budgetItemName) {
        //var costProd = this.costProd;
        //var fact = this.fact;
        //var budgetName = this.budgetName.toLowerCase();
        //var editable = fact;

        //if (costProd || (budgetName == 'marketing' && budgetItemName.toLowerCase() == 'posm'))
        //    editable = true;

        //return editable;

        // а вдруг нужно будет вернуть ручное редактирвание
        return false;
    },

    addBudgetItemField: function (budgetItemName) {
        var prefix1 = this.fact ? 'Actual' : 'Plan';
        var prefix2 = this.costProd ? 'CostProd' : '';
        var me = this;

        var trigger = Ext.create('widget.triggerfield', {
            xtype: 'triggerfield',
            name: 'budgetDet-' + prefix1 + prefix2 + budgetItemName,
            editable: false,
            fieldLabel: budgetItemName,
            trigger1Cls: 'form-info-trigger',
            cls: 'borderedField-with-lable',
            labelCls: 'borderedField-label',
            regex: /^(-?)(0|([1-9][0-9]*))(\,[0-9]+)?$/i,
            regexText: l10n.ns('tpm', 'PromoActivity').value('triggerfieldOnlyNumbers'),
            maxLength: 100,
            labelWidth: 190,
            labelSeparator: '',
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
            rawToValue: function (value) {
                var parsedValue = parseFloat(String(this.rawValue).replace(Ext.util.Format.decimalSeparator, "."))
                return isNaN(parsedValue) ? null : parsedValue;
            },
            onTrigger1Click: function () {
                    var promobudgetdetails = this.up('promobudgetdetails') || this.up('fieldset').down('promobudgetdetails'); /* || Ext.ComponentQuery.query('promobudgetdetails')[0]*/;
                var controller = App.app.getController('tpm.promo.PromoBudgetDetails');
                var promoId = promobudgetdetails.record.promoId;
                var fact = promobudgetdetails.fact;
                var editable = promobudgetdetails.getEditableField(budgetItemName);

                controller.showSubItemDetail(promoId, budgetItemName, editable, fact, promobudgetdetails);
            },
            listeners: {
                afterrender: function (me) {
                    me.setEditable(me.editable);
                }
            },

            setEditable: function (editable) {
                if (this.triggerCell && this.inputEl) {
                    if (!editable) {
                        this.triggerCell.addCls('form-info-trigger-cell');
                        this.inputEl.addCls('inputReadOnly');
                    } else {
                        this.triggerCell.removeCls('form-info-trigger-cell');
                        this.inputEl.removeCls('inputReadOnly');
                    }
                };
                if (editable != this.editable) {
                    this.editable = editable;
                    this.updateLayout();
                }
            },

        });

        if (this.needCustomPromoPanel === false) {
            trigger.addCls(this.additionalClsForTrigger);
            this.up('fieldset').add(trigger);
        } else {
            this.down('fieldset').add(trigger);
        }
    },

    items: [{
        xtype: 'fieldset',
        layout: {
            type: 'vbox',
            align: 'stretch',
            pack: 'center',
        },
        padding: '5 10 10 10',
        defaults: {
            margin: '5 0 0 0',
        },
        items: []
    },{
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
            itemId: 'addSubItem',
            text: l10n.ns('tpm', 'PromoBudgetDetails').value('addSubItem'),
            tooltip: l10n.ns('tpm', 'PromoBudgetDetails').value('addSubItem'),
            glyph: 0xf412,
            availableRoleStatusActions: {
                SupportAdministrator: App.global.Statuses.AllStatuses,
                Administrator: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                CMManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                FunctionalExpert: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                DemandPlanning: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                KeyAccountManager: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
                CustomerMarketing: App.global.Statuses.AllStatusesBeforeClosedWithoutDraft,
            }
        }]
    }]
});