Ext.define('App.view.tpm.toolbar.promosupport.PromoSupportFormTopToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.promosupportformtoptoolbar',
    cls: 'custom-top-panel',

    style: {
        'background-color': '#829cb8',
        'cursor': 'move'
    },

    height: 20,
    padding: '0 10 0 15',
    layout: {
        type: 'hbox',
        align: 'middle',
        pack: 'end'
    },

    items: [{
        xtype: 'label',
        text: l10n.ns('tpm', 'compositePanelTitles').value('PromoSupport'),
        style: { 'color': 'white' }
    }, {
        xtype: 'tbspacer',
        flex: 10
    }, {
        xtype: 'button',
        itemId: 'close',
        glyph: 0xf156,
        cls: 'promoSupportCloseTool'
    }]
});