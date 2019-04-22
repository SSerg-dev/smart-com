Ext.define('App.view.tpm.toolbar.promosupport.PromoSupportTopToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.promosupporttoptoolbar',
    cls: 'custom-top-panel',

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    defaults: {
        ui: 'gray-button-toolbar',
        padding: '5 0 5 10',
        textAlign: 'left'
    },

    items: [{
        xtype: 'label',
        name: 'client',
        cls: 'customtoptoolbarheader',
        text: '',
        height: 27,
        width: 400
    }]
});