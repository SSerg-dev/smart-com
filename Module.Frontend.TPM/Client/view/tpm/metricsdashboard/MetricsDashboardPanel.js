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
        flex: 1, 
    },
    
    width: '25.25%',  
    cls:'user-dashboard-panel',
    items: [
        {
            layout: {
                type: 'hbox',  
            },
            listeners: {
                resize: function (panel) {
                    var item = panel.up('panel').up('container');   
                    panel.setHeight(item.getHeight() * 0.35);
                    panel.down('#glyphRight').setHeight(panel.getHeight() * 0.70);
                    panel.down('#glyphRight').setWidth(panel.getHeight() * 0.70);  
                }
            },
            itemId:'glyphRightPanel',
            xtype: 'container',  
            minHeight: 1, 
            cls: 'title-glyph',
            items: [
                {
                    xtype: 'image',
                    itemId:'glyphRight', 
                    flex: 1, 
                    style: 'background-color: red',
                    cls:'glyph-image'
                },
            ],
        },
        {
            layout: {
                type: 'vbox',  
                //pack: "center",
                align: "stretch"
            },
            xtype: 'container', 
            listeners: {
                resize: function (panel) {
                    var item = panel.up('panel').up('container'); 
                    panel.setHeight(item.getHeight() * 0.45);
                    Ext.get(panel.down('#NameLabel').id).setStyle('font-size', (item.getHeight() * 0.04 + panel.getWidth() * 0.08) + 'px');
                    //Ext.get(panel.down('#NameLabel').id).setStyle('padding-top', item.getHeight() * 0.05 + 'px');
                    Ext.get(panel.down('#CountLabel').id).setStyle('font-size', (item.getHeight() * 0.04 + panel.getWidth() * 0.08) + 'px');
                    //Ext.get(panel.down('#CountLabel').id).setStyle('padding-top', item.getHeight() * 0.1 + 'px');
                    Ext.get(panel.down('#CountLabel_LSV').id).setStyle('font-size', (item.getHeight() * 0.02 + panel.getWidth() * 0.08) + 'px');
                    //Ext.get(panel.down('#CountLabel_LSV').id).setStyle('padding-top', item.getHeight() * 0.15 + 'px');
                      
                    if (panel.getHeight() < 80) {  
                        Ext.get(panel.down('#NameLabel').id).setStyle('font-size', (item.getHeight() * 0.5 + panel.getWidth() * 0.01) + 'px');
                        //Ext.get(panel.down('#NameLabel').id).setStyle('padding-top', (item.getHeight() * 0.05) + 'px ');
                        Ext.get(panel.down('#CountLabel').id).setStyle('font-size', (item.getHeight() * 0.5 + panel.getWidth() * 0.01) + 'px');
                        //Ext.get(panel.down('#CountLabel').id).setStyle('padding-top', (item.getHeight() * 0.1) + 'px ');
                        Ext.get(panel.down('#CountLabel_LSV').id).setStyle('font-size', (item.getHeight() * 0.03 + panel.getWidth() * 0.01) + 'px');
                        //Ext.get(panel.down('#CountLabel_LSV').id).setStyle('padding-top', (item.getHeight() * 0.15) + 'px ');

                    }
                }
            },
            width: '100%',  
            itemId: 'titleCountPanel',
            items: [{
                flex: 1,
                xtype: 'fieldset',
                border: false,
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                },
                items: [
                    {
                        xtype: 'label',
                        itemId: 'NameLabel',

                        cls: 'title-count',
                        width: '100%',
                        text: '0',
                        height: '50%',
                    },
                    {
                        xtype: 'label',
                        itemId: 'CountLabel',

                        cls: 'title-count',
                        width: '100%',
                        text: '0',
                        height: '50%',
                    },
                    {
                        xtype: 'label',
                        itemId: 'CountLabel_LSV',
                        
                        cls: 'title-count-lsv',
                        width: '100%',
                        text: '0',
                        height: '50%',
                    }
                ],
            }]
        },
        {
            layout: {
                type: 'hbox',  
            },
            
            xtype: 'container', 
            width: '100%',

           // height: '1%',
            listeners: {
                resize: function (panel) {
                    var item = panel.up('panel').up('container'); 
                    panel.setHeight(item.getHeight() * 0.19);
                    panel.down('button').setHeight(item.getHeight() * 0.19); 
                    panel.down('#buttonText').style = 'font-size:' + panel.down('#buttonText') * 0.5 + 'px !important';

                    Ext.get(panel.down('#buttonText').id + '-btnInnerEl').setStyle('font-size', panel.down('#buttonText').getHeight() * 0.3 + 'px');
                    Ext.get(panel.down('#buttonArrow').id + '-btnIconEl').setStyle('font-size', panel.down('button').getHeight() * 0.7 + 'px');
                    Ext.get(panel.down('#buttonArrow').id + '-btnIconEl').setStyle('padding-top', panel.down('button').getHeight() * 0.29 + 'px');

                    if (panel.down('button').getHeight() < 30) {
                        Ext.get(panel.down('#buttonArrow').id + '-btnIconEl').setStyle('padding-top','0px');

                    }
                }
            },
            itemId:'buttonPanel',
            items: [
                {
                    height: '100%',
                    cls: 'button-text-first',
                    width: '85%',
                    xtype: 'button',  
                    itemId:'buttonText'
                    
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
        },
       ],
    widget: null,
    filter: null
});