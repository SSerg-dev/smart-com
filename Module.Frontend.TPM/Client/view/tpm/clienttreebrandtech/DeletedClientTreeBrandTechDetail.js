Ext.define('App.view.tpm.clienttreebrandtech.DeletedClientTreeBrandTechDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.deletedclienttreebrandtechdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'deleteddetailform',
        items: [{
            name: 'ParentClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ParentClientTreeDemandCode'),
        }, {
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeObjectId')
        }, {
            name: 'ClientTreeName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeName')
        }, {
            text: l10n.ns('tpm', 'ClientTreeBrandTech').value('BrandTechName'),
            dataIndex: 'BrandTechName'
        }, {
            text: l10n.ns('tpm', 'ClientTreeBrandTech').value('Share'),
            dataIndex: 'Share'
        }]
    }
})