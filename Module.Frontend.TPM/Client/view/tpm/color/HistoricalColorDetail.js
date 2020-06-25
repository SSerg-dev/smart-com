Ext.define('App.view.tpm.color.HistoricalColorDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalcolordetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalColor').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalColor').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalColor').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalColor', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalColor').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Color').value('SystemName'),
        }, , {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechBrandName',
            fieldLabel: l10n.ns('tpm', 'Color').value('BrandName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechTechnologyName',
            fieldLabel: l10n.ns('tpm', 'Color').value('TechnologyName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BrandTechTechnologySubBrand',
            fieldLabel: l10n.ns('tpm', 'Color').value('SubBrandName'),
        }]
    }
});