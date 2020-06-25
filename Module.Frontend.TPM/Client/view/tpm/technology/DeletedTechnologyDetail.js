Ext.define('App.view.tpm.technology.DeletedTechnologyDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedtechnologydetail',
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
            fieldLabel: l10n.ns('tpm', 'Technology').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Tech_code',
            fieldLabel: l10n.ns('tpm', 'Technology').value('Tech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubBrand',
            fieldLabel: l10n.ns('tpm', 'Technology').value('SubBrand'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SubBrand_code',
            fieldLabel: l10n.ns('tpm', 'Technology').value('SubBrand_code'),
        }]
    }
})