Ext.define('App.view.core.rpasetting.RPASettingEditor', {
	extend: 'App.view.core.common.EditorDetailWindow',
	alias: 'widget.rpasettingeditor',
	width: 500,
	minWidth: 500,
	maxHeight: 500,
	cls: 'readOnlyFields',

	items: {
		xtype: 'editorform',
		columnsCount: 1,
		items: [{
			xtype: 'textfield',
			name: 'Json',
			fieldLabel: l10n.ns('core', 'RPASetting').value('Json')
		}, {
			xtype: 'textfield',
			name: 'Name',
			fieldLabel: l10n.ns('core', 'RPASetting').value('Name')
		}]
	}
});