Ext.define('App.view.tpm.toolbar.CustomTopToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.customtoptoolbar',
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
        name: 'promoName',
        cls: 'customtoptoolbarheader',
        text: '',
        height: 27,
        width: 750
    }, {
        xtype: 'tbspacer',
        flex: 1
    }, {
        xtype: 'button',
        itemId: 'btn_showlog',
        glyph: 0xf262,
        text: l10n.ns('tpm', 'customtoptoolbar').value('showLog'),
        cls: 'custom-additional-button',
        hidden: true,
        padding: '5 0 5 5',
        height: 27,
        width: 100
    },
    // ------------------------------
    {
        xtype: 'button',
        itemId: 'btn_history',
        glyph: 0xf2da,
        text: l10n.ns('tpm', 'customtoptoolbar').value('customHistory'),
        cls: 'custom-additional-button',
        hidden: false,
        height: 27,
        width: 100
    }]
});