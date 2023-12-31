﻿Ext.define('App.model.tpm.promo.PromoView', {
    extend: 'Sch.model.Event',
    mixins: ['Ext.data.Model'],
    idProperty: 'Id',
    breezeEntityType: 'PromoView',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTechName', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        { name: 'EventName', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsMechanicName', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'MarsMechanicTypeName', type: 'string', useNull: true, hidden: false, isDefault: false },
        { name: 'MechanicComment', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'ColorSystemName', type: 'string', useNull: true, hidden: true, isDefault: false },
        { name: 'PromoStatusColor', type: 'string', hidden: true, isDefault: false },
        { name: 'IsOnInvoice', type: 'boolean', hidden: false, isDefault: true },
        { name: 'CompetitorName', type: 'string', mapping: 'CompetitorName', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Competitor', hidden: true, isDefault: true },

        { name: 'CreatorId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ClientTreeId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'BaseClientTreeIds', useNull: true, hidden: true, isDefault: false, defaultValue: null },      
        { name: 'StartDate', useNull: true, type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DateStart', useNull: true, type: 'date', hidden: false, isDefault: false, mapping: 'StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'CalendarPriority', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'PromoStatusSystemName', type: 'string', hidden: true, isDefault: true },
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatusName', defaultFilterConfig: schedulerStatusFilter(), breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },


        { name: 'CompetitorBrandTechName', type: 'string', persist: false, mapping: 'CompetitorBrandTechName', defaultFilterConfig: { valueField: 'BrandTech' }, breezeEntityType: 'CompetitorBrandTech', hidden: false, isDefault: true },
        { name: 'Price', type: 'float', hidden: false, persist: false, isDefault: true },
        { name: 'Discount', type: 'float', hidden: false, persist: false, isDefault: true },

        { name: "InOut", type: "boolean", persist: false, hidden: true, defaultValue: true },
        { name: "TypeName", type: "string", persist: false, hidden: true, defaultValue: true },
        { name: "TypeGlyph", type: "string", persist: false, hidden: true, defaultValue: true },

        //Дублирование встроенных полей Schedule для фильтрации списка полей фильтрации
        { name: "Draggable", type: "boolean", persist: false, hidden: true, defaultValue: true },
        { name: "Resizable", persist: false, hidden: true, defaultValue: true },
        { name: "Cls", hidden: true },
        { name: "ResourceId", hidden: true },

        // Growth Acceleration
        { name: 'IsGrowthAcceleration', type: 'boolean', hidden: false, isDefault: true },

        {name: 'DeviationCoefficient', type: 'float', hidden: false, isDefault: true},

        //Apollo Export
        { name: 'IsApolloExport', type: 'boolean', hidden: false, isDefault: false },
        // Is in Exchange
        { name: 'IsInExchange', type: 'boolean', hidden: false, isDefault: true },
        { name: 'MasterPromoId', useNull: true, hidden: true, isDefault: false, defaultValue: null },

        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
        { name: 'IsOnHold', type: 'boolean', hidden: false, isDefault: false },
        { name: 'BudgetYear', type: 'int', hidden: true, isDefault: true, defaultFilterConfig: schedulerYearFilter() },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'PromoViews',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            TPMmode: TpmModes.Prod.alias
        }
    }
});

function schedulerStatusFilter() {
    var result = {
        value: 'Cancelled',
        operation: 'NotEqual',
        valueField: 'Name'
    };
    return result;
}

function schedulerYearFilter() {
    var prevousyear = new Date().getFullYear() - 1;
    var result = {
        value: prevousyear,
        operation: 'GreaterOrEqual'
    };
    return result;
}