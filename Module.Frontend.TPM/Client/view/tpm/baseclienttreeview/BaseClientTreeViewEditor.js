Ext.define('App.view.tpm.baseclienttreeview.BaseClientTreeViewEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.baseclienttreevieweditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    noChange: true,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'ResultNameStr',
            fieldLabel: l10n.ns('tpm', 'BaseClientTreeView').value('ResultNameStr')
        }, {
            xtype: 'textfield',
            name: 'BOI',
            fieldLabel: l10n.ns('tpm', 'BaseClientTreeView').value('BOI')
        }]
    }
});
