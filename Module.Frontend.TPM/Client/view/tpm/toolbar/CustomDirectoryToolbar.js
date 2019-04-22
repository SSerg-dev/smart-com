Ext.define('App.view.tpm.toolbar.CustomDirectoryToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.customdirectorytoolbar',
    ui: 'light-gray-toolbar',
    cls: 'custom-directorygrid-toolbar',
    width: 85,
    enableOverflow: true,

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start',
        overflowHandler: 'Scroller'
    },

    items: [{
        xtype: 'button',
        itemId: 'btn_promo',
        text: l10n.ns('tpm', 'promoMainTab').value('promoBasic'),
        tooltip: l10n.ns('tpm', 'promoMainTab').value('promoBasic'),
        glyph: 0xf7d1,
        scale: 'large',
        iconAlign: 'top',
        cls: 'custom-promo-main-step-button',
        isComplete: false,
        listeners: {
            render: function (el) {
                var div = document.createElement('div');
                div.innerHTML = '<span class="custom-promo-main-step-check"></span>';
                el.getEl().appendChild(div);
            }
        }
    }, {
        xtype: 'button',
        itemId: 'btn_promoBudgets',
        text: l10n.ns('tpm', 'promoMainTab').value('promoBudgets'),
        tooltip: l10n.ns('tpm', 'promoMainTab').value('promoBudgets'),
        glyph: 0xf46f,
        scale: 'large',
        iconAlign: 'top',
        cls: 'custom-promo-main-step-button',
        isComplete: false,
        listeners: {
            render: function (el) {
                var div = document.createElement('div');
                div.innerHTML = '<span class="custom-promo-main-step-check"></span>';
                el.getEl().appendChild(div);
            }
        }
    }, {
        xtype: 'button',
        itemId: 'btn_promoActivity',
        text: l10n.ns('tpm', 'promoMainTab').value('promoActivity'),
        tooltip: l10n.ns('tpm', 'promoMainTab').value('promoActivity'),
        glyph: 0xf0ef,
        scale: 'large',
        iconAlign: 'top',
        cls: 'custom-promo-main-step-button',
        isComplete: false,
        listeners: {
            render: function (el) {
                var div = document.createElement('div');
                div.innerHTML = '<span class="custom-promo-main-step-check"></span>';
                el.getEl().appendChild(div);
            }
        }
    }, {
        xtype: 'button',
        itemId: 'btn_summary',
        text: l10n.ns('tpm', 'promoMainTab').value('summary'),
        tooltip: l10n.ns('tpm', 'promoMainTab').value('summary'),
        glyph: 0xfa1c,
        scale: 'large',
        iconAlign: 'top',
        cls: 'custom-promo-main-step-button'
    }, {
        xtype: 'tbspacer',
        flex: 1
    }, {
        xtype: 'button',
        itemId: 'btn_changes',
        text: l10n.ns('tpm', 'promoMainTab').value('changeStatusHistory'),
        tooltip: l10n.ns('tpm', 'promoMainTab').value('changeStatusHistory'),
        glyph: 0xf2da,
        padding: 2,
        scale: 'large',
        iconAlign: 'top',
        cls: 'custom-promo-main-step-button'
    }]
});