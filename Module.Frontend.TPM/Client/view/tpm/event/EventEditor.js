Ext.define('App.view.tpm.event.EventEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.eventeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    afterWindowShow: function (scope, isCreating) {
        scope.down('field').focus(true, 10); // фокус на первом поле формы для корректной работы клавишных комбинаций

        if (scope.down('textfield[name=Name]').rawValue == 'Standard promo') {
            scope.down('textfield[name=Name]').setReadOnly(true);
            scope.down('textfield[name=Description]').setReadOnly(true);
        }
    },

    items: {
        xtype: 'editorform',
        columnsCount: 1,

        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Event').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Description',
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'Event').value('Description')
        }]
    }
});




