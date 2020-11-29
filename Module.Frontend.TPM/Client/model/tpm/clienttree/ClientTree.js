Ext.define('App.model.tpm.clienttree.ClientTree', {
    extend: 'Ext.data.Model',
    idProperty: 'ObjectId',
    breezeEntityType: 'ClientTree',
    fields: [
        { name: 'Id', type: 'int', hidden: true },
        { name: 'ObjectId', hidden: true },
        { name: 'depth', type: 'int', persist: true },
        { name: 'Type', type: 'string', hidden: false, isDefault: true },
        { name: 'RetailTypeName', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'FullPathName', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: true, useNull: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'leaf', type: 'bool', defaultValue: false, persist: false },
        { name: '_checked', type: 'bool', defaultValue: false, persist: false },
        { name: 'IsBaseClient', type: 'bool', defaultValue: false },
        { name: 'GHierarchyCode', type: 'string', hidden: false, isDefault: true },
        { name: 'DemandCode', type: 'string', hidden: false, isDefault: true },
        { name: 'IsBeforeStart', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysStart', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysStart', type: 'int', hidden: false, useNull: true },
        { name: 'IsBeforeEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'IsDaysEnd', type: 'bool', hidden: false, useNull: true },
        { name: 'DaysEnd', type: 'int', hidden: false, useNull: true },
        { name: 'PostPromoEffectW1', type: 'float', hidden: false, useNull: true, defaultValue: 0 },
        { name: 'PostPromoEffectW2', type: 'float', hidden: false, useNull: true, defaultValue: 0 },
        { name: 'LogoFileName', type: 'string', hidden: false, useNull: true, defaultValue: null },
        { name: 'IsOnInvoice', type: 'bool', useNull: true, defaultValue: null },
        { name: 'DMDGroup', type: 'string', hidden: false, isDefault: true  },
        {
            name: 'DeviationCoefficient', type: 'float', hidden: false, isDefault: true,
            convert: function (value) {
                if (value === null)
                    value = 0;

                return value * 100;
            }
        },
        { name: 'DistrMarkUp', type: 'float', hidden: false, useNull: true, defaultValue: null },
        { name: 'SFAClientCode', type: 'string', hidden: false, useNull: true, defaultValue: null },
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
            budgetSubItemId: null,
            needBaseClients: false,
            dateFilter: null,
            view: false,
        }
    }
});
