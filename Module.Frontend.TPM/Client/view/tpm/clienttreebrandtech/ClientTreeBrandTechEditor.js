Ext.define('App.view.tpm.clienttreebrandtech.ClientTreeBrandTechEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.clienttreebrandtecheditor',
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
            name: 'ParentClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ParentClientTreeDemandCode')
        }, {
            xtype: 'textfield',
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeObjectId')
        }, {
            xtype: 'textfield',
            name: 'ClientTreeName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeName')
        }, {
            xtype: 'textfield',
            name: 'CurrentBrandTechName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('CurrentBrandTechName')
        }, {
            xtype: 'textfield',
            name: 'Share',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('Share')
        }]
    }
});
