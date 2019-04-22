Ext.define('App.view.tpm.promostatus.DeletedPromoStatusDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedpromostatusdetail',
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
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Name')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('SystemName')
        }, {
            xtype: 'circlecolorfield',
            name: 'Color',
            editable: false,
            hideTrigger: true,
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Color'),
        }]
    }
})