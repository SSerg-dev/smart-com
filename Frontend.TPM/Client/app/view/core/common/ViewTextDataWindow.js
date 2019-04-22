Ext.define('App.view.core.common.ViewTextDataWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.viewtextdatawindow',
    constrain: true,
    title: l10n.ns('core').value('viewTextDataWindowTitle'),
    ghost: false,
    modal: true,
    layout: 'fit',
    autoScroll: true,
    width: 600,
    height: 400,
    minWidth: 400,
    minHeight: 200,
    items: [{
        xtype: 'textareafield',
        name: 'content',
        autoScroll: true,
        readOnly: true
    }],

    defaults: {
        margin: '10 15 15 15'
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }]
});