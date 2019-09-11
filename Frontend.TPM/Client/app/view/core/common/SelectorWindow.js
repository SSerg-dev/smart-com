Ext.define('App.view.core.common.SelectorWindow', {
    extend: 'App.view.core.base.BaseReviewWindow',
    alias: 'widget.selectorwindow',

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'select',
        disabled: true
    }]

});