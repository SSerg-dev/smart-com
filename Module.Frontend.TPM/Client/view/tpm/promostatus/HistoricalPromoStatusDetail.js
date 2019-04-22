Ext.define('App.view.tpm.promostatus.HistoricalPromoStatusDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalpromostatusdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoStatus').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoStatus').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoStatus').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalPromoStatus', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalPromoStatus').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Name')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('SystemName')
        }, {
            xtype: 'circlecolorfield',
            name: 'Color',
            editable: false,
            hideTrigger:true,
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Color'),
        }]
    }
});
