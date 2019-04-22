Ext.define('App.view.core.common.CombinedDirectoryTreePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.combineddirectorytreepanel',
    mixins: {
        selectable: 'App.util.core.Selectable',
        securableActions: 'App.util.core.SecurableActions'
    },

    animCollapse: false,
    frame: true,
    ui: 'selectable-panel',
    minHeight: 335,

    layout: {
        type: 'card',
        deferredRender: true
    },

    //dockedItems: [{
    //    xtype: 'standarddirectorytoolbar',
    //    dock: 'right'
    //}],

    systemHeaderItems: [{
        glyph: 0xf4e6,
        itemId: 'refresh',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('update')
    }, {
        xtype: 'expandbutton',
        glyph: 0xf063,
        glyph1: 0xf04b,
        itemId: 'collapse',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('collapse'),
        target: function () {
            return this.up('combineddirectorytreepanel');
        }
    }],

    customHeaderItems: [
        ResourceMgr.getAdditionalMenu('core').base,
        ResourceMgr.getAdditionalMenu('core').import
    ],

    customHeaderItemsDefaults: {
        border: '0 0 0 1',
        cls: 'custom-button'
    },

    systemHeaderItemsDefaults: {
        padding: '6 6 6 16'
    },

    header: {
        titlePosition: 0,
        height: 30,

        defaults: {
            xtype: 'button',
            ui: 'white-button',
            padding: 6 //TODO: временно
        }
    },

    constructor: function () {
        this.callParent(arguments);
        this.mixins.selectable.constructor.call(this, arguments);
        this.mixins.securableActions.constructor.call(this, arguments);
    },

    //initComponent: function () {
    //    var systemHeaderItems = Ext.Array.from(this.systemHeaderItems, true),
    //        customHeaderItems = Ext.Array.from(this.customHeaderItems, true);

    //    customHeaderItems.forEach(function (item) {
    //        Ext.applyIf(item, this.customHeaderItemsDefaults);
    //    }, this);

    //    systemHeaderItems.forEach(function (item) {
    //        Ext.applyIf(item, this.systemHeaderItemsDefaults);
    //    }, this);

    //    this.header.items = customHeaderItems.concat(systemHeaderItems);

    //    this.callParent(arguments);
    //},

    updateHeader: function () {
        this.updateHeaderInternal(); // workaround for correct replace customHeaderItems
        this.callParent(arguments);
    },

    updateHeaderInternal: function () {
        var systemHeaderItems = Ext.Array.from(this.systemHeaderItems, true),
            customHeaderItems = Ext.Array.from(this.customHeaderItems, true);

        customHeaderItems.forEach(function (item) {
            Ext.applyIf(item, this.customHeaderItemsDefaults);
        }, this);

        systemHeaderItems.forEach(function (item) {
            Ext.applyIf(item, this.systemHeaderItemsDefaults);
        }, this);

        this.header.items = customHeaderItems.concat(systemHeaderItems);
    },

    initItems: function () {
        this.callParent(arguments);

        // TODO: в будущем лучше убрать убрать этот код,
        // но для этого необходимо, что-бы у грида был name или itemId
        var grid = this.items.findBy(function (item) {
            return item.isXType('grid') && item.isConfigurable;
        });

        if (grid) {
            grid.initColumnsState(this.getXType());
        }
    },

    afterRender: function () {
        this.callParent(arguments);
        this.mixins.selectable.afterRender.call(this, arguments);
        this.mixins.securableActions.afterRender.call(this, arguments);

        var lastCustomBtn = this.down('button[cls="custom-button"][hidden=false]:last');
        if (lastCustomBtn) {
            lastCustomBtn.setBorder('0 1 0 1');
        }
    },

    onDestroy: function () {
        this.callParent(arguments);
        this.mixins.selectable.onDestroy.call(this, arguments);
    },

    getBaseModel: function (exactryModel) {
        var model = Ext.ModelManager.getModel(this.down('basetreegrid').getStore().model);
        var bm = exactryModel ? null : this.baseModel;
        return bm || model;
    },

    getDefaultResource: function (exactryModel) {
        return this.getBaseModel(exactryModel).getProxy().resourceName;
    },

    doCollapseExpand: function (flags, animate) {
        var me = this,
            container = me.up('[hasScroll=true]');

        this.callParent(arguments);

        if (container) {
            container.doLayout();
        }
    },

    getParent: function () {
        var view = this.up('associateddirectoryview');
        if (view) {
            return view.query('combineddirectorypanel[id!=' + this.getId() + ']').find(function (item) {
                if (item.linkConfig && Ext.isObject(item.linkConfig)) {
                    return item.linkConfig.hasOwnProperty(this.getXType());
                }
                return false;
            }, this);
        }
    },

    getChildren: function () {
        var view = this.up('associateddirectoryview');
        if (view && this.linkConfig && Ext.isObject(this.linkConfig)) {
            return view.query(Ext.Object.getKeys(this.linkConfig).join(', '));
        }
    },

    isMain: function () {
        return !this.getParent();
    }

});