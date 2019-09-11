Ext.define('App.view.core.main.Drawer', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.drawer',
    ui: 'blue-panel',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'container',
        flex: 1,

        layout: {
            type: 'vbox',
            align: 'stretch'
        },

        items: [{
            xtype: 'container',
            height: 129,
            margin: '10 20 0 20',
            items: [{
                xtype: 'image',
                cls: 'logo-image',
                src: location.origin + '/Bundles/style/images/logo.svg',
            }]

        }, {
            xtype: 'navigation',
            flex: 1
        }, {
            xtype: 'recentmenuitems',
            itemId: 'menuhistory',
            title: l10n.ns('core').value('menuHistoryTitle'),
            bodyPadding: '5 0 5 0',
            height: 162,
            hidden: true,
            animCollapse: false
        }]

    }]
});
