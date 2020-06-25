Ext.define('App.model.tpm.promodemand.PromoDemand', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoDemand',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BrandTechId', hidden: true, isDefault: true },
        { name: 'MechanicId', hidden: true, isDefault: true },
        { name: 'MechanicTypeId', useNull: true, hidden: true, isDefault: true, defaultValue: null },
        { name: 'BaseClientObjectId', type: 'int', hidden: false, isDefault: true },
        { name: 'BrandName', type: 'string', mapping: 'BrandTech.Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'Account', type: 'string', hidden: false, isDefault: true },
        { name: 'MechanicName', type: 'string', mapping: 'Mechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', hidden: false, isDefault: true },
        { name: 'MechanicTypeName', type: 'string', mapping: 'MechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', hidden: false, isDefault: true},
        { name: 'Discount', type: 'int', hidden: false, isDefault: true, defaultValue: 0 },
        { name: 'Week', type: 'string', hidden: false, isDefault: true },
        { name: 'MarsStartDate', type: 'date', hidden: false, isDefault: true, defaultValue: new Date() },
        { name: 'MarsEndDate', type: 'date', hidden: false, isDefault: true, defaultValue: new Date() },
        { name: 'Baseline', type: 'float', hidden: false, isDefault: true, defaultValue: 0 },
        { name: 'Uplift', type: 'float', hidden: false, isDefault: true, defaultValue: 0 },
        { name: 'Incremental', type: 'float', hidden: false, isDefault: true, defaultValue: 0 },
        { name: 'Activity', type: 'float', hidden: false, isDefault: true, defaultValue: 0 },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoDemands',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
