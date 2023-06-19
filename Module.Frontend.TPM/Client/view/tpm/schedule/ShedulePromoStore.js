// Стор для промо в календаре
// Должен быть унаследован от Sch.data.EventStore для корректной работы календаря
// Должен включать Ext.ux.data.ExtendedStore для корректной работы расширенного фильтра
// TODO: вынести в соответствующую папку
Ext.define('App.store.core.SchedulePromoStore', {
    extend: 'Sch.data.EventStore',
    mixins: ['Ext.ux.data.ExtendedStore'],
    alias: 'store.schedulepromostore',
    storeId: 'MyTasks',
    model: 'App.model.tpm.promo.PromoView',
    autoLoad: true,
    autoSync: false,
    remoteFilter: true,
    pageSize: 20000, // TODO: загружать буферизированно

    constructor: function (config) {
        this.model = TpmModes.isRsRaMode() ? 'App.model.tpm.promo.PromoRSView' : 'App.model.tpm.promo.PromoView';
        this.callParent(arguments);
        this.initConfig(config);
    },

    load: function (options) {
        if (this.remoteFilter) {
            if (Ext.isFunction(options)) {
                options = {
                    callback: options
                };
            } else {
                options = Ext.apply({}, options);
            }
            options.extendedFilters = this.prepareExtendedFilter();
        }

        var promoOption = {
            "operator": "and",
            "rules": []
        };
        var competitorOption = {
            "operator": "and",
            "rules": []
        };

        var promoRules = [{
                    "property": "TypeName",
                    "operation": "NotEqual",
                    "value": "Competitor"
                }];
        var competitorPromoFields = ['Name', 'CompetitorBrandTechName', 'StartDate', 'EndDate', 'Price', 'Discount']
        var excludeFromPromoFields = ['CompetitorBrandTechName', 'Price', 'Discount'];

        var calendarFilters = Ext.ComponentQuery.query('#nascheduler')[0].filter;
        if (calendarFilters) {
            var calendarCompetitorFilters = calendarFilters.rules.filter(function (el) {
                return competitorPromoFields.includes(el.property) || el.rules != null;
            });

            var competitorPromoRules = calendarCompetitorFilters;

            var unionOption = {
                "operator": "or",
                "rules": []
            };

            promoRules = promoRules.concat(calendarFilters.rules.filter(function (el) {
                return !excludeFromPromoFields.includes(el.property);
            }));

            competitorOption.rules = competitorPromoRules;//competitorPromoRules;
            unionOption.rules.push(competitorOption);
            promoOption.rules = promoRules;
            unionOption.rules.push(promoOption);
            options.extendedFilters = options.extendedFilters.filter(function (item) {
                return item.rules == null;
            });
            options.extendedFilters.push(unionOption);
        }
        return this.callParent([options]);
    }
});