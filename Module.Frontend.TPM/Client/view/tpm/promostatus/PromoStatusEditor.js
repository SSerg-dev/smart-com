Ext.define('App.view.tpm.promostatus.PromoStatusEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.promostatuseditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Name')
        }, {
            xtype: 'textfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('SystemName')
        }, {
            xtype: 'circlecolorfield',
            name: 'Color',
            fieldLabel: l10n.ns('tpm', 'PromoStatus').value('Color'),
        }]
    }
});
