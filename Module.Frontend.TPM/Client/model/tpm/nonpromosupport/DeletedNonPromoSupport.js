Ext.define('App.model.tpm.nonpromosupport.DeletedNonPromoSupport', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'NonPromoSupport',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        { name: 'NonPromoEquipmentId', hidden: true, isDefault: true },
        { name: 'PromoId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'ActualQuantity', type: 'int', hidden: false, isDefault: true },
        { name: 'PlanCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualCostTE', type: 'float', hidden: false, isDefault: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'AttachFileName', type: 'string', hidden: true, isDefault: false },
        { name: 'BorderColor', type: 'string', hidden: true, isDefault: false },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'NonPromoEquipmentEquipmentType', type: 'string', mapping: 'NonPromoEquipment.EquipmentType', defaultFilterConfig: { valueField: 'EquipmentType' },
            breezeEntityType: 'NonPromoEquipment', hidden: false, isDefault: true
        },
        {
            name: 'NonPromoEquipmentId', mapping: 'NonPromoEquipment.Id', defaultFilterConfig: { valueField: 'Id' },
            breezeEntityType: 'NonPromoEquipment', hidden: true, isDefault: false
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedNonPromoSupports',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
