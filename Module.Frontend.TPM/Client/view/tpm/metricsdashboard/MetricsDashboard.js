Ext.define('App.view.tpm.metricsdashboard.MetricsDashboard', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.metricsdashboard',
    name: 'metricsdashboard',
    bodyStyle: {
        "background-color": "#cbd5e1",
    },
    listeners: {
        resize: function (panel) {
            var item = panel.down('#panel1');
            var item2 = panel.down('#panel2');
            var titlePanel = panel.down('#labelPanel');
            titlePanel.setHeight(panel.getHeight() * 0.07);

            Ext.get(titlePanel.down('#labelFirst').id).setStyle('padding-top', titlePanel.getHeight() * 0.25 + 'px');
            Ext.get(titlePanel.down('#labelSecond').id).setStyle('padding-top', titlePanel.getHeight() * 0.3 + 'px');
            Ext.get(titlePanel.down('#labelFirst').id).setStyle('font-size', titlePanel.getHeight() * 0.5 + 'px');
            Ext.get(titlePanel.down('#labelSecond').id).setStyle('font-size', titlePanel.getHeight() * 0.4 + 'px');
            item.setHeight(panel.getHeight() * 0.45);
            item2.setHeight(panel.getHeight() * 0.45);
            for (var position in item.items.items) {
                var element = item.items.items[position];
                element.down('#glyphRightPanel').setHeight(item.getHeight() * 0.35);
                element.down('#glyphRight').setHeight(item.getHeight() * 0.35 * 0.70)
                element.down('#glyphRight').setWidth(item.getHeight() * 0.35 * 0.70);
                element.down('#titleCountPanel').setHeight(item.getHeight() * 0.45);
                element.down('#buttonPanel').setHeight(item.getHeight() * 0.19);
                element.down('#buttonPanel').down('button').setHeight(item.getHeight() * 0.19);

                Ext.get(panel.down('#buttonText').id + '-btnInnerEl').setStyle('font-size', panel.down('#buttonText').getHeight() * 0.3 + 'px');

            }
            for (var position in item2.items.items) {
                var element = item2.items.items[position];
                element.down('#glyphRightPanel').setHeight(item.getHeight() * 0.35);
                element.down('#glyphRight').setHeight(item.getHeight() * 0.35 * 0.70)
                element.down('#glyphRight').setWidth(item.getHeight() * 0.35 * 0.70);
                element.down('#titleCountPanel').setHeight(item.getHeight() * 0.45);
                element.down('#buttonPanel').setHeight(item.getHeight() * 0.19);
                element.down('#buttonPanel').down('button').setHeight(item.getHeight() * 0.19);

                Ext.get(panel.down('#buttonText').id + '-btnInnerEl').setStyle('font-size', panel.down('#buttonText').getHeight() * 0.3 + 'px');

            }
        }
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
                                    text: 'Client',
                                    itemId: 'ClientMetricsId',
                                    cls: 'client-style'
                                },
                                {
                                    xtype: 'label',
                                    text: ' :Live Metrics',
                                }
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
