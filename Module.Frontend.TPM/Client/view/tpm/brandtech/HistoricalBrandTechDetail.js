Ext.define('App.view.tpm.brandtech.HistoricalBrandTechDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalbrandtechdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBrandTech', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBrandTech').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TechnologyName',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('TechnologyName'),
        },{
            xtype: 'singlelinedisplayfield',
            name: 'Technology_Description_ru',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('Technology_Description_ru'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'TechnologySubBrand',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('SubBrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTech_code',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandTech_code'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandsegTechsub_code',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('BrandsegTechsub_code'),
        }]
    }
});
