Ext.define('App.util.core.ResourceManager', {
    alternateClassName: 'ResourceMgr',
    singleton: true,

    constructor: function () {
        this.controllersBuilder = Ext.create('App.util.core.ResourceBuilder');
        this.additionalMenuBuilder = Ext.create('App.util.core.ResourceBuilder');
        this.settingsBuilder = Ext.create('App.util.core.ResourceBuilder');
        this.roleModelMap = {};
    },

    //

    defineControllers: function (moduleName, controllers) {
        this.controllersBuilder.defineConfig('ct:' + moduleName, Ext.Array.from(controllers));
    },

    defineAdditionalMenu: function (moduleName, config) {
        this.additionalMenuBuilder.defineConfig(moduleName, config);
    },

    defineModuleSettings: function (moduleName, config) {
        this.settingsBuilder.defineConfig('ct:' + moduleName, config);
    },

    defineRoleModelMap: function (moduleName, config) {
        this.roleModelMap[moduleName] = config;
    },

    //

    getControllerList: function (moduleName) {
        if (moduleName) {
            return this.controllersBuilder.data[''][moduleName];
        } else {
            return Ext.Array.flatten(Ext.Object.getValues(this.controllersBuilder.data['']));
        }
    },

    getAdditionalMenu: function (moduleName) {
        if (moduleName) {
            return this.additionalMenuBuilder.data['.' + moduleName];
        } else {
            return this.additionalMenuBuilder.data;
        }
    },

    getModuleSettings: function (moduleName) {
        return this.settingsBuilder.data['.' + moduleName];
    },

    clearTempData: function () {
        this.controllersBuilder.clearTempData();
        this.additionalMenuBuilder.clearTempData();
        this.settingsBuilder.clearTempData();
    },

    getDtoByRole: function (moduleName, role, entityType) {
        var entityCfg = this.roleModelMap[moduleName][entityType],
            dtoName = null;

        Ext.Object.each(entityCfg, function (key, val) {
            if (key == 'roles' && Ext.Array.contains(val, role)) {
                dtoName = entityType;
                return false;
            }

            if (Ext.isObject(val) && val.roles && Ext.Array.contains(val.roles, role)) {
                dtoName = key;
                return false;
            }
        });

        return dtoName;
    },

    getWidgetByRole: function (moduleName, role, entityType) {
        var dtoName = this.getDtoByRole(moduleName, role, entityType),
            cfg = this.roleModelMap[moduleName][entityType];

        if (dtoName && dtoName !== entityType) {
            return cfg[dtoName].widget;
        } else {
            return cfg.widget;
        }
    }
});
