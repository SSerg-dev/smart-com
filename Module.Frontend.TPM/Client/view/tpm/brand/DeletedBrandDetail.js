Ext.define('App.view.tpm.brand.DeletedBrandDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedbranddetail',
    width: 500,
    minWidth: 500,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        columnsCount: 1,
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Brand_code',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Brand_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Segmen_code',
            fieldLabel: l10n.ns('tpm', 'Brand').value('Segmen_code'),
        }]
    }
})