Ext.define('App.model.tpm.btl.BTLPromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'BTLPromo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BTLId', hidden: true },
        { name: 'PromoId', hidden: true },
        { name: 'PlanPromoBTL', type: 'float', mapping: 'Promo.PlanPromoBTL', hidden: false, isDefault: true },
        { name: 'ActualPromoBTL', type: 'float', mapping: 'Promo.ActualPromoBTL', hidden: false, isDefault: true },
        { name: 'PromoNumber', type: 'int', isDefault: true, mapping: 'Promo.Number', defaultFilterConfig: { valueField: 'Number' }, isKey: true},
        { name: 'PromoName', type: 'string', isDefault: true, mapping: 'Promo.Name', defaultFilterConfig: { valueField: 'Name' } },
        { name: 'PromoEventName', type: 'string', mapping: 'Promo.Event.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Event', hidden: false, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        //{ name: 'PromoClientHierarchy', type: 'string', mapping: 'Promo.ClientHierarchy', defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true },
        { name: 'PromoStartDate', useNull: true, type: 'date', mapping: 'Promo.StartDate', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoEndDate', useNull: true, type: 'date', mapping: 'Promo.EndDate', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoBrandTechName', type: 'string', mapping: 'Promo.BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'PromoStatusName', type: 'string', mapping: 'Promo.PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
        { name: 'PromoDispatchStartDate', type: 'date', hidden: true, mapping: 'Promo.DispatchesStart', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'IsGrowthAcceleration', type: 'boolean', hidden: true, isDefault: false, mapping: 'Promo.IsGrowthAcceleration' },
        { name: 'IsInExchange', type: 'boolean', hidden: true, isDefault: false, mapping: 'Promo.IsInExchange' },

    ],
    proxy: {
        type: 'breeze',
        resourceName: 'BTLPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            TPMmode: 'Current'
        }
    }
});