Ext.define('App.view.core.role.RoleEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.roleeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
		items: [{
			xtype: 'textfield',
			name: 'SystemName',
			fieldLabel: l10n.ns('core', 'Role').value('SystemName')
		}, {
			xtype: 'textfield',
			name: 'DisplayName',
			fieldLabel: l10n.ns('core', 'Role').value('DisplayName')
		}, {
			xtype: 'booleancombobox',
			store: {
				type: 'booleannonemptystore'
			},
			name: 'IsAllow',
			fieldLabel: l10n.ns('core', 'Role').value('IsAllow')
		}]
    }
});