Ext.define('App.view.tpm.baseline.DeletedBaseLineDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedbaselinedetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'QTY',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('QTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Price'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BaselineLSV',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('BaselineLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Type'),
        }]
    }
})
