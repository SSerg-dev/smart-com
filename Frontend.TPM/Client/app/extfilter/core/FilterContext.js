Ext.define('App.extfilter.core.FilterContext', {
    alternateClassName: 'App.ExtFilterContext',
    mixins: {
        observable: 'Ext.util.Observable',
        bindable: 'Ext.util.Bindable'
    },

    needShowTextFilterFirst: false,

    constructor: function (config) {
        this.callParent(arguments);
        this.mixins.observable.constructor.call(this, config);
        this.mixins.bindable.constructor.call(this, config);

        this.addEvents(
            'filtermodelchange',
            'extfilterchange'
        );

        // Внутреннее представление фильтра.
        this.filter = null;

        this.initSupportedModels();
    },

    doSelectFilterModel: function (newModel, oldModel) {
        this.activeModel = newModel;
        this.filter = newModel.getFilter();

        if (oldModel) {
            oldModel.commit();
            this.filter = oldModel.getFilter();
        }

        if (newModel && newModel.canUpdateFromFilter) {
            newModel.updateFromFilter(this.filter);
        } else {
            newModel.clear();
        }

        this.fireEvent('filtermodelchange', this, newModel, oldModel);
    },

    selectFilterModel: function (modelId) {
        var newModel = this.supportedModels.get(modelId),
            oldModel = this.getFilterModel();

        if (!newModel) {
            console.warn('Can not find model with modelId: ', modelId);
            return;
        }

        if (oldModel === newModel) {
            console.warn('Filter model will not be actually changed');
            return;
        }

        if (!this.isEmpty() && !newModel.canUpdateFromFilter) {
            Ext.Msg.confirm(
                l10n.ns('core').value('confirmTitle'),
                l10n.ns('core', 'filter').value('clearConfirmMessage'),
                function (button) {
                    if (button === 'yes') {
                        this.clear();
                        this.doSelectFilterModel(newModel, oldModel);
                    }
                },
                this);
        } else {
            this.doSelectFilterModel(newModel, oldModel);
        }
    },

    getFilterModel: function () {
        return this.activeModel;
    },

    getFilterModelById: function (modelId) {
        var supportedModels = this.getSupportedModels();
        var model = supportedModels.filter(function (x) { return x.modelId == modelId })[0];

        if (model) {
            this.activeModel = model;
        }

        return model;
    },

    getSupportedModels: function () {
        return this.supportedModels.getRange();
    },

    getFilter: function () {
        return this.filter;
    },

    initSupportedModels: function () {
        var supportedModels = this.supportedModels;

        this.supportedModels = Ext.create('Ext.util.MixedCollection', {
            allowFunctions: false,
            getKey: function (item) {
                return item.getModelId();
            }
        });

        supportedModels = Ext.Array.from(supportedModels, true).map(function (modelCfg) {
            return Ext.create(modelCfg);
        });

        this.supportedModels.addAll(supportedModels);

        if (this.supportedModels.getCount() > 0) {
            this.selectFilterModel(this.supportedModels.first().getModelId());
            // Отложенный запуск события. Для обновления иконки индикации применения фильтра. Отложенный запуск применяется так как binding событий не успевает сработать
            Ext.defer(function() { this.fireEvent('extfilterchange', this) }, 1000, this); 
        }
    },

    commit: function (suppressReload) {
        var model = this.getFilterModel();

        if (!model) {
            return;
        }

        model.commit();
        this.filter = model.getFilter();
        this.reloadStore(suppressReload);
        this.fireEvent('extfilterchange', this);
    },

    reject: function () {
        var model = this.getFilterModel();

        if (!model) {
            return;
        }

        model.reject();
    },

    clear: function (suppressReload) {
        var model = this.getFilterModel();

        if (!model) {
            return;
        }

        model.clear();
        this.filter = model.getFilter();
        this.reloadStore(suppressReload);
        this.fireEvent('extfilterchange', this);
    },

    reloadStore: function (suppressReload) {
        var store = this.getStore();

        if (store && store.remoteFilter && !suppressReload) {
            store.load();
        }
    },

    isEmpty: function () {
        return Ext.Object.isEmpty(this.filter);
    },

    onBindStore: function (store, initial) {
        Ext.destroy(this.extFilterRelayedEvents);

        if (store) {
            this.extFilterRelayedEvents = store.relayEvents(this, ['extfilterchange']);
        }
    }

});