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
        return this.callParent([options]);
    }
});