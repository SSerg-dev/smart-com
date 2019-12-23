Ext.define('App.view.tpm.brand.HistoricalBrandDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalbranddetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalBrand').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalBrand').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBrand').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalBrand', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalBrand').value('_Operation')
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
});
