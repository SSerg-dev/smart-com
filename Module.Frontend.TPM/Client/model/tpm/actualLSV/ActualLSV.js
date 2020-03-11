Ext.define('App.model.tpm.actualLSV.ActualLSV', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'ActualLSV',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        { name: 'ClientHierarchy', type: 'string', hidden: false, isDefault: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        {
            name: 'BrandTech',
            type: 'string',
            hidden: false,
            isDefault: true,
            useNull: true,
            convert: function (value) {
                if (value === "")
                    return null;
                return value;
            }
        },
        { name: 'Event', type: 'string', hidden: false, isDefault: true },
        { name: 'Mechanic', type: 'string', hidden: false, isDefault: true },
        { name: 'MechanicIA', type: 'string', hidden: false, isDefault: true },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'MarsStartDate', type: 'string', hidden: false, isDefault: true },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'MarsEndDate', type: 'string', hidden: false, isDefault: true },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'MarsDispatchesStart', type: 'string', hidden: false, isDefault: true },
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'MarsDispatchesEnd', type: 'string', hidden: false, isDefault: true },
        { name: 'Status', type: 'string', hidden: false, isDefault: true },
        { name: 'ActualInStoreDiscount', type: 'int', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanPromoUpliftPercent', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualPromoUpliftPercent', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanPromoBaselineLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualPromoBaselineLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanPromoIncrementalLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualPromoIncrementalLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanPromoLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualPromoLSVByCompensation', type: 'float', hidden: false, isDefault: true, useNull: true, defaultFilterConfig: getDefaultFilterNotNull() },
        { name: 'ActualPromoLSV', type: 'float', hidden: false, isDefault: true, useNull: true, useNull: true },
        { name: 'PlanPromoPostPromoEffectLSVW1', type: 'float', hidden: false, isDefault: true },
        { name: 'ActualPromoPostPromoEffectLSVW1', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanPromoPostPromoEffectLSVW2', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualPromoPostPromoEffectLSVW2', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'PlanPromoPostPromoEffectLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        { name: 'ActualPromoPostPromoEffectLSV', type: 'float', hidden: false, isDefault: true, useNull: true },
        {
            name: 'InOut',
            type: 'boolean',
            hidden: false,
            isDefault: true,
            useNull: true,
            convert: function (value) {
                return (value === true || value === 'Yes') ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
            }
        }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'ActualLSVs',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        }
    }
});

function getDefaultFilterNotNull() {
    var result = {
        value: null,
        operation: 'NotNull'
    };
    return result;
}