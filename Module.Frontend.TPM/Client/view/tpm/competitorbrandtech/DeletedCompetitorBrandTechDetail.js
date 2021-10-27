Ext.define('App.view.tpm.competitorbrandtech.DeletedCompetitorBrandTechDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedcompetitorbrandtechdetail',
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
            name: 'Color',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('Color'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('CompetitorName'),
            name: 'CompetitorName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('BrandTech'),
            name: 'BrandTech',
        }]
    }
})