Ext.define('Ext.ux.data.ExtendedStore', {
    extend: 'Ext.data.Store',

    isExtendedStore: true,

    config: {
        extendedFilter: { xclass: 'App.ExtFilterContext' }
    },

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
    },

    prefetch: function (options) {
        if (!this.pageRequests[options.page]) {
            options = Ext.apply({
                extendedFilters: this.prepareExtendedFilter()
            }, options);
        }
        return this.callParent([options]);
    },

    prepareExtendedFilter: function () {
        var ef = this.getExtendedFilter(),
            ffNodes = this.fixedFilters ? Ext.Object.getValues(this.fixedFilters) : [],
            efNodes = ef && !ef.isEmpty() ? ef.getFilter() : [];

        return ffNodes.concat(efNodes);
    },

    setFixedFilter: function (id, node) {
        var filter = this.fixedFilters || {};
        filter[id] = node;
        this.fixedFilters = filter;
        if (this.remoteFilter) {
            this.load();
        }
    },

    //обработка нескольких фильтров
    setSeveralFixedFilters: function (ids, node, autoLoad) {
        var filter = this.fixedFilters || {};
        if (Ext.isArray(node)) {
            Ext.Array.each(node, function (itemnode, index) {
                filter[ids[index]] = itemnode;
            });
        }
        this.fixedFilters = filter;
        if (this.remoteFilter && autoLoad !== false) {
            this.load();
        }
    },

    removeFixedFilter: function (id) {
        var filter = this.fixedFilters;
        if (filter && filter[id]) {
            delete filter[id];
            if (this.remoteFilter) {
                this.load();
            }
        }
    },

    clearFixedFilters: function (suppressEvent) {
        delete this.fixedFilters;
        if (this.remoteFilter && !suppressEvent) {
            this.load();
        }
    },

    applyExtendedFilter: function (ctx) {
        return Ext.create(ctx);
    },

    updateExtendedFilter: function (newCtx, oldCtx) {
        if (newCtx) {
            newCtx.bindStore(this);
        }
    },

    clearAllFilters: function (suppressReload) {
        this.clearFilter(true);
        this.clearFixedFilters(true);

        var extFilter = this.getExtendedFilter();

        if (extFilter) {
            extFilter.clear(suppressReload);
        }
    }

});