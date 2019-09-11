Ext.define('App.view.core.common.EditorDetailWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.editordetailwindow',
    autoScroll: true,
    cls: 'scrollable',

    width: 500,
    minWidth: 500,
    maxHeight: 500,
    resizeHandles: 'w e',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    defaults: {
        flex: 0,
        margin: '10 8 15 15'
    },

    afterWindowShow: function (scope, isCreating) {
        scope.down('field').focus(true, 10); // фокус на первом поле формы для корректной работы клавишных комбинаций
    },

    listeners: {
        show: function (window) {
            // Workaround для решения бага с прокрукой, если она должная появиться непосредственно при открытии окна
            window.doLayout();
        }
    },

    buttons: [{
        text: l10n.ns('core', 'buttons').value('close'),
        itemId: 'close'
    }, {
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'canceledit'
    }, {
        text: l10n.ns('core', 'buttons').value('edit'),
        itemId: 'edit',
        ui: 'green-button-footer-toolbar',
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        itemId: 'ok',
        ui: 'green-button-footer-toolbar',
    }]
});