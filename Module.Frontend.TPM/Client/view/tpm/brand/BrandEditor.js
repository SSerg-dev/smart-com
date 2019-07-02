Ext.define('App.view.tpm.brand.BrandEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.brandeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Name'),
        }]
    }
});     