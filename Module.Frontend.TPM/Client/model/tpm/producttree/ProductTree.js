Ext.define('App.model.tpm.producttree.ProductTree', {
    extend: 'Ext.data.Model',
    idProperty: 'ObjectId',
    fields: [
        { name: 'Id', type: 'int', hidden: true },
        { name: 'ObjectId', hidden: true },
        { name: 'BrandId', hidden: true, isDefault: true },
        { name: 'TechnologyId', hidden: true, isDefault: true },
        { name: 'depth', type: 'int', defaultValue: 0, persist: true, convert: null },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'FullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'Abbreviation', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: true },
        { name: 'EndDate', type: 'date', hidden: true, useNull: true },
        { name: 'leaf', type: 'bool', defaultValue: false, persist: false },
        { name: 'Filter', type: 'string', hidden: true },
        { name: 'Target', type: 'bool', hidden: true, persist: false },
        { name: 'NodePriority', type: 'int', persist: true, useNull: true, defaultValue: null },
        { name: 'LogoFileName', type: 'string', useNull: true, defaultValue: null }
    ],
    proxy: {
        type: 'ajax',
        resourceName: 'ProductTrees',
        api: {
            create: '/odata/ProductTrees',
            read: '/odata/ProductTrees',
            update: '/odata/ProductTrees/UpdateNode',
            //destroy: 'destroyPersons'
        },
        extraParams: {
            filterParameter: null,
            productTreeObjectIds: null,
            dateFilter: null,
            promoId: null,
            view: false,
        }
    }
});
