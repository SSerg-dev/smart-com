Ext.define('App.model.tpm.clienttreebrandtech.DeletedClientTreeBrandTech', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'ClientTreeBrandTech',
    fields: [
        { name: 'Id', hidden: true, isDefault: false },
        { name: 'DeletedDate', type: 'date', isDefault: true },      
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'ParentClientTreeDemandCode', type: 'string', hidden: false, isDefault: true },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, hidden: false, isDefault: true,
        },
        {
            name: 'ClientTreeName', type: 'string', mapping: 'ClientTree.Name', tree: true,
            defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'BrandTechName', type: 'string', mapping: 'BrandTech.BrandsegTechsub',
            defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true,
        },
        { name: 'Share', type: 'float', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedClientTreeBrandTeches',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
