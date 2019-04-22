Ext.define('App.view.tpm.nodetype.NodeTypeEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.nodetypeeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'combobox',
            name: 'Type',
            editable: false,
            allowBlank: false,
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Type'),
            queryMode: 'local',
            valueField: 'id',
            forceSelection: true,
            store: {
                type: 'nodetypestore'
            }
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Name'),
            name: 'Name',
        }, {
            xtype: 'numberfield',
            fieldLabel: l10n.ns('tpm', 'NodeType').value('Priority'),
            name: 'Priority',
            minValue: 0,
            step: 1,
            allowDecimals: false,
            allowBlank: true,
            allowOnlyWhitespace: false
        }]
    }
});
