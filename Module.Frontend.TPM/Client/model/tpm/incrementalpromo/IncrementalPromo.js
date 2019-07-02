Ext.define('App.model.tpm.incrementalpromo.IncrementalPromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'IncrementalPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'PromoId', hidden: true, isDefault: false },
        {
            name: 'PromoNumber', type: 'int', mapping: 'Promo.Number', defaultFilterConfig: { valueField: 'Number' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },
        {
            name: 'PromoName', type: 'string', mapping: 'Promo.Name', defaultFilterConfig: { valueField: 'Name' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },
        {
            name: 'PromoBrandTechName', type: 'string', mapping: 'Promo.BrandTech.Name', defaultFilterConfig: { valueField: 'BrandTechName' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },
        {
            name: 'PromoStartDate', type: 'date', mapping: 'Promo.StartDate', defaultFilterConfig: { valueField: 'StartDate' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },
        {
            name: 'PromoEndDate', type: 'date', mapping: 'Promo.EndDate', defaultFilterConfig: { valueField: 'EndDate' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },
        {
            name: 'PromoDispatchesStart', type: 'date', mapping: 'Promo.DispatchesStart', defaultFilterConfig: { valueField: 'DispatchesStart' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },
        {
            name: 'PromoDispatchesEnd', type: 'date', mapping: 'Promo.DispatchesEnd', defaultFilterConfig: { valueField: 'DispatchesEnd' },
            breezeEntityType: 'Promo', hidden: false, isDefault: true
        },
        { name: 'ProductId', hidden: true, isDefault: false },
        {
            name: 'ProductZREP', type: 'string', mapping: 'Product.ZREP', defaultFilterConfig: { valueField: 'ZREP' },
            breezeEntityType: 'Product', hidden: false, isDefault: true
        },
        { name: 'IncrementalCaseAmount', type: 'int', hidden: false, isDefault: true },
        { name: 'IncrementalLSV', type: 'float', hidden: false, isDefault: true },
        { name: 'IncrementalPrice', type: 'float', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'IncrementalPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});