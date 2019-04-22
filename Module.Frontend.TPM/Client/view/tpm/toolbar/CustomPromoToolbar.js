Ext.define('App.view.tpm.toolbar.CustomPromoToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.custompromotoolbar',
    ui: 'light-gray-toolbar',
    cls: 'custom-promo-toolbar',
    width: 300,
    enableOverflow: true,

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start',
        overflowHandler: 'Scroller'
    },

    defaults: {
        ui: 'gray-button-toolbar',
        padding: '5 0 5 10',
        textAlign: 'left'
    },

    items: [{ }]
});