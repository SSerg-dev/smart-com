Ext.define('App.view.tpm.color.DeletedColorDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedcolordetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Color').value('SystemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DisplayName',
            fieldLabel: l10n.ns('tpm', 'Color').value('DisplayName')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandName'),
            name: 'BrandName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('TechnologyName'),
            name: 'TechnologyName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrandName'),
            name: 'SubBrandName',
        }]
    }
})