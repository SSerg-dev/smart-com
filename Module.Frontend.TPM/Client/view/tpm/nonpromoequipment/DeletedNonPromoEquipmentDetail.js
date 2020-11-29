Ext.define('App.view.tpm.nonpromoequipment.DeletedNonPromoEquipmentDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletednonpromoequipmentdetail',
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
            name: 'EquipmentType',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
        }]
    }
})