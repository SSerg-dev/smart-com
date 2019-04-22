Ext.define('App.view.tpm.nonenego.DeletedNoneNegoDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletednonenegodetail',

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
            name: 'ClientTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductTreeFullPathName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ProductTreeFullPathName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ProductTreeObjectId')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicTypeName',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicTypeName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('Discount')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'FromDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('FromDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ToDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ToDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('CreateDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }]
    }
})