Ext.define('App.view.tpm.brand.PromoFinanceDetailsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promofinancedetailswindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoFinanceDetailsWindow'),
    cls: 'promo-finance-details-window',

    width: "70%",
    height: 420,
    minWidth: 800,
    minHeight: 420,

    items: {
        xtype: 'container',
        maxHeight: '100%',
        cls: 'custom-promo-panel-container',
        layout: 'fit',
        items: [{
            xtype: 'custompromopanel',
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
                    title: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('Plan'),
                    layout: 'vbox',
                    padding: '5 10 10 10',
                    defaults: {
                        cls: 'borderedField-with-lable promo-finance-details-window-field',
                        labelCls: 'borderedField-label',
                        width: '100%',
                        labelWidth: 205,
                        labelSeparator: '',
                        readOnly: true,
                    },
                    items: [{
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetIncrementalLSV'),
                        name: 'PlanPromoNetIncrementalLSV',
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetLSV'),
                        name: 'PlanPromoNetLSV',
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PostPromoEffectLSV'),
                        name: 'PlanPromoPostPromoEffectLSV',
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetIncrementalNSV'),
                        name: 'PlanPromoNetIncrementalNSV',
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetNSV'),
                        name: 'PlanPromoNetNSV',
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetIncrementalEarnings'),
                        name: 'PlanPromoNetIncrementalEarnings',
                    }, {
                        xtype: 'numberfield',
                        fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetROIPercent'),
                        name: 'PlanPromoNetROIPercent',
                    }]
                }]
            }, {
                    xtype: 'container',
                    flex: 1,
                    items: [{
                        xtype: 'fieldset',
                        title: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('Actual'),
                        padding: '5 10 10 10',
                        margin: '0 18 10 0',
                        layout: 'vbox',
                        defaults: {
                            cls: 'borderedField-with-lable promo-finance-details-window-field',
                            labelCls: 'borderedField-label',
                            width: '100%',
                            labelWidth: 205,
                            labelSeparator: '',
                            readOnly: true,
                        },
                        items: [{
                            xtype: 'numberfield',
                            fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetIncrementalLSV'),
                            name: 'ActualPromoNetIncrementalLSV',
                        }, {
                            xtype: 'numberfield',
                            fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetLSV'),
                            name: 'ActualPromoNetLSV',
                        }, {
                            xtype: 'numberfield',
                            fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PostPromoEffectLSV'),
                            name: 'ActualProductPostPromoEffectLSV',
                        }, {
                            xtype: 'numberfield',
                            fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetIncrementalNSV'),
                            name: 'ActualPromoNetIncrementalNSV',
                        }, {
                            xtype: 'numberfield',
                            fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetNSV'),
                            name: 'ActualPromoNetNSV',
                        }, {
                            xtype: 'numberfield',
                            fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetIncrementalEarnings'),
                            name: 'ActualPromoNetIncrementalEarnings',
                        }, {
                            xtype: 'numberfield',
                            fieldLabel: l10n.ns('tpm', 'PromoFinanceDetailsWindow').value('PromoNetROIPercent'),
                            name: 'ActualPromoNetROIPercent',
                        }]
                    }] 
                }]
            }]
        },


    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'close'
    }]
});