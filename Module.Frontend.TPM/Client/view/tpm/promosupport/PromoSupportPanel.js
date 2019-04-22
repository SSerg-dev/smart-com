Ext.define('App.view.tpm.promosupport.PromoSupportPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.promosupportpanel',
    cls: 'promosupport-left-toolbar-panel',

    deletedPromoLinkedIds: [],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    margin: '5px 5px 2px 5px',
    saved: false,

    items: [{
        xtype: 'container',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [{
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            style: {
                "border-bottom": "1px solid #ced3db",
                "padding": "5px"
            },
            items: [{
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'text',
                    text: 'Promo Support Type: ',
                    style: {
                        "font-weight": "700",
                    }
                }, {
                    xtype: 'text',
                    itemId: 'promoSupportTypeText',
                    needClear: true,
                    style: {
                        "font-weight": "700",
                    }
                }, {
                    xtype: 'tbspacer',
                    flex: 1
                }, {
                    xtype: 'text',
                    text: 'Promo linked: ',
                }, {
                    xtype: 'text',
                    itemId: 'promoLinkedText',
                    text: '0',
                    style: {
                        "font-weight": "700",
                    },
                    margin: '0 3'
                }]
            }, {
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'text',
                    text: 'Equipment Type: ',
                    style: {
                        "font-weight": "700",
                    }
                }, {
                    xtype: 'text',
                    itemId: 'equipmentTypeText',
                    needClear: true,
                    margin: '0 0 0 3',
                    style: {
                        "font-weight": "700",
                    }
                }]
            }]
        }, {
            //Period
            xtype: 'container',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            style: {
                "padding": "8px 0px 5px 5px"
            },
            items: [{
                xtype: 'text',
                text: 'Period: ',
                style: {
                    "font-weight": "700",
                    "color": "#9ea4a8"
                }
            }, {
                xtype: 'text',
                itemId: 'periodText',
                margin: '0 0 0 3',
                needClear: true
            }, {
                xtype: 'tbspacer',
                flex: 1
            }]
        }, {
            //Plan
            xtype: 'container',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            style: {
                "padding": "10px 0 0 5px"
            },
            items: [{
                xtype: 'text',
                text: 'Plan quantity: ',
                style: {
                    "font-weight": "700",
                    "color": "#798388"
                }
            }, {
                xtype: 'text',
                itemId: 'planQuantityText',
                needClear: true,
                margin: '0 0 0 3'
            }, {
                xtype: 'tbspacer',
                flex: 1
            }, {
                xtype: 'text',
                text: 'Plan Cost TE: ',
                style: {
                    "font-weight": "700",
                    "color": "#798388"
                }
            }, {
                xtype: 'text',
                itemId: 'planCostTEText',
                needClear: true,
                margin: '0 0 0 3'
            }, {
                xtype: 'tbspacer',
                flex: 1
            }]
        }, {
            //Actual
            xtype: 'container',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            style: {
                "padding": "5px 0px 10px 5px"
            },
            items: [{
                xtype: 'text',
                text: 'Actual quantity: ',
                style: {
                    "font-weight": "700",
                    "color": "#798388"
                }
            }, {
                xtype: 'text',
                itemId: 'actualQuantityText',
                needClear: true,
                margin: '0 0 0 3'
            }, {
                xtype: 'tbspacer',
                flex: 1
            }, {
                xtype: 'text',
                text: 'Actual Cost TE: ',
                style: {
                    "font-weight": "700",
                    "color": "#798388"
                }
            }, {
                xtype: 'text',
                itemId: 'actualCostTEText',
                needClear: true,
                margin: '0 0 0 3'
            }, {
                xtype: 'tbspacer',
                flex: 1
            }]
        }, {
            //Attach
            xtype: 'container',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            style: {
                "padding": "8px 0px 5px 5px"
            },
            items: [{
                xtype: 'text',
                text: 'File attached: ',
                style: {
                    "font-weight": "700",
                    "color": "#9ea4a8"
                }
            }, {
                xtype: 'text',
                itemId: 'attachFileText',
                needClear: true,
                margin: '0 0 0 3',
                maxWidth: 280,
            }, {
                xtype: 'tbspacer',
                flex: 1
            }]
        }]
    }]
})