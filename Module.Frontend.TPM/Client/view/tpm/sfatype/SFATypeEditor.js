Ext.define('App.view.tpm.sfatype.SFATypeEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.sfatypeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',            name: 'Name',            fieldLabel: l10n.ns('tpm', 'SFAType').value('Name'),
        }]
    }
});

             
