Ext.define('App.model.tpm.promo.Promo', {
    extend: 'Sch.model.Event',
    mixins: ['Ext.data.Model'],
    idProperty: 'Id',
    //resourceIdField: '',
    breezeEntityType: 'Promo',
    fields: [
        { name: 'Id', hidden: true },
        { name: 'BrandId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'TechnologyId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'BrandTechId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'PromoStatusId', useNull: true, hidden: true, isDefault: false },
        { name: 'MarsMechanicId', useNull: true, hidden: true, isDefault: false },
        { name: 'MarsMechanicTypeId', useNull: true, hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicId', useNull: true, hidden: true, isDefault: false },
        { name: 'PlanInstoreMechanicTypeId', useNull: true, hidden: true, isDefault: false },
        { name: 'ActualInStoreMechanicId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ActualInStoreMechanicTypeId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ColorId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'RejectReasonId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'EventId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'CreatorId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ClientTreeId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ClientTreeKeyId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        //{ name: 'ProductTreeId', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'BaseClientTreeIds', useNull: true, hidden: true, isDefault: false, defaultValue: null },

        { name: 'PromoEventName', type: 'string', mapping: 'Event.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Event' },
        { name: 'PromoEventDescription', type: 'string', mapping: 'Event.Description', defaultFilterConfig: { valueField: 'Description' }, breezeEntityType: 'Event' },

        //Дата последнего вхождения в статус Approved
        { name: 'LastApprovedDate', type: 'date', hidden: true, isDefault: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'IsAutomaticallyApproved', type: "boolean", useNull: true, hidden: true, isDefault: false },
        { name: 'IsCMManagerApproved', type: "boolean", useNull: true, hidden: true, isDefault: false },
        { name: 'IsDemandPlanningApproved', type: "boolean", useNull: true, hidden: true, isDefault: false },
        { name: 'IsDemandFinanceApproved', type: "boolean", useNull: true, hidden: true, isDefault: false },

        /***Порядок полей для расширенного фильтра***/
        // Поле из "Basic"
        { name: 'Number', type: 'int', hidden: false, isDefault: true },
        // Поле из "Basic"
        { name: 'ClientHierarchy', type: 'string', hidden: false, isDefault: true },
        // Поле из "Basic"
        { name: 'Name', type: 'string', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'BrandTechName', type: 'string', mapping: 'BrandTech.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'BrandTech', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'EventName', type: 'string', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'Mechanic', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'MarsMechanicName', type: 'string', mapping: 'MarsMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        // Поле из "Calculation"
        { name: 'MechanicIA', type: 'string', useNull: true, hidden: false, isDefault: true },
        { name: 'PlanInstoreMechanicName', type: 'string', mapping: 'PlanInstoreMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        // Поле из "MarsDates"
        { name: 'MarsStartDate', type: 'string', hidden: false, isDefault: true },
        // Поле из "MarsDates"
        { name: 'MarsDispatchesStart', type: 'string', hidden: false, isDefault: true },
        // Поле из "Calculation"
        { name: 'PromoStatusName', type: 'string', mapping: 'PromoStatus.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'PromoStatus', hidden: false, isDefault: true },
        /********************************************/

        { name: 'ProductHierarchy', type: 'string', hidden: false, isDefault: false },
        { name: 'MechanicComment', type: 'string', hidden: false, isDefault: false },
        { name: 'MarsMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'PlanInstoreMechanicDiscount', type: 'float', hidden: false, isDefault: false },
        { name: 'LastChangedDate', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'LastChangedDateDemand', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'LastChangedDateFinance', useNull: true, type: 'date', hidden: true, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'StartDate', useNull: true, type: 'date', hidden: true, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DateStart', useNull: true, type: 'date', hidden: false, isDefault: false, mapping: 'StartDate', timeZone: +3, convert: dateConvertTimeZone },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: false, timeZone: +3, convert: dateConvertTimeZone },
        { name: 'OtherEventName', type: 'string', hidden: true, isDefault: false },
        { name: 'CalendarPriority', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'NeedRecountUplift', type: 'bool', hidden: true, isDefault: false, defaultValue: true },
        { name: 'PromoDuration', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'DispatchDuration', useNull: true, type: 'int', hidden: false, isDefault: false },
        { name: 'InvoiceNumber', type: 'string', hidden: false, isDefault: false },
        { name: 'DocumentNumber', type: 'string', hidden: false, isDefault: false },

        // Calculation
        { name: 'PlanPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBranding', useNull: false, type: 'float', hidden: false, isDefault: false, defaultValue: 0  },
        { name: 'PlanPromoCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoBTL', useNull: false, type: 'float', hidden: false, isDefault: false, defaultValue: 0 },
        { name: 'PlanPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoPostPromoEffectLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoPostPromoEffectLSVW1', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoPostPromoEffectLSVW2', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
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
        { name: 'PlanPromoNetROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualInStoreDiscount', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false },
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
        { name: 'ActualPromoBranding', useNull: false, type: 'float', hidden: false, isDefault: false, defaultValue: 0 },
        { name: 'ActualPromoBTL', useNull: false, type: 'float', hidden: false, isDefault: false, defaultValue: 0 },
        { name: 'ActualPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoCost', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: 0 },
        { name: 'ActualPromoLSVByCompensation', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPostPromoEffectLSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPostPromoEffectLSVW1', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoPostPromoEffectLSVW2', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoROIPercent', useNull: true, type: 'double', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PromoStatusSystemName', type: 'string', mapping: 'PromoStatus.SystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'PromoStatusColor', type: 'string', mapping: 'PromoStatus.Color', defaultFilterConfig: { valueField: 'Color' }, breezeEntityType: 'PromoStatus', hidden: true, isDefault: false },
        { name: 'MarsMechanicTypeName', type: 'string', mapping: 'MarsMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },
        { name: 'PlanInstoreMechanicTypeName', type: 'string', mapping: 'PlanInstoreMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },
        { name: 'BrandName', type: 'string', mapping: 'Brand.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Brand', hidden: false, isDefault: false },
        { name: 'ColorSystemName', type: 'string', mapping: 'Color.SystemName', defaultFilterConfig: { valueField: 'SystemName' }, breezeEntityType: 'Color', useNull: true, hidden: true, isDefault: false },
        { name: 'ColorDisplayName', type: 'string', mapping: 'Color.DisplayName', defaultFilterConfig: { valueField: 'DisplayName' }, breezeEntityType: 'Color', useNull: true, hidden: false, isDefault: false },
        { name: 'RejectReasonName', type: 'string', mapping: 'RejectReason.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'RejectReason', useNull: true, hidden: true, isDefault: false },
        { name: 'ActualInStoreMechanicName', type: 'string', mapping: 'ActualInStoreMechanic.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'Mechanic', useNull: true, hidden: false, isDefault: false },
        { name: 'ActualInStoreMechanicTypeName', type: 'string', mapping: 'ActualInStoreMechanicType.Name', defaultFilterConfig: { valueField: 'Name' }, breezeEntityType: 'MechanicType', useNull: true, hidden: false, isDefault: false },

        //MarsDates
        { name: 'MarsEndDate', type: 'string', hidden: false, isDefault: false },
        { name: 'MarsDispatchesEnd', type: 'string', hidden: false, isDefault: false },

        //Дублирование встроенных полей Schedule для фильтрации списка полей фильтрации
        { name: "Draggable", type: "boolean", persist: false, hidden: true, defaultValue: true },
        { name: "Resizable", persist: false, hidden: true, defaultValue: true },
        { name: "Cls", hidden: true },
        { name: "ResourceId", hidden: true },

        // Для записи выбранных продуктов (из-за множественного выбора subrange)
        { name: 'ProductTreeObjectIds', type: 'string', hidden: true, useNull: true },

        // Показывает, производится ли расчет по данному промо
        { name: 'Calculating', type: "boolean", useNull: true, hidden: true, isDefault: false },
        //
        { name: 'PlanPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'PlanPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false },
        { name: 'ActualPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false },

        // поля по клиенту
        { name: 'PromoClientName', type: 'string', mapping: 'ClientTree.Name', defaultFilterConfig: { valueField: 'Name' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientObjectId', type: 'string', mapping: 'ClientTree.ObjectId', defaultFilterConfig: { valueField: 'ObjectId' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientType', type: 'string', mapping: 'ClientTree.Type', defaultFilterConfig: { valueField: 'Type' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientGHierarchyCode', type: 'string', mapping: 'ClientTree.GHierarchyCode', defaultFilterConfig: { valueField: 'GHierarchyCode' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientDemandCode', type: 'string', mapping: 'ClientTree.DemandCode', defaultFilterConfig: { valueField: 'DemandCode' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientIsBaseClient', type: 'string', mapping: 'ClientTree.IsBaseClient', defaultFilterConfig: { valueField: 'IsBaseClient' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientRetailTypeName', type: 'string', mapping: 'ClientTree.RetailTypeName', defaultFilterConfig: { valueField: 'RetailTypeName' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientPPEW1', type: 'string', mapping: 'ClientTree.PostPromoEffectW1', defaultFilterConfig: { valueField: 'PostPromoEffectW1' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },
        { name: 'PromoClientPPEW2', type: 'string', mapping: 'ClientTree.PostPromoEffectW2', defaultFilterConfig: { valueField: 'PostPromoEffectW2' }, /*breezeEntityType: 'ClientTree',*/ hidden: false, isDefault: false },

        // привет ExtJS, Odata и Breeze за удобную работу с моделями
        // на самом деле это объект в формате JSON
        { name: 'PromoBasicProducts', type: 'string', persist: false },

        { name: 'LoadFromTLC', hidden: true },

        // InOut
        { name: 'InOut', useNull: true, type: 'boolean', hidden: false, isDefault: false },
        { name: 'InOutProductIds', type: 'string', hidden: true, isDefault: false },
        { name: 'InOutExcludeAssortmentMatrixProductsButtonPressed', type: 'boolean', hidden: true, isDefault: false, defaultValue: false },
        //Regular
        { name: 'RegularExcludedProductIds', type: 'string', hidden: true, isDefault: false },
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'Promoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            // параметр указывающий на то, нужно ли фильтровать записи по признаку возможности перевода в другой статус
            canChangeStateOnly: false
        }
    }
});
