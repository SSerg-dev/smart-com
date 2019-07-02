Ext.define('App.view.tpm.clienttreesharesview.ClientTreeSharesViewEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.clienttreesharesvieweditor',
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
            fieldLabel: l10n.ns('tpm', 'ClientTreeSharesView').value('ResultNameStr')
        }, {
            xtype: 'textfield',
            name: 'BOI',
            fieldLabel: l10n.ns('tpm', 'ClientTreeSharesView').value('BOI')
        }, {
            xtype: 'numberfield',
            name: 'LeafShare',
            fieldLabel: l10n.ns('tpm', 'ClientTreeSharesView').value('LeafShare')
        }, {
            xtype: 'textfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'ClientTreeSharesView').value('DemandCode')
        }]
    }
});
