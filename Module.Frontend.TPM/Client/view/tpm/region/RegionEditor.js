Ext.define('App.view.tpm.region.RegionEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.regioneditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Region').value('Name')
        }]
    }
});
