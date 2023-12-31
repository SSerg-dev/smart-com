﻿Ext.define('App.view.tpm.assortmentmatrix.DeletedAssortmentMatrixDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedassortmentmatrixdetail',
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
            name: 'ClientTreeName',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('ClientTreeName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductEAN_PC',
            fieldLabel: l10n.ns('tpm', 'AssortmentMatrix').value('EAN_PC')
        }]
    }
})