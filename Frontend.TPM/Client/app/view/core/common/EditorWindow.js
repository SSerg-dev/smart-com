Ext.define('App.view.core.common.EditorWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.editorwindow',
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
    }
});