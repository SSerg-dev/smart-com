Ext.define('App.view.tpm.brandtech.DeletedBrandTechDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedbrandtechdetail',
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
                name: 'BrandName',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TechnologyName',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('TechnologyName'),
            }, {
                xtype: 'textfield',
                name: 'BrandTech_code',
                fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandTech_code'),
            }]
    }
})