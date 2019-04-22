Ext.define('App.view.core.common.ExpandButton', {
    extend: 'Ext.button.Button',
    alias: 'widget.expandbutton',

    constructor: function (config) {
        Ext.apply(this, config);

        this.defaultGlyph = config.glyph;

        Ext.apply(this, {
            listeners: {
                scope: this,
                click: this.toggleHander
            }
        });

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
        App.Util.callWhenRendered(this, this.updateGlyph, this);
    },

    toggleHander: function () {
        this.toggleCollapse();
        this.updateGlyph();
    },

    updateGlyph: function () {
        var glyph = this.isCollapsed() ? this.defaultGlyph : this.glyph1
        this.setGlyph(glyph);
    },

    getTarget: function () {
        if (Ext.isString(this.target)) {
            this.target = Ext.ComponentQuery.query(this.target)[0];
        } else if (Ext.isFunction(this.target)) {
            this.target = Ext.callback(this.target, this, [this]);
        }

        return this.target;
    },

    toggleCollapse: function () {
        this.getTarget().toggleCollapse();
    },

    isCollapsed: function () {
        return this.getTarget().collapsed;
    }

});