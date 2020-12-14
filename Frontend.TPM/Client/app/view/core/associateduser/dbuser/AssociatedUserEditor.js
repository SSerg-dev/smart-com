Ext.define('App.view.core.associateduser.dbuser.AssociatedUserEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.associateduserusereditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Name')
        }, {
            xtype: 'textfield',
            name: 'Email',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Email'),
            allowOnlyWhitespace: false,
            allowBlank: false
        }]
    }
});