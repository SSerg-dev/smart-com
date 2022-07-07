Ext.define('App.view.tpm.toolbar.CustomTopToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.customtoptoolbar',
    cls: 'custom-top-panel',

    layout: {
        type: 'hbox',
        align: 'stretch',
        pack: 'center'
    },

    defaults: {
        ui: 'gray-button-toolbar',
        padding: '5 5 5 5',
        margin: { right: 0 },
        margin: { left: 0 },
        textAlign: 'left'
    },

    items: [
        {
            xtype: 'label',
            name: 'promoName',
            cls: 'customtoptoolbarheader custom-top-panel-item',
            text: '',
            height: 27,
        },
        {
            xtype: 'button',
            itemId: 'btn_promoInOut',
            glyph: 0xfac3,
            cls: 'in-out-promo-header-button custom-top-panel-item',
            text: 'InOut Promo',
            height: 20,
            hidden: true,
            disabled: true
        },
        {
            xtype: 'button',
            itemId: 'btn_promoGrowthAcceleration',
            glyph: 0xfbd9,
            cls: 'in-out-promo-header-button custom-top-panel-item',
            text: 'Growth Acceleration',
            height: 20,
            hidden: true,
            disabled: true
        },
        {
            xtype: 'button',
            itemId: 'btn_promoIsInExchange',
            glyph: 0xfbdf,
            cls: 'in-out-promo-header-button custom-top-panel-item',
            text: 'InExchange',
            height: 20,
            hidden: true,
            disabled: true
        },
        {
            xtype: 'button',
            itemId: 'btn_promoOnHold',
            glyph: 0xfbd3,
            cls: 'in-out-promo-header-button custom-top-panel-item',
            text: 'On Hold',
            height: 20,
            hidden: true,
            disabled: true
        },
        {
            xtype: 'tbspacer',
            flex: 1
        },
        {
            xtype: 'button',
            itemId: 'btn_showlog',
            glyph: 0xf262,
            text: l10n.ns('tpm', 'customtoptoolbar').value('showLog'),
            cls: 'custom-additional-button custom-top-panel-item',
            hidden: false,
            disabled: true,
            //padding: '5 0 5 5',
            height: 27,
            //width: 100
        },
        // ------------------------------
        {
            xtype: 'button',
            itemId: 'btn_history',
            glyph: 0xf2da,
            text: l10n.ns('tpm', 'customtoptoolbar').value('customHistory'),
            cls: 'custom-additional-button custom-top-panel-item',
            hidden: false,
            height: 27,
            //width: 100
        }
    ]
});