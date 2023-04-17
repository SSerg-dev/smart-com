Ext.define('App.model.tpm.discountrange.DiscountRange', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'DiscountRange',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'MinValue', type: 'int', hidden: false, isDefault: true },
        { name: 'MaxValue', type: 'int', hidden: false, isDefault: true }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DiscountRanges',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});
