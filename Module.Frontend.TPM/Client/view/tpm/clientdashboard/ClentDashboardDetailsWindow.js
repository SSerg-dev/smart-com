Ext.define('App.view.tpm.promo.ClentDashboardDetailsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.clentdashboarddetailswindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientDetailsWindow'),
    cls: 'promo-finance-details-window',

    minWidth: 600,
    width: 700,
    minHeight: 265,
    height: 350,
    items: [{
        xtype: 'container',
        cls: 'clientDashboardDetailsWindow',
        height: '100%',
        maxHeight: '100%',
        cls: 'custom-promo-panel-container',
        layout: 'fit',
        items: [{
            flex: 47,
            xtype: 'panel',
            layout: {
                type: 'vbox',
                aling: 'stretch'
            },
            bodyStyle: {
                "border": "1px solid #ccc !Important",
            },
            cls: 'panel-account-information-left',
            itemId: 'panelShoMark',
            width: '100%',
            items: [{
                xtype: 'container',
                flex: 12,
                layout: {
                    type: 'hbox',
                    align: 'bottom'
                },
                width: '100%',
                itemId: 'panelShoMarkTitle',
                cls: 'client-dashboard-account-panel-label',
                style: 'text-align: center;',
                items: [{
                    xtype: 'label',
                    text: '',
                    width: '40%',
                }, {
                    xtype: 'label',
                    text: 'Plan',
                    width: '11%',
                }, {
                    xtype: 'label',
                    text: '',
                    width: '9%',
                }, {
                    xtype: 'label',
                    text: 'YTD',
                    width: '11%',
                }, {
                    xtype: 'label',
                    text: '',
                    width: '9%',
                }, {
                    xtype: 'label',
                    text: 'YEE',
                    width: '11%'
                }, {
                    xtype: 'label',
                    text: '',
                    width: '9%',
                }],
            }, {
                xtype: 'container',
                width: '100%',
                height: '1px',
                items: [{
                    xtype: 'box',
                    width: '96%',
                    padding: '0 0 0 2%',
                    height: '0.5pt',
                    style: 'border: none; background-color: #d1d1ee; margin-block-start: 0; margin-block-end: 0;',
                    autoEl: {
                        tag: 'hr',
                    },
                }]
            }, {
                // панель promo cost
                xtype: 'container',
                flex: 37,
                width: '100%',
                layout: {
                    type: 'vbox',
                    align: 'middle',
                    pack: 'center'
                },
                items: [{
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                    },
                    width: '100%',
                    items: [{
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'middle'
                        },
                        width: '100%',
                        items: [{
                            xtype: 'fieldset',
                            border: false,
                            html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Promo TI cost, %</br></span>',
                            width: '40%',
                            cls: 'client-dashboard-account-panel-border-title'
                        }, {
                            itemId: 'PromoTiCostPlanPercent',
                            xtype: 'label',
                            text: '0%',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            percent: true,
                            valueField: true,
                        }, {
                            itemId: 'PromoTiCostPlanPercentArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                        }, {
                            itemId: 'PromoTiCostYTDPercent',
                            xtype: 'label',
                            text: '0%',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            percent: true,
                            valueField: true,
                        }, {
                            itemId: 'PromoTiCostYTDPercentArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: left;',
                        }, {
                            itemId: 'PromoTiCostYEEPercent',
                            xtype: 'label',
                            text: '0%',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            percent: true,
                            valueField: true,
                        }, {
                            itemId: 'PromoTiCostYEEPercentArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: left;',
                        }]
                    }, {
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                        },
                        width: '100%',
                        items: [{
                            xtype: 'label',
                            text: 'Mln',
                            cls: 'client-dashboard-account-panel-text',
                            width: '40%',
                        }, {
                            itemId: 'PromoTiCostPlan',
                            xtype: 'label',
                            text: '0',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            valueField: true,
                        }, {
                            itemId: 'PromoTiCostPlanArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            style: 'text-align: left;',
                        }, {
                            itemId: 'PromoTiCostYTD',
                            xtype: 'label',
                            text: '0',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            valueField: true,
                        }, {
                            itemId: 'PromoTiCostYTDArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            style: 'text-align: left;',
                        }, {
                            itemId: 'PromoTiCostYEE',
                            xtype: 'label',
                            text: '0',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            valueField: true,
                        }, {
                            itemId: 'PromoTiCostYEEArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: left;',
                        }]
                    }]
                }]
            }, {
                xtype: 'container',
                width: '100%',
                height: '1px',
                items: [{
                    xtype: 'box',
                    width: '96%',
                    padding: '0 0 0 2%',
                    height: '1px',
                    style: 'border: none; background-color: #d1d1ee; margin-block-start: 0; margin-block-end: 0;',
                    autoEl: {
                        tag: 'hr',
                    },
                }]
            }, {
                // панель  Non-promo cost
                xtype: 'container',
                flex: 37,
                width: '100%',
                layout: {
                    type: 'vbox',
                    align: 'middle',
                    pack: 'center'
                },
                items: [{
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                    },
                    width: '100%',
                    items: [{
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'middle'
                        },
                        width: '100%',
                        items: [{
                            xtype: 'fieldset',
                            border: false,
                            html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Non promo TI cost, %</br></span>',
                            width: '40%',
                            cls: 'client-dashboard-account-panel-border-title',
                        }, {

                            itemId: 'NonPromoTiCostPlanPercent',
                            xtype: 'label',
                            text: '0%',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            percent: true,
                            valueField: true,
                        }, {
                            itemId: 'NonPromoTiCostPlanPercentArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            style: 'text-align: left;',
                        }, {
                            itemId: 'NonPromoTiCostYTDPercent',
                            xtype: 'label',
                            text: '0%',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            percent: true,
                            valueField: true,
                        }, {
                            itemId: 'NonPromoTiCostYTDPercentArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: left;',
                        }, {
                            itemId: 'NonPromoTiCostYEEPercent',
                            xtype: 'label',
                            text: '0%',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            percent: true,
                            valueField: true,
                        }, {
                            itemId: 'NonPromoTiCostYEEPercentArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            style: 'text-align: left;',
                        }]
                    }, {
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                        },
                        width: '100%',
                        items: [{
                            xtype: 'label',
                            text: 'Mln',
                            cls: 'client-dashboard-account-panel-text',
                            width: '40%',
                        }, {
                            itemId: 'NonPromoTiCostPlan',
                            xtype: 'label',
                            text: '0',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            valueField: true,
                        }, {
                            itemId: 'NonPromoTiCostPlanArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            style: 'text-align: left;',
                        }, {
                            itemId: 'NonPromoTiCostYTD',
                            xtype: 'label',
                            text: '0',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            valueField: true,
                        }, {
                            itemId: 'NonPromoTiCostYTDArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            style: 'text-align: left;',
                        }, {
                            itemId: 'NonPromoTiCostYEE',
                            xtype: 'label',
                            text: '0',
                            width: '11%',
                            cls: 'client-dashboard-account-panel-blue-values',
                            style: 'text-align: center;',
                            valueField: true,
                        }, {
                            itemId: 'NonPromoTiCostYEEArrow',
                            xtype: 'label',
                            text: '',
                            width: '9%',
                            style: 'text-align: left;',
                        }]
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