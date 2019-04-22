Ext.define('App.view.tpm.toolbar.promosupport.PromoSupportBottomToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.promosupportbottomtoolbar',
    style: {
        'background-color': '#829cb8',
        'border-top-color': '#c3cbcf',
        'border-top-style': 'solid',
        'border-top-width': '1px !important'
    },

    height: 53,
    padding: '0 10 0 0',
    layout: {
        type: 'hbox',
        align: 'middle',
        pack: 'end'
    },

    items: [{
        xtype: 'button',
        itemId: 'savePromoSupportForm',
        text: l10n.ns('core', 'buttons').value('save'),
        margin: '5 5 8 0',
        maxHeight: 32,
        cls: 'x-btn-white-button-footer-toolbar-small'
    }, {
        xtype: 'button',
        itemId: 'cancelPromoSupportForm',
        text: l10n.ns('core', 'buttons').value('cancel'),
        margin: '5 5 8 0',
        maxHeight: 32,
        cls: 'x-btn-white-button-footer-toolbar-small'
    }, {
        xtype: 'button',
        itemId: 'editPromoSupportEditorButton',
        text: l10n.ns('core', 'buttons').value('edit'),
        margin: '5 5 8 0',
        maxHeight: 32,
        cls: 'x-btn-white-button-footer-toolbar-small',
        hidden: true
    }, {
        xtype: 'button',
        itemId: 'close',
        text: l10n.ns('core', 'buttons').value('close'),
        margin: '5 5 8 0',
        maxHeight: 32,
        cls: 'x-btn-white-button-footer-toolbar-small'
    }]
});