Ext.define('App.view.core.main.RecentMenuItems', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.recentmenuitems',
    ui: 'blue-panel',
    cls: 'menu-history-panel',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    header: {
        titlePosition: 0,
        height: 40,

        items: [{
            xtype: 'expandbutton',
            ui: 'white-button',
            padding: '5 0 5 0', //TODO: временно
            glyph: 0xf063,
            glyph1: 0xf04b,
            target: function () {
                return this.up('recentmenuitems');
            }
        }]

    }

});
