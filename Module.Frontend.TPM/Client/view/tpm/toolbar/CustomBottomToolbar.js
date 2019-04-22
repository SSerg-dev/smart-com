Ext.define('App.view.tpm.toolbar.CustomBottomToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.custombottomtoolbar',
    cls: 'custom-bottom-panel',

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    defaults: {
        ui: 'gray-button-toolbar',
        padding: '0 0 0 0',
        textAlign: 'left'
    },

    items: [{
        xtype: 'button',
        cls: 'completedlabelbutton',
        glyph: 0xf133,
        text: '- Step is completed',
        width: 150
    }, {
        xtype: 'button',
        cls: 'notcompletedlabelbutton',
        glyph: 0xf130,
        text: '- Step is not completed',
        width: 170
    }]
});