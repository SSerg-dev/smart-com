Ext.define('App.view.tpm.mechanic.DeletedMechanicDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedmechanicdetail',
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
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('SystemName'),
            }, {
                xtype: 'singlelinedisplayfield',
                name: 'PromoTypeName',
                fieldLabel: l10n.ns('tpm', 'Mechanic').value('PromoType.Name'),
            }]
    }
})