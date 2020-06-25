Ext.define('App.view.tpm.technology.HistoricalTechnologyDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicaltechnologydetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalTechnology').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalTechnology').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalTechnology').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalTechnology', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalTechnology').value('_Operation')
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
});
