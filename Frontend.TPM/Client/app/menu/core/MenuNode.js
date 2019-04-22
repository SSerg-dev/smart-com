Ext.define('App.menu.core.MenuNode', {
    parent: null,
    buttonCfg: null,
    buttons: null,
    isSearchResult: false,

    constructor: function (config) {
        this.callParent(arguments);
        Ext.apply(this, config);
    },

    isLeaf: function () {
        return !this.getButtons();
    },

    getParent: function () {
        return this.parent;
    },

    getButtonCfg: function () {
        return this.buttonCfg;
    },

    getButtons: function () {
        return this.buttons;
    }
});