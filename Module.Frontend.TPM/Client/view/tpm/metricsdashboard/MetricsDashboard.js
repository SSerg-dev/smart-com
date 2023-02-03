Ext.define('App.view.tpm.metricsdashboard.MetricsDashboard', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.metricsdashboard',
    name: 'metricsdashboard',
    bodyStyle: {
        "background-color": "#cbd5e1",
    },
    layout: {
        type: 'hbox',
        align: 'stretch',
    },
    defaults: {
        xtype: 'panel',
        flex: 1,
        height: 2000,
    },
    minHeight: 500,
    minWidth: 750,
    height: '100%',
    cls: 'metrics-dashboard',
    items: [

        {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            xtype: 'panel',
            bodyStyle: {
                "background-color": "#cbd5e1",
                "padding": "5px 5px 0px 5px",
                "border": "1px solid #ccc !Important",
            },

            items: [

                {
                    layout: {
                        type: 'hbox',
                    },
                    xtype: 'container',
                    padding: '10 0 20 0',
                    itemId: 'labelPanel',
                    items: [
                        {
                            xtype: 'label',
                            text: 'Metrics Dashboard',
                            margin: '0 0 0 10',
                            cls: 'title-first',
                            itemId: 'labelFirst'
                        }, {
                            xtype: 'label',
                            text: 'Summary of your Actions',
                            margin: '0 0 0 10',
                            cls: 'title-second',
                            itemId: 'labelSecond'
                        },
                    ],
                },
                {
                    xtype: 'panel',
                    cls: 'panel-metrics',
                    itemId: 'clickPanel',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            cls: 'title-panel-first',
                            flex: 1,
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'label',
                                    text: 'Live Metrics: ',
                                },
                                {
                                    xtype: 'label',
                                    text: 'Client',
                                    itemId: 'ClientMetricsId',
                                    cls: 'client-style'
                                },                                
                            ]



                        },
                        {
                            xtype: 'container',
                            cls: 'title-panel-first',
                            flex: 1,
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'label',
                                    text: 'Period Metrics: ',
                                },
                                {
                                    xtype: 'label',
                                    text: '',
                                    itemId: 'PeriodMetricsId'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    flex: 1,
                    padding: '0 0 14 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    layout: {
                                        type: 'hbox',
                                    },
                                    xtype: 'container',
                                    itemId: 'panel1',
                                    cls: 'panel-element-first',
                                    flex: 1,
                                    items: [

                                    ],
                                },
                                {
                                    layout: {
                                        type: 'hbox',
                                    },
                                    xtype: 'container',
                                    itemId: 'panel2',
                                    cls: 'panel-element-second',                                    
                                    flex: 1,
                                    items: [

                                    ],
                                },
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    layout: {
                                        type: 'hbox',
                                    },
                                    xtype: 'container',
                                    itemId: 'panel3',
                                    cls: 'panel-element-first',
                                    flex: 1,
                                    items: [

                                    ],
                                },
                                {
                                    layout: {
                                        type: 'hbox',
                                    },
                                    xtype: 'container',
                                    itemId: 'panel4',
                                    cls: 'panel-element-second',
                                    flex: 1,
                                    items: [

                                    ],
                                },
                            ]
                        },
                    ]
                },
            ],
        },

    ],
});
