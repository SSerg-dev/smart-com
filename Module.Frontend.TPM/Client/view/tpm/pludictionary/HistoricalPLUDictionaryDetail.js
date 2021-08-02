Ext.define('App.view.tpm.pludictionary.HistoricalPLUDictionaryDetail', {
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
            fieldLabel: l10n.ns('tpm', 'HistoricalPLUDictionary').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
                fieldLabel: l10n.ns('tpm', 'HistoricalPLUDictionary').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                fieldLabel: l10n.ns('tpm', 'HistoricalPLUDictionary').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalAssortmentMatrix', 'OperationType'),
                fieldLabel: l10n.ns('tpm', 'HistoricalPLUDictionary').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
                fieldLabel: l10n.ns('tpm', 'PLUDictionary').value('ClientTreeName'),
            name: 'ClientTreeName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEANPC',
                fieldLabel: l10n.ns('tpm', 'PLUDictionary').value('EAN_PC')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
                fieldLabel: l10n.ns('tpm', 'PLUDictionary').value('PluCode')
        }]
    }
});
