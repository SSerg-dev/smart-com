Ext.define('App.view.tpm.toolbar.nonpromosupport.NonPromoSupportBottomToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.nonpromosupportbottomtoolbar',
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
        itemId: 'saveNonPromoSupportForm',
        text: l10n.ns('core', 'buttons').value('save'),
        margin: '5 5 8 0',
        maxHeight: 32,
        cls: 'x-btn-white-button-footer-toolbar-small'
    }, {
        xtype: 'button',
        itemId: 'cancelNonPromoSupportForm',
        text: l10n.ns('core', 'buttons').value('cancel'),
        margin: '5 5 8 0',
        maxHeight: 32,
        cls: 'x-btn-white-button-footer-toolbar-small'
    }, {
        xtype: 'button',
        itemId: 'editNonPromoSupportEditorButton',
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