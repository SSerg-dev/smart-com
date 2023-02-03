Ext.define('App.view.tpm.metricsdashboard.MetricsDashboardPanel', {

    extend: 'Ext.panel.Panel',
    alias: 'widget.metricsdashboadpanel',
    name: 'metricsdashboadpanel',

    layout: {
        type: 'vbox',
    },
    defaults: {
        xtype: 'panel',
        header: false,
    },
    width: '50%',
    height: '100%',
    cls: 'metrics-dashboard-panel',
    items: [
        {
            layout: {
                type: 'hbox',
            },
            flex: 2,
            xtype: 'container',
            //minHeight: 1,
            width: '100%',
            //cls: 'title-glyph',
            items: [
                {
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    //flex: 1,
                    height: '100%',
                    xtype: 'container',
                    cls: 'glyph-container',
                    items: [
                        {
                            xtype: 'image',
                            shrinkWrap: 3,
                            itemId: 'glyphRight',
                            style: 'background-color: red',
                            //flex: 1,
                            cls: 'glyph-image'
                        },
                    ],
                    listeners: {
                        resize: function (panel) {

                            panel.setWidth(panel.getHeight());
                            panel.down('#glyphRight').setHeight(panel.down('#glyphRight').getWidth());
                            //panel.down('#glyphRight').setWidth(panel.getHeight());
                        }
                    },
                },
                {
                    layout: {
                        type: 'vbox',
                        align: 'stretch '
                    },
                    flex: 2,
                    height: '100%',
                    cls: 'container-metrics',
                    xtype: 'container',
                    items: [
                        {
                            xtype: 'label',
                            itemId: 'NameLabel',

                            cls: 'title-name-metrics',
                            text: '0',
                            width: '100%',
                            flex: 1,
                        },
                        {
                            xtype: 'label',
                            itemId: 'CountLabel',
                            cls: 'title-count-metrics',
                            width: '100%',
                            text: '0',
                            //height: '50%',
                            flex: 1,
                        },
                    ],
                    listeners: {
                        resize: function (panel) {
                            Ext.get(panel.down('#NameLabel').id).setStyle('font-size', (panel.getHeight() * 0.30) + 'px');
                            Ext.get(panel.down('#CountLabel').id).setStyle('font-size', (panel.getHeight() * 0.30) + 'px');

                        }
                    },
                },
            ],
        },
        {
            layout: {
                type: 'vbox',
                pack: "center",
                align: "stretch"
            },
            flex: 2,
            xtype: 'container',
            width: '100%',
            itemId: 'titleCountPanel',
            items: [{
                xtype: 'container',
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'center'
                },
                items: [
                    {
                        xtype: 'label',
                        itemId: 'CountLabel_LSV',

                        cls: 'title-count-metrics-lsv',
                        width: '100%',
                        //height: '50%',
                        text: '0',
                        flex: 1,
                        listeners: {
                            render: function (label) {
                                var view = label;
                                label.tip = Ext.create('Ext.tip.ToolTip', {
                                    target: view.el,
                                    delegate: view.itemSelector,
                                    trackMouse: true,
                                    style: 'font-size: 25px !important',
                                    renderTo: Ext.getBody(),
                                    listeners: {
                                        beforeshow: function updateTipBody(tip) {
                                            if (view.rawText == 0) {
                                                tip.update('0');
                                            }
                                            else {
                                                tip.update(view.rawText);
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }
                ],
                listeners: {
                    resize: function (panel) {
                        Ext.get(panel.down('#CountLabel_LSV').id).setStyle('font-size', (panel.getHeight() * 0.30) + 'px');
                    }
                },
            }]
        },
        {
            layout: {
                type: 'hbox',
            },
            flex: 1,
            xtype: 'container',
            width: '100%',
            itemId: 'buttonPanel',
            items: [
                {
                    height: '100%',
                    cls: 'button-text-first',
                    width: '85%',
                    xtype: 'button',
                    itemId: 'buttonText'

                },
                {
                    height: '100%',
                    itemId: 'buttonArrow',
                    cls: 'button-text-second',
                    width: '15%',
                    xtype: 'button',
                    glyph: 0xF142,
                }
            ],
            listeners: {
                resize: function (panel) {
                    var item = panel.up('panel').up('container');
                    Ext.get(panel.down('#buttonArrow').id + '-btnIconEl').setStyle('font-size', panel.down('button').getHeight() * 0.7 + 'px');
                    Ext.get(panel.down('#buttonArrow').id + '-btnIconEl').setStyle('padding-top', panel.down('button').getHeight() * 0.29 + 'px');

                }
            },
        },
    ],
    widget: null,
    filter: null
});