Ext.define('App.view.core.filebuffer.ViewLogWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.viewfiledatawindow',
    constrain: true,
    title: l10n.ns('core', 'FileBuffer').value('FileDataTitle'),
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
        name: 'logtext',
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