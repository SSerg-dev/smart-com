Ext.define('App.view.core.constraint.ConstraintEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.constrainteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'combobox',
            name: 'Prefix',
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'Constraint').value('Prefix'),
            editable: false,
            queryMode: 'local',
            valueField: 'id',
            store: {
                type: 'constraintprefixstore'
            }
        }, {
            xtype: 'textfield',
            name: 'Value',
            allowBlank: false,
            fieldLabel: l10n.ns('core', 'Constraint').value('Value')
        }]
    }
});