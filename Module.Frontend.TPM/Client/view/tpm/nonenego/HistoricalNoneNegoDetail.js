Ext.define('App.view.tpm.nonenego.HistoricalNoneNegoDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicalnonenegodetail',

    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',

        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalNoneNego', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalNoneNego').value('_Operation')
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
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('FromDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ToDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ToDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('CreateDate')
        }]
    }
})