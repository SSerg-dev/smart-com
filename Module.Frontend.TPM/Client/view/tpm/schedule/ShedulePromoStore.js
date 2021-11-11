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
        var competitorOption = {
            "operator": "and",
            "rules": []
        };
        var competitorPromoRules = [{
                    "property": "TypeName",
                    "operation": "Equals",
                    "value": "Competitor"
                }];
        var competitorPromoFields = ['PromoStatusName', 'Name', 'CompetitorBrandTechName']
        var excludeFromPromoFields = ['CompetitorBrandTechName'];
        var unionOption = {
            "operator": "or",
            "rules": []
        };
        options.extendedFilters.forEach(function (item) {
            if (item.rules != null) {
                //var compRules = item.rules.filter(function (el){
                //    return competitorPromoFields.includes(el.property);
                //});
                item.rules.forEach(function(el){
                    if(competitorPromoFields.includes(el.property))
                        competitorPromoRules.push(el);
                });
                //compRules.push(competitorPromoRules);
                //competitorPromoRules = compRules;
                item.rules = item.rules.filter(function (el){
                    return !excludeFromPromoFields.includes(el.property);
                });
                unionOption.rules.push(item);
            }
        });
        competitorOption.rules = competitorPromoRules;
        unionOption.rules.push(competitorOption);
        options.extendedFilters = options.extendedFilters.filter(function (item){
            return item.rules == null;
        });
        options.extendedFilters.push(unionOption);

        return this.callParent([options]);
    }
});