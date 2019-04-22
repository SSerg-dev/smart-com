﻿Ext.define('App.view.tpm.commercialsubnet.DeletedCommercialSubnetDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedcommercialsubnetdetail',
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
            fieldLabel: l10n.ns('tpm', 'CommercialSubnet').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CommercialNetName',
            fieldLabel: l10n.ns('tpm', 'CommercialSubnet').value('CommercialNetName'),
        }]
    }
})