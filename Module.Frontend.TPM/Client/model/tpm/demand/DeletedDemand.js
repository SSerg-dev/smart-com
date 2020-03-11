Ext.define('App.model.tpm.demand.DeletedDemand', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Demand',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'ClientId', useNull: true, hidden: true, isDefault: true },
        { name: 'BrandId', useNull: true, hidden: true, isDefault: true },
        { name: 'BrandTechId', useNull: true, hidden: true, isDefault: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PlanBaseline', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'PlanDuration', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'PlanUplift', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'PlanIncremental', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'PlanActivity', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'PlanSteal', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'FactBaseline', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'FactDuration', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'FactUplift', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'FactIncremental', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'FactActivity', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'FactSteal', useNull: true, type: 'int', hidden: false, isDefault: true },
        { name: 'ClientCommercialSubnetCommercialNetName', type: 'string', mapping: 'Client.CommercialSubnet.CommercialNet.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'CommercialNet', hidden: false, isDefault: true },
        { name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedDemands',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
