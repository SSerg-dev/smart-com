﻿Ext.define('App.view.tpm.sfatype.DeletedSFATypeDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedsfatypedetail',
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
            xtype: 'singlelinedisplayfield',            name: 'Name',            fieldLabel: l10n.ns('tpm', 'SFAType').value('Name'),
        }]
    }
})