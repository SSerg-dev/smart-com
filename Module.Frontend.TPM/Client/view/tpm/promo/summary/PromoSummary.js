Ext.define('App.view.tpm.promo.PromoSummary', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promosummary',
    name: 'summary',
    bodyStyle: {
        "background-color": "#eceff1",
        "padding": "5px 5px 0px 5px",
        "border": "1px solid #ccc !Important"
    },
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    defaults: {
        xtype: 'panel',
        cls: 'promo-summary-panel',
        header: false,
        flex: 1
    },
    items: [{
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        defaults: {
            xtype: 'container',
        },
        items: [
            {
                //Левая панель
                flex: 3,
                minWidth: 537,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                cls: 'summary-leftPanel',
                items: [
                    {
                        //Контейнер с Promo information и др.
                        flex: 2,
                        name: 'promoInformationPanel',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        cls: 'summary-promoInformationPanel bigSummaryPanels',
                        items: [
                            {
                                //Promo information
                                flex: 1,
                                xtype: 'fieldset',
                                cls: 'fatheader',
                                border: false,
                                title: l10n.ns('tpm', 'PromoSummary').value('InformationTitle'),
                                items: [{
                                    xtype: 'label',
                                    name: 'promoNameLabel',
                                }]
                            },
                            {
                                //Ниже Promo information
                                flex: 2,
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                items: [
                                    {
                                        //Client status
                                        flex: 1,
                                        cls: 'borderright',
                                        layout: {
                                            type: 'vbox',
                                            align: 'stretch'
                                        },
                                        items: [
                                            {
                                                flex: 1,
                                                xtype: 'fieldset',
                                                border: false,
                                                title: l10n.ns('tpm', 'PromoSummary').value('Client'),
                                                items: [{
                                                    xtype: 'label',
                                                    name: 'clientLabel',
                                                }]
                                            },
                                            {
                                                flex: 1,
                                                xtype: 'fieldset',
                                                name: 'summaryStatusField',
                                                border: false,
                                                title: l10n.ns('tpm', 'PromoSummary').value('Status'),
                                                tpl: '<span class="border-left-box" style="border-color: {StatusColor}; padding-right: 5px"><span style="text-align: left;">{StatusName}</br></span>',
                                            }
                                        ]
                                    },
                                    {
                                        //Mechanics
                                        cls: 'borderright',
                                        layout: {
                                            type: 'vbox',
                                            align: 'stretch'
                                        },
                                        flex: 1,
                                        items: [
                                            {
                                                flex: 1,
                                                xtype: 'fieldset',
                                                border: false,
                                                title: l10n.ns('tpm', 'PromoSummary').value('MarsMechanic'),
                                                items: [{
                                                    xtype: 'label',
                                                    name: 'marsMechLabel',
                                                }]
                                            },
                                            {
                                                flex: 1,
                                                xtype: 'fieldset',
                                                border: false,
                                                title: l10n.ns('tpm', 'PromoSummary').value('InstoreMechanic'),
                                                items: [{
                                                    xtype: 'label',
                                                    name: 'instoreMechLabel',
                                                }]
                                            }
                                        ]
                                    },
                                    {
                                        //Dates
                                        flex: 1,
                                        minWidth: 210,
                                        layout: {
                                            type: 'vbox',
                                            align: 'stretch'
                                        },
                                        items: [
                                            {
                                                flex: 1,
                                                xtype: 'fieldset',
                                                border: false,
                                                title: l10n.ns('tpm', 'PromoSummary').value('DurationDate'),
                                                items: [{
                                                    xtype: 'label',
                                                    name: 'durationDateLabel',
                                                }]
                                            },
                                            {
                                                flex: 1,
                                                xtype: 'fieldset',
                                                border: false,
                                                title: l10n.ns('tpm', 'PromoSummary').value('DispatchDate'),
                                                items: [{
                                                    xtype: 'label',
                                                    name: 'dispatchDateLabel',
                                                }]
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        //Контейнер с Activity, Uplift и Budgets
                        flex: 3,
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                //Activity, Uplift
                                flex: 1,
                                cls: 'bigSummaryPanels',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                items: [
                                    {
                                        //Activity
                                        title: l10n.ns('tpm', 'PromoSummary').value('Activity'),
                                        minWidth: 159,
                                        name: 'activityFieldsetContainer',
                                        flex: 1,
                                        cls: 'borderright',
                                        header: {
                                            cls: 'header-height',
                                            top: 0,
                                            titlePosition: 0,
                                            items: [{
                                                xtype: 'button',
                                                itemId: 'activityDetailButton',
                                                windowName: 'promoactivitydetailswindow',
                                                text: l10n.ns('tpm', 'PromoSummary').value('Detail'),
                                                cls: 'summary-detail-btn'
                                            }]
                                        },
                                        layout: {
                                            type: 'vbox',
                                            align: 'stretch'
                                        },
                                        items: [{
                                            name: 'activityFieldset',
                                            //style: 'padding-left: 10px; padding-right: 20px',
                                            //layout: {
                                            //    type: 'fit',
                                            //    align: 'stretch'
                                            //},                                            
                                        }]
                                    },
                                    {
                                        //Uplift
                                        xtype: 'fieldset',
                                        hidden: true,
                                        flex: 1,
                                        name: 'UpliftFieldset',
                                        cls: 'fatheader header-height',
                                        border: false,
                                        title: l10n.ns('tpm', 'PromoSummary').value('Uplift'),
                                        layout: {
                                            type: 'vbox',
                                            align: 'stretch'
                                        },
                                        items: [
                                            {
                                                //Пустой контейнер для форматирования
                                                flex: 1
                                            },
                                            {
                                                flex: 2,
                                                xtype: 'fieldset',
                                                cls: 'summary-uplift bigSummaryText',
                                                border: false,
                                                name: 'planUpliftLabel',
                                                layout: {
                                                    type: 'vbox',
                                                    align: 'stretch'
                                                },
                                                title: '0%',
                                                labelAlign: 'top',
                                                items: [{
                                                    xtype: 'label',
                                                    style: 'font-weight: normal;',
                                                    text: l10n.ns('tpm', 'PromoSummary').value('Plan'),
                                                }]
                                            },
                                            {
                                                flex: 2,
                                                xtype: 'fieldset',
                                                cls: 'summary-uplift bigSummaryText',
                                                border: false,
                                                name: 'actualUpliftLabel',
                                                layout: {
                                                    type: 'vbox',
                                                    align: 'stretch'
                                                },
                                                title: '0%',
                                                items: [{
                                                    xtype: 'label',
                                                    style: 'font-weight: normal;',
                                                    text: l10n.ns('tpm', 'PromoSummary').value('Actual'),
                                                }]
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                //Budgets
                                cls: 'bigSummaryPanels',
                                title: l10n.ns('tpm', 'PromoSummary').value('Budgets'),
                                name: 'budgetsFieldset',
                                header: {
                                    cls: 'header-height',
                                    top: 0,
                                    titlePosition: 0,
                                    items: [{
                                        xtype: 'button',
                                        itemId: 'budgetDetailButton',
                                        windowName: 'promobudgetsdetailswindow',
                                        text: l10n.ns('tpm', 'PromoSummary').value('Detail'),
                                        cls: 'summary-detail-btn'
                                    }]
                                },
                                layout: {
                                    type: 'fit',
                                    align: 'stretch'
                                },
                                flex: 1
                            }
                        ]
                    }
                ],
            },
            {
                //Правая панель
                cls: 'bigSummaryPanels',
                flex: 2,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [
                    {
                        //Верхняя половина
                        flex: 3,
                        minHeight: 270,
                        cls: 'fatheader header-height summary-uplift summary-finance',
                        title: l10n.ns('tpm', 'PromoSummary').value('Finance'),
                        style: 'padding-bottom: 10px',
                        header: {
                            style: 'text-align: left'
                        },
                        layout: {
                            type: 'table',
                            columns: 3,
                            tableAttrs: {
                                style: {
                                    width: '100%',
                                    height: '100%',
                                    'border-collapse': 'collapse',
                                }
                            }
                        },
                        items: [
                            {
                                xtype: 'container',
                                cls: 'finance-planactual',
                                items: [{
                                    xtype: 'fieldset',
                                    cls: 'planactual-width',

                                    border: false,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [{
                                        xtype: 'label',
                                        cls: 'bigSummaryText-lineHeight',
                                        text: l10n.ns('tpm', 'PromoSummary').value('Actual'),
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText-lineHeight',
                                        text: l10n.ns('tpm', 'PromoSummary').value('Plan'),
                                    }]
                                }],
                            },
                            {
                                //Incrementals
                                xtype: 'container',
                                items: [{
                                    flex: 1,
                                    xtype: 'fieldset',
                                    name: 'IncNSVfieldset',
                                    border: false,
                                    title: l10n.ns('tpm', 'PromoSummary').value('IncrementalNSV'),
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [{
                                        xtype: 'label',
                                        cls: 'bigSummaryText',
                                        name: 'actualIncNSVLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'planIncNSVLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'percentIncNSVLabel',
                                    }]
                                }]
                            },
                            {
                                xtype: 'container',
                                items: [{
                                    flex: 1,
                                    xtype: 'fieldset',
                                    name: 'IncLSVfieldset',
                                    border: false,
                                    title: l10n.ns('tpm', 'PromoSummary').value('IncrementalLSV'),
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [{
                                        xtype: 'label',
                                        cls: 'bigSummaryText',
                                        name: 'actualIncLSVLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'planIncLSVLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'percentIncLSVLabel',
                                    }]
                                }]
                            },
                            {
                                cls: 'finance-planactual',
                                xtype: 'container',
                                items: [{
                                    xtype: 'fieldset',
                                    cls: 'planactual-width',

                                    border: false,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [{
                                        xtype: 'label',
                                        cls: 'bigSummaryText-lineHeight',
                                        text: l10n.ns('tpm', 'PromoSummary').value('Actual'),
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText-lineHeight',
                                        text: l10n.ns('tpm', 'PromoSummary').value('Plan'),
                                    }]
                                }],
                            },
                            {
                                //Promo NSV, Earnings
                                xtype: 'container',
                                items: [{
                                    flex: 1,
                                    xtype: 'fieldset',
                                    name: 'NSVfieldset',
                                    border: false,
                                    title: l10n.ns('tpm', 'PromoSummary').value('PromoNSV'),
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [{
                                        xtype: 'label',
                                        cls: 'bigSummaryText',
                                        name: 'actualPromoNSVLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'planPromoNSVLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'percentPromoNSVLabel',
                                    }]
                                }]
                            },
                            {
                                xtype: 'container',
                                items: [{
                                    flex: 1,
                                    xtype: 'fieldset',
                                    name: 'Earningsfieldset',
                                    border: false,
                                    title: l10n.ns('tpm', 'PromoSummary').value('Earnings'),
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [{
                                        xtype: 'label',
                                        cls: 'bigSummaryText',
                                        name: 'actualEarningsLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'planEarningsLabel',
                                    }, {
                                        xtype: 'label',
                                        cls: 'mediumSummaryText',
                                        name: 'percentEarningsLabel',
                                    }]
                                }]
                            }
                        ]
                    },
                    {
                        //ROI
                        name: 'roiFieldset',
                        title: l10n.ns('tpm', 'PromoSummary').value('ROI'),
                        cls: 'fatheader roichart',
                        header: {
                            cls: 'header-height',
                            top: 0,
                            titlePosition: 0,
                            titleAlign: 'center'
                        },
                        style: "padding: 5px 0px 0px 0px",
                        layout: {
                            type: 'fit',
                            align: 'stretch'
                        },
                        flex: 2
                    },
                    {
                        //Footer
                        name: 'budgetsFieldset',
                        header: {
                            top: 0,
                            titlePosition: 0,
                            style: 'margin-bottom: 5px;',
                            items: [{
                                xtype: 'button',
                                itemId: 'financeDetailButton',
                                windowName: 'promofinancedetailswindow',
                                text: l10n.ns('tpm', 'PromoSummary').value('Detail'),
                                cls: 'summary-detail-btn'
                            }]
                        },
                        layout: {
                            type: 'fit',
                            align: 'stretch'
                        },
                    }
                ]
            }
        ]
    }]
});
