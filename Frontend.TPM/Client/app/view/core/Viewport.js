Ext.define('App.view.core.Viewport', {
    extend: 'Ext.container.Viewport',

    layout: {
        type: 'border'
    },

    items: [{
        xtype: 'container',
        cls: 'shadow-target',
        region: 'center',

        layout: {
            type: 'border'
        },

        items: [{
            xtype: 'container',
            cls: 'toptoolbar-container',
            region: 'north',
            weight: 1,

            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            items: [{
                xtype: 'toptoolbar',
                itemId: 'header',
                height: 30
            }, {
                xtype: 'container',
                itemId: 'topcurtain',
                cls: 'topcurtain',
                height: 0
            }]
        }, {
            xtype: 'panel',
            id: 'viewcontainer',
            ui: 'marengo-panel',
            bodyCls: 'bg-logo',
            region: 'center',
            autoScroll: true,

            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            defaults: {
                flex: 1,
                margin: '10 8 20 20'
            }
        }]

    }, {
        xtype: 'drawer',
        id: 'drawer',
        region: 'west',
        weight: 2,
        width: 250,
        collapseMode: undefined,
        header: false
    }, {
        xtype: 'system',
        id: 'systempanel',
        region: 'south',
        weight: 1,
        height: 320,
        tabPosition: 'bottom',
        collapsible: true,
        collapsed: true,
        collapseMode: 'header',
        hideCollapseTool: true,
        collapseDirection: 'bottom',
        animCollapse: false,
        headerPosition: 'bottom',
        hideHeaders: true,
        header: false
    }]

});