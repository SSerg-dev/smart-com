Ext.define('App.view.tpm.clientdashboard.AccountInformation', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.accountinformation',
    itemId: 'accountinformation',
    cls: 'client-dashboard promo-weeks-panels-container',
    layout: 'fit',

    items: [{
        xtype: 'container',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        overflowY: 'scroll',
        height: '100%',
        items: [{
            xtype: 'container',
            width: '100%',
            flex: 1,
            minHeight: 400,
            minWidth: 750,
            layout: 'fit',
            items: [{
                xtype: 'container',
                layout: 'hbox',
                height: '100%',
                items: [{
                    //левая панель
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        aling: 'stretch'
                    },
                    cls: 'panel-vertical-account-information-left',
                    height: '100%',
                    items: [{
                        //панель с кнопкой
                        flex: 6,
                        minHeight: 30,
                        xtype: 'panel',
                        layout: {
                            type: 'hbox',
                        },
                        bodyStyle: {
                            "border": "1px solid #ccc !Important",
                        },
                        cls: ['panel-account-information-left', 'client-dashboard-account-panel-button-panel'],
                        itemId: 'panelButton',
                        width: '100%',
                        items: [{
                            xtype: 'panel',
                            height: '100%',
                            width: '5%',
                            layout: {
                                type: 'vbox',
                                align: 'middle',
                                pack: 'center'
                            },
                            items: [{
                                height: '20px',
                                width: '20px',
                                padding: 0,
                                cls: ['button-account-information-client', 'button-account-information-client-glyph'],
                                xtype: 'button',
                                glyph: 0xF865,
                            }],
                            listeners: {
                                resize: function (panel, width, height, oldWidth, oldHeight, eOpts) {
                                    var me = this,
                                        button = me.items.items[0];

                                    button.setSize(width, height);
                                },
                            }
                        }, {
                            xtype: 'panel',
                            layout: {
                                type: 'hbox',
                                align: 'left'
                            },
                            width: '45%',
                            height: '100%',
                            items: [{
                                xtype: 'label',
                                text: 'Client:',
                                style: 'padding-top: 0.6ex; padding-right: 3px; padding-left: 5px',
                                cls: 'client-dashboard-account-panel-button-client-label'
                            }, {
                                xtype: 'label',
                                text: '',
                                itemId: 'accountInformationClientText',
                              //  width: '60%',
                                style: 'padding-top: 0.6ex',
                                cls: 'client-dashboard-account-panel-button-client-value'
                            }]
                        }, {
                            xtype: 'panel',
                            layout: {
                                type: 'hbox',
                                pack: 'end'
                            },
                            width: '47%',
                            height: '100%',
                            items: [{
                                xtype: 'label',
                                text: 'Year:',
                                width: '80%',
                                style: 'padding-top: 0.6ex;text-align: right; padding-right: 3px;',
                                cls: 'client-dashboard-account-panel-button-year-label'
                            }, {
                                xtype: 'label',
                                text: new Date().getFullYear(),
                                itemId: 'accountInformationYearText',
                                style: 'padding-top: 0.6ex;text-align: left;',
                                cls: 'client-dashboard-account-panel-button-year-value'
                            }]
                        }]
                    }, {
                        // панель shopper, marketing
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
                            //заголовок  shopper, marketing
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
                            // панель shopper 
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
                                itemId: 'panelShoMarkFirst',
                                items: [{
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'middle'
                                    },
                                    itemId: 'panelShoMarkFirst1',
                                    width: '100%',
                                    items: [{
                                        xtype: 'fieldset',
                                        border: false,
                                        html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Shooper TI, %</br></span>',
                                        width: '40%',
                                        cls: 'client-dashboard-account-panel-border-title'
                                    }, {
                                        itemId: 'shopperTIPlanPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'shopperTIPlanPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                    }, {
                                        itemId: 'shopperTIYTDPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'shopperTIYTDPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'shopperTIYEEPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'shopperTIYEEPercentArrow',
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
                                    itemId: 'panelShoMarkFirst2',
                                    width: '100%',
                                    items: [{
                                        xtype: 'label',
                                        text: 'Mln',
                                        cls: 'client-dashboard-account-panel-text',
                                        width: '40%',
                                    }, {
                                        itemId: 'shopperTIPlanMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'shopperTIPlanMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'shopperTIYTDMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'shopperTIYTDMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'shopperTIYEEMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'shopperTIYEEMlnArrow',
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
                            // панель  marketing
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
                                itemId: 'panelShoMarkSecond',
                                items: [{
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'middle'
                                    },
                                    itemId: 'panelShoMarkSecond1',
                                    width: '100%',
                                    items: [{
                                        xtype: 'fieldset',
                                        border: false,
                                        html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Marketing TI, %</br></span>',
                                        width: '40%',
                                        cls: 'client-dashboard-account-panel-border-title',
                                    }, {
                                        itemId: 'marketingTIPlanPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'marketingTIPlanPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'marketingTIYTDPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'marketingTIYTDPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'marketingTIYEEPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'marketingTIYEEPercentArrow',
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
                                    itemId: 'panelShoMarkSecond2',
                                    width: '100%',
                                    items: [{
                                        xtype: 'label',
                                        text: 'Mln',
                                        cls: 'client-dashboard-account-panel-text',
                                        width: '40%',
                                    }, {
                                        itemId: 'marketingTIPlanMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'marketingTIPlanMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'marketingTIYTDMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'marketingTIYTDMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'marketingTIYEEMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'marketingTIYEEMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }]
                                }]
                            }]
                        }, {
                            // панель shopper, marketing кнопочка details
                            flex: 8,
                            xtype: 'container',
                            height: '30px',
                            layout: {
                                type: 'hbox',
                                pack: 'end',
                                align: 'top'
                            },
                            width: '100%',
                            itemId: 'panelShoMarkButton',
                            items: [{
                                height: '100%',
                                cls: 'button-account-information-client',
                                width: '15%',
                                xtype: 'button',
                                itemId: 'detailsButton',
                                text: l10n.ns('tpm', 'ClientDashboard').value('Details'),
                                POSMInClientYTD: 0,
                                CatalogueYTD: 0,
                                XSitesYTD: 0,
                                CatalogueYEE: 0,
                                POSMInClientTiYEE: 0,
                                XSitesYEE: 0
                            }]
                        }]
                    }, {
                        //панель Production, branding,btl
                        xtype: 'panel',
                        flex: 47,
                        layout: {
                            type: 'vbox',
                        },
                        bodyStyle: {
                            "border": "1px solid #ccc !Important",
                        },
                        cls: 'panel-account-information-left',
                        itemId: 'panelPBB',
                        width: '100%',
                        items: [{
                            //панель Production 
                            xtype: 'container',
                            flex: 1,
                            width: '100%',
                            layout: {
                                type: 'vbox',
                                align: 'middle',
                                pack: 'center'
                            },
                            items: [{
                                xtype: 'container',
                                layout: 'vbox',
                                width: '100%',
                                itemId: 'panelPBBFirst',
                                items: [{
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'middle'
                                    },
                                    itemId: 'panelPBBFirst1',
                                    width: '100%',
                                    items: [{
                                        xtype: 'fieldset',
                                        border: false,
                                        html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Production</br></span>',
                                        width: '40%',
                                        cls: 'client-dashboard-account-panel-border-title',
                                    }, {
                                        itemId: 'productionPlanPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'productionPlanPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'productionYTDPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'productionYTDPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'productionYEEPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'productionYEEPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }]
                                }, {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        //align: 'middle'
                                    },
                                    itemId: 'panelPBBFirst2',
                                    width: '100%',
                                    items: [{
                                        xtype: 'label',
                                        text: 'Mln',
                                        cls: 'client-dashboard-account-panel-text',
                                        width: '40%',
                                    }, {
                                        itemId: 'productionPlanMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'productionPlanMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'productionYTDMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'productionYTDMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'productionYEEMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'productionYEEMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
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
                                height: '0.5pt',
                                style: 'border: none; background-color: #d1d1ee; margin-block-start: 0; margin-block-end: 0;',
                                autoEl: {
                                    tag: 'hr',
                                }
                            }]
                        }, {
                            //панель branding 
                            xtype: 'container',
                            flex: 1,
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
                                itemId: 'panelPBBSecond',
                                items: [{
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'middle'
                                    },
                                    itemId: 'panelPBBSecond1',
                                    width: '100%',
                                    items: [{
                                        xtype: 'fieldset',
                                        border: false,
                                        html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Branding</br></span>',
                                        width: '40%',
                                        cls: 'client-dashboard-account-panel-border-title',
                                    }, {
                                        itemId: 'brandingPlanPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'brandingPlanPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'brandingYTDPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'brandingYTDPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'brandingYEEPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'brandingYEEPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    },]
                                }, {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        //align: 'middle'
                                    },
                                    itemId: 'panelPBBSecond2',
                                    width: '100%',
                                    items: [{
                                        xtype: 'label',
                                        text: 'Mln',
                                        cls: 'client-dashboard-account-panel-text',
                                        width: '40%',
                                    }, {
                                        itemId: 'brandingPlanMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'brandingPlanMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'brandingYTDMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'brandingYTDMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'brandingYEEMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'brandingYEEMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
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
                                height: '0.5pt',
                                style: 'border: none; background-color: #d1d1ee; margin-block-start: 0; margin-block-end: 0;',
                                autoEl: {
                                    tag: 'hr',
                                }
                            }]
                        }, {
                            //панель btl
                            xtype: 'container',
                            flex: 1,
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
                                itemId: 'panelPBBThreth',
                                items: [{
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'middle'
                                    },
                                    itemId: 'panelPBBThreth1',
                                    width: '100%',
                                    items: [{
                                        xtype: 'fieldset',
                                        border: false,
                                        html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">BTL</br></span>',
                                        width: '40%',
                                        cls: 'client-dashboard-account-panel-border-title',
                                    }, {
                                        itemId: 'btlPlanPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'btlPlanPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'btlYTDPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'btlYTDPercentArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'btlYEEPercent',
                                        xtype: 'label',
                                        text: '0%',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        percent: true,
                                        valueField: true,
                                    }, {
                                        itemId: 'btlYEEPercentArrow',
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
                                    itemId: 'panelPBBThreth2',
                                    width: '100%',
                                    items: [{
                                        xtype: 'label',
                                        text: 'Mln',
                                        cls: 'client-dashboard-account-panel-text',
                                        width: '40%',
                                    }, {
                                        itemId: 'btlPlanMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'btlPlanMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'btlYTDMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'btlYTDMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }, {
                                        itemId: 'btlYEEMln',
                                        xtype: 'label',
                                        text: '0',
                                        width: '11%',
                                        cls: 'client-dashboard-account-panel-blue-values',
                                        style: 'text-align: center;',
                                        valueField: true,
                                    }, {
                                        itemId: 'btlYEEMlnArrow',
                                        xtype: 'label',
                                        text: '',
                                        width: '9%',
                                        style: 'text-align: left;',
                                    }]
                                }]
                            }],
                        }]
                    }]
                }, {
                    //правая панель
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                    },
                    height: '100%',
                    cls: 'panel-vertical-account-information-right',
                    items: [{
                        //панели ROI LSV
                        xtype: 'container',
                        flex: 0.8,
                        layout: {
                            type: 'hbox',
                        },
                        itemId: 'panelInformationRightFirst',

                        width: '100%',
                        items: [{
                            //панели ROI с графиком
                            xtype: 'panel',
                            flex: 1,
                            height: '100%',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: {
                                "border": "1px solid #ccc !Important",
                            },

                            width: '50%',
                            itemId: 'ROIContainer',
                            items: [{
                                flex: 1,
                                height: '100%',
                                xtype: 'panel',
                                layout: {
                                    type: 'hbox',
                                    align: 'bottom'
                                },
                                itemId: 'ROIHeader',
                                width: '100%',
                                items: [{
                                    xtype: 'fieldset',
                                    border: false,
                                    html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">ROI</br></span>',
                                    cls: 'client-dashboard-account-panel-border-title',
                                }]
                            }, {
                                flex: 4.8,
                                layout: {
                                    type: 'hbox',
                                    align: 'middle'
                                },
                                itemId: 'ROIChartPanel',
                            }, {
                                flex: 0.2,
                                layout: {
                                    type: 'hbox',
                                    align: 'middle'
                                },
                                itemId: 'ROILegendPanel',
                            }]
                        }, {
                            //панели LSV
                            xtype: 'panel',
                            flex: 1,
                            height: '100%',
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                pack: 'center',
                            },
                            bodyStyle: {
                                "border": "1px solid #ccc !Important",
                            },
                            cls: 'panel-vertical-account-information-right-second',
                            width: '100%',
                            itemId: 'panelInformationRightSecond1',
                            items: [{
                                flex: 3,
                                xtype: 'panel',
                                layout: {
                                    type: 'hbox',
                                    align: 'bottom'
                                },
                                itemId: 'panelInformationRightSecond11',
                                width: '100%',
                                items: [{
                                    xtype: 'fieldset',
                                    border: false,
                                    html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">LSV</br></span>',
                                    cls: 'client-dashboard-account-panel-border-title',
                                }]
                            }, {
                                flex: 0.9,
                                xtype: 'panel',
                                width: '100%',
                            }, {
                                flex: 4.4,
                                xtype: 'panel',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle'
                                },
                                width: '100%',
                                items: [{
                                    xtype: 'label',
                                    text: 'Plan, ',
                                    cls: 'client-dashboard-account-panel-LSV-bold-label',
                                }, {
                                    xtype: 'label',
                                    text: 'Mln',
                                    cls: 'client-dashboard-account-panel-LSV-thin-label',
                                }, {
                                    itemId: 'lsvPlanMln',
                                    xtype: 'label',
                                    text: '0',
                                    cls: 'client-dashboard-account-panel-LSV-text',
                                    valueField: true,
                                }],
                            }, {
                                flex: 4.4,
                                xtype: 'panel',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle'
                                },
                                width: '100%',
                                items: [{
                                    xtype: 'label',
                                    text: 'YTD, ',
                                    cls: 'client-dashboard-account-panel-LSV-bold-label',
                                }, {
                                    xtype: 'label',
                                    text: 'Mln',
                                    cls: 'client-dashboard-account-panel-LSV-thin-label',
                                }, {
                                    itemId: 'lsvYTDMln',
                                    xtype: 'label',
                                    text: '0',
                                    cls: 'client-dashboard-account-panel-LSV-text',
                                    valueField: true,
                                }],
                            }, {
                                flex: 4.4,
                                xtype: 'panel',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle',
                                },
                                width: '100%',
                                items: [{
                                    xtype: 'label',
                                    text: 'YEE, ',
                                    cls: 'client-dashboard-account-panel-LSV-bold-label',
                                }, {
                                    xtype: 'label',
                                    text: 'Mln',
                                    cls: 'client-dashboard-account-panel-LSV-thin-label',
                                }, {
                                    itemId: 'lsvYEEMln',
                                    xtype: 'label',
                                    text: '0',
                                    cls: 'client-dashboard-account-panel-LSV-text',
                                    valueField: true,
                                }],
                            }, {
                                flex: 0.9,
                                xtype: 'panel',
                                width: '100%',
                            }]
                        }]
                    }, {
                        //панели nsv(incremental promo)
                        xtype: 'panel',
                        flex: 1,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        itemId: 'panelInformationRightSecond',
                        bodyStyle: {
                            "border": "1px solid #ccc !Important",
                        },
                        cls: 'panel-information-right-second',
                        width: '100%',
                        items: [{
                            flex: 0.95,
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'bottom'
                            },
                            width: '100%',
                            itemId: 'panelIncPromoTitle',
                            cls: 'client-dashboard-account-panel-label',
                            items: [{
                                xtype: 'label',
                                text: '',
                                width: '34%',
                            }, {
                                itemId: 'incrementalPlanTitle',
                                xtype: 'label',
                                text: 'Plan',
                                width: '22%',
                                style: {
                                    'text-align': 'center',
                                }
                            }, {
                                itemId: 'incrementalYTDTitle',
                                xtype: 'label',
                                text: 'YTD',
                                width: '22%',
                                style: {
                                    'text-align': 'center',
                                }
                            }, {
                                itemId: 'incrementalYEETitle',
                                xtype: 'label',
                                text: 'YEE',
                                width: '22%',
                                style: {
                                    'text-align': 'center',
                                }
                            }]
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
                            flex: 4,
                            xtype: 'panel',
                            width: '100%',
                            height: '100%',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            itemId: 'panelIncPromoFirst',
                            items: [{
                                flex: 1,
                                xtype: 'container',
                                width: '100%',
                                height: 200,
                                itemId: 'panelIncPromoFirst1',
                                cls: 'incrementalHeader',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle'
                                },
                                items: [{
                                    xtype: 'fieldset',
                                    border: false,
                                    html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Incremental NSV</br></span>',
                                    cls: 'client-dashboard-account-panel-border-title',
                                }]
                            }, {
                                flex: 4,
                                xtype: 'container',
                                layout: {
                                    type: 'hbox',
                                },
                                itemId: 'panelIncPromoFirst2',
                                width: '100%',
                                items: [{
                                    itemId: 'incrementalMlnLabel',
                                    xtype: 'label',
                                    text: 'Mln',
                                    cls: 'client-dashboard-account-panel-NSV-text',
                                    width: '34%',
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
                                height: '0.5pt',
                                style: 'border: none; background-color: #d1d1ee; margin-block-start: 0; margin-block-end: 0;box-sizing: border-box;',
                                autoEl: {
                                    tag: 'hr',
                                },
                            }]
                        }, {
                            flex: 4,
                            xtype: 'panel',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            width: '100%',
                            height: '100%',
                            itemId: 'panelIncPromoSecond',
                            items: [{
                                flex: 1,
                                xtype: 'container',
                                cls: 'incrementalHeader',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle'
                                },
                                itemId: 'panelIncPromoSecond1',
                                width: '100%',
                                items: [{
                                    xtype: 'fieldset',
                                    border: false,
                                    html: '<span class="border-left-box" style="border-color: #00D7B8; padding-right: 5px"><span style="text-align: left; padding-left: 5px;">Promo NSV</br></span>',
                                    cls: 'client-dashboard-account-panel-border-title',
                                }]
                            }, {
                                flex: 4,
                                xtype: 'container',
                                layout: {
                                    type: 'hbox',
                                },
                                itemId: 'panelIncPromoSecond2',
                                width: '100%',
                                items: [{
                                    xtype: 'label',
                                    text: 'Mln',
                                    cls: 'client-dashboard-account-panel-NSV-text',
                                    width: '34%',
                                }]
                            }]
                        }]
                    }]
                }]
            }]
        }]
    }]
});