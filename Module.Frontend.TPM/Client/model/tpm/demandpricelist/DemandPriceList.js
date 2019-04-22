﻿Ext.define('App.model.tpm.demandpricelist.DemandPriceList', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BaseLine',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ProductId', hidden: true, isDefault: false },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: false },
        { name: 'Price', type: 'float', hidden: false, isDefault: true },
        {
            name: 'ProductZREP', type: 'string', mapping: 'Product.ZREP', defaultFilterConfig: { valueField: 'ZREP' },
            breezeEntityType: 'Product', hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeDemandCode', type: 'string', mapping: 'ClientTree.DemandCode', tree: true,
            defaultFilterConfig: { valueField: 'DemandCode' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BaseLines',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});