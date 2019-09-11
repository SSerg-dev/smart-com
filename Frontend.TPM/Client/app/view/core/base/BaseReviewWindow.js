Ext.define('App.view.core.base.BaseReviewWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.basereviewwindow',
    autoScroll: true,
    cls: 'scrollable',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    defaults: {
        flex: 1,
        margin: '10 8 15 15'
    },

    width: '90%',
    height: '90%',
    minHeight: 450,
    minWidth: 600,

    title: l10n.ns('core').value('reviewWindowTitle'),

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }]
});