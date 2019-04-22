Ext.define('App.view.tpm.toolbar.promosupport.PromoSupportLeftToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.promosupportlefttoolbar',

    minWidth: 438,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'container',
        itemId: 'buttonPromoSupportLeftToolbarContainer',
        style: {
            "background-color": "#fafafa",
            "border-bottom": "1px solid #ced3db",
            "padding": "5px 0px 5px 5px"
        }, 
        layout: {
            type: 'hbox',
            align: 'middle',
            pack: 'center'
        },
        items: [{
            xtype: 'button',
            itemId: 'createPromoSupport',
            text: l10n.ns('tpm', 'PromoSupport').value('CreatePromoSupportButton'),
            tooltip: l10n.ns('tpm', 'PromoSupport').value('CreatePromoSupportButton'), 
            glyph: 0xf416,
            cls: 'promosupport-left-toolbar-button',
            disabled: true,
            minWidth: 170,
            flex: 1,
        }, {
            xtype: 'button',
            itemId: 'createPromoSupportOnTheBasis',
            text: l10n.ns('tpm', 'PromoSupport').value('CreateOnTheBasisButton'),
            tooltip: l10n.ns('tpm', 'PromoSupport').value('CreateOnTheBasisButton'), 
            glyph: 0xf334,
            cls: 'promosupport-left-toolbar-button',
            disabled: true,
            minWidth: 170,
            flex: 1,
        }, {
            xtype: 'button',
            itemId: 'deletePromoSupport',
            text: l10n.ns('core', 'buttons').value('delete'),
            tooltip: l10n.ns('core', 'buttons').value('delete'), 
            glyph: 0xf1c0,
            cls: 'promosupport-left-toolbar-button',
            disabled: true,
            minWidth: 100,
            maxWidth: 120
        }]
    }, {
        xtype: 'container',
        itemId: 'mainPromoSupportLeftToolbarContainer',
        flex: 1,
        layout: {
        type: 'vbox',
            align: 'stretch'
        },
        cls: 'scrollpanel promo-support-left-toolbar-main-container',
        autoScroll: true,
    }, {
        xtype: 'tbspacer',
    }, {
        xtype: 'container',
        height: 53,
        itemId: 'bottomPromoSupportLeftToolbarContainer',
        style: {
            "background-color": "#eceff1",
            "border": "1px solid #ced3db",
            "padding": "5px 0px 5px 5px"
        },
        layout: {
            type: 'hbox',
            align: 'middle',
        },
        items: [{
            xtype: 'text',
            text: 'Promo support: ',
        }, {
            xtype: 'text',
            itemId: 'promoSupportCounter',
            text: '0',
            margin: '0 0 0 3'
        }]
    }]
});
