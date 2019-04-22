Ext.define('App.model.tpm.clienttree.ClientTree', {
    extend: 'Ext.data.Model',
    idProperty: 'ObjectId',

    fields: [
        { name: 'Id', type: 'int', hidden: true },
        { name: 'ObjectId', hidden: true },
        { name: 'depth', type: 'int', persist: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'RetailTypeName', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'FullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: true },
        { name: 'EndDate', type: 'date', hidden: true, useNull: true },
        { name: 'leaf', type: 'bool', defaultValue: false, persist: false },
        { name: 'IsBaseClient', type: 'bool', defaultValue: false },
        { name: 'ExecutionCode', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'Share', type: 'int' },
        { name: 'IsBeforeStart', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysStart', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysStart', type: 'int', hidden: false, useNull: true },
        { name: 'IsBeforeEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysEnd', type: 'int', hidden: false, useNull: true },
        { name: 'PostPromoEffectW1', type: 'float', hidden: false, useNull: true, defaultValue: 0 },
        { name: 'PostPromoEffectW2', type: 'float', hidden: false, useNull: true, defaultValue: 0 }

    ],

    proxy: {
        type: 'ajax',
        resourceName: 'ClientTrees',
        api: {
            create: '/odata/ClientTrees',
            read: '/odata/ClientTrees',
            update: '/odata/ClientTrees/UpdateNode',
            //destroy: 'destroyPersons'
        },
        extraParams: {
            filterParameter: null,
            clientObjectId: null,
            needBaseClients: false,
            dateFilter: null,
            view: false,
        }
    }
});
