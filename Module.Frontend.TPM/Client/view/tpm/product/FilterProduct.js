Ext.define('App.view.tpm.product.FilterProduct', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.filterproduct',
    title: 'Product filter',
    closable: false,
    tools: [{
        type: 'close',
    }],

    width: "100%",
    height: "100%",
    //width: 500,
    //height: 480,
    minWidth: 500,
    minHeight: 480,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    defaults: {
        flex: 1,
        margin: '0 8 0 20'
    },

    items: [{
        xtype: 'filterconstructor',
        flex: 1,
        height: 215,
        minHeight: 100,
        collapsed: false
    }, {
        flex: 0,
        xtype: 'splitter',
        cls: 'associated-splitter'
    }, {
        xtype: 'product',
        flex: 1,
        minHeight: 100,
        height: 220,
        collapsed: false,

        dockedItems: [],
        customHeaderItems: [],
        systemHeaderItems: [{
            xtype: 'expandbutton',
            glyph: 0xf063,
            glyph1: 0xf04b,
            itemId: 'collapse',
            tooltip: l10n.ns('core', 'selectablePanelButtons').value('collapse'),
            target: function () {
                return this.up('combineddirectorypanel');
            }
        }],
    }],

    buttons: [{
        text: 'Back',//l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'back'
    }, {
        text: 'Save',//l10n.ns('core', 'buttons').value('ok'),
        itemId: 'save'//'ok'
    }]
})