Ext.define('App.model.tpm.promo.HistoricalPromo', {
    extend: 'Ext.data.Model',
    idProperty: '_Id',
    breezeEntityType: 'Promo',
    fields: [
        { name: '_Id', type: 'string', hidden: true },
        { name: '_ObjectId', hidden: true },
        { name: '_User', type: 'string', isDefault: true },
        { name: '_Role', type: 'string', isDefault: true },
        { name: '_EditDate', type: 'date', isDefault: true, timeZone: +3, convert: dateConvertTimeZone},
        { name: '_Operation', type: 'string', isDefault: true },

        { name: 'PromoEventName', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        { name: 'PromoEventDescription', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },

        { name: 'CreatorLogin', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },

        { name: 'IsAutomaticallyApproved', type: "boolean", useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'IsCMManagerApproved', type: "boolean", useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'IsDemandPlanningApproved', type: "boolean", useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'IsDemandFinanceApproved', type: "boolean", useNull: true, hidden: true, isDefault: false, defaultValue: null },

        /***Порядок полей для расширенного фильтра***/
        // Поле из "Basic"
        { name: 'ClientHierarchy', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        // Поле из "Basic"
        { name: 'Name', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        // Поле из "Calculation"
        { name: 'BrandTechName', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        // Поле из "Calculation"
        { name: 'EventName', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        // Поле из "Calculation"
        { name: 'Mechanic', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        { name: 'MarsMechanicName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        // Поле из "Calculation"
        { name: 'MechanicIA', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        { name: 'PlanInstoreMechanicName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        // Поле из "Calculation"
        { name: 'PromoStatusName', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        /********************************************/

        { name: 'ProductHierarchy', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'MechanicComment', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'MarsMechanicDiscount', type: 'float', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanInstoreMechanicDiscount', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'StartDate', useNull: true, type: 'date', hidden: false, isDefault: false, defaultValue: null },
        { name: 'EndDate', useNull: true, type: 'date', hidden: false, isDefault: false, defaultValue: null },
        { name: 'DispatchesStart', useNull: true, type: 'date', hidden: false, isDefault: false, defaultValue: null },
        { name: 'DispatchesEnd', useNull: true, type: 'date', hidden: false, isDefault: false, defaultValue: null },
        { name: 'OtherEventName', type: 'string', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'CalendarPriority', useNull: true, type: 'int', hidden: false, isDefault: false, defaultValue: null },
        { name: 'NeedRecountUplift', type: 'bool', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'InvoiceNumber', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'DocumentNumber', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'IsOnInvoice', type: 'boolean', hidden: false, isDefault: true },

        // Calculation
        { name: 'PlanPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoBranding', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoCost', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoBTL', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoPostPromoEffectLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoPostPromoEffectLSVW1', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoPostPromoEffectLSVW2', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoCostProdXSites', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoCostProdCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoCostProdPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoXSites', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoCostProdXSites', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoCostProdCatalogue', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoCostProdPOSMInClient', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoBaselineLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualInStoreDiscount', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanInStoreShelfPrice', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoTotalCost', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetIncrementalEarnings', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoBaselineBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },

        // Promo Closure
        { name: 'ActualPromoTIShopper', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoTIMarketing', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoBranding', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoBTL', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoCostProduction', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoCost', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoUpliftPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoIncrementalLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoLSVByCompensation', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoPostPromoEffectLSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoPostPromoEffectLSVW1', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoPostPromoEffectLSVW2', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoROIPercent', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetIncrementalNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoIncrementalMAC', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PromoStatusColor', type: 'string', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'MarsMechanicTypeName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanInstoreMechanicTypeName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'BrandName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'ColorSystemName', type: 'string', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ColorDisplayName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'RejectReasonName', type: 'string', useNull: true, hidden: true, isDefault: false, defaultValue: null },
        { name: 'ActualInStoreMechanicName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualInStoreMechanicTypeName', type: 'string', useNull: true, hidden: false, isDefault: false, defaultValue: null },

        //
        { name: 'PlanPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PlanPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetIncrementalBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetIncrementalCOGS', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNetBaseTI', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ActualPromoNSV', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },

        // InOut
        { name: 'InOut', useNull: true, type: 'boolean', hidden: false, isDefault: false, defaultValue: null },

        { name: 'LastChangedDate', useNull: true, type: 'date', hidden: false, isDefault: false, defaultValue: null },
        { name: 'LastChangedDateDemand', useNull: true, type: 'date', hidden: false, isDefault: false, defaultValue: null },
        { name: 'LastChangedDateFinance', useNull: true, type: 'date', hidden: false, isDefault: false, defaultValue: null },
        { name: 'PromoDuration', useNull: true, type: 'int', hidden: false, isDefault: false, defaultValue: null },
        { name: 'DispatchDuration', useNull: true, type: 'int', hidden: false, isDefault: false, defaultValue: null },
        { name: 'Number', useNull: true, type: 'int', hidden: false, isDefault: false, defaultValue: null, isKey: true },
        { name: 'Comment', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        { name: 'ProductZREP', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        { name: 'InstoreMechanicName', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        { name: 'InstoreMechanicTypeName', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },
        { name: 'InstoreMechanicDiscount', useNull: true, type: 'float', hidden: false, isDefault: false, defaultValue: null },
        { name: 'Priority', useNull: true, type: 'int', hidden: false, isDefault: false, defaultValue: null },
        { name: 'ProductSubrangesList', type: 'string', useNull: true, hidden: false, isDefault: true, defaultValue: null },

        // Growth Acceleration
        { name: 'IsGrowthAcceleration', useNull: true, type: 'boolean', hidden: false, isDefault: false, defaultValue: null },

        {
            name: 'DeviationCoefficient', type: 'float', hidden: false, isDefault: true,
            convert: function (value) {
                return value * 100;
            }
        },

        //Apollo Export
        { name: 'IsApolloExport', type: 'boolean', hidden: false, isDefault: false }
    ],
    proxy: {
        type: 'breeze',
        resourceName: 'HistoricalPromoes',
        reader: {
            type: 'json',
            totalProperty: 'inlineCount',
            root: 'results'
        },
        extraParams: {
            //Id промо для метода GetById в истории
            promoIdHistory: null
        }
    }
});
