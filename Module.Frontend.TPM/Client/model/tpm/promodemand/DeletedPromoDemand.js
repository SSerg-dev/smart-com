Ext.define('App.model.tpm.promodemand.DeletedPromoDemand', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoDemand',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'MechanicId', hidden: true, isDefault: true },
        { name: 'MechanicTypeId', hidden: true, isDefault: true },
        { name: 'BaseClientObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'Discount', type: 'int', hidden: false, isDefault: true },
        { name: 'Week', type: 'string', hidden: false, isDefault: true },
        { name: 'MarsStartDate', type: 'date', hidden: false, isDefault: false },
        { name: 'MarsEndDate', type: 'date', hidden: false, isDefault: false },
        { name: 'Baseline', type: 'float', hidden: false, isDefault: true },
        { name: 'Uplift', type: 'float', hidden: false, isDefault: true },
        { name: 'Incremental', type: 'float', hidden: false, isDefault: true },
        { name: 'Activity', type: 'float', hidden: false, isDefault: true },
        { name: 'Account', type: 'string', hidden: false, isDefault: true },
        { name: 'BrandName', type: 'string', mapping: 'BrandTech.Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'MechanicName', type: 'string', mapping: 'Mechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', hidden: false, isDefault: true },
        { name: 'MechanicTypeName', type: 'string', mapping: 'MechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedPromoDemands',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
