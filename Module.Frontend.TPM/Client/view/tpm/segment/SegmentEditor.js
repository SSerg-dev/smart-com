Ext.define('App.view.tpm.segment.SegmentEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.segmenteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Segment').value('Name'),
        }]
    }
});