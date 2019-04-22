Ext.define('App.model.tpm.tradeinvestment.TradeInvestment', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'TradeInvestment',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'StartDate', type: 'date', hidden: false, isDefault: true },
        { name: 'EndDate', type: 'date', hidden: false, isDefault: true },
        { name: 'BrandTechId', hidden: true, useNull: true, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'ClientTreeId', hidden: true, isDefault: true },
        {
            name: 'ClientTreeFullPathName', type: 'string', mapping: 'ClientTree.FullPathName', tree: true,
            defaultFilterConfig: { valueField: 'FullPathName' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        {
            name: 'ClientTreeObjectId', type: 'int', mapping: 'ClientTree.ObjectId',
            defaultFilterConfig: { valueField: 'ObjectId' }, breezeEntityType: 'ClientTree', hidden: false, isDefault: true
        },
        { name: 'TIType', type: 'string', hidden: false, isDefault: true },
        { name: 'TISubType', type: 'string', hidden: false, isDefault: true },
        { name: 'SizePercent', type: 'int', hidden: false, isDefault: true },
        { name: 'MarcCalcROI', type: 'bool', hidden: false, isDefault: true },
        { name: 'MarcCalcBudgets', type: 'bool', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'TradeInvestments',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
