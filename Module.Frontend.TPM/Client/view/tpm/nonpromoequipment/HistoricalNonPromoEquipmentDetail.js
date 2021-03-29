Ext.define('App.view.tpm.nonpromoequipment.HistoricalNonPromoEquipmentDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalnonpromoequipmentdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoEquipment').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoEquipment').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoEquipment').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalNonPromoEquipment', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNonPromoEquipment').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EquipmentType',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetItemName',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('BudgetItemName'),
        }]
    }
});
