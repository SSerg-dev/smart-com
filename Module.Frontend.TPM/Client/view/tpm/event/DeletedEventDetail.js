﻿Ext.define('App.view.tpm.event.DeletedEventDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedeventdetail',
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
            fieldLabel: l10n.ns('tpm', 'Event').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Year',
            fieldLabel: l10n.ns('tpm', 'Event').value('Year'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Period',
            fieldLabel: l10n.ns('tpm', 'Event').value('Period'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description',
            fieldLabel: l10n.ns('tpm', 'Event').value('Description'),
        }, ]
    }
})