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
        type: 'vbox',
        align: 'stretch'
    },
    defaults: {
        xtype: 'panel',
        cls: 'promo-summary-panel',
        header: { height: 15 },
        flex: 1
    },
    items: [{
        name: 'promoInformationPanel',
        title: l10n.ns('tpm', 'PromoSummary').value('InformationTitle'),
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        defaults: {
            xtype: 'container',
        },
        items: [{
            flex: 3,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'container',
                flex: 1,
                layout: {
                    type: 'vbox',
                    pack: 'start',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'label',
                    name: 'promoNameLabel',
                    cls: 'promo-name-label',
                }]
            }, {
                xtype: 'container',
                flex: 1,
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [
                    {
                        xtype: 'fieldset',
                        title: l10n.ns('tpm', 'PromoSummary').value('MarsMechanic'),
                        items: [{
                            xtype: 'label',
                            name: 'marsMechanicLabel',
                            cls: 'promo-mechanic-label',
                        }],
                        flex: 1,
                        margin: 5
                    }, {
                        xtype: 'fieldset',
                        title: l10n.ns('tpm', 'PromoSummary').value('InstoreMechanic'),
                        items: [{
                            xtype: 'label',
                            name: 'instoreMechanicLabel',
                            cls: 'promo-mechanic-label',
                        }],
                        flex: 1,
                        margin: 5
                    }, {
                        xtype: 'fieldset',
                        title: l10n.ns('tpm', 'PromoSummary').value('DurationDate'),
                        margin: 5,
                        flex: 2,
                        items: [{
                            xtype: 'label',
                            name: 'durationDatesLabel',
                            cls: 'promo-dates-label',
                        }]
                    }
                ]
            }]
        }, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'container',
                flex: 1,
                layout: {
                    type: 'fit',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoSummary').value('Client'),
                    items: [{
                        xtype: 'label',
                        name: 'hierarchyLabel',
                        tpl: '{formattedHierarchy}'
                    }],
                    margin: 5
                }]
            }, {
                xtype: 'container',
                flex: 1,
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoSummary').value('BrandTech'),
                    layout: {
                        type: 'vbox',
                        pack: 'center',
                        align: 'left'
                    },
                    items: [{
                        xtype: 'button',
                        name: 'brandTechButton',
                        itemId: 'brandTechButton',
                        windowName: 'promoproductsubrangedetailswindow',
                        cls: ['promo-mechanic-label', 'summary-detail-btn'],
                    }],
                    flex: 2,
                    margin: 5
                }, {
                    xtype: 'fieldset',
                    name: 'summaryStatusField',
                    title: l10n.ns('tpm', 'PromoSummary').value('Status'),
                    tpl: '<div class="border-left-box promo-grid-row" style="height: 18px; border-color: {StatusColor};"><span style="text-align: left;"><b>Status:</b> {StatusName}</br></div>',
                    flex: 2,
                    margin: 5
                }, {
                    xtype: 'fieldset',
                    title: l10n.ns('tpm', 'PromoSummary').value('DispatchDate'),
                    items: [{
                        xtype: 'label',
                        name: 'dispatchesDatesLabel',
                        cls: 'promo-dates-label',
                    }],
                    flex: 3,
                    margin: 5
                }]
            }],
            flex: 3
        }, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'fieldset',
                title: l10n.ns('tpm', 'PromoSummary').value('Event'),
                flex: 2,
                margin: 5,
                layout: {
                    type: 'vbox',
                    pack: 'center',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'button',
                    glyph: 0xf9d2,
                    scale: 'large',
                    name: 'promoEventNameButton',
                    iconAlign: 'top',
                    cls: 'custom-event-button',
                }]
            }],
            flex: 1
        }],
        flex: 3,
    }, {
        title: l10n.ns('tpm', 'PromoSummary').value('ActivityTitle'),
        header: {
            height: 15,
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
            type: 'hbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'fieldset',
            title: l10n.ns('tpm', 'PromoSummary').value('Plan'),
            flex: 1,
            margin: 5,
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'container',
                name: 'planActivityContainer',
                layout: {
                    type: 'fit',
                    align: 'stretch'
                },
                flex: 3
            }, {
                xtype: 'container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'label',
                    name: 'planupliftlabel',
                    tpl: '<div class="uplift-result-label">{value}</div><div class="uplift-text">Uplift</div>',
                    flex: 1
                }, {
                    xtype: 'label',
                    name: 'plannetupliftlabel',
                    tpl: '<div class="uplift-result-label">{value}</div><div class="uplift-text">Net Uplift</div>',
                    flex: 1
                }],
                flex: 1
            }]
        }, {
            xtype: 'fieldset',
            layout: 'fit',
            title: l10n.ns('tpm', 'PromoSummary').value('Fact'),
            flex: 1,
            margin: 5,
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'container',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'label',
                    name: 'factupliftlabel',
                    tpl: '<div class="uplift-result-label">{value}</div><div class="uplift-text">Uplift</div>',
                    flex: 1
                }, {
                    xtype: 'label',
                    name: 'factnetupliftlabel',
                    tpl: '<div class="uplift-result-label">{value}</div><div class="uplift-text">Net Uplift</div>',
                    flex: 1
                }],
                flex: 1
            }, {
                xtype: 'container',
                name: 'factActivityContainer',
                layout: {
                    type: 'fit',
                    align: 'stretch'
                },
                flex: 3
            }]
        }],
        flex: 3
    }, {
        title: l10n.ns('tpm', 'PromoSummary').value('BudgetsTitle'),
        name: 'budgetsFieldset',
        header: {
            height: 15,
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
        flex: 3
    }, {
        title: l10n.ns('tpm', 'PromoSummary').value('FinanceTitle'),
        header: {
            height: 15,
            top: 0,
            titlePosition: 0,
            items: [{
                xtype: 'button',
                itemId: 'financeDetailButton',
                windowName: 'promofinancedetailswindow',
                text: l10n.ns('tpm', 'PromoSummary').value('Detail'),
                cls: 'summary-detail-btn'
            }]
        },
        layout: {
            type: 'hbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'fieldset',
            name: 'financeFieldset',
            title: l10n.ns('tpm', 'PromoSummary').value('FinanceTitle'),
            margin: 5,
            layout: {
                type: 'fit',
                align: 'stretch'
            },
            flex: 4
        }, {
            xtype: 'fieldset',
            title: l10n.ns('tpm', 'PromoSummary').value('Plan'),
            margin: 5,
            flex: 1,
            items: [{
                xtype: 'label',
                name: 'planroilabel',
                tpl: '<div class="roi-result-label">{value}</div><div class="roi-text">Net ROI</div>',
                flex: 1
            }]
        }, {
            xtype: 'fieldset',
            title: l10n.ns('tpm', 'PromoSummary').value('Fact'),
            margin: 5,
            flex: 1,
            items: [{
                xtype: 'label',
                name: 'factroilabel',
                tpl: '<div class="roi-result-label">{value}</div><div class="roi-text">Net ROI</div>'
            }]
        }],
        flex: 3
    }]
});
