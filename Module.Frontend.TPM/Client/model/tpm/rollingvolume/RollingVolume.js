Ext.define('App.model.tpm.rollingvolume.RollingVolume', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'RollingVolume',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ProductId', useNull: true, hidden: true, isDefault: true },
        { name: 'DemandGroup', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'DMDGroup', useNull: true, type: 'string', hidden: false, isDefault: false },
        { name: 'Week', useNull: true, type: 'string', hidden: false, isDefault: true },
        { name: 'PlanProductIncrementalQTY', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'Actuals', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'OpenOrders', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'ActualOO', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'Baseline', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'ActualIncremental', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'PreliminaryRollingVolumes', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'PreviousRollingVolumes', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'PromoDifference', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'RollingVolumesCorrection', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'RollingVolumesTotal', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'ManualRollingTotalVolumes', useNull: true, type: 'float', hidden: false, isDefault: true },
        { name: 'ZREP', type: 'string',  mapping: 'Product.ZREP', isDefault: true, defaultFilterConfig: { valueField: 'ZREP' }, breezeEntityType: 'Product', hidden: false},
        { name: 'SKU', type: 'string', mapping: 'Product.ProductEN', isDefault: true, defaultFilterConfig: { valueField: 'ProductEN' }, breezeEntityType: 'Product', hidden: false},
        { name: 'BrandTech', type: 'string', mapping: 'Product.BrandsegTechsub', isDefault: true, defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false},
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'RollingVolumes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
