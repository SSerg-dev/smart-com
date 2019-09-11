Ext.define('App.view.core.associateduser.aduser.AssociatedUserEditor', {
    extend: 'App.view.core.common.EditorWindow',
    alias: 'widget.associatedusereditor',
    items: {
        xtype: 'editorform',
        items: [{ 
			xtype: 'textfield',
			name: 'Name',
            allowOnlyWhitespace: false,
			allowBlank: false,
			fieldLabel: l10n.ns('core', 'AssociatedUser').value('Name')
		}]
    }
});