Ext.define('App.localization.core.Localization', {
    alternateClassName: 'l10n',
    singleton: true,

    constructor: function () {
        this.resourceBuilder = Ext.create('App.util.core.ResourceBuilder');
    },

    ns: function () {
        var context = this.createContext();
        return context.ns.apply(context, arguments);
    },

    value: function (name) {
        return this.createContext().value(name);
    },

    createContext: function () {
        return Ext.create('App.localization.core.LocalizationContext', this.resourceBuilder.data);
    },

    defineLocalization: function (moduleName, config) {
        this.resourceBuilder.defineConfig(moduleName, config);
    },

    clearTempData: function() {
        this.resourceBuilder.clearTempData();
    }
});
