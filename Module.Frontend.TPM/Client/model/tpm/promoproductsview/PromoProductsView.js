// TO DO
Ext.define('App.model.tpm.promoproductsview.PromoProductsView', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'PromoProductsView',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'ZREP', type: 'string', hidden: false, isDefault: true },
        { name: 'ProductEN', type: 'string', hidden: false, isDefault: true },
        { name: 'PlanProductBaselineLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductUpliftPercent', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductIncrementalLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductBaselineCaseQty', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductIncrementalCaseQty', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanProductCaseQty', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'SumInvoiceProduct',  type: 'float', hidden: false, isDefault: false, useNull: true },
        {
            name: 'AverageMarker', type: 'bool', hidden: false, isDefault: true,
            convert: function (value) {
                return (value === true || value === 'Yes') ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
            }
        },
        {
            name: 'IsCorrection', type: 'bool', hidden: false, isDefault: true,
            convert: function (value) {
                return (value === true || value === 'Yes') ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
            }
        },
    ],

    proxy: {
        type: 'breeze',
        resourceName: 'PromoProductsViews',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            promoId: null
        }
    },
});