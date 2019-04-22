Ext.define('App.view.tpm.actualproductsview.ActualProductList', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.actualproductlist',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ProductList'),
    autoScroll: true,
    cls: 'scrollable',

    width: '100%',
    height: '100%',

    layout: 'fit',

    defaults: {
        flex: 0,
        margin: '10 8 15 15'
    },

    items: [{
        xtype: 'actualproduct',

        //убираем тулбар справа и кнопки сверху
        dockedItems: [],
        systemHeaderItems: [],
        customHeaderItems: [],
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }]
});