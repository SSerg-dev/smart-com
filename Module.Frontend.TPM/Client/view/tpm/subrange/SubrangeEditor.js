Ext.define('App.view.tpm.subrange.SubrangeEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.subrangeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Subrange').value('Name'),
        }]
    }
});