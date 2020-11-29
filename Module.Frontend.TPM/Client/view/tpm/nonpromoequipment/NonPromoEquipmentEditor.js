Ext.define('App.view.tpm.nonpromoequipment.NonPromoEquipmentEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.nonpromoequipmenteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'EquipmentType',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('EquipmentType'),
        }, {
            xtype: 'textfield',
            name: 'Description_ru',
            fieldLabel: l10n.ns('tpm', 'NonPromoEquipment').value('Description_ru'),
            allowBlank: true,
            allowOnlyWhitespace: true
        }]
    }
});     