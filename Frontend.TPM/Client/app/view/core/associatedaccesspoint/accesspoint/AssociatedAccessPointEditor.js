Ext.define('App.view.core.associatedaccesspoint.accesspoint.AssociatedAccessPointEditor', {
	extend: 'App.view.core.common.EditorDetailWindow',
	alias: 'widget.associatedaccesspointeditor',
	width: 500,
	minWidth: 500,
	maxHeight: 500,
	cls: 'readOnlyFields',

	items: {
		xtype: 'editorform',
		columnsCount: 1,
		items: [{
			xtype: 'textfield',
			name: 'Resource',
			fieldLabel: l10n.ns('core', 'AssociatedAccessPoint').value('Resource')
		}, {
			xtype: 'textfield',
			name: 'Action',
			fieldLabel: l10n.ns('core', 'AssociatedAccessPoint').value('Action')
		}, {
			xtype: 'textfield',
			name: 'Description',
			allowBlank: true,
			allowOnlyWhitespace: true,
			fieldLabel: l10n.ns('core', 'AssociatedAccessPoint').value('Description')
		},{ 
			xtype: 'checkboxfield', 
			allowBlank: true, 
			allowOnlyWhitespace: true,
			name: 'TPMmode',
			fieldLabel: l10n.ns('core', 'AssociatedAccessPoint').value('TPMmode')
		}]
	}
});