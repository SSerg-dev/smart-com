﻿Ext.define('App.model.tpm.promo.DeletedPromo', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    breezeEntityType: 'Promo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'DeletedDate', type: 'date', isDefault: true },
        { name: 'BrandId', useNull: true, hidden: true, isDefault: false },
        { name: 'BrandTechId', useNull: true, hidden: true, isDefault: false },
        { name: 'PromoStatusId', useNull: true, hidden: true, isDefault: false },
        { name: 'MarsMechanicId', useNull: true, hidden: true, isDefault: false },
        { name: 'MarsMechanicTypeId', useNull: true, hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicId', useNull: true, hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicTypeId', useNull: true, hidden: true, isDefault: false },
        { name: 'ActualInStoreMechanicId', useNull: true, hidden: true, isDefault: false },
        { name: 'ActualInStoreMechanicTypeId', useNull: true, hidden: true, isDefault: false },
        { name: 'CreatorId', useNull: true, hidden: true, isDefault: false },
        { name: 'ClientTreeId', useNull: true, hidden: true, isDefault: false },
        //{ name: 'BaseClientTreeId', useNull: true, hidden: true, isDefault: false },
        { name: 'BaseClientTreeIds', useNull: true, hidden: true, isDefault: false },

        /***Порядок полей для расширенного фильтра***/
        // Поле из "Basic"
        { name: 'Number', type: 'int', hidden: false, isDefault: true, isKey: true },
        // Поле из "Basic"
        { name: 'ClientHierarchy', type: 'string', hidden: false, isDefault: true },
        // Поле из "Basic"
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.BrandsegTechsub', defaultFilterConfig: { valueField: 'BrandsegTechsub' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'EventName', type: 'string', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'MarsMechanicName', type: 'string', mapping: 'MarsMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'PlanInstoreMechanicName', type: 'string', mapping: 'PlanInstoreMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: true },
        // Поле из "MarsDates"
        { name: 'MarsStartDate', type: 'string', hidden: false, isDefault: true },
        // Поле из "MarsDates"
        { name: 'MarsDispatchesStart', type: 'string', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
        /********************************************/


        { name: 'ColorId', useNull: true, hidden: true, isDefault: false },
        { name: 'ProductHierarchy', type: 'string', hidden: false, isDefault: false },
        { name: 'LastChangedDate', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'LastChangedDateDemand', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'LastChangedDateFinance', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'ProductFilter', type: 'string', hidden: false, isDefault: false },
        { name: 'ProductFilterDisplay', type: 'string', hidden: false, isDefault: false },
        { name: 'MechanicComment', type: 'string', hidden: false, isDefault: false },
        { name: 'MarsMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'PlanInstoreMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'CalendarPriority', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PromoDuration', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'DispatchDuration', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: false },
        { name: 'DocumentNumber', type: 'string', hidden: false, isDefault: false },
        { name: 'IsOnInvoice', type: 'boolean', hidden: false, isDefault: true },

        // Calculation
        { name: 'PlanPromoTIShopper', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoTIMarketing', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoBranding', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoCost', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoBTL', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalLSV', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalLSV', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoPostPromoEffectLSV', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalNSV', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalNSV', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalMAC', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'NeedRecountUplift', type: 'bool', hidden: true, isDefault: false, defaultValue: true },
        { name: 'PlanPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProdXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProdCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoCostProdPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProdXSites', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProdCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProdPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoROIPercent', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'PlanPromoNetUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualInStoreDiscount', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        // Promo Closure
        { name: 'ActualPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBranding', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBTL', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCost', useNull: true, type: 'float', hidden: false, isDefault: false },       
        { name: 'ActualPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoLSVByCompensation', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPostPromoEffectLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },

        { name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: true },
        { name: 'PromoStatusSystemName', type: 'string', mapping: 'PromoStatus.SystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'MarsMechanicTypeName', type: 'string', mapping: 'MarsMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },
        { name: 'PlanInstoreMechanicTypeName', type: 'string', mapping: 'PlanInstoreMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },
        { name: 'ColorSystemName', type: 'string', mapping: 'Color.SystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'Color', useNull: true, hidden: false, isDefault: false },
        { name: 'ColorDisplayName', type: 'string', mapping: 'Color.DisplayName', defaultFilterConfig: { valueField: 'DisplayName' }, breezeEntityType: 'Color', useNull: true, hidden: false, isDefault: false },
        { name: 'ActualInStoreMechanicName', type: 'string', mapping: 'ActualInStoreMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        { name: 'ActualInStoreMechanicTypeName', type: 'string', mapping: 'ActualInStoreMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },

         //MarsDates
        { name: 'MarsStartDate', type: 'string', hidden: false, isDefault: false },
        { name: 'MarsEndDate', type: 'string', hidden: false, isDefault: false },
        { name: 'MarsDispatchesStart', type: 'string', hidden: false, isDefault: false },
        { name: 'MarsDispatchesEnd', type: 'string', hidden: false, isDefault: false },
        { name: 'Mechanic', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MechanicIA', type: 'string', useNull: true, hidden: false, isDefault: true },
        //Дата последнего вхождения в статус Approved
        { name: 'LastApprovedDate', type: 'date', hidden: true, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },

        //
        { name: 'PlanPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        // InOut
        { name: 'InOut', useNull: true, type: 'boolean', hidden: false, isDefault: false },
        { name: 'InOutProductIds', type: 'string', hidden: true, isDefault: false },
        { name: 'InOutExcludeAssortmentMatrixProductsButtonPressed', type: 'boolean', hidden: true, isDefault: false, defaultValue: false },

        // Growth Acceleration
        { name: 'IsGrowthAcceleration', useNull: true, type: 'boolean', hidden: false, isDefault: false },

        //Apollo Export
        { name: 'IsApolloExport', type: 'boolean', hidden: false, isDefault: false },

        {
            name: 'DeviationCoefficient', type: 'float', hidden: false, isDefault: true,
            convert: function (value) {
                return value * 100;
            }
        },
        { name: 'TPMmode', type: 'string', hidden: false, isDefault: true },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'DeletedPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            //Id промо для метода GetById в истории
            promoIdHistory: null,
            TPMmode: TpmModes.Prod.alias
        }
    }
});
