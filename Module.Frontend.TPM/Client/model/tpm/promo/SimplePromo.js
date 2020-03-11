Ext.define('App.model.tpm.promo.SimplePromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Promo',
    fields: [
        //{ name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true, mapping: 'Promo.Number', isKey: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true, mapping: 'Promo.Name' },
        { name: 'BrandTechName', type: 'string', mapping: 'Promo.BrandTech.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.EndDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.DispatchesStart', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: true, mapping: 'Promo.DispatchesEnd', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoStatusName', type: 'string', mapping: 'Promo.PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
    ],

    proxy: {
        resourceName: 'Promoes', // нужно для точек доступа
        type: 'memory',
        data: [],
        reader: {
            type: 'json',
        },
        writer: {
            type: 'json',
        }
    }
});